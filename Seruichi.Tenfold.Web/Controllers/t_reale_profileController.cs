using Models.Tenfold.t_reale_new;
using Models.Tenfold.t_reale_purchase;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_reale_new;
using Seruichi.BL.Tenfold.t_reale_purchase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_profileController : BaseController
    {
        // GET: t_reale_profile
        public ActionResult Index(string RealECD)
        {
            if (string.IsNullOrEmpty(RealECD))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            t_reale_newBL t_bl = new t_reale_newBL();
            t_reale_newModel model = Assign_Model_Value(t_bl.Get_t_Reale_Profile_Data(RealECD),RealECD);
            ViewBag.ProfileModel = model;

            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            ViewBag.MultPurposeDropDownListItems = bl.GetDropDownListItemsOfMultPurpose(110);
            ViewBag.CourseDropDownListItems = bl.GetDropDownListItemsOfCourse();

            ViewBag.Url = System.Web.HttpContext.Current.Request.UrlReferrer;
            return View();
        }
        [HttpPost]
        public ActionResult get_t_reale_CompanyInfo(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_t_reale_CompanyInfo(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_reale_CompanyCountingInfo(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_t_reale_CompanyCountingInfo(model);
            return OKResult(DataTableToJSON(dt));
        }
        public t_reale_newModel Assign_Model_Value(DataTable dt,string realECD)
        {
            t_reale_newModel model = new t_reale_newModel();
            model.RealECD = realECD;
            model.REName = dt.Rows[0]["REName"].ToString();
            model.REKana = dt.Rows[0]["REKana"].ToString();
            model.President = dt.Rows[0]["President"].ToString();
            model.ZipCode1 = dt.Rows[0]["ZipCode1"].ToString();
            model.ZipCode2 = dt.Rows[0]["ZipCode2"].ToString();
            model.PrefCD = dt.Rows[0]["PrefCD"].ToString();
            model.PrefName = dt.Rows[0]["PrefName"].ToString();
            model.CityName = dt.Rows[0]["CityName"].ToString();
            model.TownName = dt.Rows[0]["TownName"].ToString();
            model.Address1 = dt.Rows[0]["Address1"].ToString();
            model.BusinessHours = dt.Rows[0]["BusinessHours"].ToString();
            model.CompanyHoliday = dt.Rows[0]["CompanyHoliday"].ToString();
            model.PICName = dt.Rows[0]["PICName"].ToString();
            model.PICKana = dt.Rows[0]["PICKana"].ToString();
            model.HousePhone = dt.Rows[0]["HousePhone"].ToString();
            model.Fax = dt.Rows[0]["Fax"].ToString();
            model.MailAddress = dt.Rows[0]["MailAddress"].ToString();
            model.SourceBankCD = dt.Rows[0]["SourceBankCD"].ToString();
            model.SourceBankName = dt.Rows[0]["BankName"].ToString();
            model.SourceBranchCD = dt.Rows[0]["SourceBranchCD"].ToString();
            model.SourceBranchName = dt.Rows[0]["BranchName"].ToString();
            model.SourceAccountType = dt.Rows[0]["SourceAccountType"].ToString() == "1" ? 1 : 2;
            model.SourceAccount = dt.Rows[0]["SourceAccount"].ToString();
            model.SourceAccountName = dt.Rows[0]["SourceAccountName"].ToString();
            model.LicenceNo1 = dt.Rows[0]["LicenceNo1"].ToString();
            model.LicenceNo2 = dt.Rows[0]["LicenceNo2"].ToString();
            model.LicenceNo3 = dt.Rows[0]["LicenceNo3"].ToString();
            model.CourseCD = dt.Rows[0]["CourseCD"].ToString();
            model.CourseName = dt.Rows[0]["CourseName"].ToString();
            model.NextCourseCD = dt.Rows[0]["NextCourseCD"].ToString();
            model.NextCourseName = dt.Rows[0]["NextCourseName"].ToString();
            model.JoinedDate = dt.Rows[0]["JoinedDate"].ToString();
            model.InitialFee = dt.Rows[0]["InitialFee"].ToString();
            model.InvoiceDate = dt.Rows[0]["InvoiceDate"].ToString();
            model.Remark = dt.Rows[0]["Remark"].ToString();
            model.Password = dt.Rows[0]["Password"].ToString();
            return model;
        }

        [HttpPost]
        public ActionResult CheckAll(t_reale_newModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            t_reale_newBL bl = new t_reale_newBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            return OKResult();
        }

        [HttpPost]
        public ActionResult Update_t_reale_profile(t_reale_newModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.LoginName = base.GetOperatorName();

            t_reale_newBL bl = new t_reale_newBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.Update_t_reale_profile(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
        

    }
}