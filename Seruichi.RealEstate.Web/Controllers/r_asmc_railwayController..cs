using Models.RealEstate.r_asmc_address;
using Seruichi.BL.RealEstate.r_asmc_address;
using Seruichi.BL.RealEstate.r_asmc_railway;
using Seruichi.Common;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_railwayController : BaseController
    {
        // GET: r_asmc_railway
        [HttpGet]
        public ActionResult Index(string rc)
        {
            if (string.IsNullOrEmpty(rc))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            ViewBag.PageID = "r_asmc_railway";
            ViewBag.CurrentRegionCD = rc;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Render_tab_menu(string rc)
        {
            r_asmc_addressBL bl = new r_asmc_addressBL();
            ViewBag.Regions = bl.GetRegions();
            ViewBag.CurrentRegionCD = rc;
            ViewBag.PageID = "r_asmc_railway";
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
                r_asmc_railwayBL bl = new r_asmc_railwayBL();
                model = bl.GetLinesByRegionCD(rc, base.GetOperator("RealECD"));
            }

            ViewBag.PageID = pid;
            return PartialView("~/Views/r_asmc_address/_r_asmc_address_searchbox_main.cshtml", model);
        }

        [HttpPost]
        public ActionResult Render_searchbox_detail(string linecdCsv, int settings1, int settings2)
        {
            r_asmc_addressDetailModel model;
            if (string.IsNullOrEmpty(linecdCsv))
            {
                model = new r_asmc_addressDetailModel();
            }
            else
            {
                r_asmc_railwayBL bl = new r_asmc_railwayBL();
                model = bl.GetStationsByLineCD(linecdCsv, base.GetOperator("RealECD"));
            }

            model.Settings1 = settings1;
            model.Settings2 = settings2;
            return PartialView("~/Views/r_asmc_address/_r_asmc_address_searchbox_detail.cshtml", model);
        }

        [HttpPost]
        public ActionResult GotoNextPage()
        {
            string selected_lines_csv = Request.Form["selected_lines"].ToStringOrEmpty();
            string selected_stations_csv = Request.Form["selected_stations"].ToStringOrEmpty();

            if (string.IsNullOrEmpty(selected_lines_csv) && string.IsNullOrEmpty(selected_stations_csv))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            TempData["selected_lines"] = selected_lines_csv;
            TempData["selected_stations"] = selected_stations_csv;
            return RedirectToAction("Index", "r_asmc_set_train");
        }

    }
}