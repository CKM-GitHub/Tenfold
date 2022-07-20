using Models;
using Newtonsoft.Json.Linq;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_assess_guide;
using Seruichi.Common;
using Seruichi.Tenfold.Web.Controllers.Common;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Models.Tenfold.t_assess_guide;
using System; 
using Ionic.Zip;
using Aspose.Zip;
using Aspose.Zip.Saving;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace Seruichi.Tenfold.Web.Controllers
{
    [AllowAnonymous]
    public class CommonApiController : BaseApiController
    {
        [HttpPost]
        public HttpResponseMessage GetMessage([FromBody] MessageModel model)
        {
            if (model == null) return BadRequestResult();
            return OKResult(StaticCache.GetMessage(model.MessageID));
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfPrefecture()
        {
            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfPrefecture());
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfCity([FromBody] JToken token)
        {
            var model = new
            {
                PrefCD = token["PrefCD"].ToStringOrEmpty()
            };
            if (string.IsNullOrEmpty(model.PrefCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfCity(model.PrefCD));
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfTown([FromBody] JToken token)
        {
            var model = new
            {
                PrefCD = token["PrefCD"].ToStringOrEmpty(),
                CityCD = token["CityCD"].ToStringOrEmpty()
            };
            if (string.IsNullOrEmpty(model.PrefCD) || string.IsNullOrEmpty(model.CityCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfTown(model.PrefCD, model.CityCD));
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfLine([FromBody] JToken token)
        {
            var model = new
            {
                PrefCD = token["PrefCD"].ToStringOrEmpty(),
            };
            if (string.IsNullOrEmpty(model.PrefCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfLine(model.PrefCD));
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfStation([FromBody] JToken token)
        {
            var model = new
            {
                LineCD = token["LineCD"].ToStringOrEmpty(),
            };
            if (string.IsNullOrEmpty(model.LineCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfStation(model.LineCD));
        }

        [HttpPost]
        public HttpResponseMessage GetBuildingAge([FromBody] string constYYYYMM)
        {
            if (string.IsNullOrEmpty(constYYYYMM))
            {
                return OKResult("");
            }
            CommonBL bl = new CommonBL();
            return OKResult(bl.GetBuildingAge(constYYYYMM).ToStringOrEmpty());
        }

        [HttpPost]
        public HttpResponseMessage GetNearestStations([FromBody] JToken token)
        {
            string prefName = token["PrefName"].ToStringOrEmpty();
            string cityName = token["CityName"].ToStringOrEmpty();
            string townName = token["TownName"].ToStringOrEmpty();
            string address = token["Address"].ToStringOrEmpty();

            CommonBL blCmm = new CommonBL();
            var longitude_latitude = blCmm.GetLongitudeAndLatitude(prefName, cityName, townName, address);
            var nearestStations = blCmm.GetNearestStations(longitude_latitude);

            if (nearestStations.Count == 0)
            {
                return OKResult();
            }
            else
            {
                return OKResult(base.ConvertToJson(nearestStations));
            }
        }

        [IgnoreVerificationToken]
        [HttpPost]
        public HttpResponseMessage CheckBirthday([FromBody] string birthday)
        {
            if (string.IsNullOrEmpty(birthday))
            {
                return OKResult();
            }

            Validator validator = new Validator();
            if (!validator.CheckBirthday(birthday, out string errorcd, out string formattedDate))
            {
                return ErrorMessageResult(errorcd, formattedDate);
            }
            else
            {
                return OKResult(formattedDate);
            }
        }
        
        [HttpPost]
        public async Task<string> UploadFiles()
        {
            var OFU = "Original File";
            var ZFS = "Zip Saved File";
            var DF = "Downloaded File";
            var ZTemp = "ZTemp";
            var DTS = DateTime.Now.ToString("yyyy-MM-dd");
            var Temp = "temp";

            //base.GetOperator();
            if (!Request.Content.IsMimeMultipartContent())
            {
                return GetUnsupportedMediaTypeResult();
            }

            var model = base.GetFromRequestForm<t_assess_guideModel>();
            if (model == null || string.IsNullOrEmpty(model.UserCD))
            {
                return GetBadRequestResult();
            }
            
            if (string.IsNullOrEmpty(StaticCache.AttachmentInfo.Destin))
            {
                throw new CustomException("ファイルを保存するフォルダが設定されていません。");
            } 
            string path = Path.Combine(StaticCache.AttachmentInfo.Destin, OFU , model.UserCD);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var provider = new MultipartFormDataStreamProvider(path);
            await Request.Content.ReadAsMultipartAsync(provider);

            if (!Directory.Exists(Path.Combine(path ,  DTS)))
                Directory.CreateDirectory(Path.Combine(path, DTS));
            if (!Directory.Exists(Path.Combine(path, DTS, Temp)))
                Directory.CreateDirectory(Path.Combine(path, DTS,Temp));

            var bl = new t_assess_guideBL();
            foreach (var file in provider.FileData)
            {

                string ChunkSize = file.Headers.ContentDisposition.Name;

                string fileName = file.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                var fileFullName = Path.Combine(path, DTS, Temp, fileName);
                if (File.Exists(fileFullName))
                {
                    try
                    {
                        using (FileStream stream = File.Open(fileFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            stream.Dispose();
                        }
                        File.Delete(fileFullName);
                    }
                    catch
                    {
                        try
                        { File.Delete(fileFullName); }
                        catch (Exception ex) { var msg = ex.Message; }
                    }
                }
                File.Move(Path.Combine(path, DTS, Temp, file.LocalFileName), fileFullName);

                model.AttachFileName = fileName;
                model.AttachSize = ChunkSize.Split('_')[1].ToString().Split('.')[0];
                model.AttachFileType = Path.GetExtension(fileName);
                model.AttachSEQ = (ChunkSize.Split('_')[2].ToString().Replace("\"", "").ToInt32() + 1).ToString();
                //Save OUF
                var dtOF = bl.CreateGuideAttach(model);
                if (dtOF != null)
                {

                    //Save ZSF
                    model.AttachSEQ = dtOF.Rows[0]["AttachedSEQ"].ToString();
                    var IsExist = true;
                    var ZsfName = "";
                    while (IsExist)
                    {
                        ZsfName = model.UserCD + "-" + DateTime.Now.ToString("yyyy-MM-dd:hhmmss").Replace(":", "").Replace("-", "") + "-" + (new Random().Next(0, 1000000)).ToString().PadLeft(6, '0');
                        IsExist = bl.CheckExistZippedName(ZsfName);
                    }
                    IsExist = true;
                    var pas = model.UserCD + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + (new Random().Next(0, 1000000)).ToString().PadLeft(6, '0');

                    if (!Directory.Exists(Path.Combine(path, DTS, ZTemp)))
                        Directory.CreateDirectory(Path.Combine(path, DTS, ZTemp));
                    if (!Directory.Exists(Path.Combine(StaticCache.AttachmentInfo.Destin, ZFS, DTS)))
                        Directory.CreateDirectory(Path.Combine(StaticCache.AttachmentInfo.Destin, ZFS, DTS));

                    try
                    {
                        Archive archive = new Archive(new ArchiveEntrySettings(encryptionSettings: new TraditionalEncryptionSettings(pas)));

                        archive.CreateEntry(Path.GetFileName(fileFullName), fileFullName);

                        //Save the archive
                        archive.Save(Path.Combine(StaticCache.AttachmentInfo.Destin, ZFS, DTS, ZsfName + ".zip"));
                        archive.Dispose();
                        //CreateGuideAttachZipped
                        model.ZippedFileName = ZsfName;
                        model.AttachFileUnzipPW = pas;
                        if (bl.CreateGuideAttachZipped(model))
                        {
                            model.AttachFilePath = Path.Combine(StaticCache.AttachmentInfo.Destin, ZFS, DTS);
                            if (bl.CreateGuideAttachZippedFilePath(model))
                            { 
                                FileInfo currentFile = new FileInfo(Path.Combine(StaticCache.AttachmentInfo.Destin, ZFS, DTS, ZsfName + ".zip"));
                                currentFile.MoveTo(Path.Combine(StaticCache.AttachmentInfo.Destin, ZFS, DTS, ZsfName));
                            }
                        }
                    }
                    catch
                    {

                    }
                    
                }
            }

            try
            {
                //Delete All Under OFU
                Directory.Delete(Path.Combine( path), true);
            }
            catch (Exception Ex)
            {

            }

            return GetSuccessResult();
        }
        public bool  ExtractToDownload(t_assess_guideModel model, out string path)
        {
            path = "";
            t_assess_guideBL tbl = new t_assess_guideBL();
            var CheckPermission = tbl.CheckUserPermission(model.RealECD, model.ReStaffCD);
            if (!model.UserCD.Contains("T"))
            if (!CheckPermission)
                return false; 
            string DF = Path.Combine(StaticCache.AttachmentInfo.Destin, "Downloaded File" , model.UserCD );
            if (!Directory.Exists(DF))
            {
                Directory.CreateDirectory(DF);
            }
            string[] files = Directory.GetFiles(DF, "*.", SearchOption.AllDirectories);
            foreach (string s in files)
            {
                try
                {
                    File.Delete(s);
                }
                catch { }
            }
            try
            {
                FileInfo currentFile = null;
                if (File.Exists(Path.Combine(model.AttachFileZippedFullPathName)))
                {
                    currentFile = new FileInfo(model.AttachFileZippedFullPathName);
                    currentFile.MoveTo(Path.Combine(model.AttachFileZippedFullPathName + ".zip"));
                } 
                using (FileStream zipFile = File.Open(model.AttachFileZippedFullPathName + ".zip", FileMode.Open))
                { 
                    using (var archive = new Archive(zipFile, new ArchiveLoadOptions() { DecryptionPassword = model.AttachFileUnzipPW }))
                    { 
                        archive.ExtractToDirectory(DF);
                        path = Path.Combine(DF);
                    }
                }
                if (File.Exists(Path.Combine(model.AttachFileZippedFullPathName + ".zip")))
                {
                    currentFile = new FileInfo(Path.Combine(model.AttachFileZippedFullPathName + ".zip"));
                    currentFile.MoveTo(Path.Combine(model.AttachFileZippedFullPathName));
                }
            }
            catch (Exception ex)
            {
                //return false;
            }
            return true; 
        }

    }
}
