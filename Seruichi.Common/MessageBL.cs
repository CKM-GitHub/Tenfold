using Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Seruichi.Common
{
    public class MessageBL
    {
        public Dictionary<string, MessageModel> SelecetAtApplicationStart()
        {
            var result = new Dictionary<string, MessageModel>();

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_Message_Select_AtApplicationStart");

            //未登録
            if (dt.Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow dr in dt.Rows)
            {
                var msg = new MessageModel()
                {
                    MessageID = dr["MessageID"].ToStringOrEmpty(),
                    MessageText1 = dr["MessageText1"].ToStringOrEmpty(),
                    MessageText2 = dr["MessageText2"].ToStringOrEmpty(),
                    MessageText3 = dr["MessageText3"].ToStringOrEmpty(),
                    MessageText4 = dr["MessageText4"].ToStringOrEmpty(),
                    MessageMark = dr["MessageMark"].ToInt32(0),
                    MessageIcon = GetMessageIcon(dr["MessageMark"].ToInt32(0)),
                    MessageButton = dr["MessageButton"].ToInt32(0),
                    ButtonValues = dr["ButtonValues"].ToInt32(0),
                };
                result.Add(msg.MessageID, msg);
            }

            return result;
        }

        public MessageModel SelectMessage(string messageid)
        {
            var sqlParams = new SqlParameter[] {
                new SqlParameter("@MessageID", SqlDbType.VarChar) { Value = messageid }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_M_Message_Select_ByID", sqlParams);

            //未登録
            if (dt.Rows.Count == 0)
            {
                return new MessageModel();
            }

            DataRow dr = dt.Rows[0];
            return new MessageModel()
            {
                MessageID = dr["MessageID"].ToStringOrEmpty(),
                MessageText1 = dr["MessageText1"].ToStringOrEmpty(),
                MessageText2 = dr["MessageText2"].ToStringOrEmpty(),
                MessageText3 = dr["MessageText3"].ToStringOrEmpty(),
                MessageText4 = dr["MessageText4"].ToStringOrEmpty(),
                MessageMark = dr["MessageMark"].ToInt32(0),
                MessageIcon = GetMessageIcon(dr["MessageMark"].ToInt32(0)),
                MessageButton = dr["MessageButton"].ToInt32(0),
                ButtonValues = dr["ButtonValues"].ToInt32(0),
            };
        }

        public string GetMessageIcon(int messageMark)
        {
            if (messageMark == 1)
            {
                return "success";
            }
            if (messageMark == 2)
            {
                return "error";
            }
            if (messageMark == 3)
            {
                return "warning";
            }
            if (messageMark == 4)
            {
                return "info";
            }

            return "";
        }
    }
}
