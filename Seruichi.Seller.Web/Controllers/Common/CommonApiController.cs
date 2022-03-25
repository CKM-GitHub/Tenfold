using Models;
using Newtonsoft.Json.Linq;
using Seruichi.BL;
using Seruichi.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Seruichi.Seller.Web.Controllers
{
    public class testtest
    {
        public string PrefCD { get; set; }
    }

    public class CommonApiController : BaseApiController
    {
        [HttpPost]
        public HttpResponseMessage GetMessage([FromBody] MessageModel model)
        {
            if (model == null) return BadRequestResult();
            return OKResult(StaticCache.GetMessage(model.MessageID));
        }

        [HttpPost]
        public HttpResponseMessage GetMessageAll()
        {
            return OKResult(StaticCache.MessageDictionary);

            //Dictionary<string, string> dictionary = new Dictionary<string, string>();
            //dictionary.Add("E101", StaticCache.GetMessage("E101").MessageText1);
            //dictionary.Add("E102", StaticCache.GetMessage("E102").MessageText1);
            //dictionary.Add("E103", StaticCache.GetMessage("E103").MessageText1);
            //dictionary.Add("E104", StaticCache.GetMessage("E104").MessageText1);
            //dictionary.Add("E105", StaticCache.GetMessage("E105").MessageText1);
            //dictionary.Add("E106", StaticCache.GetMessage("E106").MessageText1);
            //dictionary.Add("E107", StaticCache.GetMessage("E107").MessageText1);
            //dictionary.Add("E108", StaticCache.GetMessage("E108").MessageText1);
            //dictionary.Add("E109", StaticCache.GetMessage("E109").MessageText1);
            //dictionary.Add("E201", StaticCache.GetMessage("E201").MessageText1);

            //return OKResult(dictionary);
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfPrefecture()
        {
            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfPrefecture());
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfCity([FromBody] JToken token)
        {
            var model = new
            {
                PrefCD = token["PrefCD"].ToStringOrEmpty()
            };
            if (string.IsNullOrEmpty(model.PrefCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfCity(model.PrefCD));
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfTown([FromBody] JToken token)
        {
            var model = new
            {
                PrefCD = token["PrefCD"].ToStringOrEmpty(),
                CityCD = token["CityCD"].ToStringOrEmpty()
            };
            if (string.IsNullOrEmpty(model.PrefCD) || string.IsNullOrEmpty(model.CityCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfTown(model.PrefCD, model.CityCD));
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfLine([FromBody] JToken token)
        {
            var model = new
            {
                PrefCD = token["PrefCD"].ToStringOrEmpty(),
            };
            if (string.IsNullOrEmpty(model.PrefCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfLine(model.PrefCD));
        }

        [HttpPost]
        public HttpResponseMessage GetDropDownListItemsOfStation([FromBody] JToken token)
        {
            var model = new
            {
                LineCD = token["LineCD"].ToStringOrEmpty(),
            };
            if (string.IsNullOrEmpty(model.LineCD)) return BadRequestResult();

            CommonBL bl = new CommonBL();
            return OKResult(bl.GetDropDownListItemsOfStation(model.LineCD));
        }

        [HttpPost]
        public HttpResponseMessage GetBuildingAge([FromBody]string constYYYYMM)
        {
            if (string.IsNullOrEmpty(constYYYYMM))
            {
                return OKResult("");
            }
            CommonBL bl = new CommonBL();
            return OKResult(bl.GetBuildingAge(constYYYYMM).ToStringOrEmpty());
        }
    }
}
