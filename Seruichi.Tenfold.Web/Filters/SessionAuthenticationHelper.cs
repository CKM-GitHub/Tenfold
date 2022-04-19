using Models;
using System;
using System.Web;
using Seruichi.Common;

namespace Seruichi.Tenfold.Web
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
            var userInfo = new LoginUser()
            {
                UserID = "unknown",
                VerificationToken = NewVerificationToken
            };
            HttpContext.Current.Session[SESSION_KEY] = userInfo;
        }

        public static void CreateLoginUser(string id)
        {
            var userInfo = new LoginUser()
            {
                UserID = id,
                VerificationToken = NewVerificationToken
            };

            HttpContext.Current.Session[SESSION_KEY] = userInfo;
        }

        public static void CreateLoginUser(LoginUser user)
        {
            user.VerificationToken = NewVerificationToken;
            HttpContext.Current.Session[SESSION_KEY] = user;
        }

        public static LoginUser GetUserFromSession()
        {
            return HttpContext.Current.Session[SESSION_KEY] as LoginUser;
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

        public static bool ValidateUser(LoginUser user)
        {
            if (user == null || user.UserID == "unknown")
            {
                return false;
            }
            return true;
        }
    }
}