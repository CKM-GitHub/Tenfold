using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Seruichi.Common;
using System.Data;
using Models.Tenfold.t_seller_list;
using System.Threading.Tasks;
using System.Linq;

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
               // new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
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

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            Parallel.ForEach(e, item =>
            {
                item["売主名"] = crypt.DecryptFromBase64(item.Field<string>("売主名"), decryptionKey);
            });
            if (!string.IsNullOrEmpty(model.SellerName))
            {
                var query = e.Where(dr => dr.Field<string>("売主名").Contains(model.SellerName) || dr.Field<string>("売主CD").Contains(model.SellerName));
                if (query.Any())
                {
                    int i = 0;
                    foreach (var row in query)
                    {
                        i++;
                        row["NO"] = i;
                    }
                    return query.CopyToDataTable();
                }
                else
                {
                    DataTable newTable = dt.Clone();
                    return newTable;
                }
            }
            return dt;
        }
        public DataTable Generate_CSV(t_seller_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                  new SqlParameter("@ValidCheck", SqlDbType.TinyInt){ Value = model.ValidCheck.ToByte(0) },
                new SqlParameter("@InValidCheck", SqlDbType.TinyInt){ Value = model.InValidCheck.ToByte(0) },
               // new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
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

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sellerName = dt.Rows[i]["売主名"].ToString();
                dt.Rows[i]["売主名"] = !string.IsNullOrEmpty(sellerName) ? crypt.DecryptFromBase64(sellerName, decryptionKey) : sellerName;

                string SellerKana = dt.Rows[i]["カナ名"].ToString();
                dt.Rows[i]["カナ名"] = !string.IsNullOrEmpty(SellerKana) ? crypt.DecryptFromBase64(SellerKana, decryptionKey) : SellerKana;

                string MailAddress = dt.Rows[i]["メールアドレス"].ToString();
                dt.Rows[i]["メールアドレス"] = !string.IsNullOrEmpty(MailAddress) ? crypt.DecryptFromBase64(MailAddress, decryptionKey) : MailAddress;

                string TownName = dt.Rows[i]["町域名"].ToString();
                dt.Rows[i]["町域名"] = !string.IsNullOrEmpty(TownName) ? crypt.DecryptFromBase64(TownName, decryptionKey) : TownName;

                string Address1 = dt.Rows[i]["番地"].ToString();
                dt.Rows[i]["番地"] = !string.IsNullOrEmpty(Address1) ? crypt.DecryptFromBase64(Address1, decryptionKey) : Address1;

                string Address2 = dt.Rows[i]["建物名･部屋番号"].ToString();
                dt.Rows[i]["建物名･部屋番号"] = !string.IsNullOrEmpty(Address2) ? crypt.DecryptFromBase64(Address2, decryptionKey) : Address2;

                string HandyPhone = dt.Rows[i]["携帯電話番号"].ToString();
                dt.Rows[i]["携帯電話番号"] = !string.IsNullOrEmpty(HandyPhone) ? crypt.DecryptFromBase64(HandyPhone, decryptionKey) : HandyPhone;

                string HousePhone = dt.Rows[i]["固定電話番号"].ToString();
                dt.Rows[i]["固定電話番号"] = !string.IsNullOrEmpty(HousePhone) ? crypt.DecryptFromBase64(HousePhone, decryptionKey) : HousePhone;

                string Fax = dt.Rows[i]["FAX番号"].ToString();
                dt.Rows[i]["FAX番号"] = !string.IsNullOrEmpty(Fax) ? crypt.DecryptFromBase64(Fax, decryptionKey) : Fax;

                string bd = dt.Rows[i]["生年月日"].ToString();
                dt.Rows[i]["生年月日"] = !string.IsNullOrEmpty(bd) ? crypt.DecryptFromBase64(bd, decryptionKey) : bd;

            }

            if (!string.IsNullOrEmpty(model.SellerName))
            {
                var dtLinq = dt.AsEnumerable().Where(dr => dr.Field<string>("売主名").Contains(model.SellerName) || dr.Field<string>("売主CD").Contains(model.SellerName)).CopyToDataTable();
                for (int i = 0; i < dtLinq.Rows.Count; i++)
                {
                    dtLinq.Rows[i]["NO"] = i + 1;
                }
                return dtLinq;
            }
            return dt;
        }
        public Dictionary<string, string> ValidateAll(t_seller_listModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckMaxLenght("SellerName", model.SellerName, 10);//E105

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
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0) },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
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
