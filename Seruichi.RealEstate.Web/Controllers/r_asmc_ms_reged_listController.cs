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

        [HttpPost]
        public ActionResult Get_Rating(RatingModel model)
        {
            //For Rating
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_asmc_ms_reged_listBL bl = new r_asmc_ms_reged_listBL();
            //List<RatingModel> RatingList = new List<RatingModel>();
            DataTable dtRating = bl.Get_Rating(user.RealECD);
            //if(dtRating.Rows.Count>0)
            //{
            //    RatingList = (from DataRow dr in dtRating.Rows
            //                    select new RatingModel()
            //{
            //Rating = dr["Rating"].ToString()
            //                    }).ToList();
            //ViewBag.Rating = RatingList;
            //}
            //else
            //{
            //    ViewBag.Rating = "0";
            //}
            return OKResult(DataTableToJSON(dtRating));
        }


        [HttpPost]
        public ActionResult Get_DataList(r_asmc_ms_reged_listModel model)
        {
            DataTable dt = new DataTable();
            return OKResult(DataTableToJSON(dt));
        }
        }
}