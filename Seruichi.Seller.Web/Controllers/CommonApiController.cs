using System.Web.Http;
using Models;
using OkameiProduction.BL;

namespace OkameiProduction.Web.Controllers
{
    public class CommonApiController : BaseApiController
    {
        [HttpPost]
        public string GetMessage([FromBody] MessageInfo model)
        {
            if (model == null) return GetBadRequestResult();
            return ConvertToJsonResult(StaticCache.GetMessageInfo(model.MessageID));
        }

        [HttpPost]
        public string CheckValid([FromBody] ValidateModel model)
        {
            if (model == null) return GetBadRequestResult();

            var bl = new CommonBL();
            string msgid = "";
            string outVal = "";

            if (model.IsDateType)
            {
                if (!bl.CheckAndFormatDate(model.InputValue1, out msgid, out outVal))
                {
                    return GetErrorResult(msgid, outVal);
                }
            }
            if (model.IsDateYYMM)
            {
                if (!bl.CheckAndFormatYMDate(model.InputValue1, out msgid, out outVal))
                {
                    return GetErrorResult(msgid, outVal);
                }
            }

            if (model.IsCompareDate)
            {
                if (!bl.CheckCompareDate(model.ComparisonValue, model.InputValue1, out msgid))
                {
                    return GetErrorResult(msgid, outVal);
                }
            }

            if (model.IsDoubleByte)
            {
                if (!bl.CheckByteCount(model.InputValue1, model.MaxLength.ToInt32(0), out msgid, out outVal))
                {
                    return GetErrorResult(msgid, outVal);
                }
            }

            if (model.IsDoubleByteOnly)
            {
                if (!bl.CheckIsDoubleByte(model.InputValue1, out msgid))
                {
                    return GetErrorResult(msgid);
                }
            }

            if (model.IsHalfWidth)
            {
                if (!bl.CheckIsHalfWidth(model.InputValue1, out msgid, out outVal))
                {
                    return GetErrorResult(msgid, outVal);
                }
            }

            if (model.IsNumeric)
            {
                if (!bl.CheckIsNumeric(model.InputValue1, model.Integerdigits, model.Decimaldigits, out msgid, out outVal))
                {
                    return GetErrorResult(msgid);
                }
            }

            return ConvertToJsonResult(new { ReturnValue = outVal }); //変換後の値を返却
        }
    }
}
