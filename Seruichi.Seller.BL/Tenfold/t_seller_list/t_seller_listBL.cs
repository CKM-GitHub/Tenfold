using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Seruichi.Common;
using System.Data;
using Models.Tenfold.t_seller_list;


namespace Seruichi.BL.Tenfold.t_seller_list
{
    public class t_seller_listBL
    {
        public List<DropDownListItem> GetDropDownListForPref()
        {
            string spName = "pr_t_seller_list_Select_DropDownlistofPref";
            return GetDropDownListItems(spName);
        }

        private List<DropDownListItem> GetDropDownListItems(string spName, params SqlParameter[] sqlParams)
        {
            var items = new List<DropDownListItem>();

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable(spName, sqlParams);

            foreach (DataRow dr in dt.Rows)
            {
                var option = new DropDownListItem()
                {
                    Value = dr["Value"].ToStringOrEmpty(),
                    DisplayText = dr["DisplayText"].ToStringOrEmpty()
                };
                items.Add(option);
            }

            return items;
        }

        public DataTable GetM_SellerList(t_seller_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@ValidCheck", SqlDbType.TinyInt){ Value = model.ValidCheck.ToByte(0) },
                new SqlParameter("@InValidCheck", SqlDbType.TinyInt){ Value = model.InValidCheck.ToByte(0) },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
                new SqlParameter("@PrefNameSelect", SqlDbType.VarChar){ Value = model.PrefNameSelect.ToString() },
                new SqlParameter("@RangeSelect", SqlDbType.TinyInt){ Value = model.RangeSelect.ToByte(0) },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value =  model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull()},
                new SqlParameter("@expectedCheck", SqlDbType.TinyInt){ Value = model.expectedCheck.ToByte(0) },
                new SqlParameter("@negtiatioinsCheck", SqlDbType.TinyInt){ Value = model.negtiatioinsCheck.ToByte(0) },
                new SqlParameter("@endCheck", SqlDbType.TinyInt){ Value = model.endCheck.ToByte(0) }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_List_Select_M_SellerData", sqlParams);
            return dt;
        }
        public DataTable Generate_CSV(t_seller_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                  new SqlParameter("@ValidCheck", SqlDbType.TinyInt){ Value = model.ValidCheck.ToByte(0) },
                new SqlParameter("@InValidCheck", SqlDbType.TinyInt){ Value = model.InValidCheck.ToByte(0) },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
                new SqlParameter("@PrefNameSelect", SqlDbType.VarChar){ Value = model.PrefNameSelect.ToString() },
                new SqlParameter("@RangeSelect", SqlDbType.TinyInt){ Value = model.RangeSelect.ToByte(0) },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value =  model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull()},
                new SqlParameter("@expectedCheck", SqlDbType.TinyInt){ Value = model.expectedCheck.ToByte(0) },
                new SqlParameter("@negtiatioinsCheck", SqlDbType.TinyInt){ Value = model.negtiatioinsCheck.ToByte(0) },
                new SqlParameter("@endCheck", SqlDbType.TinyInt){ Value = model.endCheck.ToByte(0) }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_List_csv_generate", sqlParams);
            return dt;
        }
        public Dictionary<string, string> ValidateAll(t_seller_listModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckMaxLenghtForHalfWidthandFullwidth("SellerName", model.SellerName, 10);//E105

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("CheckBoxError", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public void InsertM_Seller_L_Log(t_seller_l_log_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LogDateTime", SqlDbType.VarChar){ Value = model.LogDateTime },
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.Operator },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@ProcessKBN", SqlDbType.TinyInt){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_L_Log_Insert", false, sqlParams);
        }

    }
}
