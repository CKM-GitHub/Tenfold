using Models.RealEstate.r_contact;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_contact;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_contactController : BaseController
    {
        // GET: r_contact
        public ActionResult Index()
        {
            CommonBL bl = new CommonBL();
            ViewBag.ContactTypeDropDownListItems = bl.GetDropDownListItemsOfContactType();
            return View();
        }

        // Ajax: RegisterContact
        [HttpPost]
        public ActionResult RegisterContact(r_contactModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            model.ContactTime = Utilities.GetSysDateTime();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.REStaffName = base.GetOperatorName();

            r_contactBL bl = new r_contactBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            //if (!bl.InsertContactData(model, out string errorcd))
            //{
            //    return ErrorMessageResult(errorcd);
            //}

            SendMail.SendSmtp(bl.GetContactTenfoldMailInfo(model));
            SendMail.SendSmtp(bl.GetContactPersonMailInfo(model));

            return OKResult();
        }
    }
}