using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL;
using Models.RealEstate.r_login;
using Seruichi.BL.RealEstate.r_asmc_ms_reged_list;
using Models.RealEstate.r_asmc_ms_reged_list;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_ms_reged_listController : BaseController
    {
        // GET: r_asmc_ms_reged_list
        public ActionResult Index()
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }

            ViewBag.Url = System.Web.HttpContext.Current.Request.UrlReferrer;
            r_asmc_ms_reged_listBL bl = new r_asmc_ms_reged_listBL();
            List<M_Pref> prefList = new List<M_Pref>();
            DataTable dt = bl.GetM_Pref();
            prefList = (from DataRow dr in dt.Rows
                        select new M_Pref()
                        {
                            PrefCD = dr["PrefCD"].ToString(),
                            PrefName = dr["PrefName"].ToString()
                        }).ToList();
            List<M_Pref_And_CityGPCD> prefcitygpcdList = new List<M_Pref_And_CityGPCD>();
            DataTable dt1 = bl.Get_Prefcd_and_CityGPCD();
            prefcitygpcdList = (from DataRow dr in dt1.Rows
                                select new M_Pref_And_CityGPCD()
                                {
                                    PrefCD = dr["PrefCD"].ToString(),
                                    PrefName = dr["PrefName"].ToString(),
                                    CityGPCD = dr["CityGPCD"].ToString(),
                                    CityGPName = dr["CityGPName"].ToString(),
                                    AddressLevel = dr["AddressLevel"].ToString()
                                }).ToList();
            List<M_Pref_And_CityGPCD_And_CityCD> cityList = new List<M_Pref_And_CityGPCD_And_CityCD>();
            DataTable dt2 = bl.Get_Prefcd_and_CityGPCD_and_CityCD();
            cityList = (from DataRow dr in dt2.Rows
                        select new M_Pref_And_CityGPCD_And_CityCD()
                        {
                            PrefCD = dr["PrefCD"].ToString(),
                            PrefName = dr["PrefName"].ToString(),
                            CityGPCD = dr["CityGPCD"].ToString(),
                            CityGPName = dr["CityGPName"].ToString(),
                            CityCD = dr["CityCD"].ToString(),
                            CityName = dr["CityName"].ToString()
                        }).ToList();

            string strUrl = ViewBag.Url.Segments[1].Replace("/","");
            ViewBag.PrefCD = prefList;
            ViewBag.CityGPCD = prefcitygpcdList;
            ViewBag.CityCD = cityList;
            ViewBag.StrURL = strUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Get_DataList(r_asmc_ms_reged_listModel model)
        {
            if (String.IsNullOrWhiteSpace(model.MansionName) && String.IsNullOrWhiteSpace(model.CityCD) && String.IsNullOrWhiteSpace(model.CityGPCD) && String.IsNullOrWhiteSpace(model.StartYear) && String.IsNullOrWhiteSpace(model.EndYear) && String.IsNullOrWhiteSpace(model.Radio_Rating) && String.IsNullOrWhiteSpace(model.Check_Expired))
            {
                return ErrorMessageResult("E303");
            }
            else
            {
                DataTable dt = new DataTable();
                r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
                r_asmc_ms_reged_listBL bl = new r_asmc_ms_reged_listBL();
                var validationResult = bl.ValidateAll(model);
                if (validationResult.Count > 0)
                {
                    return ErrorResult(validationResult);
                }
                dt = bl.Get_DataList(model, user.RealECD);
                return OKResult(DataTableToJSON(dt));
            }
        }

        [HttpPost]
        public ActionResult Insert_l_log(r_asmc_ms_reged_listModel model)
        {
            r_asmc_ms_reged_listBL bl = new r_asmc_ms_reged_listBL();
            model = Getlogdata(model);
            bl.Insert_r_asmc_ms_reged_list_L_Log(model);
            return OKResult();
        }

        public r_asmc_ms_reged_listModel Getlogdata(r_asmc_ms_reged_listModel model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 2;
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();
            model.PageID = "r_asmc_ms_reged_list";
            model.ProcessKBN = "link";
            model.Remarks = "r_asmc_set_ms" +" "+model.MansionCD+" "+ model.MansionName;
            return model;
        }
    }
}