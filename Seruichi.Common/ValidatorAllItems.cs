using Models;
using System.Collections.Generic;

namespace Seruichi.Common
{
    public class ValidatorAllItems
    {
        private Validator validator = new Validator();
        private Dictionary<string, string> result = new Dictionary<string, string>();

        public bool IsValid { get { return result.Count == 0; } }

        public bool IsContains(string key)
        {
            return result.ContainsKey(key);
        }

        public Dictionary<string, string> GetValidationResult()
        {
            return result;
        }

        public void AddValidationResult(string elementId, string errorcd)
        {
            if (!result.ContainsKey(elementId))
                result.Add(elementId, StaticCache.GetMessageText1(errorcd));
        }

        public void CheckRequired(string elementId, string inputText)
        {
            if (!validator.CheckRequired(inputText, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckRequiredNumber(string elementId, string inputValue, bool notAllowZero)
        {
            if (!validator.CheckRequiredNumber(inputValue, notAllowZero, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckSelectionRequired(string elementId, string inputText)
        {
            if (!validator.CheckSelectionRequired(inputText, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckSelectionRequired(string elementId, byte inputValue)
        {
            if (!validator.CheckSelectionRequired(inputValue, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }
        public void CheckYM(string elementId, string inputText)
        {
            if (!validator.CheckAndFormatYM(inputText, out string errorcd, out string outVal))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckYMDate(string elementId, string inputText)
        {
            if (!validator.CheckAndFormatYMDate(inputText, out string errorcd, out string outVal))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckDate(string elementId, string inputText)
        {
            if (!validator.CheckAndFormatDate(inputText, out string errorcd, out string outVal))
                AddValidationResult(elementId, errorcd);
        }
        public void CheckCompareYM(string elementId, string fromDate, string toDate)
        {
            if (!validator.CheckCompareYM(fromDate, toDate, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckCompareDate(string elementId, string fromDate, string toDate)
        {
            if (!validator.CheckCompareDate(fromDate, toDate, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckCompareNum(string elementId, string fromNum, string toNum)
        {
            if (!validator.CheckCompareNum(fromNum, toNum, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckByteCount(string elementId, string inputText, int maxLength)
        {
            if (!validator.CheckByteCount(inputText, maxLength, out string errorcd, out string outVal))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckIsOnlyOneCharacter(string elementId, string inputText)
        {
            if (!validator.CheckIsOnlyOneCharacter(inputText,out string errorcd))
                AddValidationResult(elementId, errorcd);
        }


        public void CheckIsHalfWidth(string elementId, string inputText, int maxLength)
        {
            if (!validator.CheckIsHalfWidth(inputText, maxLength, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckIsHalfWidth(string elementId, string inputText, int maxLength, RegexFormat regexFormat)
        {
            if (!validator.CheckIsHalfWidth(inputText, maxLength, regexFormat, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckIsDoubleByte(string elementId, string inputText, int maxByteLength)
        {
            if (!validator.CheckIsDoubleByte(inputText, maxByteLength, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckIsDoubleByteKana(string elementId, string inputText, int maxLength)
        {
            if (!validator.CheckIsDoubleByteKana(inputText, maxLength, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckIsMoney(string elementId, string inputText, int digits)
        {
            if (!validator.CheckIsMoney(inputText, digits, out string errorcd, out string outVal))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckIsNumeric(string elementId, string inputText, int integerdigits, int decimaldigits)
        {
            if (!validator.CheckIsNumeric(inputText, integerdigits, decimaldigits, out string errorcd, out string outVal))
                AddValidationResult(elementId, errorcd);
        }
        public void CheckCheckboxLenght(string elementId,List<string> lstInputText)
        {
            if (!validator.CheckCheckBoxLenght(lstInputText, out string errorcd))
                 AddValidationResult(elementId, errorcd);

        }

        public void CheckMaxLenght(string elementId, string inputText, int maxLength)
        {
            if (!validator.CheckMaxLenght(inputText, maxLength, out string errorcd))
                AddValidationResult(elementId, errorcd);

        }

        public void CheckMinLenght(string elementId, string inputText, int maxLength)
        {
            if (!validator.CheckMinLenght(inputText, maxLength, out string errorcd))
                AddValidationResult(elementId, errorcd);

        }

        public void CheckIsValidEmail(string elementId, string mailAddress)
        {
            if (!validator.CheckIsValidEmail(mailAddress, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckREStaffMailAddress(string elementId, string mailAddress)
        {
            if (!validator.CheckREStaffMailAddress(mailAddress, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckSellerMailAddress(string elementId, string mailAddress)
        {
            if (!validator.CheckSellerMailAddress(mailAddress, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckBirthday(string elementId, string birthday)
        {
            if (!validator.CheckBirthday(birthday, out string errorcd, out string outVal))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckComparePassword(string elementId, string password,string confirmpassword)
        {
            if (!validator.CheckComparePassword(password,confirmpassword, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }
    }
}
