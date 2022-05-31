using Models.Tenfold.t_mansion;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_mansion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_mansionController : BaseController
    {
        // GET: t_mansion
        public ActionResult Index(string MansionCD)
        {
            t_mansionBL bl = new t_mansionBL();
            var dt = bl.GetM_MansionData(MansionCD);
            t_mansionModel model = Get_MansionModel(dt);
            //CommonBL bl = new CommonBL();
            //ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            return View();
        }

        public t_mansionModel Get_MansionModel(DataTable dt)
        {
            t_mansionModel mm = new t_mansionModel();
            mm.MansionCD = dt.Rows[0]["MansionCD"].ToString();
            mm.MansionName = dt.Rows[0]["MansionName"].ToString();
            mm.PrefCD = dt.Rows[0]["PrefCD"].ToString();
            mm.PrefName = dt.Rows[0]["PrefName"].ToString();
            mm.ZipCode1 = dt.Rows[0]["ZipCode1"].ToString();
            mm.ZipCode2 = dt.Rows[0]["ZipCode2"].ToString();
            mm.CityCD = dt.Rows[0]["CityCD"].ToString();
            mm.CityName = dt.Rows[0]["CityName"].ToString();
            mm.TownCD = dt.Rows[0]["TownCD"].ToString();
            mm.TownName = dt.Rows[0]["TownName"].ToString();
            mm.Address = dt.Rows[0]["Address"].ToString();
            mm.StructuralKBN = dt.Rows[0]
            return mm;
        }
    }
}