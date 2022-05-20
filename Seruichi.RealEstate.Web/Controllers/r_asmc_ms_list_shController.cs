using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL;
using Models.RealEstate.r_asmc_ms_list_sh;
using Models.RealEstate.r_login;
using Seruichi.BL.RealEstate.r_asmc_ms_list_sh;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_ms_list_shController : Controller
    {
        // GET: r_asmc_ms_list_sh
        public ActionResult Index()
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }

            r_asmc_ms_list_shBL bl = new r_asmc_ms_list_shBL();
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
                                    CityGPName = dr["CityGPName"].ToString()
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

            ViewBag.PrefCD = prefList;
            ViewBag.CityGPCD = prefcitygpcdList;
            ViewBag.CityCD = cityList;
            return View();
        }
    }
}