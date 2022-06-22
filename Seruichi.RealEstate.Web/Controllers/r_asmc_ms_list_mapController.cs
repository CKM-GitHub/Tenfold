using Models.RealEstate;
using Models.RealEstate.r_asmc_ms_list_map;
using Seruichi.BL.RealEstate.r_asmc_ms_list_map;
using Seruichi.Common;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_ms_list_mapController : BaseController
    {
        // GET: r_asmc_ms_list_map
        public ActionResult Index()
        {
            string selected_cities = TempData["selected_cities"].ToStringOrEmpty();
            string selected_towns = TempData["selected_towns"].ToStringOrEmpty();

            if (string.IsNullOrEmpty(selected_cities) && string.IsNullOrEmpty(selected_towns))
            {
                return RedirectToAction("Index", "r_asmc");
            }

            r_asmc_ms_list_mapBL bl = new r_asmc_ms_list_mapBL();
            ViewBag.TreeItem = bl.Get_Pref_CityGP_City_Town(selected_cities, selected_towns);
            ViewBag.CityCDList = selected_cities;
            ViewBag.TownCDList = selected_towns;
            return View();
        }

        [HttpPost]
        public ActionResult GetMansionData(r_asmc_ms_list_mapModel model)
        {
            r_asmc_ms_list_mapBL bl = new r_asmc_ms_list_mapBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            model.RealECD = base.GetOperator("RealECD");
            var dt = bl.GetMansionData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GotoNextPage()
        {
            string mansionCD = Request.Form["MansionCD"].ToStringOrEmpty();
            string mansionName = Request.Form["MansionName"].ToStringOrEmpty();

            if (string.IsNullOrEmpty(mansionCD) && string.IsNullOrEmpty(mansionName))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            RealEstate_L_Log_Model model = new RealEstate_L_Log_Model();
            model.LoginKBN = 2;
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();
            model.PageID = "r_asmc_ms_list_map";
            model.ProcessKBN = "link";
            model.Remarks = "r_asmc_set_ms" + " " + mansionCD + " " + mansionName;

            r_asmc_ms_list_mapBL bl = new r_asmc_ms_list_mapBL();
            bl.Insert_L_Log(model);

            return RedirectToAction("Index", "r_asmc_set_ms", new { mc = mansionCD });
        }
    }
}