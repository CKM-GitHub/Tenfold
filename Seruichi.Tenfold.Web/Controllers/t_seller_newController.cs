using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_seller_new;
using Seruichi.BL.Tenfold.t_seller_new;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_newController : BaseController
    {
        // GET: t_seller_new
        public ActionResult Index()
        {
            return View();
        }
    }
}