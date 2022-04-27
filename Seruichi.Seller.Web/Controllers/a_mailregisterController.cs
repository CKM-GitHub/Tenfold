using Models;
using Seruichi.BL;
using Seruichi.Common;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    [AllowAnonymous]
    public class a_mailregisterController : BaseController
    {
        // GET: a_mailregister
        [HttpGet]
        public ActionResult Index(string mail, string setupid)
        {
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(setupid))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            if (SessionAuthenticationHelper.GetUserFromSession() == null)
            {
                Session.Clear();
                SessionAuthenticationHelper.CreateAnonymousUser();
            }

            a_mailregisterBL bl = new a_mailregisterBL();
            if (!bl.CheckAndUpdateCertification(mail, setupid, out string errorcd, out string sellerCD))
            {
                return RedirectToAction("BusinessError", "Error", new { id = errorcd });
            }

            a_mailregisterModel model = new a_mailregisterModel()
            {
                MailAddress = bl.GetMailAddress(sellerCD),
                NewMailAddress = mail,
                SellerCD = sellerCD
            };

            return View(model);
        }

        // POST: 
        [HttpPost]
        public ActionResult GotoNextPage()
        {
            return RedirectToAction("Index", "a_login");
        }

        // Ajax: UpdateSellerData
        [HttpPost]
        public ActionResult UpdateMailAddress(a_mailregisterModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();

            a_mailregisterBL bl = new a_mailregisterBL();
            model.SellerName = bl.GetSellerName(model.SellerCD);

            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            if (!bl.UpdateMailAddress(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
    }
}
