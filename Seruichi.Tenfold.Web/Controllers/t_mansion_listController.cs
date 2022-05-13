using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_mansion_list;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_mansion_list;
using Models.Tenfold.t_mansion_list;
using Seruichi.BL.Tenfold.t_mansion_list;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_mansion_listController : BaseController
    {
        // GET: t_mansion_list
        public ActionResult Index()
        {
            t_mansion_listBL bl = new t_mansion_listBL();
            List<M_Pref> prefList = new List<M_Pref>();
            DataTable dt = bl.GetM_Pref();
            prefList = (from DataRow dr in dt.Rows
                           select new M_Pref()
                           {
                               PrefCD = dr["PrefCD"].ToString(),
                               PrefName = dr["PrefName"].ToString()
                           }).ToList();
            List<M_Pref_And_CityGPCD> prefcitygpcdList = new List<M_Pref_And_CityGPCD>();
            DataTable dt1= bl.Get_Prefcd_and_CityGPCD();
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
        public ActionResult GetM_MansionList(t_mansion_listModel model)
        {
            t_mansion_listBL bl = new t_mansion_listBL();

            List<string> chk_lst = new List<string>();
            
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.GetM_MansionList(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}