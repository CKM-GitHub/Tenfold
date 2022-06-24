using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_statistic;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_statistic;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_statisticController : BaseController
    {
        // GET: r_statistic
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> get_issueslist_Data(r_statisticModel model)
        {
            r_statisticBL bl = new r_statisticBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.dac_route.ToString());
            chk_lst.Add(model.dac_apartment.ToString());
            chk_lst.Add(model.top5_route.ToString());
            chk_lst.Add(model.top5_apartment.ToString());
            chk_lst.Add(model.contracts_route.ToString());
            chk_lst.Add(model.contracts_apartment.ToString());
            chk_lst.Add(model.bd_route.ToString());
            chk_lst.Add(model.bd_apartment.ToString());
            chk_lst.Add(model.sd_route.ToString());
            chk_lst.Add(model.sd_apartment.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = await bl.get_r_statistic_displayData(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}