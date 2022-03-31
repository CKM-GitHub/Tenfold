using Models;
using System.Collections.Generic;

namespace Seruichi.Common
{
    public class ValidatorAllItems
    {
        private Validator validator = new Validator();
        private Dictionary<string, string> result = new Dictionary<string, string>();

        public bool IsValid { get { return result.Count == 0; } }

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

        //CheckSelectionRequired ----->
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
        //CheckSelectionRequired <-----

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

        public void CheckCompareDate(string elementId, string fromDate, string toDate)
        {
            if (!validator.CheckCompareDate(fromDate, toDate, out string errorcd))
                AddValidationResult(elementId, errorcd);
        }

        public void CheckByteCount(string elementId, string inputText, int maxLength)
        {
            if (!validator.CheckByteCount(inputText, maxLength, out string errorcd, out string outVal))
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

        public void CheckIsDoubleByte(string elementId, string inputText, int maxLength)
        {
            if (!validator.CheckIsDoubleByte(inputText, maxLength, out string errorcd))
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
    }
}
