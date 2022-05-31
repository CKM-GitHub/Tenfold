using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.Tenfold.t_reale_purchase;

namespace Seruichi.BL.Tenfold.t_reale_purchase
{
    public class t_reale_purchaseBL
    {
        public Dictionary<string, string> ValidateAll(t_reale_purchaseModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("chk_Purchase", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public DataTable get_t_reale_CompanyInfo(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyInfo", sqlParams);

            return dt;
        }

        public DataTable get_t_reale_CompanyCountingInfo(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyCountingInfo", sqlParams);

            return dt;
        }

        public DataTable get_t_reale_purchase_DisplayData(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@chk_Purchase", SqlDbType.TinyInt){ Value = model.chk_Purchase.ToByte(0) },
                new SqlParameter("@chk_Checking", SqlDbType.TinyInt){ Value = model.chk_Checking.ToByte(0) },
                new SqlParameter("@chk_Nego", SqlDbType.TinyInt){ Value = model.chk_Nego.ToByte(0) },
                new SqlParameter("@chk_Contract", SqlDbType.TinyInt){ Value = model.chk_Contract.ToByte(0) },
                new SqlParameter("@chk_SellerDeclined", SqlDbType.TinyInt){ Value = model.chk_SellerDeclined.ToByte(0) },
                new SqlParameter("@chk_BuyerDeclined", SqlDbType.TinyInt){ Value = model.chk_BuyerDeclined.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_purchase_get_DisplayData", sqlParams);

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            return dt;
        }

        public DataTable get_t_reale_purchase_CSVData(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@chk_Purchase", SqlDbType.TinyInt){ Value = model.chk_Purchase.ToByte(0) },
                new SqlParameter("@chk_Checking", SqlDbType.TinyInt){ Value = model.chk_Checking.ToByte(0) },
                new SqlParameter("@chk_Nego", SqlDbType.TinyInt){ Value = model.chk_Nego.ToByte(0) },
                new SqlParameter("@chk_Contract", SqlDbType.TinyInt){ Value = model.chk_Contract.ToByte(0) },
                new SqlParameter("@chk_SellerDeclined", SqlDbType.TinyInt){ Value = model.chk_SellerDeclined.ToByte(0) },
                new SqlParameter("@chk_BuyerDeclined", SqlDbType.TinyInt){ Value = model.chk_BuyerDeclined.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_purchase_get_CSVData", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            return dt;
        }

        public void Insert_L_Log(t_reale_purchase_l_log_Model model)
        {

            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0)},
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.Page },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.Processing },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_mansion_list_Insert_L_Log", false, sqlParams);
        }

        public DataTable Get_Pills_Home(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_mansion_popup_get_pills_home", sqlParams);
            return dt;
        }
        
        public DataTable Get_Pills_Profile(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_mansion_popup_get_pills_profile", sqlParams);
            return dt;
        }
        
        public DataTable Get_Pills_Contact(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_mansion_popup_get_pills_contact", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sellerName = dt.Rows[i]["SellerName"].ToString();
                dt.Rows[i]["SellerName"] = !string.IsNullOrEmpty(sellerName) ? crypt.DecryptFromBase64(sellerName, decryptionKey) : sellerName;
                string SellerKana = dt.Rows[i]["SellerKana"].ToString();
                dt.Rows[i]["SellerKana"] = !string.IsNullOrEmpty(SellerKana) ? crypt.DecryptFromBase64(SellerKana, decryptionKey) : SellerKana;
                string TownName = dt.Rows[i]["TownName"].ToString();
                dt.Rows[i]["TownName"] = !string.IsNullOrEmpty(TownName) ? crypt.DecryptFromBase64(TownName, decryptionKey) : TownName;
                string Address1 = dt.Rows[i]["Address1"].ToString();
                dt.Rows[i]["Address1"] = !string.IsNullOrEmpty(Address1) ? crypt.DecryptFromBase64(Address1, decryptionKey) : Address1;
                string Address2 = dt.Rows[i]["Address2"].ToString();
                dt.Rows[i]["Address2"] = !string.IsNullOrEmpty(Address2) ? crypt.DecryptFromBase64(Address2, decryptionKey) : Address2;
                dt.Rows[i]["Address"] = dt.Rows[i]["PrefName"].ToString() + dt.Rows[i]["CityName"].ToString() + dt.Rows[i]["TownName"] + dt.Rows[i]["Address1"] + dt.Rows[i]["Address2"];
                string Phone = dt.Rows[i]["Phone"].ToString();
                dt.Rows[i]["Phone"] = !string.IsNullOrEmpty(Phone) ? crypt.DecryptFromBase64(Phone, decryptionKey) : Phone;
                string Mobile_Phone = dt.Rows[i]["Mobile_Phone"].ToString();
                dt.Rows[i]["Mobile_Phone"] = !string.IsNullOrEmpty(Mobile_Phone) ? crypt.DecryptFromBase64(Mobile_Phone, decryptionKey) : Mobile_Phone;
                string MailAddress = dt.Rows[i]["MailAddress"].ToString();
                dt.Rows[i]["MailAddress"] = !string.IsNullOrEmpty(MailAddress) ? crypt.DecryptFromBase64(MailAddress, decryptionKey) : MailAddress;
            }
            return dt;
        }
    }
}
