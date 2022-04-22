using Models;
using Seruichi.BL;
using System.Web.Mvc;
using System.Linq;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_uinfoController : BaseController
    {
        // GET: a_mypage_uinfo
        [AllowAnonymous]
        public ActionResult Index()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "a_login");
            }

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            a_mypage_uinfoModel model = bl.GetSellerData(user.UserID);

            CommonBL cmmbl = new CommonBL();
            ViewBag.PrefDropDownListItems = cmmbl.GetDropDownListItemsOfAllPrefecture();
            return View(model);
        }

        // Ajax: CheckZipCode
        [HttpPost]
        public ActionResult CheckZipCode(string zipCode1, string zipCode2)
        {
            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            if (!bl.CheckZipCode(zipCode1, zipCode2, out string errorcd, out string prefCD, out string cityName, out string townName))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult(new { prefCD, cityName, townName });
        }

        // Ajax: CheckAll
        [HttpPost]
        public ActionResult CheckAll(a_mypage_uinfoModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }

        // Ajax: InsertSellerData
        [HttpPost]
        public ActionResult UpdateSellerData(a_mypage_uinfoModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.SellerCD = base.GetOperator();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.UpdateSellerData(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
    }
}