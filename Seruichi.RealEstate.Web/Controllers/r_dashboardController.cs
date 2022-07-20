using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_dashboard;
using Seruichi.BL.RealEstate.r_dashboard;
using Seruichi.BL.RealEstate.r_asmc;
using System.Data;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_dashboardController : BaseController
    {
        static string mindate = string.Empty;
        static string date = string.Empty;

        // GET: r_dashboard
        public ActionResult Index()
        {
            r_asmcBL bl = new r_asmcBL();
            ViewBag.MapRegions = bl.GetRegions();
            return View();
           
        }

        [HttpPost]
        public ActionResult GetREFaceImg(r_dashboardModel model)
        {
 
            model.RealECD = base.GetOperator("RealECD");
            model.REStaffCD = base.GetOperator("REStaffCD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetREFaceImg(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetREStaffName(r_dashboardModel model)
        {
            model.RealECD = base.GetOperator("RealECD");
            model.REStaffCD = base.GetOperator("REStaffCD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetREStaffName(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetREName(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetREName(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetCustomerInfo(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetCustomerInfo(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetSpecificplan(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetSpecificplan(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetArea(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetArea(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetWay(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetWay(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetApartment(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetApartment(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetOldestDate(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetOldestDate(model);
            mindate = dt.Rows[0]["MinDate"].ToString();
            if (string.IsNullOrWhiteSpace(mindate))
            {
               mindate = DateTime.Now.ToString();
            }
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetOldestDatecount(r_dashboardModel model)
        {
            if (string.IsNullOrWhiteSpace(mindate))
            {
                date  = DateTime.Now.ToString();
            }
            else
            {
                date = mindate;
            }
            model.ConfDateTime= Convert.ToDateTime(date);  
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetOldestDatecount(model);
            return OKResult(DataTableToJSON(dt));

        }

        [HttpPost]
        public ActionResult GetNewRequestData(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetNewRequestData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetNegotiationsData(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetNegotiationsData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetNumberOfCompletedData(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetNumberOfCompletedData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetNumberOfDeclineData(r_dashboardModel model)
        {
            //model.RealECD = strRealECD;
            model.RealECD = base.GetOperator("RealECD");
            r_dashboardBL bl = new r_dashboardBL();
            var dt = bl.GetNumberOfDeclineData(model);
            return OKResult(DataTableToJSON(dt));
        }


    }
}