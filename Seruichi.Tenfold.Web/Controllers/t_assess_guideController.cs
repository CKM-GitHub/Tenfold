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
using System.Net;  
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using Models.Tenfold.t_reale_purchase;
using Seruichi.BL.Tenfold.t_reale_purchase;

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
            var res = new DataTable();
            var dt = bl.get_t_asses_guide_DisplayData(model);
            if (model.IsCSV)
            {

                dt.Columns["SellerMansionID"].ColumnName = "物件CD";
                dt.Columns["SellerCD"].ColumnName = "売主CD";
                dt.Columns["RoomNumber"].ColumnName = "部屋";

                //部屋
                res = SetColumnsOrder(dt); 
                res.AcceptChanges();
                return OKResult(DataTableToJSON(res));
            }
            return OKResult(DataTableToJSON(dt));
        }
        private DataTable SetColumnsOrder(DataTable dt)
        {
            string[] str = new string[] { "No", "ステータス", "査定ID", "物件CD", "物件名", "部屋", "住所", "売主CD", "売主名", "不動産会社CD", "不動産会社", "管理担当", "登録日時", "簡易査定日時", "詳細査定日時", "買取依頼日時", "経過時間", "送客予定日" };
            var colList = dt.Columns;
            var dtClone = dt.Clone();
            foreach (DataColumn col in dtClone.Columns)
            {
                if (!str.Contains(col.ColumnName))
                    dt.Columns.Remove(dt.Columns[col.ColumnName]);
                dt.AcceptChanges(); 
                
            }
            int i = 0;
            foreach (var col in str)
            { 
                dt.Columns[col].SetOrdinal(i);
                i++;
            }
            return dt;
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
        public ActionResult DownloadAttachZippedFilePath(t_assess_guideModel model )
        {
            if (model.AttachSEQ == null && Session["OctetBytesDownload"] != null)
                return File(Session["OctetBytesDownload"] as byte[], "application/force-download", model.AttachFileName);
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
                Session["OctetBytesDownload"] = bytes;
                ////Delete DownedFile
                if (model.AttachSEQ == null)
                {
                    if (System.IO.File.Exists(Path.Combine(outPath, model.AttachFileName)))
                    {
                        System.IO.File.Delete(Path.Combine(outPath, model.AttachFileName));
                    }
                } 
                return File(bytes, "application/force-download", model.AttachFileName);
            } 
                return this.HttpNotFound();

        } 
        public ActionResult Insert_L_Log(t_reale_purchase_l_log_Model model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            model.LoginKBN = 3;
            model.RealECD = null;
            model.Page = "t_assess_guide";
            //model.Processing = model.LogFlg == "3" ? "csv" : "display"; 
            bl.Insert_L_Log(model);
            return OKResult();
        }
    }
}