using Models;
using Seruichi.BL;
using Seruichi.Common;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    [AllowAnonymous]
    public class a_contactController : BaseController
    {
        // GET: a_contact
        [HttpGet]
        public ActionResult Index()
        {
            if (SessionAuthenticationHelper.GetUserFromSession() == null)
            {
                Session.Clear();
                SessionAuthenticationHelper.CreateAnonymousUser();
            }

            CommonBL bl = new CommonBL();
            ViewBag.ContactTypeDropDownListItems = bl.GetDropDownListItemsOfContactType();
            return View();
        }

        // POST: 
        [HttpPost]
        public ActionResult GotoNextPage()
        {
            return RedirectToAction("Index", "a_index");
        }

        // Ajax: RegisterContact
        [HttpPost]
        public ActionResult RegisterContact(a_contactModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            model.ContactTime = Utilities.GetSysDateTime();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.SellerName = base.GetOperatorName();

            a_contactBL bl = new a_contactBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            if (!bl.InsertContactData(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            SendMail.SendSmtp(bl.GetContactTenfoldMailInfo(model));
            SendMail.SendSmtp(bl.GetContactPersonMailInfo(model));

            return OKResult();
        }

    }
}