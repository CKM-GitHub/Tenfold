using Models.RealEstate;
using Models.RealEstate.r_asmc_set_ms;
using Seruichi.BL.RealEstate.r_asmc_set_ms;
using Seruichi.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_set_ms_checkController : BaseController
    {
        // GET: r_asmc_set_ms_check
        [HttpGet]
        public ActionResult Index(string tp)
        {
            var model = Session[SessionKey.r_asmc_set_ms_data] as r_asmc_set_ms_Model;
            if (model == null)
            {
                return RedirectToAction("BadRequest", "Error");
            }

            ViewBag.AsmcTopPage = tp;
            ViewBag.RECondManOptList = model.RECondManOptList;
            ViewBag.InputMode = false;
            return View(model);
        }




        
        // Ajax: InsertAll
        [HttpPost]
        public ActionResult InsertAll(string validFlg)
        {
            var model = Session[SessionKey.r_asmc_set_ms_data] as r_asmc_set_ms_Model;
            if (model == null)
            {
                return BadRequestResult();
            }

            model.ValidFLG = validFlg.ToByte(0);
            model.RealECD = base.GetOperator("RealECD");
            model.Operator = base.GetOperator("UserID");
            model.IPAddress = base.GetClientIP();

            if (!string.IsNullOrEmpty(model.RECondManOptJson))
            {
                model.RECondManOptList = ConvertJsonToObject<List<RECondManOptTable>>(model.RECondManOptJson);
            }

            r_asmc_set_msBL bl = new r_asmc_set_msBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.InsertAll(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            Session[SessionKey.r_asmc_set_ms_data] = null;
            return OKResult();
        }

        // Ajax: SaveTemplate
        [HttpPost]
        public ActionResult SaveTemplate(string templateName)
        {
            var model = Session[SessionKey.r_asmc_set_ms_data] as r_asmc_set_ms_Model;
            if (model == null)
            {
                return BadRequestResult();
            }

            model.RealECD = base.GetOperator("RealECD");
            model.Operator = base.GetOperator("UserID");
            model.IPAddress = base.GetClientIP();
            model.RECondManOptList = ConvertJsonToObject<List<RECondManOptTable>>(model.RECondManOptJson);

            r_asmc_set_msBL bl = new r_asmc_set_msBL();
            var validationResult = bl.ValidateTemplateName(templateName);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            validationResult = bl.ValidateAll(model, true);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            if (!bl.SaveTemplate(model, templateName, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }





        // Ajax: GotoBack
        [HttpPost]
        public ActionResult GotoBack()
        {
            var model = Session[SessionKey.r_asmc_set_ms_data] as r_asmc_set_ms_Model;
            if (model == null)
            {
                return BadRequestResult();
            }

            return RedirectToAction("Index", "r_asmc_set_ms", new { mc = model.MansionCD });
        }

        // Ajax: GotoAsmcTop
        [HttpPost]
        public ActionResult GotoAsmcTop(string asmcTopPage)
        {
            return RedirectToAction("Index", asmcTopPage);
        }

    }
}