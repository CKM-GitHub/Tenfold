using Seruichi.BL.RealEstate.r_asmc;
using Seruichi.Common;
using System.Web.Mvc;
using Models;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmcController : BaseController
    {
        // GET: r_asmc
        [HttpGet]
        public ActionResult Index()
        {
            r_asmcBL bl = new r_asmcBL();
            ViewBag.MapRegions = bl.GetRegions();
            return View();
        }

        [HttpPost]
        public ActionResult redirect_206railway()
        {
            string regionCD = Request.Form["Region"].ToStringOrEmpty();
            if (string.IsNullOrEmpty(regionCD))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            return RedirectToAction("Index", "r_asmc_railway", new { rc = regionCD, op = "" });
        }

        [HttpPost]
        public ActionResult redirect_207address()
        {
            string regionCD = Request.Form["Region"].ToStringOrEmpty();
            if (string.IsNullOrEmpty(regionCD))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            return RedirectToAction("Index", "r_asmc_address", new { rc = regionCD, op = "" });
        }

        [HttpPost]
        public ActionResult redirect_209ms_map_add()
        {
            string regionCD = Request.Form["Region"].ToStringOrEmpty();
            if (string.IsNullOrEmpty(regionCD))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            return RedirectToAction("Index", "r_asmc_ms_map_add", new { rc = regionCD, op = "" });
        }

        [HttpPost]
        public ActionResult redirect_208ms_list_sh()
        {
            string mansionName = Request.Form["MansionName"].ToStringOrEmpty();
            if (string.IsNullOrEmpty(mansionName))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            return RedirectToAction("Index", "r_asmc_ms_list_sh", new { MansionName = mansionName });
        }

        [HttpPost]
        public ActionResult redirect_210ms_reged_list()
        {
            return RedirectToAction("Index", "r_asmc_ms_reged_list");
        }
    }
}