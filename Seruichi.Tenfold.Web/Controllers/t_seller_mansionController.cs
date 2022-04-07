using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_seller_mansion;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_seller_mansion;
using Seruichi.Common;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_mansionController : BaseController
    {
        // GET: t_seller_mansion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetM_SellerMansionList(t_seller_mansionModel model)
        {
            t_seller_mansionBL bl = new t_seller_mansionBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.Chk_Mi.ToString());
            chk_lst.Add(model.Chk_Kan.ToString());
            chk_lst.Add(model.Chk_Satei.ToString());
            chk_lst.Add(model.Chk_Kaitori.ToString());
            chk_lst.Add(model.Chk_Kakunin.ToString());
            chk_lst.Add(model.Chk_Kosho.ToString());
            chk_lst.Add(model.Chk_Seiyaku.ToString());
            chk_lst.Add(model.Chk_Urinushi.ToString());
            chk_lst.Add(model.Chk_Kainushi.ToString());

            var validationResult = bl.ValidateAll(model,chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            var dt = bl.GetM_SellerMansionList(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult InsertM_SellerMansion_L_Log(t_seller_mansion_l_log_Model model)
        {
            t_seller_mansionBL bl = new t_seller_mansionBL();
            bl.InsertM_SellerMansion_L_Log(model);
            return OKResult();
        }
        [HttpPost]
        public ActionResult Insert_l_log(t_seller_mansion_l_log_Model model)
        {
            t_seller_mansionBL bl = new t_seller_mansionBL();
            model = Getlogdata(model);
            bl.InsertM_SellerMansion_L_Log(model);
            return OKResult();
        }
        public t_seller_mansion_l_log_Model Getlogdata(t_seller_mansion_l_log_Model model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress= base.GetClientIP();
            if (model.LogStatus == "t_mansion_detail")
                model.Page = model.LogStatus;
            model.Processing = "www.seruichi.com" + model.LogId;
            model.Remarks = model.LogId + " " + bl.GetMansionNamebyMansioncd(model.LogId);
            return model;
        }

        [HttpPost]
        public ActionResult Generate_M_SellerMansionCSV(t_seller_mansionModel model)
        {
            t_seller_mansionBL bl = new t_seller_mansionBL();
            var dt = bl.Generate_M_SellerMansionCSV(model);
            if (dt != null && dt.Rows.Count > 0)
            {
                bool aa = Generate_CSV(dt);
                string userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string strFilePath = userProfileFolder + "\\Downloads\\";
                CommonBL cBl = new CommonBL();
            } 
            return OKResult();
        }
        public bool Generate_CSV(DataTable dt)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + ',';
            }

            //Add new line.
            csv += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            //Download the CSV file.
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(csv);
            Response.Flush();
            Response.End();
            return true;
        }
    }
}