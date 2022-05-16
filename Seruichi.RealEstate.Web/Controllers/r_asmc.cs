using Seruichi.BL;
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
            return View();
        }

        [HttpPost]
        public ActionResult redirect_207address()
        {
            string regionName = Request.Form["Region"].ToStringOrEmpty();
            if (string.IsNullOrEmpty(regionName))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            string regionCD = ((int)regionName.ToEnum<Regions>()).ToString().PadLeft(2, '0');
            return RedirectToAction("Index", "r_asmc_address", new { rc = regionCD });
        }

        [HttpPost]
        public ActionResult redirect_208railway()
        {
            string regionName = Request.Form["Region"].ToStringOrEmpty();
            if (string.IsNullOrEmpty(regionName))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            string regionCD = ((int)regionName.ToEnum<Regions>()).ToString().PadLeft(2, '0');
            return RedirectToAction("Index", "r_asmc_railway", new { rc = regionCD });
        }
    }
}