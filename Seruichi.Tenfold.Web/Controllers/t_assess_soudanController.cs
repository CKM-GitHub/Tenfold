using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.Tenfold.t_assess_soudan;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_assess_soudan;
using Seruichi.Common;
using System.Threading.Tasks;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_assess_soudanController : BaseController
    {
        // GET: t_assess_soudan
        public ActionResult Index()
        {
            CommonBL bl = new CommonBL();
            ViewBag.ContactTypeDropDownListItems = bl.GetDropDownListItemsOfTenfoldStaff();
            return View();
        }

        [HttpPost]
        public ActionResult get_t_assess_soudan_DisplayData(t_assess_soudanModel model)
        {
            t_assess_soudanBL bl = new t_assess_soudanBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.untreated.ToString());
            chk_lst.Add(model.processing.ToString());
            chk_lst.Add(model.solution.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            var dt = bl.get_t_assess_soudan_DisplayData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_assess_soudan_CSVData(t_assess_soudanModel model)
        {
            t_assess_soudanBL bl = new t_assess_soudanBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.untreated.ToString());
            chk_lst.Add(model.processing.ToString());
            chk_lst.Add(model.solution.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            var dt = bl.get_t_assess_soudan_CSVData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_Modal_infotrainData(t_assess_soudanModel model)
        {
            t_assess_soudanBL bl = new t_assess_soudanBL();
            var dt = bl.get_Modal_infotrainData(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}