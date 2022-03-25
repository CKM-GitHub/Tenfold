using System.Text;

namespace Seruichi.Common
{
    public class ReadIni
    {
        private readonly string InifilePath = @"C:\Tenfold\Seruichi.ini";

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

                var crypt = new AESCryption();
                //string crypted = crypt.EncryptToBase64(iniString, AESCryption.DefaultKey);
            }
            else
            {
                var crypt = new AESCryption();
                iniString = crypt.DecryptFromBase64(iniString_Encrypted, AESCryption.DefaultKey);
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

        public LogInfoEntity GetLogInfo()
        {
            return new LogInfoEntity()
            {
                Path = GetValue("Log", "LOG_PATH").ToStringOrEmpty(),
                Level = GetValue("Log", "LOG_LEVEL").ToInt32(3) //未指定時、3:Error
            };
        }
    }
}
