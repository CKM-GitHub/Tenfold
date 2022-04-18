using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Seruichi.RealState.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        protected string GetClientIP()
        {
            IEnumerable<string> headerValues;
            var clientIp = "";
            if (ControllerContext.Request.Headers.TryGetValues("X-Forwarded-For", out headerValues) == true)
            {
                var xForwardedFor = headerValues.FirstOrDefault();
                clientIp = xForwardedFor.Split(',').GetValue(0).ToString().Trim();
            }
            else
            {
                if (ControllerContext.Request.Properties.ContainsKey("MS_HttpContext"))
                {
                    clientIp = ((HttpContextWrapper)ControllerContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                }
            }

            if (clientIp != "::1") //localhost
            {
                clientIp = clientIp.Split(':').GetValue(0).ToString().Trim();
            }
            return clientIp;
        }

        protected HttpResponseMessage BadRequestResult()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        protected HttpResponseMessage OKResult()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { isOK = true });
        }

        protected HttpResponseMessage OKResult<T>(T data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { isOK = true, data });
        }

        protected HttpResponseMessage ErrorMessageResult(string messageId)
        {
            return ErrorMessageResult(messageId, "");
        }

        protected HttpResponseMessage ErrorMessageResult<T>(string messageId, T data)
        {
            if (messageId.Length > 0)
            {
                var message = StaticCache.GetMessage(messageId);
                return Request.CreateResponse(HttpStatusCode.OK, new { isOK = false, message, data });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        protected HttpResponseMessage ErrorResult<T>(T data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { isOK = false, data });
        }

        protected string ConvertToJson<T>(T data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        protected string DataTableToJSON(DataTable datatable)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(datatable);
        }

        protected T GetFromRequestForm<T>() where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            var requestForm = HttpContext.Current.Request.Form;

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
    }
}
