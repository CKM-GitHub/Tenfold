using Models.RealEstate;
using Models.RealEstate.r_asmc_set_ms;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_asmc_set_ms;
using Seruichi.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_asmc_set_msController : BaseController
    {
        // GET: r_asmc_set_ms
        [HttpGet]
        public ActionResult Index(string mc)
        {
            var model = Session[SessionKey.r_asmc_set_ms_data] as r_asmc_set_ms_Model;
            Session[SessionKey.r_asmc_set_ms_data] = null;

            var previousController = base.GetPreviousController();
            if (previousController != "r_asmc_set_ms" && previousController != "r_asmc_set_ms_check")
            {
                model = null;
            }

            string realECD = base.GetOperator("RealECD");

            if (model == null)
            {
                if (string.IsNullOrEmpty(mc))
                {
                    return RedirectToAction("BadRequest", "Error");
                }

                r_asmc_set_msBL bl = new r_asmc_set_msBL();
                model = bl.Get_M_RECondManAll(realECD, mc);
            }

            CommonBL cmmBL = new CommonBL();
            ViewBag.REStaffDropDownListItems = cmmBL.GetDropDownListItemsOfStaff_by_RealECD(realECD);
            ViewBag.TemplateDropDownListItems = cmmBL.GetDropDownListItemsOfTemplate_for_ManOpt(realECD);
            ViewBag.RECondManOptList = model.RECondManOptList;
            ViewBag.InputMode = true;
            return View(model);
        }





        // Ajax: GetTemplateOptContent
        [HttpPost]
        public ActionResult GetTemplateOptContent(string templateNo)
        {
            if (string.IsNullOrEmpty(templateNo))
            {
                return BadRequestResult();
            }

            r_asmc_set_msBL bl = new r_asmc_set_msBL();
            ViewBag.RECondManOptList = bl.Get_M_TemplateOpt(base.GetOperator("RealECD"), templateNo.ToInt32(0));
            ViewBag.InputMode = true;
            return PartialView("_r_asmc_RECondManOptTable");
        }





        // Ajax: GotoComfirm
        [HttpPost]
        public ActionResult GotoComfirm(r_asmc_set_ms_Model model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.RealECD = base.GetOperator("RealECD");
            model.ExpDate = model.ExpDate.ToStringOrEmpty();

            if (!string.IsNullOrEmpty(model.RECondManOptJson))
            {
                model.RECondManOptList = ConvertJsonToObject<List<RECondManOptTable>>(model.RECondManOptJson);
            }

            r_asmc_set_msBL bl = new r_asmc_set_msBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                CommonBL cmmBL = new CommonBL();
                ViewBag.REStaffDropDownListItems = cmmBL.GetDropDownListItemsOfStaff_by_RealECD(model.RealECD);
                ViewBag.TemplateDropDownListItems = cmmBL.GetDropDownListItemsOfTemplate_for_ManOpt(model.RealECD);
                ViewBag.RECondManOptList = model.RECondManOptList;
                ViewBag.InputMode = true;
                model.ValidationResultJson = ConvertToJson(validationResult);
                return View("Index", model);
            }

            Session[SessionKey.r_asmc_set_ms_referrer] = base.GetPreviousUrl();
            Session[SessionKey.r_asmc_set_ms_data] = model;
            return RedirectToAction("Index", "r_asmc_set_ms_check");
        }
    }
}