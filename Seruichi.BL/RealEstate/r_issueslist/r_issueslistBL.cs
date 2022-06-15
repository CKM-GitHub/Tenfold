using System;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_issueslist;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_issueslist
{
    public class r_issueslistBL
    {
        public Dictionary<string, string> ValidateAll(r_issueslistModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckMaxLenght("txtFreeWord", model.FreeWord, 50);//E105

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("chk_New", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public async Task<DataTable> get_issueslist_Data(r_issueslistModel model)
        {
            return await Task.Run(() =>
            {
                AESCryption crypt = new AESCryption();
                string cryptionKey = StaticCache.GetDataCryptionKey();

                var sqlParams = new SqlParameter[]
                {
                new SqlParameter("@chk_New", SqlDbType.TinyInt){ Value = model.chk_New.ToByte(0) },
                new SqlParameter("@chk_Nego", SqlDbType.TinyInt){ Value = model.chk_Nego.ToByte(0) },
                new SqlParameter("@chk_Contract", SqlDbType.TinyInt){ Value = model.chk_Contract.ToByte(0) },
                new SqlParameter("@chk_SellerDeclined", SqlDbType.TinyInt){ Value = model.chk_SellerDeclined.ToByte(0) },
                new SqlParameter("@chk_BuyerDeclined", SqlDbType.TinyInt){ Value = model.chk_BuyerDeclined.ToByte(0) },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToString() },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@FreeWord", SqlDbType.VarChar){ Value = model.FreeWord.ToStringOrNull() }
               };

                DBAccess db = new DBAccess();
                var dt = db.SelectDatatable("pr_r_issueslist_getDisplayData", sqlParams);

                var e = dt.AsEnumerable();
                foreach(DataRow row in dt.Rows)
                {
                    row["売主_カナ"] = crypt.DecryptFromBase64(row.Field<string>("売主_カナ"), cryptionKey);
                    row["お客様名"] = crypt.DecryptFromBase64(row.Field<string>("お客様名"), cryptionKey);
                    row["売主_住所３"] = crypt.DecryptFromBase64(row.Field<string>("売主_住所３"), cryptionKey);
                    row["売主_住所４"] = crypt.DecryptFromBase64(row.Field<string>("売主_住所４"), cryptionKey);
                    row["売主_住所５"] = crypt.DecryptFromBase64(row.Field<string>("売主_住所５"), cryptionKey);
                    row["売主_固定電話番号"] = crypt.DecryptFromBase64(row.Field<string>("売主_固定電話番号"), cryptionKey);
                    row["売主_携帯電話番号"] = crypt.DecryptFromBase64(row.Field<string>("売主_携帯電話番号"), cryptionKey);
                }

                if (!string.IsNullOrEmpty(model.FreeWord))
                {
                    var query = dt.AsEnumerable().Where(dr => dr.Field<string>("物件名").Contains(model.FreeWord) || dr.Field<string>("お客様名").Contains(model.FreeWord));
                    if (query.Any())
                    {
                        int i = 0;
                        foreach (var row in query)
                        {
                            i++;
                            row["NO"] = i;
                        }
                        return query.OrderBy(row => row["査定依頼ID"]).CopyToDataTable();
                    }
                    else
                    {
                        DataTable newTable = dt.Clone();
                        return newTable;
                    }
                }
                return dt;
            });
        }

        public void Insertr_issueslist_L_Log(r_issueslistModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0)},
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_RealEstate_Insert_L_Log", false, sqlParams);
        }

        public DataTable generate_r_issueslist_CSV(r_issueslistModel model)
        {
            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@chk_New", SqlDbType.TinyInt){ Value = model.chk_New.ToByte(0) },
                new SqlParameter("@chk_Nego", SqlDbType.TinyInt){ Value = model.chk_Nego.ToByte(0) },
                new SqlParameter("@chk_Contract", SqlDbType.TinyInt){ Value = model.chk_Contract.ToByte(0) },
                new SqlParameter("@chk_SellerDeclined", SqlDbType.TinyInt){ Value = model.chk_SellerDeclined.ToByte(0) },
                new SqlParameter("@chk_BuyerDeclined", SqlDbType.TinyInt){ Value = model.chk_BuyerDeclined.ToByte(0) },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToString() },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@FreeWord", SqlDbType.VarChar){ Value = model.FreeWord.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_issueslist_getCSVData", sqlParams);

            var e = dt.AsEnumerable();
            foreach (DataRow row in dt.Rows)
            {
                row["売主カナ"] = crypt.DecryptFromBase64(row.Field<string>("売主カナ"), cryptionKey);
                row["売主名"] = crypt.DecryptFromBase64(row.Field<string>("売主名"), cryptionKey);
                row["売主_町域名"] = crypt.DecryptFromBase64(row.Field<string>("売主_町域名"), cryptionKey);
                row["売主_番地"] = crypt.DecryptFromBase64(row.Field<string>("売主_番地"), cryptionKey);
                row["売主_建物名部屋番号"] = crypt.DecryptFromBase64(row.Field<string>("売主_建物名部屋番号"), cryptionKey);
                row["売主_固定電話番号"] = crypt.DecryptFromBase64(row.Field<string>("売主_固定電話番号"), cryptionKey);
                row["売主_携帯電話番号"] = crypt.DecryptFromBase64(row.Field<string>("売主_携帯電話番号"), cryptionKey);
            }

            if (!string.IsNullOrEmpty(model.FreeWord))
            {
                var query = dt.AsEnumerable().Where(dr => dr.Field<string>("物件名").Contains(model.FreeWord) || dr.Field<string>("売主名").Contains(model.FreeWord));
                if (query.Any())
                {
                    int i = 0;
                    foreach (var row in query)
                    {
                        i++;
                        row["NO"] = i;
                    }
                    return query.OrderBy(row => row["査定依頼ID"]).CopyToDataTable();
                }
                else
                {
                    DataTable newTable = dt.Clone();
                    return newTable;
                }
            }
            return dt;
        }

        public DataTable get_SellerDetails_Data(r_issueslistModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() },
                new SqlParameter("@SellerID", SqlDbType.VarChar){ Value = model.SellerID.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_issueslist_get_SellerDetails_Data", sqlParams);
            return dt;
        }
    }
}
