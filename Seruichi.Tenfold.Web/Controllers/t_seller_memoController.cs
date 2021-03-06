using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_seller_memo;
using Seruichi.BL.Tenfold.t_seller_memo;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_memoController : BaseController
    {
        // GET: t_seller_memo
        public ActionResult Index(string SellerCD)
        {
            if (string.IsNullOrEmpty(SellerCD))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            t_seller_memoBL bl = new t_seller_memoBL();
            string sellername = bl.get_t_sellerName(SellerCD);
            ViewBag.Title = "管理－売主－" + sellername;
            ViewBag.LoginID = base.GetOperator();
            return View();
        }

        [HttpPost]
        public ActionResult get_t_seller_Info(t_seller_memoModel model)
        {
            t_seller_memoBL bl = new t_seller_memoBL();
            var dt = bl.get_t_seller_Info(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_seller_memo_DisplayData(t_seller_memoModel model)
        {
            t_seller_memoBL bl = new t_seller_memoBL();
            var dt = bl.get_t_seller_memo_DisplayData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Insert_L_Log(t_seller_memoModel model)
        {
            t_seller_memoBL bl = new t_seller_memoBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            bl.Insert_L_Log(model);
            return OKResult();
        }

        [HttpPost]
        public ActionResult Modify_MemoText(t_seller_memoModel model)
        {
            t_seller_memoBL bl = new t_seller_memoBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            bl.Modify_MemoText(model);
            return OKResult();
        }

        [HttpPost]
        public ActionResult Delete_MemoText(t_seller_memoModel model)
        {
            t_seller_memoBL bl = new t_seller_memoBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            bl.Delete_MemoText(model);
            return OKResult();
        }
    }
}