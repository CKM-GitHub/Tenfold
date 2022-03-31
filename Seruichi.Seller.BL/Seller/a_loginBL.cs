using Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;

namespace Seruichi.BL
{
    public class a_loginBL
    {
        public Dictionary<string, string> ValidateLogin(string userID, string password)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            if (string.IsNullOrEmpty(userID))
            {
                validator.AddValidationResult("UserID", "E202"); //メールアドレスが入力されていません
            }
            if (string.IsNullOrEmpty(password))
            {
                validator.AddValidationResult("Password", "E205"); //パスワードが入力されていません
            }
            if (!CheckSellerLogin(userID, password, out string errorcd))
            {
                validator.AddValidationResult("Password", errorcd);
            }
            return validator.GetValidationResult();
        }

        public bool CheckSellerLogin(string userID, string password, out string errorcd)
        {
            errorcd = "";

            var sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = password.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_SellerByPassword", sqlParams);
            if (dt.Rows.Count == 0)
            {
                errorcd = "E206"; //メールアドレスとパスワードの組み合わせが正しくありません
                return false;
            }

            var dr = dt.Rows[0];
            if (string.IsNullOrEmpty(dr["RegionCD"].ToStringOrEmpty()))
            {
                errorcd = "E206"; //メールアドレスとパスワードの組み合わせが正しくありません
                return false;
            }

            return true;
        }

    }
}
