﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_seller_list;
using Seruichi.BL.Tenfold.t_seller_list;
using Seruichi.Common;


namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_listController : BaseController
    {
        // GET: t_seller_list
        public ActionResult Index()
        {
            t_seller_listBL bl = new t_seller_listBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListForPref();
            return View();
        }

        //[HttpPost]
        //public ActionResult CheckSellerName(string SellerName)
        //{
        //    if (SellerName == null)
        //    {
        //        return BadRequestResult();
        //    }

        //    //Validator validator = new Validator();
        //    //string errorcd = "";
        //    //if (!validator.CheckIsHalfWidth(SellerName, 10, out errorcd))
        //    //{
        //    //    return ErrorMessageResult(errorcd);
        //    //}
        //    return OKResult();
        //}

        [HttpPost]
        public ActionResult GetM_SellerList(t_seller_listModel model)
        {
            t_seller_listBL bl = new t_seller_listBL();

            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.ValidCheck.ToString());
            chk_lst.Add(model.InValidCheck.ToString());
            chk_lst.Add(model.expectedCheck.ToString());
            chk_lst.Add(model.negtiatioinsCheck.ToString());
            chk_lst.Add(model.endCheck.ToString());
            
            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            
            var dt = bl.GetM_SellerList(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Generate_CSV(t_seller_listModel model)
        {
            t_seller_listBL bl = new t_seller_listBL();
            var dt1 = bl.Generate_CSV(model);
            //Generate CSV filel
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                // Change encoding to Shift-JIS
                string userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string strFilePath = userProfileFolder + "\\Downloads\\";
 
                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }
                var fileName = "売主リスト" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var filePath = Path.Combine(strFilePath, fileName + ".csv");
                StreamWriter sw = new StreamWriter(filePath, false);
                //headers    
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    sw.Write(dt1.Columns[i]);
                    if (i < dt1.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt1.Rows)
                {
                    for (int i = 0; i < dt1.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dt1.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            return OKResult();
        }

        [HttpPost]
        public ActionResult InsertM_SellerMansion_L_Log(t_seller_mansion_l_log_Model model)
        {
            t_seller_listBL bl = new t_seller_listBL();
            bl.InsertM_Seller_L_Log(model);
            return OKResult();
        }
    }
}