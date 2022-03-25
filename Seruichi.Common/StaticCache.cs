using Models;
using System.Collections.Generic;

namespace Seruichi.Common
{
    public static class StaticCache
    {
        public static DBInfoEntity DBInfo { get; private set; }

        public static LogInfoEntity LogInfo { get; private set; }

        public static Dictionary<string, MessageModel> MessageDictionary { get; private set; } 
            = new Dictionary<string, MessageModel>();

        private static readonly object _lockObject = new object();



        public static void SetIniInfo()
        {
            ReadIni ini = new ReadIni();
            DBInfo = ini.GetDBInfo();
            LogInfo = ini.GetLogInfo();
        }

        public static void SetMessageCache()
        {
            MessageBL bl = new MessageBL();
            MessageDictionary = bl.SelecetAtApplicationStart();
        }

        public static MessageModel GetMessage(string id)
        {
            lock (_lockObject)
            {
                if (MessageDictionary.TryGetValue(id, out MessageModel message))
                {
                    return message;
                }
                else
                {
                    MessageBL bl = new MessageBL();
                    message = bl.SelectMessage(id);
                    if (!string.IsNullOrEmpty(message.MessageID))
                    {
                        MessageDictionary.Add(message.MessageID, message);
                    }
                    return message;
                }
            }
        }

        public static string GetMessageText1(string id)
        {
            var message = GetMessage(id);
            return message.MessageText1;
        }
    }
}
