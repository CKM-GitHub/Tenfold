using System.Text;
using System.IO;

namespace Seruichi.Common
{
    public class ReadIni
    {
        private readonly string InifilePath = @"C:\Tenfold\Seruichi.ini";
        private readonly string DefaultLogPath = @"C:\Tenfold\Log";

        private static readonly Encoding encoding = Encoding.GetEncoding("Shift_JIS");


        public string GetValue(string category, string key)
        {
            IniFileReader ifr = new IniFileReader(InifilePath);
            return ifr.IniReadValue(category, key);
        }

        public DBInfoEntity GetDBInfo()
        {
            string iniString = "";
            string iniString_Encrypted = GetValue("Database", "Seruichi");

            if (string.IsNullOrEmpty(iniString_Encrypted))
            {
                iniString = GetValue("Database", "Seruichi_DEBUG");
            }
            else
            {
                var crypt = new AESCryption();
                iniString = crypt.DecryptFromBase64(iniString_Encrypted, AESCryption.DefaultKey2);
            }

            string[] dbconfig = iniString.Split(',');
            if (dbconfig.Length < 4)
            {
                throw new InitialSetupException("INIファイルが正しく設定されていません。");
            }

            return new DBInfoEntity(dbconfig[0], dbconfig[1], dbconfig[2], dbconfig[3])
            {
                ConnectionTimeout = GetValue("Database", "ConnectionTimeout").ToInt32(15), //未指定時、デフォルト値
                CommandTimeout = GetValue("Database", "CommandTimeout").ToInt32(30) //未指定時、デフォルト値
            };
        }
        public AttachmentEntity GetAttachmentInfo()
        {
            try
            {
                string maxsize = GetValue("ATTACHMENT", "MAX_SIZE");
                string maxfile = GetValue("ATTACHMENT", "MAX_FILE");
                string exts = GetValue("ATTACHMENT", "EXTENSION");
                string destin = GetValue("ATTACHMENT", "DESTINATION");
                if (!string.IsNullOrEmpty(maxsize) && !string.IsNullOrEmpty(maxfile) && !string.IsNullOrEmpty(exts) && !string.IsNullOrEmpty("destin"))
                    return new AttachmentEntity()
                    {
                        Max_Size = maxsize,
                        Max_File = maxfile,
                        Exts = exts,
                        Destin=destin
                    };
                else
                   return new AttachmentEntity();
            }
            catch
            {
                throw new InitialSetupException("INIファイルが正しく設定されていません。");
            }
        }

        public LogInfoEntity GetLogInfo()
        {
            return new LogInfoEntity()
            {
                Path = GetValue("Log", "LOG_PATH").ToStringOrNull()?? DefaultLogPath,
                Level = GetValue("Log", "LOG_LEVEL").ToInt32(3) //未指定時、3:Error
            };
        }

        public string GetDataCryptionKey()
        {
            string dataKeyPath = GetValue("PATH", "DATA_KEY").ToStringOrEmpty();
            string encryptedDataCryptionKey = "";

            if (!string.IsNullOrEmpty(dataKeyPath) && File.Exists(dataKeyPath))
            {
                using (StreamReader sr = new StreamReader(dataKeyPath, encoding))
                {
                    encryptedDataCryptionKey = sr.ReadToEnd();
                }
            }

            return encryptedDataCryptionKey;
        }

        public string GetMapsKey()
        {
            string mapsKeyPath = GetValue("PATH", "MAPS_KEY").ToStringOrEmpty();
            string key = "";

            if (!string.IsNullOrEmpty(mapsKeyPath) && File.Exists(mapsKeyPath))
            {
                using (StreamReader sr = new StreamReader(mapsKeyPath, encoding))
                {
                    key = sr.ReadToEnd();
                }
            }

            return key;
        }
    }
}
