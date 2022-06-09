using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_reale_memo;
using Seruichi.BL.Tenfold.t_reale_memo;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_memoController : BaseController
    {
        // GET: t_reale_memo
        public ActionResult Index(string RealECD)
        {
            t_reale_memoBL bl = new t_reale_memoBL();
            //string sellername = bl.get_t_sellerName(RealECD);
            //ViewBag.Title = "管理－売主－" + sellername;
            ViewBag.LoginID = base.GetOperator();
            return View();
        }
    }
}