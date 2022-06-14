using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_reale_purchase;
using Models.Tenfold.t_reale_asmhis;
using Seruichi.BL.Tenfold.t_reale_purchase;
using Seruichi.BL.Tenfold.t_reale_asmhis;
using System.Data;
namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_asmhisController : BaseController
    {
        // GET: t_reale_asmhis
        public ActionResult Index(string RealECD)
        {
            if (String.IsNullOrWhiteSpace(RealECD))
            {
                return BadRequestResult();
            }
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
        [HttpPost]

        public ActionResult get_t_reale_asmhis_DisplayData(t_reale_asmhisModel model)
        {
            t_reale_asmhisBL bl = new t_reale_asmhisBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.Chk_Area.ToString());
            chk_lst.Add(model.Chk_Mansion.ToString());
            chk_lst.Add(model.Chk_SendCustomer.ToString());
            chk_lst.Add(model.Chk_Top5.ToString());
            chk_lst.Add(model.Chk_Top5Out.ToString());
            chk_lst.Add(model.Chk_NonMemberSeller.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.get_t_reale_asmhis_DisplayData(model);
            if (model.IsCSV)
            {
                dt.Columns["AssessType"].ColumnName = "種類";
                dt.Columns["SendCustomer"].ColumnName = "送客";
                dt.Columns["AssessReqID"].ColumnName = "査定依頼ID";
                dt.Columns["SellerMansionID"].ColumnName = "物件CD";
                dt.Columns["MansionName"].ColumnName = "物件名";
                dt.Columns["RoomNo"].ColumnName = "部屋";
                dt.Columns["SellerCD"].ColumnName = "売主CD";
                dt.Columns["SellerName"].ColumnName = "売主名";

                dt.Columns["InsertDate"].ColumnName = "登録日時";
                dt.Columns["EasyDate"].ColumnName = "簡易査定日時";
                dt.Columns["DeepDate"].ColumnName = "詳細査定日時";
                dt.Columns["PurchaseDate"].ColumnName = "買取依頼日時";
                dt.Columns["AssessAmount"].ColumnName = "買取依頼金額";
                dt.Columns["IntroDate"].ColumnName = "送客日時";
                dt.Columns["Rank"].ColumnName = "ランキング";

                dt.Columns.Remove("RoomNumber");

                SetColumnsOrder(dt);

                dt.AcceptChanges();

            }
            return OKResult(DataTableToJSON(dt));
        }
        public static void SetColumnsOrder( DataTable table)
        {
            String[] columnNames = new string[] {"NO", "種類", "送客", "査定依頼ID", "物件CD", "物件名", "部屋", "売主CD", "売主名", "登録日時", "簡易査定日時", "詳細査定日時", "買取依頼日時", "買取依頼金額", "送客日時", "ランキング" };
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }
    }
}