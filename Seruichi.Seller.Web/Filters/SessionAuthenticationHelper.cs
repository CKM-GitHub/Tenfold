using Models;
using System;
using System.Web;
using Seruichi.Common;

namespace Seruichi.Seller.Web
{
    public static class SessionAuthenticationHelper
    {
        public static string SESSION_KEY = "USER_AUTHENTICATION";
        public static string COOKIE_NAME = "ASP.NET_SessionId";

        public static string NewVerificationToken {
            get
            {
                return new AESCryption().GenerateRandomDataBase64(32);
            }
        }

        public static void ReCreateSession()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(COOKIE_NAME, ""));
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
            var userAuth = GetUserFromSession();
            if (userAuth == null)
            {
                return "";
            }
            else
            {
                return userAuth.VerificationToken;
            }
        }
    }
}