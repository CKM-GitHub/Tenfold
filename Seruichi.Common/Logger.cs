using System;
using System.IO;
using System.Text;

namespace Seruichi.Common
{
    public enum LogLevel : Int32
    {
        None,
        Debug,
        Info,
        Error
    }

    public class Logger
    {
        private static Logger _singleton;
        private readonly string _logFilePath;
        private readonly LogLevel _logLevel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Logger()
        {
            _logFilePath = StaticCache.LogInfo.Path;
            _logLevel = (LogLevel)StaticCache.LogInfo.Level;
        }

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static Logger GetInstance()
        {
            if (_singleton == null)
            {
                _singleton = new Logger();
            }
            return _singleton;
        }

        public void Debug(string msg)
        {
            if (CheckLogLevel(LogLevel.Debug))
            {
                Write(msg, LogLevel.Debug);
            }
        }

        public void Info(string msg)
        {
            if (CheckLogLevel(LogLevel.Info))
            {
                Write(msg, LogLevel.Info);
            }
        }

        public void Error(string msg)
        {
            if (CheckLogLevel(LogLevel.Error))
            {
                Write(msg, LogLevel.Error);
            }
        }

        public void Error(Exception ex)
        {
            if (CheckLogLevel(LogLevel.Error))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                Write(sb.ToString(), LogLevel.Error);
            }
        }

        public void Error(Exception ex, string additonalMsg)
        {
            if (CheckLogLevel(LogLevel.Error))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(additonalMsg);
                sb.AppendLine(ex.StackTrace);
                Write(sb.ToString(), LogLevel.Error);
            }
        }

        private void Write(string msg, LogLevel level)
        {
            var fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            var absolutePath = Path.Combine(_logFilePath, fileName);

            if (!Directory.Exists(_logFilePath))
            {
                Directory.CreateDirectory(_logFilePath);
            }
            using (StreamWriter sw = new StreamWriter(absolutePath, true, Encoding.GetEncoding("Shift-JIS")))
            using (TextWriter tw = TextWriter.Synchronized(sw))
            {
                tw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                tw.Write("\t");
                switch (level)
                {
                    case LogLevel.Debug:
                        tw.Write("[Debug]\t");
                        break;
                    case LogLevel.Info:
                        tw.Write("[Info]\t");
                        break;
                    case LogLevel.Error:
                        tw.Write("[Error]\t");
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                tw.WriteLine(msg);
                tw.WriteLine();
            }
        }

        private bool CheckLogLevel(LogLevel level)
        {
            if (_logFilePath == "")
            {
                return false;
            }
            if (level < _logLevel)
            {
                return false;
            }
            return true;
        }
    }
}
