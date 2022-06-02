using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.Tenfold.t_reale_purchase;
using Models.Tenfold.t_reale_asmhis;

namespace Seruichi.BL.Tenfold.t_reale_asmhis
{
    public class t_reale_asmhisBL
    {
        public Dictionary<string, string> ValidateAll(t_reale_asmhisModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111
            List<string> AreaMansion = new List<string>();
            AreaMansion.Add(model.Chk_Area.ToString());
            AreaMansion.Add(model.Chk_Mansion.ToString());
            validator.CheckCheckboxLenght("CheckBoxError", AreaMansion);//E112

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
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_purchase_get_DisplayData", sqlParams);

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            foreach (DataRow row in dt.Rows)
            {
                row["売主名"] = crypt.DecryptFromBase64(row.Field<string>("売主名"), decryptionKey);
            }
            return dt;
        }
        public DataTable get_t_reale_asmhis_DisplayData(t_reale_asmhisModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@Chk_Area", SqlDbType.TinyInt){ Value = model.Chk_Area.ToByte(0) },
                new SqlParameter("@Chk_Mansion", SqlDbType.TinyInt){ Value = model.Chk_Mansion.ToByte(0) },
                new SqlParameter("@Chk_SendCustomer", SqlDbType.TinyInt){ Value = model.Chk_SendCustomer.ToByte(0) },
                new SqlParameter("@Chk_Top5", SqlDbType.TinyInt){ Value = model.Chk_Top5.ToByte(0) },
                new SqlParameter("@Chk_Top5Out", SqlDbType.TinyInt){ Value = model.Chk_Top5Out.ToStringOrNull() },
                new SqlParameter("@Chk_NonMemberSeller", SqlDbType.TinyInt){ Value = model.Chk_NonMemberSeller.ToStringOrNull() }, 
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_asmhis_get_DisplayData", sqlParams);

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            foreach (DataRow row in dt.Rows)
            {
                row["SellerName"] = crypt.DecryptFromBase64(row.Field<string>("SellerName"), decryptionKey);
            }
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
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_purchase_get_CSVData", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            foreach (DataRow row in dt.Rows)
            {
                row["売主名"] = crypt.DecryptFromBase64(row.Field<string>("売主名"), decryptionKey);
            }
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
                new SqlParameter("@Page", SqlDbType.VarChar){ Value = model.Page },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.Processing },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_Tenfold_Insert_L_Log", false, sqlParams);
        }

        public DataTable get_Modal_HomeData(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_tenfold_get_Modal_HomeData", sqlParams);
            return dt;
        }

        public DataTable get_Modal_ProfileData(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_tenfold_get_Modal_ProfileData", sqlParams);
            return dt;
        }

        public DataTable get_Modal_ContactData(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_tenfold_get_Modal_ContactData", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            foreach (DataRow row in dt.Rows)
            {
                row["カナ名"] = crypt.DecryptFromBase64(row.Field<string>("カナ名"), decryptionKey);
                row["漢字名"] = crypt.DecryptFromBase64(row.Field<string>("漢字名"), decryptionKey);
                row["TownName"] = crypt.DecryptFromBase64(row.Field<string>("TownName"), decryptionKey);
                row["Address1"] = crypt.DecryptFromBase64(row.Field<string>("Address1"), decryptionKey);
                row["Address2"] = crypt.DecryptFromBase64(row.Field<string>("Address2"), decryptionKey);
                row["固定電話"] = crypt.DecryptFromBase64(row.Field<string>("固定電話"), decryptionKey);
                row["携帯電話"] = crypt.DecryptFromBase64(row.Field<string>("携帯電話"), decryptionKey);
                row["メールアドレス"] = crypt.DecryptFromBase64(row.Field<string>("メールアドレス"), decryptionKey);
            }
            return dt;
        }

        public DataTable get_Modal_DetailData(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_tenfold_get_Modal_DetailData", sqlParams);
            return dt;
        }

      
    }
}
