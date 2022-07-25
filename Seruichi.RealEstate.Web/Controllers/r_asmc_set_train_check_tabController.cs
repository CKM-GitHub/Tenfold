using Seruichi.BL.RealEstate.r_asmc_set_train_check_tab;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_set_train_check_tabController : BaseController
    {
        // GET: r_asmc_set_train_check_tab
        [HttpGet]
        public ActionResult Index(string sc)
        {
            if (string.IsNullOrEmpty(sc))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            r_asmc_set_train_check_tabBL bl = new r_asmc_set_train_check_tabBL();
            var model = bl.GetM_RECondLineSta(base.GetOperator("RealECD"), sc);
            ViewBag.RECondLineRate = bl.GetM_RECondLineRate(model);
            ViewBag.RECondLineRent = bl.GetM_RECondLineRent(model);
            ViewBag.RECondLineOptList = bl.GetM_RECondLineOpt(model);
            ViewBag.InputMode = false;
            return View(model);
        }
    }
}