using Models.RealEstate.r_asmc_address;
using Seruichi.BL.RealEstate.r_asmc_address;
using Seruichi.BL.RealEstate.r_asmc_ms_map_add;
using Seruichi.Common;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_ms_map_addController : BaseController
    {
        // GET: r_asmc_ms_map_add
        [HttpGet]
        public ActionResult Index(string rc)
        {
            if (string.IsNullOrEmpty(rc))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            ViewBag.PageID = "r_asmc_ms_map_add";
            ViewBag.CurrentRegionCD = rc;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Render_tab_menu(string rc)
        {
            r_asmc_addressBL bl = new r_asmc_addressBL();
            ViewBag.Regions = bl.GetRegions();
            ViewBag.CurrentRegionCD = rc;
            ViewBag.PageID = "r_asmc_ms_map_add";
            return PartialView("~/Views/r_asmc_address/_r_asmc_address_tab_menu.cshtml");
        }

        [ChildActionOnly]
        public ActionResult Render_searchbox_main(string rc, string pid)
        {
            r_asmc_addressModel model;
            if (string.IsNullOrEmpty(rc))
            {
                model = new r_asmc_addressModel();
            }
            else
            {
                r_asmc_ms_map_addBL bl = new r_asmc_ms_map_addBL();
                model = bl.GetCitiesByRegionCD(rc, base.GetOperator("RealECD"));
            }

            ViewBag.PageID = pid;
            return PartialView("~/Views/r_asmc_address/_r_asmc_address_searchbox_main.cshtml", model);
        }

        [HttpPost]
        public ActionResult Render_searchbox_detail(string citycdCsv, int settings1, int settings2)
        {
            r_asmc_addressDetailModel model;
            if (string.IsNullOrEmpty(citycdCsv))
            {
                model = new r_asmc_addressDetailModel();
            }
            else
            {
                r_asmc_ms_map_addBL bl = new r_asmc_ms_map_addBL();
                model = bl.GetTownsByCityCD(citycdCsv, base.GetOperator("RealECD"));
            }

            model.Settings1 = settings1;
            model.Settings2 = settings2;
            model.DisabledOpenCheckTab = true; //「最新の登録条件確認」は表示しない
            return PartialView("~/Views/r_asmc_address/_r_asmc_address_searchbox_detail.cshtml", model);
        }

        [HttpPost]
        public ActionResult GotoNextPage()
        {
            string selected_cities_csv = Request.Form["selected_cities"].ToStringOrEmpty();
            string selected_towns_csv = Request.Form["selected_towns"].ToStringOrEmpty();

            if (string.IsNullOrEmpty(selected_cities_csv) && string.IsNullOrEmpty(selected_towns_csv))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            TempData["selected_cities"] = selected_cities_csv;
            TempData["selected_towns"] = selected_towns_csv;
            return RedirectToAction("Index", "r_asmc_ms_list_map");
        }

    }
}