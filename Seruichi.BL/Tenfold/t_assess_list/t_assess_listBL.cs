using Models.Tenfold.t_assess_list;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_assess_list
{
    public class t_assess_listBL
    {
        public Dictionary<string, string> ValidateAll(t_assess_listModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("Chk_Shinki", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public DataTable GetM_SellerMansionList(t_assess_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@Chk_Shinki", SqlDbType.TinyInt){ Value = model.Chk_Shinki.ToByte(0) },
                new SqlParameter("@Chk_Kosho", SqlDbType.TinyInt){ Value = model.Chk_Kosho.ToByte(0) },
                new SqlParameter("@Chk_Seiyaku", SqlDbType.TinyInt){ Value = model.Chk_Seiyaku.ToByte(0) },
                new SqlParameter("@Chk_Urinushi", SqlDbType.TinyInt){ Value = model.Chk_Urinushi.ToByte(0) },
                new SqlParameter("@Chk_Kainushi", SqlDbType.TinyInt){ Value = model.Chk_Kainushi.ToByte(0) },
                new SqlParameter("@Chk_Hide", SqlDbType.TinyInt){ Value = model.Chk_Hide.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_assess_list_Select_M_SellerMansionData", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            Parallel.ForEach(e, item =>
            {
                item["売主名"] = crypt.DecryptFromBase64(item.Field<string>("売主名"), decryptionKey);
            });
            return dt;
        }
        public void InsertM_SellerMansion_L_Log(t_assess_list_l_log_Model model)
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
            db.InsertUpdateDeleteData("pr_t_assess_list_Insert_L_Log", false, sqlParams);
        }

        public DataTable Generate_M_SellerMansionCSV(t_assess_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@Chk_Shinki", SqlDbType.TinyInt){ Value = model.Chk_Shinki.ToByte(0) },
                new SqlParameter("@Chk_Kosho", SqlDbType.TinyInt){ Value = model.Chk_Kosho.ToByte(0) },
                new SqlParameter("@Chk_Seiyaku", SqlDbType.TinyInt){ Value = model.Chk_Seiyaku.ToByte(0) },
                new SqlParameter("@Chk_Urinushi", SqlDbType.TinyInt){ Value = model.Chk_Urinushi.ToByte(0) },
                new SqlParameter("@Chk_Kainushi", SqlDbType.TinyInt){ Value = model.Chk_Kainushi.ToByte(0) },
                new SqlParameter("@Chk_Hide", SqlDbType.TinyInt){ Value = model.Chk_Hide.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_assess_list_csv_generate", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            Parallel.ForEach(e, item =>
            {
                item["売主名"] = crypt.DecryptFromBase64(item.Field<string>("売主名"), decryptionKey);
            });
            return dt;
        }

    }
}
