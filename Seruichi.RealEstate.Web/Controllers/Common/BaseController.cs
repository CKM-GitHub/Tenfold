using Models;
using Seruichi.Common;
using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string GetOperator(string type)
        {
            var user = SessionAuthenticationHelper.GetUserFromSession();
            if (user == null)
            {
                return "unknown";
            }
            else
            {
                string return_value = "";
                switch (type)
                {
                    case "UserID": return_value = user.REStaffCD; break;
                    case "UserName": return_value = user.REStaffName; break;
                    case "VerificationToken": return_value = user.VerificationToken; break;
                    case "RealECD": return_value = user.RealECD; break;
                    case "REStaffCD": return_value = user.REStaffCD; break;
                    case "PermissionChat": return_value = user.PermissionChat.ToString(); break;
                    case "PermissionSetting": return_value = user.PermissionSetting.ToString(); break;
                    case "PermissionPlan": return_value = user.PermissionPlan.ToString(); break;
                    case "PermissionInvoice": return_value = user.PermissionInvoice.ToString(); break;
                    case "REStaffName": return_value = user.REStaffName.ToString(); break;
                }
                return return_value;
            }
        }

        public static string GetLoginStaffName()
        {
            var user = SessionAuthenticationHelper.GetUserFromSession();
            if (user == null)
            {
                return "unknown";
            }
            else
            {
                return user.REStaffName;
            }
        }

        protected string GetClientIP()
        {
            var clientIp = "";
            var xForwardedFor = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(xForwardedFor) == false)
            {
                clientIp = xForwardedFor.Split(',').GetValue(0).ToString().Trim();
            }
            else
            {
                clientIp = Request.UserHostAddress;
            }

            if (clientIp != "::1") //localhost
            {
                clientIp = clientIp.Split(':').GetValue(0).ToString().Trim();
            }

            return clientIp;
        }

        protected ActionResult BadRequestResult()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
        }

        protected ActionResult OKResult()
        {
            return Json(new { isOK = true });
        }

        protected ActionResult OKResult<T>(T data)
        {
            return Json(new { isOK = true, data });
        }

        protected ActionResult ErrorMessageResult(string messageId)
        {
            var message = string.IsNullOrEmpty(messageId)? new MessageModel("Internal Server Error") : StaticCache.GetMessage(messageId);
            return Json(new { isOK = false, message });
        }

        protected ActionResult ErrorMessageResult<T>(string messageId, T data)
        {
            var message = string.IsNullOrEmpty(messageId) ? new MessageModel("Internal Server Error") : StaticCache.GetMessage(messageId);
            return Json(new { isOK = false, message, data });
        }

        protected ActionResult ErrorResult()
        {
            return Json(new { isOK = false });
        }

        protected ActionResult ErrorResult<T>(T data)
        {
            return Json(new { isOK = false, data });
        }

        protected string GetPreviousUrl()
        {
            return Session[BrowsingHistoryAttribute.PREVIOUS_URL].ToStringOrEmpty();
        }

        protected string GetPreviousController()
        {
            var arrayURL = Session[BrowsingHistoryAttribute.PREVIOUS_URL].ToStringOrEmpty().Split('/');
            if (arrayURL.Length >= 2)
            {
                return arrayURL[1];
            }
            return "";
        }

        protected T GetFromQueryString<T>() where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            var queryString = System.Web.HttpContext.Current.Request.QueryString;

            if (queryString.Count == 0)
            {
                return obj;
            }

            foreach (var property in properties)
            {
                var valueAsString = queryString[property.Name];
                var value = ParseAndDecode(valueAsString, property.PropertyType);

                if (value == null)
                    continue;

                property.SetValue(obj, value, null);
            }
            return obj;
        }

        private object ParseAndDecode(string val, Type type)
        {
            if (val == null) return val;
            return Parse(HttpUtility.UrlDecode(val), type);
        }

        protected T GetFromRequestForm<T>() where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            var requestForm = System.Web.HttpContext.Current.Request.Form;

            if (requestForm.Count == 0)
            {
                return obj;
            }

            foreach (var property in properties)
            {
                var valueAsString = requestForm[property.Name];
                var value = Parse(valueAsString, property.PropertyType);

                if (value == null)
                    continue;

                property.SetValue(obj, value, null);
            }
            return obj;
        }

        private object Parse(string val, Type type)
        {
            if (val == null) return val;

            if (val.Trim() == "" &&
                (type == typeof(Int32?)
                || type == typeof(Int16?)
                || type == typeof(bool?)
                || type == typeof(decimal?)
                || type == typeof(DateTime?)
                || type == typeof(Byte?)
                )) return null;

            if (type == typeof(Int32) || type == typeof(Int32?))
            {
                return val.ToInt32(0);
            }
            if (type == typeof(Int16) || type == typeof(Int16?))
            {
                return val.ToInt16(0);
            }
            if (type == typeof(bool) || type == typeof(bool?))
            {
                return val.ToBoolean();
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return val.ToDecimal(0);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return val.ToDateTime();
            }
            else if (type == typeof(Byte) || type == typeof(Byte?))
            {
                return val.ToByte(0);
            }
            else
            {
                return val;
            }
        }
        protected string ConvertToJson<T>(T data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        protected string DataTableToJSON(DataTable datatable)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(datatable);
        }

        protected T ConvertJsonToObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}