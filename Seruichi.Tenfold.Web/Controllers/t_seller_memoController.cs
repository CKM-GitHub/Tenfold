using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_seller_memo;
using Seruichi.BL.Tenfold.t_seller_memo;


namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_memoController : Controller
    {
        // GET: t_seller_memo
        public ActionResult Index()
        {
            return View();
        }
    }
}