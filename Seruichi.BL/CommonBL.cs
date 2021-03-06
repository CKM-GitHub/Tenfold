using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Seruichi.BL
{
    public class CommonBL
    {
        //DropDownList
        private List<DropDownListItem> GetDropDownListItems(string spName, params SqlParameter[] sqlParams)
        {
            var items = new List<DropDownListItem>();

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable(spName, sqlParams);

            var hasHiddenItem = dt.Columns.Contains("HiddenItem");

            foreach (DataRow dr in dt.Rows)
            {
                var option = new DropDownListItem()
                {
                    Value = dr["Value"].ToStringOrEmpty(),
                    DisplayText = dr["DisplayText"].ToStringOrEmpty(),
                    DisplayOrder = dr["DisplayOrder"].ToInt32(0),
                    HiddenItem = hasHiddenItem ? dr["HiddenItem"].ToStringOrEmpty() : ""
                };                
                items.Add(option);
            }

            return items;
        }

        public List<DropDownListItem> GetDropDownListItemsOfAllPrefecture()
        {
            string spName = "pr_Common_Select_DropDownListOfPrefAll";
            return GetDropDownListItems(spName);
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

        public List<DropDownListItem> GetDropDownListItemsOfContactType()
        {
            string spName = "pr_Common_Select_DropDownListOfContactType";
            return GetDropDownListItems(spName);
        }

        public List<DropDownListItem> GetDropDownListItemsOfMultPurpose(int dataID)
        {
            string spName = "pr_Common_Select_DropDownListOfMultPurpose";
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@DataID", SqlDbType.Int) { Value = dataID },
            };

            return GetDropDownListItems(spName, sqlParams);
        }

        public List<DropDownListItem> GetDropDownListItemsOfStaff_by_RealECD(string realECD)
        {
            string spName = "pr_Common_Select_DropDownListOfREStaff";
            SqlParameter sqlParam = new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = realECD };

            return GetDropDownListItems(spName, sqlParam);
        }

        public List<DropDownListItem> GetDropDownListItemsOfTemplate_for_ManOpt(string realECD)
        {
            string spName = "pr_Common_Select_DropDownListOfTemplate_for_ManOpt";
            SqlParameter sqlParam = new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = realECD };

            return GetDropDownListItems(spName, sqlParam);
        }
        public List<DropDownListItem> GetDropDownListItemsOfCourse()
        {
            string spName = "pr_t_reale_new_Select_M_Course";
            return GetDropDownListItems(spName);
        }





        public List<DropDownListItem> GetDropDownListItemsOfTenfoldStaff()
        {
            string spName = "pr_Common_Select_DropDownListOfTenfoldStaff";
            return GetDropDownListItems(spName);
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

        public List<MansionStation> GetNearestStations(params decimal[] longitudeAndLatitude)
        {
            List<MansionStation> result = new List<MansionStation>();
            if (longitudeAndLatitude == null || longitudeAndLatitude.Length < 2)
            {
                return result;
            }

            string arg1 = longitudeAndLatitude[0].ToStringOrEmpty();
            string arg2 = longitudeAndLatitude[1].ToStringOrEmpty();

            string url = string.Format("https://express.heartrails.com/api/json?method=getStations&x={0}&y={1}", arg1, arg2);

            string responseBody = "";
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                using (System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    responseBody = response.Content.ReadAsStringAsync().Result;
                }
            }

            var def = new {
                response = new {
                    station = new[]
                    {
                        new {
                            name = "",
                            prefecture = "",
                            line = "",
                            x = (decimal)0,
                            y = (decimal)0,
                            postal = "",
                            distance = "",
                            prev = "",
                            next = ""
                        }
                    }
                }
            };

            var deserialized = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(responseBody, def);
            if (deserialized == null)
            {
                return result;
            }

            DataTable dtParam = new DataTable();
            dtParam.Columns.Add("RowNo", typeof(int));
            dtParam.Columns.Add("LineName", typeof(string));
            dtParam.Columns.Add("StationName", typeof(string));
            dtParam.Columns.Add("Distance", typeof(int));

            int rowNo = 0;
            foreach (var data in deserialized.response.station)
            {
                rowNo++;
                DataRow dr = dtParam.NewRow();
                dr["LineName"] = data.line;
                dr["StationName"] = data.name;
                dr["Distance"] = data.distance.Replace("m", "").ToInt32(0);
                dr["RowNo"] = rowNo;
                dtParam.Rows.Add(dr);
            }

            var sqlParm = new SqlParameter("@MansionStationNameTable", SqlDbType.Structured)
            {
                TypeName = "dbo.T_MansionStationName",
                Value = dtParam
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_Common_Select_LineChange", sqlParm);

            foreach (DataRow dr in dt.Rows)
            {
                result.Add(dr.ToEntity<MansionStation>());
            }

            return result;
        }

        public int GetBuildingAge(string yearMonth)
        {
            DateTime startDate = (yearMonth.Replace("/", "").Replace("-", "") + "01").ToDateTime(DateTime.MaxValue);
            DateTime endDate = Utilities.GetSysDateTime();

            if (startDate > endDate) return 0;

            int diffMonth = (endDate.Month + (endDate.Year - startDate.Year) * 12) - startDate.Month;
            if (diffMonth == 0)
                return 1;
            else
                //return diffMonth % 12 > 0 ? diffMonth / 12 + 1 : diffMonth / 12;
                return diffMonth / 12 + 1;
        }

        public string GetTenstaffNamebyTenstaffcd(string staffcd)
        {
            string staffName = string.Empty;
            var sqlParm = new SqlParameter("@TenStaffCD", SqlDbType.VarChar) { Value = staffcd };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_common_t_login_select_TenStaffName_by_TenStaffCD", sqlParm);
            if (dt.Rows.Count > 0)
            {
                staffName = dt.Rows[0]["TenStaffName"].ToString();
            }
            return staffName;
        }

        public string GetSellerNamebySellerCD(string sellerCD)
        {
            string SellerName = string.Empty;
            var sqlParm = new SqlParameter("@SellerCD", SqlDbType.VarChar) { Value = sellerCD };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_common_t_seller_select_SellerName_by_SellerCD", sqlParm);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            if (dt.Rows.Count > 0)
            {
                string StrSellerName = dt.Rows[0]["SellerName"].ToString();
                SellerName = !string.IsNullOrEmpty(StrSellerName) ? crypt.DecryptFromBase64(StrSellerName, decryptionKey) : StrSellerName;
            }
            return SellerName;
        }

        public string GetMansionNamebyMansioncd(string mansionCD)
        {
            string mansionName = string.Empty;
            var sqlParm = new SqlParameter("@MansionCD", SqlDbType.VarChar) { Value = mansionCD };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_common_tenfold_select_MansionName_by_MansionCD", sqlParm);
            if (dt.Rows.Count > 0)
            {
                mansionName = dt.Rows[0]["MansionName"].ToString();
            }
            return mansionName;
        }

        public string GetRealEstateNamebyRealECD(string realECD)
        {
            string rename = string.Empty;
            var sqlParm = new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = realECD };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_common_tenfold_select_REName_by_RealECD", sqlParm);
            if (dt.Rows.Count > 0)
            {
                rename = dt.Rows[0]["REName"].ToString();
            }
            return rename;
        }

        public SendMailInfo GetMailSender(SendMailInfo mailInfo = null)
        {
            if (mailInfo == null)
            {
                mailInfo = new SendMailInfo();
            }

            var sqlParm = new SqlParameter("@DataKey", SqlDbType.Int) { Value = 1 };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_Mail_Select_by_DataKey", sqlParm);
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
            var dt = db.SelectDatatable("pr_M_MailSendTo_Select_by_MailKBN", sqlParm);
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
            var dt = db.SelectDatatable("pr_M_MailText_Select_by_MailKBN", sqlParm);
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

        //for at most five mail address 
        public List<SendMailInfo.Recipient> GetPersonMailRecipients(MailKBN mailKbn,SendMailInfo mailInfo, String mailAdd1 = null, String mailAdd2 = null, String mailAdd3 = null, String mailAdd4 = null, String mailAdd5 = null)
        {

            List<SendMailInfo.Recipient> recipients = new List<SendMailInfo.Recipient>();
            var sqlParm = new SqlParameter("@MailKBN", SqlDbType.Int) { Value = (int)mailKbn };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_MailSendTo_Select_by_MailKBN", sqlParm);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                recipients.Add(new SendMailInfo.Recipient()
                {
                    MailAddress = dr["SendToAddress"].ToStringOrEmpty(),
                    SendType = (SendMailInfo.SendTypes)dr["SendType"].ToInt32(0)
                });
            }

            //more than one mail 
            string[] parameterList = new string[] { mailAdd1, mailAdd2, mailAdd3, mailAdd4, mailAdd5 };
            string[] addressList = parameterList.Where(item => item != null).ToArray();
            foreach (string mailAddress in addressList)
            {
                recipients.Add(new SendMailInfo.Recipient()
                    {
                        MailAddress = mailAddress,
                        SendType = SendMailInfo.SendTypes.To
                    });
            }
            if (mailInfo != null)
            {
                mailInfo.Recipients = recipients;
            }
            return recipients;
        }

        public string Get_RECondOpt_OptionKBNName(int optionKbn)
        {
            switch (optionKbn)
            {
                case  1 : return "総戸数";
                case  2 : return "所在階";
                case  3 : return "所在階";
                case  4 : return "専有面積";
                case  5 : return "バルコニー";
                case  6 : return "主採光";
                case  7 : return "角部屋";
                case  8 : return "部屋数";
                case  9 : return "バストイレ";
                case  10 : return "土地権利";
                case  11 : return "賃貸状況";
                case  12 : return "管理状況";
                case  13 : return "売却希望時期";
                default: return "";
            }
        }

        public string Get_RECondOpt_ValueText(int optionKbn, int categoryKbn, int value1)
        {
            switch (optionKbn)
            {
                case 2: return "1階";
                case 3: return "最上階";
                case 5: return "バルコニーなし";
                case 6: return "北向き";
                case 7: return "角部屋";
                case 10: return "借地権";
            }

            if (optionKbn == 9)
            {
                switch (categoryKbn)
                {
                    case 1: return "セパレート";
                    case 2: return "ユニットバス";
                    case 3: return "シャワーブース";
                    default: return "";
                }
            }

            if (optionKbn == 11)
            {
                switch (categoryKbn)
                {
                    case 1: return "賃貸中";
                    case 2: return "サブリース";
                    case 3: return "空室";
                    default: return "";
                }
            }

            if (optionKbn == 12)
            {
                switch (categoryKbn)
                {
                    case 1: return "自主管理";
                    case 2: return "管理委託";
                    default: return "";
                }
            }

            if (optionKbn == 13)
            {
                switch (categoryKbn)
                {
                    case 1: return "2週間以上";
                    case 2: return "1ヶ月以上";
                    case 3: return "3ヶ月以上";
                    case 4: return "6ヶ月以上";
                    case 5: return "1年以上";
                    case 6: return "その他";
                    default: return "";
                }
            }

            return value1.ToStringOrEmpty();
        }

        public string Get_RECondOpt_HandlingKBN1Text(int handlingKbn1)
        {
            switch (handlingKbn1)
            {
                case 1: return "以内";
                case 2: return "～";
                default: return "";
            }
        }

        public string Get_RECondOpt_NotApplicableFLGText(int notApplicableFlg)
        {
            return notApplicableFlg == 1 ? "査定・買取対象外" : "";
        }

        public string Get_RECond_ValidFLGText(int validFlg)
        {
            return validFlg == 1 ? "(公開済)" : "(未公開)";
        }

        public string Get_RECond_PrecedenceFlgText(int precedenceFlg)
        {
            return precedenceFlg == 1? "優先" : "通常";
        }

        public string Get_RECond_NotApplicableFLGText(int notApplicableFlg)
        {
            return notApplicableFlg == 1 ? "査定買取対象外" : "";
        }
    }
}
