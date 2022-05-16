using Models;
using Seruichi.Common;
using System.Web;

namespace Seruichi.Seller.Web
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

        public static string GetAndStoreVerificationToken()
        {
            var user = GetUserFromSession();
            if (user == null)
            {
                return "";
            }
            else
            {
                //user.VerificationToken = NewVerificationToken;
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

        public static bool Logout()
        {
            HttpContext.Current.Session.Abandon();
            return true;
        }

        public static bool ChangeToAnonymousUser()
        {
            var user = GetUserFromSession();
            user.UserID = "unknown";
            user.UserName = "";
            return true;
        }
    }
}