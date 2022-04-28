using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models.Tenfold.t_seller_assessment;

namespace Seruichi.BL.Tenfold.t_seller_assessment
{
    public class t_seller_assessmentBL
    {
        public Dictionary<string, string> ValidateAll(t_seller_assessmentModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("Chk_Mi", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public t_seller_assessment_header_Model GetM_SellerBy_SellerCD(string SellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_M_Seller", sqlParams);

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
            t_seller_assessment_header_Model model = DataTableExtentions.ToEntity<t_seller_assessment_header_Model>(dt.Rows[0]);

            return model;
        }

        public DataTable GetM_SellerMansionList(t_seller_assessmentModel model,string SellerCD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD },
                new SqlParameter("@Chk_Mi", SqlDbType.TinyInt){ Value = model.Chk_Mi.ToByte(0) },
                new SqlParameter("@Chk_Kan", SqlDbType.TinyInt){ Value = model.Chk_Kan.ToByte(0) },
                new SqlParameter("@Chk_Satei", SqlDbType.TinyInt){ Value = model.Chk_Satei.ToByte(0) },
                new SqlParameter("@Chk_Kaitori", SqlDbType.TinyInt){ Value = model.Chk_Kaitori.ToByte(0) },
                new SqlParameter("@Chk_Kakunin", SqlDbType.TinyInt){ Value = model.Chk_Kakunin.ToByte(0) },
                new SqlParameter("@Chk_Kosho", SqlDbType.TinyInt){ Value = model.Chk_Kosho.ToByte(0) },
                new SqlParameter("@Chk_Seiyaku", SqlDbType.TinyInt){ Value = model.Chk_Seiyaku.ToByte(0) },
                new SqlParameter("@Chk_Urinushi", SqlDbType.TinyInt){ Value = model.Chk_Urinushi.ToByte(0) },
                new SqlParameter("@Chk_Kainushi", SqlDbType.TinyInt){ Value = model.Chk_Kainushi.ToByte(0) },
                new SqlParameter("@Chk_Sakujo", SqlDbType.TinyInt){ Value = model.Chk_Sakujo.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_M_SellerMansionData", sqlParams);
            return dt;
        }
        public void InsertM_SellerMansion_L_Log(t_seller_assessment_l_log_Model model)
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
            db.InsertUpdateDeleteData("pr_t_seller_assessment_list_Insert_L_Log", false, sqlParams);
        }
        
        public DataTable Generate_M_SellerMansionCSV(t_seller_assessmentModel model, string SellerCD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD },
                new SqlParameter("@Chk_Mi", SqlDbType.TinyInt){ Value = model.Chk_Mi.ToByte(0) },
                new SqlParameter("@Chk_Kan", SqlDbType.TinyInt){ Value = model.Chk_Kan.ToByte(0) },
                new SqlParameter("@Chk_Satei", SqlDbType.TinyInt){ Value = model.Chk_Satei.ToByte(0) },
                new SqlParameter("@Chk_Kaitori", SqlDbType.TinyInt){ Value = model.Chk_Kaitori.ToByte(0) },
                new SqlParameter("@Chk_Kakunin", SqlDbType.TinyInt){ Value = model.Chk_Kakunin.ToByte(0) },
                new SqlParameter("@Chk_Kosho", SqlDbType.TinyInt){ Value = model.Chk_Kosho.ToByte(0) },
                new SqlParameter("@Chk_Seiyaku", SqlDbType.TinyInt){ Value = model.Chk_Seiyaku.ToByte(0) },
                new SqlParameter("@Chk_Urinushi", SqlDbType.TinyInt){ Value = model.Chk_Urinushi.ToByte(0) },
                new SqlParameter("@Chk_Kainushi", SqlDbType.TinyInt){ Value = model.Chk_Kainushi.ToByte(0) },
                new SqlParameter("@Chk_Sakujo", SqlDbType.TinyInt){ Value = model.Chk_Sakujo.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_csv_generate", sqlParams);
            return dt;
        }

        public DataTable Get_PopupFor_Home(t_seller_assessment_Popup_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_ForPopup_Home", sqlParams);
            return dt;
        }
         public DataTable Get_PopupFor_ResultType_1(t_seller_assessment_Popup_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_ForPopup_ResultType_1", sqlParams);
            return dt;
        }
        public DataTable Get_PopupFor_ResultType_2(t_seller_assessment_Popup_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_ForPopup_ResultType_2", sqlParams);
            return dt;
        }
        
        public DataTable Get_PopupFor_Detail(string strSellerMansionID)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = strSellerMansionID }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_ForPopup_Detail", sqlParams);
            return dt;
        }
        
        public DataTable Get_PopupFor_Seller(string SellerCD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_ForPopup_Seller", sqlParams);
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
