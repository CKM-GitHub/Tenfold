using Models.RealEstate.r_asmc_address;
using Seruichi.BL.RealEstate.r_asmc_address;
using Seruichi.Common;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_addressController : BaseController
    {
        // GET: r_asmc_address
        [HttpGet]
        public ActionResult Index(string rc)
        {
            if (string.IsNullOrEmpty(rc))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            ViewBag.CurrentRegionCD = rc;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Render_tab_menu(string rc)
        {
            r_asmc_addressBL bl = new r_asmc_addressBL();
            ViewBag.Regions = bl.GetRegions();
            ViewBag.CurrentRegionCD = rc;
            return PartialView("_r_asmc_address_tab_menu");
        }

        [ChildActionOnly]
        public ActionResult Render_searchbox_main(string rc)
        {
            r_asmc_addressModel model;
            if (string.IsNullOrEmpty(rc))
            {
                model = new r_asmc_addressModel();
            }
            else
            {
                r_asmc_addressBL bl = new r_asmc_addressBL();
                model = bl.GetCitiesByRegionCD(rc, base.GetOperator("RealECD"));
            }
            return PartialView("_r_asmc_address_searchbox_main", model);
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
                r_asmc_addressBL bl = new r_asmc_addressBL();
                model = bl.GetTownsByCitycdCsv(citycdCsv, base.GetOperator("RealECD"));
            }

            model.Settings1 = settings1;
            model.Settings2 = settings2;
            return PartialView("_r_asmc_address_searchbox_detail", model);
        }

        [HttpPost]
        public ActionResult GotoNextPage()
        {
            string selectedListCsv_cities = Request.Form["selectedList_Cities"].ToStringOrEmpty();
            string selectedListCsv_towns = Request.Form["selectedList_Towns"].ToStringOrEmpty();

            if (string.IsNullOrEmpty(selectedListCsv_cities) && string.IsNullOrEmpty(selectedListCsv_towns))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            TempData["selectedList_Cities"] = selectedListCsv_cities;
            TempData["selectedList_Towns"] = selectedListCsv_towns;
            return RedirectToAction("Index", "r_asmc_set_area");
        }

    }
}