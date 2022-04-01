using Models;
using System;
using System.Security.Cryptography;
using System.Web;

namespace Seruichi.Tenfold.Web
{
    public static class SessionAuthenticationHelper
    {
        public static string SESSION_KEY = "USER_AUTHENTICATION";
        public static string COOKIE_NAME = "ASP.NET_SessionId";

        public static string NewVerificationToken { get { return GenerateRandomDataBase64(32); } }

        public static void ReCreateSession()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(COOKIE_NAME, ""));
        }

        public static SessionAuthenticationInfo CreateAnonymousUser()
        {
            var userInfo = new SessionAuthenticationInfo()
            {
                UserID = "unknown",
                VerificationToken = NewVerificationToken
            };
            HttpContext.Current.Session[SESSION_KEY] = userInfo;
            return userInfo;
        }

        public static SessionAuthenticationInfo CreateLoginUser(string id)
        {
            var userInfo = new SessionAuthenticationInfo()
            {
                UserID = id,
                VerificationToken = NewVerificationToken
            };

            HttpContext.Current.Session[SESSION_KEY] = userInfo;
            return userInfo;
        }

        public static SessionAuthenticationInfo GetUserFromSession()
        {
            return HttpContext.Current.Session[SESSION_KEY] as SessionAuthenticationInfo;
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

        public static string GenerateRandomDataBase64(int length)
        {
            var rnd = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(rnd);
            }
            return Convert.ToBase64String(rnd);
        }

    }
}