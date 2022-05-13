using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_mansion_list;
using Models.Tenfold.t_mansion_list;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_mansion_listController : BaseController
    {
        // GET: t_mansion_list
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetM_MansionList(t_mansion_listModel model)
        {
            t_mansion_listBL bl = new t_mansion_listBL();

            List<string> chk_lst = new List<string>();
            
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.GetM_MansionList(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}