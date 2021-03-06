using Seruichi.BL.RealEstate.r_asmc_set_area_check_tab;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_set_area_check_tabController : BaseController
    {
        // GET: r_asmc_set_area_check_tab
        [HttpGet]
        public ActionResult Index(string cc, string tc)
        {
            if (string.IsNullOrEmpty(cc) && string.IsNullOrEmpty(tc))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            r_asmc_set_area_check_tabBL bl = new r_asmc_set_area_check_tabBL();
            var model = bl.GetM_RECondAreaSec(base.GetOperator("RealECD"), cc, tc);
            ViewBag.RECondAreaRate = bl.GetM_RECondAreaRate(model);
            ViewBag.RECondAreaRent = bl.GetM_RECondAreaRent(model);
            ViewBag.RECondAreaOptList = bl.GetM_RECondAreaOpt(model);
            ViewBag.InputMode = false;
            return View(model);
        }
    }
}