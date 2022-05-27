using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Seruichi.Common;
using System.Data;
using Models.Tenfold.t_reale_list;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_reale_list
{
    public class t_reale_listBL
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

        public DataTable getM_RealList(t_reale_listModel model)        {            var sqlParams = new SqlParameter[]             {                                new SqlParameter("@PrefNameSelect", SqlDbType.VarChar){ Value = model.PrefNameSelect.ToString() },                new SqlParameter("@RealEStateCom", SqlDbType.TinyInt){ Value = model.RealEStateCom.ToString() },                new SqlParameter("@EffectiveChk", SqlDbType.TinyInt){ Value = model.EffectiveChk.ToByte(0) },                new SqlParameter("@InValidCheck", SqlDbType.TinyInt){ Value = model.InValidCheck.ToByte(0) }                             };            DBAccess db = new DBAccess();            var dt = db.SelectDatatable("pr_t_reale_List_Select_M_RealEstate", sqlParams);            return dt;        }
        public async Task<DataTable> Generate_CSV(t_reale_listModel model)
        {
            return await Task.Run(() =>
            {
                var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@PrefNameSelect", SqlDbType.VarChar){ Value = model.PrefNameSelect.ToString() },                new SqlParameter("@RealEStateCom", SqlDbType.TinyInt){ Value = model.RealEStateCom.ToString() },                new SqlParameter("@EffectiveChk", SqlDbType.TinyInt){ Value = model.EffectiveChk.ToByte(0) },                new SqlParameter("@InValidCheck", SqlDbType.TinyInt){ Value = model.InValidCheck.ToByte(0) }
             };

                DBAccess db = new DBAccess();
                var dt = db.SelectDatatable("pr_t_reale_List_csv_generate", sqlParams);

                return dt;
            });
        }
        public Dictionary<string, string> ValidateAll(t_reale_listModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckMaxLenght("RealEstateCom", model.RealEStateCom, 50);//E105

            validator.CheckCheckboxLenght("CheckBoxError", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public void InsertM_Seller_L_Log(t_reale_l_log_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LogDateTime", SqlDbType.VarChar){ Value = model.LogDateTime },
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0) },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_mansion_list_Insert_L_Log", false, sqlParams);
        }
    }
}
