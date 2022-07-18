using Seruichi.Common;
using System;
using Seruichi.BL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL.Tenfold.t_assess_guide;
using Models.Tenfold.t_assess_guide;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_assess_guideController : BaseController
    {
        // GET: t_assess_guide
        public ActionResult Index()
        {
            var bl = new t_assess_guideBL();
            ViewBag.TenstaffList = bl.GetDropDownListItems();
            ViewBag.Exts = StaticCache.AttachmentInfo.Exts;
            ViewBag.UserCD = base.GetOperator();
            ViewBag.Size = StaticCache.AttachmentInfo.Max_Size.Replace("MB", "").Replace(" ", "");
            return View();

        }

        public ActionResult get_t_assess_guide_DisplayData(t_assess_guideModel model)
        {
            t_assess_guideBL bl = new t_assess_guideBL();

            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.get_t_asses_guide_DisplayData(model);
            if (model.IsCSV)
            {
                dt.Columns["AssessType"].ColumnName = "種類";
                dt.Columns["SendCustomer"].ColumnName = "送客";
                dt.Columns["AssessReqID"].ColumnName = "査定依頼ID";
                dt.Columns["SellerMansionID"].ColumnName = "物件CD";
                dt.Columns["MansionName"].ColumnName = "物件名";
                dt.Columns["RoomNo"].ColumnName = "部屋";
                dt.Columns["SellerCD"].ColumnName = "売主CD";
                dt.Columns["SellerName"].ColumnName = "売主名";

                dt.Columns["InsertDate"].ColumnName = "登録日時";
                dt.Columns["EasyDate"].ColumnName = "簡易査定日時";
                dt.Columns["DeepDate"].ColumnName = "詳細査定日時";
                dt.Columns["PurchaseDate"].ColumnName = "買取依頼日時";
                dt.Columns["AssessAmount"].ColumnName = "買取依頼金額";
                dt.Columns["IntroDate"].ColumnName = "送客日時";
                dt.Columns["Rank"].ColumnName = "ランキング";

                dt.Columns.Remove("RoomNumber");

                // SetColumnsOrder(dt);

                dt.AcceptChanges();

            }
            return OKResult(DataTableToJSON(dt));
        }

        public ActionResult get_t_assess_guide_AttachFiles(t_assess_guideModel model)
        {
            t_assess_guideBL bl = new t_assess_guideBL();
            var dt = bl.get_t_assess_guide_AttachFiles(model);
            foreach (DataRow dr in dt.Rows)
            {
                var flt = (Convert.ToDouble(dr["AttachFileSize"].ToString()) / (1024 * 1024));
                dr["AttachFileSize"] = Math.Round(flt, 3) + " MB";
            }
            return OKResult(DataTableToJSON(dt));
        }
        public ActionResult DeleteAttachZippedFilePath(t_assess_guideModel model)
        {
            t_assess_guideBL bl = new t_assess_guideBL();
            if (!String.IsNullOrEmpty(model.AttachSEQ))
            {
                var items = model.AttachSEQ.Split(' ');
                foreach (var itms in items)
                {
                    itms.ToString().TrimStart();
                    if (!string.IsNullOrEmpty(itms.Trim()))
                    {
                        model.IntroReqID = itms.Split('_')[1].ToString();
                        model.AttachSEQ = itms.Split('_')[2].ToString();
                        model.UserCD = itms.Split('_')[3].ToString();
                        var dts = bl.DeleteAttachZippedFilePath(model);
                        var file = Path.Combine(dts.Rows[0]["AttachFilePath"].ToString(), dts.Rows[0]["ZippedFileName"].ToString());
                        if (System.IO.File.Exists(file))
                            System.IO.File.Delete(file);
                    }
                }


            }

            return OKResult();
        }
        public ActionResult DownloadAttachZippedFilePath(t_assess_guideModel model)
        {
            t_assess_guideBL bl = new t_assess_guideBL();
            if (!string.IsNullOrEmpty(model.AttachSEQ.Trim()))
            {
                model.IntroReqID = model.AttachSEQ.Split('_')[1].ToString();
                model.UserCD = model.AttachSEQ.Split('_')[3].ToString();
                model.AttachSEQ = model.AttachSEQ.Split('_')[2].ToString();
            }
            var AttachDown = bl.DownAttachZippedFilePath(model);
            CommonApiController cmi = new CommonApiController();
            model.AttachFileZippedFullPathName = Path.Combine(AttachDown.Rows[0]["AttachFilePath"].ToStringOrEmpty(), AttachDown.Rows[0]["ZippedFileName"].ToStringOrEmpty());
            model.AttachFileUnzipPW = AttachDown.Rows[0]["AttachFileUnzipPW"].ToStringOrEmpty();
            model.AttachFileName = AttachDown.Rows[0]["AttachFileName"].ToStringOrEmpty() + AttachDown.Rows[0]["AttachFileType"].ToStringOrEmpty();
            byte[] bytes = null;
            //Create Download
            if (cmi.ExtractToDownload(model, out string outPath))
            {  //SaveLog 
                bl.AttachFileDownLog(model);
                bytes = System.IO.File.ReadAllBytes(Path.Combine(outPath, model.AttachFileName));
                //Delete DownedFile
                if (System.IO.File.Exists(Path.Combine(outPath, model.AttachFileName)))
                {
                    System.IO.File.Delete(Path.Combine(outPath, model.AttachFileName));
                }
            }
            else
                return this.HttpNotFound();
          
            return File(bytes, "application/octet-stream", model.AttachFileName);
        }
    }
}