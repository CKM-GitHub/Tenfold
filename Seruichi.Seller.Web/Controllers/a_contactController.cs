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

            return View();
        }

        // Ajax: RegisterContact
        [HttpPost]
        public ActionResult RegisterContact(a_contactModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            a_contactBL bl = new a_contactBL();

            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            //if (!bl.InsertCertificationData(mailAddress, out string certificationCD, out DateTime effectiveDateTime))
            //{
            //    return ErrorResult();
            //}

            SendMail.SendSmtp(bl.GetContactTenfoldMailInfo(model));
            SendMail.SendSmtp(bl.GetContactPersonMailInfo(model));

            return OKResult();
        }

    }
}