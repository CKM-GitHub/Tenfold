using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models.Tenfold.t_seller_mansion;
using System.Threading.Tasks;
using System.Linq;

namespace Seruichi.BL.Tenfold.t_seller_mansion
{
    public class t_seller_mansionBL
    {
        public Dictionary<string, string> ValidateAll(t_seller_mansionModel model,List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckMaxLenght("MansionName",model.MansionName,50);//E105
            validator.CheckIsDoubleByte("MansionName", model.MansionName, 50);//E107

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("Chk_Mi", lst_checkBox);//E112

            return validator.GetValidationResult();
        }
        public DataTable GetM_SellerMansionList(t_seller_mansionModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@Chk_Mi", SqlDbType.TinyInt){ Value = model.Chk_Mi.ToByte(0) },
                new SqlParameter("@Chk_Kan", SqlDbType.TinyInt){ Value = model.Chk_Kan.ToByte(0) },
                new SqlParameter("@Chk_Satei", SqlDbType.TinyInt){ Value = model.Chk_Satei.ToByte(0) },
                new SqlParameter("@Chk_Kaitori", SqlDbType.TinyInt){ Value = model.Chk_Kaitori.ToByte(0) },
                new SqlParameter("@Chk_Kakunin", SqlDbType.TinyInt){ Value = model.Chk_Kakunin.ToByte(0) },
                new SqlParameter("@Chk_Kosho", SqlDbType.TinyInt){ Value = model.Chk_Kosho.ToByte(0) },
                new SqlParameter("@Chk_Seiyaku", SqlDbType.TinyInt){ Value = model.Chk_Seiyaku.ToByte(0) },
                new SqlParameter("@Chk_Urinushi", SqlDbType.TinyInt){ Value = model.Chk_Urinushi.ToByte(0) },
                new SqlParameter("@Chk_Kainushi", SqlDbType.TinyInt){ Value = model.Chk_Kainushi.ToByte(0) },
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value = model.MansionName.ToStringOrNull() },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_mansion_Select_M_SellerMansionData", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            Parallel.ForEach(e, item =>
            {
                item["売主名"] = crypt.DecryptFromBase64(item.Field<string>("売主名"), decryptionKey);
            });
            if (!string.IsNullOrEmpty(model.MansionName))
            {
                var query = e.Where(dr => dr.Field<string>("マンション名").Contains(model.MansionName));
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
        public void InsertM_SellerMansion_L_Log(t_seller_mansion_l_log_Model model)
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
        public DataTable Generate_M_SellerMansionCSV(t_seller_mansionModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@Chk_Mi", SqlDbType.TinyInt){ Value = model.Chk_Mi.ToByte(0) },
                new SqlParameter("@Chk_Kan", SqlDbType.TinyInt){ Value = model.Chk_Kan.ToByte(0) },
                new SqlParameter("@Chk_Satei", SqlDbType.TinyInt){ Value = model.Chk_Satei.ToByte(0) },
                new SqlParameter("@Chk_Kaitori", SqlDbType.TinyInt){ Value = model.Chk_Kaitori.ToByte(0) },
                new SqlParameter("@Chk_Kakunin", SqlDbType.TinyInt){ Value = model.Chk_Kakunin.ToByte(0) },
                new SqlParameter("@Chk_Kosho", SqlDbType.TinyInt){ Value = model.Chk_Kosho.ToByte(0) },
                new SqlParameter("@Chk_Seiyaku", SqlDbType.TinyInt){ Value = model.Chk_Seiyaku.ToByte(0) },
                new SqlParameter("@Chk_Urinushi", SqlDbType.TinyInt){ Value = model.Chk_Urinushi.ToByte(0) },
                new SqlParameter("@Chk_Kainushi", SqlDbType.TinyInt){ Value = model.Chk_Kainushi.ToByte(0) },
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value = model.MansionName.ToStringOrNull() },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_mansion_csv_generate", sqlParams);
            return dt;
        }
    }
}
