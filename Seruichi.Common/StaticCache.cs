using Models;
using Models.Common;
using System.Collections.Generic;

namespace Seruichi.Common
{
    public static class StaticCache
    {
        public static DBInfoEntity DBInfo { get; private set; }

        public static LogInfoEntity LogInfo { get; private set; }
        public static AttachmentEntity AttachmentInfo{ get; private set; }
        public static Dictionary<string, MessageModel> MessageDictionary { get; private set; } 
            = new Dictionary<string, MessageModel>(); 

        public static string MapsKey { get; private set; }

        private static string _encryptedDataCryptionKey = "";

        private static readonly object _lockObject = new object();
      //  public static string AttachedFilePath = @"C:\Tenfold\Attachment\TF\T_assess_guide\";  //{ get; private set; }

        public static void SetIniInfo()
        {
            ReadIni ini = new ReadIni();
            LogInfo = ini.GetLogInfo();
            DBInfo = ini.GetDBInfo();
            MapsKey = ini.GetMapsKey();
            AttachmentInfo = ini.GetAttachmentInfo();
            _encryptedDataCryptionKey = ini.GetDataCryptionKey();
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

        public static Dictionary<string, string> GetClientMessages()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in MessageDictionary)
            {
                dic.Add(item.Key, item.Value.MessageText1);
            }
            return dic;
        }

        public static string GetDataCryptionKey()
        {
            AESCryption cypt = new AESCryption();
            return cypt.DecryptFromBase64(_encryptedDataCryptionKey, AESCryption.DefaultKey);
        } 
    }
}
