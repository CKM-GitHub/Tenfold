using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_asmc_ms_list_sh;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_asmc_ms_list_sh;
namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_ms_list_shController : Controller
    {
        // GET: r_asmc_ms_list_sh
        public ActionResult Index()
        {
            return View();
        }
    }
}