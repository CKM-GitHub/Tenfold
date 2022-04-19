using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Seruichi.BL
{
    public class CommonBL
    {
        private List<DropDownListItem> GetDropDownListItems(string spName, params SqlParameter[] sqlParams)
        {
            var items = new List<DropDownListItem>();

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable(spName, sqlParams);

            foreach (DataRow dr in dt.Rows)
            {
                var option = new DropDownListItem()
                {
                    Value = dr["Value"].ToStringOrEmpty(),
                    DisplayText = dr["DisplayText"].ToStringOrEmpty(),
                    DisplayOrder = dr["DisplayOrder"].ToInt32(0)
                };
                items.Add(option);
            }

            return items;
        }

        public List<DropDownListItem> GetDropDownListItemsOfPrefecture()
        {
            string spName = "pr_Common_Select_DropDownListOfPref";
            return GetDropDownListItems(spName);
        }

        public List<DropDownListItem> GetDropDownListItemsOfCity(string prefCD)
        {
            string spName = "pr_Common_Select_DropDownListOfCity";
            SqlParameter sqlParam = new SqlParameter("@PrefCD", SqlDbType.VarChar) { Value = prefCD.ToStringOrNull() };

            return GetDropDownListItems(spName, sqlParam);
        }

        public List<DropDownListItem> GetDropDownListItemsOfTown(string prefCD, string cityCD)
        {
            string spName = "pr_Common_Select_DropDownListOfTown";
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@PrefCD", SqlDbType.VarChar) { Value = prefCD.ToStringOrNull() },
                new SqlParameter("@CityCD", SqlDbType.VarChar) { Value = cityCD.ToStringOrNull() }
            };

            return GetDropDownListItems(spName, sqlParams);
        }

        public List<DropDownListItem> GetDropDownListItemsOfLine(string prefCD)
        {
            string spName = "pr_Common_Select_DropDownListOfLine";
            SqlParameter sqlParam = new SqlParameter("@PrefCD", SqlDbType.VarChar) { Value = prefCD.ToStringOrNull() };

            return GetDropDownListItems(spName, sqlParam);
        }

        public List<DropDownListItem> GetDropDownListItemsOfStation(string lineCD)
        {
            string spName = "pr_Common_Select_DropDownListOfStation";
            SqlParameter sqlParam = new SqlParameter("@LineCD", SqlDbType.VarChar) { Value = lineCD.ToStringOrNull() };

            return GetDropDownListItems(spName, sqlParam);
        }

        public bool CheckExistsCounterMaster(CounterKey counterKey, out string errorID)
        {
            errorID = "";
            var sqlParm = new SqlParameter("@CounterKey", SqlDbType.Int) { Value = (int)counterKey };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_Counter_Select_Exists", sqlParm);
            if (dt.Rows.Count == 0)
            {
                errorID = "E106"; //システム内部で必要な設定が足りていません
                return false;
            }

            return true;
        }

        public decimal[] GetLongitudeAndLatitude(params string[] address)
        {
            string param = "";
            if (address != null)
            {
                param = String.Join("", address);
            }

            string url = string.Format("{0}?{1}={2}",
                "https://msearch.gsi.go.jp/address-search/AddressSearch", "q", System.Net.WebUtility.UrlEncode(param));

            string responseBody = "";
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                using (System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    responseBody = response.Content.ReadAsStringAsync().Result;
                }
            }

            var def = new[]
            {
                new {
                    geometry = new { coordinates = new decimal[2], type = "" },
                    type = "",
                    properties = new { addressCode = "", title = "" }
                }
            };

            var deserialized = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(responseBody, def);
            if (deserialized == null || deserialized.Length == 0)
            {
                return new decimal[] { 0, 0 };
            }
            else
            {
                return deserialized[0].geometry.coordinates;
            }
        }

        public int GetBuildingAge(string yearMonth)
        {
            DateTime startDate = (yearMonth.Replace("/", "").Replace("-", "") + "01").ToDateTime(DateTime.MaxValue);
            DateTime endDate = Utilities.GetSysDateTime();

            if (startDate > endDate) return 0;

            int diffMonth = (endDate.Month + (endDate.Year - startDate.Year) * 12) - startDate.Month;
            return diffMonth % 12 > 0 ? diffMonth / 12 + 1 : diffMonth / 12;
        }

        public SendMailInfo GetMailSender(SendMailInfo mailInfo = null)
        {
            if (mailInfo == null)
            {
                mailInfo = new SendMailInfo();
            }

            var sqlParm = new SqlParameter("@DataKey", SqlDbType.Int) { Value = 1 };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_Mail_Select_ByDataKey", sqlParm);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                mailInfo.SenderAddress = dr["SenderAddress"].ToStringOrEmpty();
                mailInfo.SenderServer = dr["SenderServer"].ToStringOrEmpty();
                mailInfo.SenderAccount = dr["SenderAccount"].ToStringOrEmpty();
                mailInfo.SenderPassword = dr["SenderPassword"].ToStringOrEmpty();
            }

            return mailInfo;
        }

        public List<SendMailInfo.Recipient> GetMailRecipients(MailKBN mailKbn, SendMailInfo mailInfo = null)
        {
            List<SendMailInfo.Recipient> recipients = new List<SendMailInfo.Recipient>();
            var sqlParm = new SqlParameter("@MailKBN", SqlDbType.Int) { Value = (int)mailKbn };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_MailSendTo_Select_ByMailKBN", sqlParm);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                recipients.Add(new SendMailInfo.Recipient() {
                    MailAddress = dr["SendToAddress"].ToStringOrEmpty(),
                    SendType = (SendMailInfo.SendTypes)dr["SendType"].ToInt32(0)
                });
            }

            if (mailInfo != null)
            {
                mailInfo.Recipients = recipients;
            }
            return recipients;
        }

        public SendMailInfo GetMailTitleAndText(MailKBN mailKbn, SendMailInfo mailInfo = null)
        {
            if (mailInfo == null)
            {
                mailInfo = new SendMailInfo();
            }

            var sqlParm = new SqlParameter("@MailKBN", SqlDbType.Int) { Value = (int)mailKbn };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_MailText_Select_ByMailKBN", sqlParm);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                mailInfo.Subject = dr["MailTitle"].ToStringOrEmpty();
                mailInfo.Text1 = dr["MailText01"].ToStringOrEmpty();
                mailInfo.Text2 = dr["MailText02"].ToStringOrEmpty();
                mailInfo.Text3 = dr["MailText03"].ToStringOrEmpty();
                mailInfo.Text4 = dr["MailText04"].ToStringOrEmpty();
                mailInfo.Text5 = dr["MailText05"].ToStringOrEmpty();
            }

            return mailInfo;
        }
    }
}
