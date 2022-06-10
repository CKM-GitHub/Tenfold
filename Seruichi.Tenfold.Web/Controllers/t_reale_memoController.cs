using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_reale_memo;
using Seruichi.BL.Tenfold.t_reale_memo;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_memoController : BaseController
    {
        // GET: t_reale_memo
        public ActionResult Index(string RealECD)
        {
            ViewBag.LoginID = base.GetOperator();
            return View();
        }

        [HttpPost]
        public ActionResult get_t_reale_CompanyInfo(t_reale_memoModel model)
        {
            t_reale_memoBL bl = new t_reale_memoBL();
            var dt = bl.get_t_reale_CompanyInfo(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_reale_CompanyCountingInfo(t_reale_memoModel model)
        {
            t_reale_memoBL bl = new t_reale_memoBL();
            var dt = bl.get_t_reale_CompanyCountingInfo(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_reale_memo_DisplayData(t_reale_memoModel model)
        {
            t_reale_memoBL bl = new t_reale_memoBL();
            var dt = bl.get_t_reale_memo_DisplayData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Insert_L_Log(t_reale_memoModel model)
        {
            t_reale_memoBL bl = new t_reale_memoBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            bl.Insert_L_Log(model);
            return OKResult();
        }

        [HttpPost]
        public ActionResult Modify_MemoText(t_reale_memoModel model)
        {
            t_reale_memoBL bl = new t_reale_memoBL();
            model.LoginID = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            bl.Modify_MemoText(model);
            return OKResult();
        }

        [HttpPost]
        public ActionResult Delete_MemoText(t_reale_memoModel model)
        {
            t_reale_memoBL bl = new t_reale_memoBL();
            model.LoginID = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            bl.Delete_MemoText(model);
            return OKResult();
        }
    }
}