using Models;
using System;
using System.Web;
using Seruichi.Common;
using Models.RealEstate.r_login;

namespace Seruichi.RealEstate.Web
{
    public static class SessionAuthenticationHelper
    {
        public static string SESSION_KEY = "USER_AUTHENTICATION";

        public static string NewVerificationToken {
            get
            {
                return new AESCryption().GenerateRandomDataBase64(32);
            }
        }

        public static void CreateAnonymousUser()
        {
            var userInfo = new r_loginModel()
            {
                UserID = "unknown",
                VerificationToken = NewVerificationToken
            };
            HttpContext.Current.Session[SESSION_KEY] = userInfo;
        }

        public static void CreateLoginUser(r_loginModel model)
        {
            model.VerificationToken = NewVerificationToken;
            HttpContext.Current.Session[SESSION_KEY] = model;
        }

        public static r_loginModel GetUserFromSession()
        {
            return HttpContext.Current.Session[SESSION_KEY] as r_loginModel;
        }

        public static string GetVerificationToken()
        {
            var user = GetUserFromSession();
            if (user == null)
            {
                return "";
            }
            else
            {
                return user.VerificationToken;
            }
        }

        public static bool ValidateUser(r_loginModel user)
        {
            if (user == null || user.UserID == "unknown")
            {
                return false;
            }
            return true;
        }
    }
}