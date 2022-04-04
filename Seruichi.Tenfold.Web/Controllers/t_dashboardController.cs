using Models.Tenfold.t_dashboard;
using Seruichi.BL.Tenfold.t_dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_dashboardController : BaseController
    {
        // GET: t_dashboard
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetCustomerInformationWaitingCount(t_dashboardModel model)
        {
            t_dashboardBL bl = new t_dashboardBL();
            var dt = bl.GetCustomerInformationWaitingCount();
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetChatConfirmationWaitingCount(t_dashboardModel model)
        {
            t_dashboardBL bl = new t_dashboardBL();
            var dt = bl.GetChatConfirmationWaitingCount();
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetNewRequestCasesCount(t_dashboardModel model)
        {
            t_dashboardBL bl = new t_dashboardBL();
            var dt = bl.GetNewRequestCasesCount();
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetDuringnegotiationsCasesCount(t_dashboardModel model)
        {
            t_dashboardBL bl = new t_dashboardBL();
            var dt = bl.GetDuringnegotiationsCasesCount();
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetContractCasesCount(t_dashboardModel model)
        {
            t_dashboardBL bl = new t_dashboardBL();
            var dt = bl.GetContractCasesCount();
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetDeclineCasesCount(t_dashboardModel model)
        {
            t_dashboardBL bl = new t_dashboardBL();
            var dt = bl.GetDeclineCasesCount();
            return OKResult(DataTableToJSON(dt));
        }
    }
}