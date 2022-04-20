using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Seruichi.Common
{
    public class Validator
    {
        private Encoding encoding = Encoding.GetEncoding("Shift_JIS");
        private Encoding encodingMyan = Encoding.GetEncoding("UTF-8");
        public bool CheckRequired(string inputText, out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputText))
            {
                errorcd = "E101"; //入力が必要です
                return false;
            }
            return true;
        }

        public bool CheckRequiredNumber(string inputValue, bool notAllowZero, out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputValue))
            {
                errorcd = "E101"; //入力が必要です
                return false;
            }

            if (notAllowZero)
            {
                if (decimal.TryParse(inputValue, NumberStyles.Number, CultureInfo.CurrentCulture, out var decimalValue) && decimalValue == 0)
                {
                    errorcd = "E101"; //入力が必要です
                    return false;
                }
            }

            return true;
        }

        public bool CheckSelectionRequired(string inputText, out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputText))
            {
                errorcd = "E102"; //必ず１つ選択してください
                return false;
            }
            return true;
        }

        public bool CheckSelectionRequired(byte inputValue, out string errorcd)
        {
            errorcd = "";

            if (inputValue == 0)
            {
                errorcd = "E102"; //必ず１つ選択してください
                return false;
            }
            return true;
        }

        public bool CheckAndFormatYMDate(string inputText, out string errorcd, out string outVal)
        {
            errorcd = "";
            outVal = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            if (!CheckIsHalfWidth(inputText, 7, out errorcd))
            {
                return false;
            }

            if (inputText.Length > 7)
            {
                errorcd = "E108"; //正しい日付を入力してください
                return false;
            }

            var sysDateTime = Utilities.GetSysDateTime();

            if (inputText.Contains("/"))
            {
                var split = inputText.Split('/');
                if (split.Length == 2)
                {
                    //yyyyMM -> yyyy/MM/dd
                    inputText = split[0] + "/" + split[1] + "/" + "01";
                }

            }
            else if (inputText.Contains("-"))
            {
                var split = inputText.Split('-');
                if (split.Length == 2)
                {
                    //yyyyMM -> yyyy/MM/dd
                    inputText = split[0] + "/" + split[1] + "/" + "01";
                }
            }
            else
            {
                if (inputText.Length == 6)
                {
                    //yyyyMM -> yyyyMMdd
                    inputText = inputText.ToString().Substring(0, 4) + "/" + inputText.ToString().Substring(6 - 2) + "/" + "01";
                }
                else if (inputText.Length == 4)
                {
                    //yyyy -> yyyyMMdd
                    inputText = inputText.ToString() + "/" + sysDateTime.Month.ToString().PadLeft(2, '0') + "/" + "01";
                }
                else if (inputText.Length == 2)
                {
                    //mm -> yyyyMMdd
                    inputText = sysDateTime.Year.ToString().PadLeft(4, '0') + "/" + inputText.ToString() + "/" + "01";
                }
                else if (inputText.Length == 1)
                {
                    //m -> yyyyMMdd
                    inputText = sysDateTime.Year.ToString().PadLeft(4, '0') + "/" + inputText.ToString().PadLeft(2, '0') + "/" + "01";
                }
            }

            if (inputText.ToDateTime() == null)
            {
                errorcd = "E108"; //正しい日付を入力してください
                return false;
            }

            outVal = inputText.ToDateTime(sysDateTime).ToString(DateTimeFormat.yyyyMMdd);
            outVal = outVal.Substring(0, 7).Replace("-", "/");
            return true;
        }


        public bool CheckAndFormatDate(string inputText, out string errorcd, out string outVal)
        {
            errorcd = "";
            outVal = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            //if (!CheckIsHalfWidth(inputText, 10, out errorcd))
            //{
            //    return false;

            //}

            if (inputText.Length > 10)
            {
                errorcd = "E108"; //正しい日付を入力してください
                return false;
            }

            var sysDateTime = Utilities.GetSysDateTime();

            if (inputText.Contains("/"))
            {
                var split = inputText.Split('/');
                if (split.Length <= 2)
                {
                    //MM/dd -> yyyy/MM/dd
                    inputText = sysDateTime.Year.ToString() + "/" + inputText;
                }
                else if (split.Length == 3)
                {
                    //yy/MM/dd -> yyyy/MM/dd
                    inputText = sysDateTime.Year.ToString().Substring(0, 4 - split[0].Length) + inputText;
                }
            }
            else
            {
                if (inputText.Length <= 4)
                {
                    //MMdd -> yyyyMMdd
                    inputText = sysDateTime.Year.ToString() + inputText.PadLeft(4, '0');
                }
                else if (inputText.Length < 8)
                {
                    //yyMMdd -> yyyyMMdd
                    inputText = sysDateTime.Year.ToString().Substring(0, 8 - inputText.Length) + inputText;
                }
            }

            if (inputText.ToDateTime() == null)
            {
                errorcd = "E108"; //正しい日付を入力してください
                return false;
            }

            outVal = inputText.ToDateTime(sysDateTime).ToString(DateTimeFormat.yyyyMMdd);
            return true;
        }

        public bool CheckCompareDate(string fromDate, string toDate, out string errorcd)
        {
            errorcd = "";

            //if (!CheckAndFormatDate(fromDate, out errorcd, out string correctFromDate))
            //{
            //    return false;
            //}

            //if (!CheckAndFormatDate(toDate, out errorcd, out string correctToDate))
            //{
            //    return false;
            //}

            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate)) return true;
            
            if (fromDate.Length>10 || toDate.Length > 10) return true;

            if (fromDate.ToDateTime() > toDate.ToDateTime())
            {
                errorcd = "E111"; //入力された値が正しくありません ★エラーメッセージ未定
                return false;
            }
            return true;
        }

        public bool CheckByteCount(string inputText, int maxLength, out string errorcd, out string cutString)
        {
            errorcd = "";
            cutString = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            cutString = inputText.GetByteString(maxLength);
            if (inputText != cutString)
            {
                errorcd = "E105"; //入力できる桁数を超えています。
                return false;
            }
            return true;
        }


        public bool CheckIsOnlyOneCharacter(string inputText,out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputText)) return true;
            if (encoding.GetByteCount(inputText) != inputText.Length)
            {
                errorcd = "E104"; //入力できない文字です。
                return false;
            }

            if (encodingMyan.GetByteCount(inputText) != inputText.Length)
            {
                errorcd = "E104"; //入力できない文字です。
                return false;
            }
            return true;
        }
        public bool CheckIsHalfWidth(string inputText, int maxLength, out string errorcd)
        {
            return CheckIsHalfWidth(inputText, maxLength, RegexFormat.None, out errorcd);
        }

        public bool CheckIsHalfWidth(string inputText, int maxLength, RegexFormat regexFormat, out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            if (!CheckByteCount(inputText, maxLength, out errorcd, out string dummy))
            {
                return false;
            }

            if (encoding.GetByteCount(inputText) != inputText.Length)
            {
                errorcd = "E104"; //入力できない文字です。
                return false;
            }

            if (regexFormat != RegexFormat.None)
            {
                var pattern = "";
                if (regexFormat == RegexFormat.Number)
                    pattern = "^[0-9]+$";
                if (regexFormat == RegexFormat.Alphabet)
                    pattern = "^[a-zA-Z]+$";
                if (regexFormat == RegexFormat.NumAlphaLowUp)
                    pattern = "^[0-9a-zA-Z]+$";

                if (!System.Text.RegularExpressions.Regex.IsMatch(inputText, pattern))
                {
                    errorcd = "E104"; //入力できない文字です。
                    return false;
                }
            }

            return true;
        }

        public bool CheckIsDoubleByte(string inputText, int maxLength, out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            if (!CheckByteCount(inputText, maxLength, out errorcd, out string dummy))
            {
                return false;
            }

            if (encoding.GetByteCount(inputText) != (inputText.Length * 2))
            {
                errorcd = "E107"; //全角文字で入力してください
                return false;
            }
            return true;
        }

        public bool CheckIsDoubleByteKana(string inputText, int maxLength, out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            if (!CheckIsDoubleByte(inputText, maxLength, out errorcd))
            {
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(inputText, "^[ァ-ヶー　]+$"))
            {
                errorcd = "E104"; //入力できない文字です。
                return false;
            }

            return true;
        }

        public bool CheckIsMoney(string inputText, int digits, out string errorcd, out string outVal)
        {
            errorcd = "";
            outVal = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            if (!CheckIsNumeric(inputText, digits, 0, out errorcd, out string decimalFormatValue))
            {
                return false;
            }

            outVal = decimalFormatValue.ToDecimal().ToStringOrEmpty("#,##0");
            return true;
        }

        public bool CheckIsNumeric(string inputText, int integerdigits, int decimaldigits, out string errorcd, out string outVal)
        {
            errorcd = "";
            outVal = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            if (!Decimal.TryParse(inputText, out decimal decimalValue))
            {
                errorcd = "E104"; //入力できない文字です。
                return false;
            }

            //小数以下桁数で切り捨て
            if (decimaldigits > 0)
            {
                decimal power = Math.Pow(10, decimaldigits).ToDecimal(0);
                decimalValue = Math.Truncate(decimalValue * power) / power;
            }

            //min max
            string maxValue = "";
            if (integerdigits == 0)
            {
                maxValue = "0";
            }
            else
            {
                maxValue = new string('9', integerdigits);
            }
            if (decimaldigits > 1)
            {
                maxValue += "." + new string('9', decimaldigits);
            }

            if (maxValue.ToDecimal(0) < decimalValue || (maxValue.ToDecimal(0) * -1) > decimalValue)
            {
                errorcd = "E105";
                return false; //入力できる桁数を超えています
            }

            //out val
            if (decimaldigits == 0)
            {
                outVal = decimalValue.ToString("#0");
            }
            else
            {
                var format = "#0." + new String('0', decimaldigits);
                outVal = decimalValue.ToString(format);
            }

            return true;
        }
        public bool CheckCheckBoxLenght(List<string> inputText, out string errorcd)
        {
            errorcd = "";
            int total = inputText.Sum(x => Convert.ToInt32(x));
            if(total == 0)
            {
                errorcd = "E112"; //'１つ以上選択してください'
                return false;
            }            
            return true;
        }

        //add by pnz
        public bool CheckMaxLenghtForHalfWidthandFullwidth(string inputText, int maxLength, out string errorcd)
        {
            errorcd = "";

            if (string.IsNullOrEmpty(inputText)) return true;

            if (inputText.Length > maxLength)
            {
                errorcd = "E105"; //入力できる桁数を超えています。
                return false;
            }
            return true;
        }

        
    }
}
