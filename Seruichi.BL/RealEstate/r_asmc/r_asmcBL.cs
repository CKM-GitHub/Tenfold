using Models.RealEstate.r_asmc;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_asmc
{
    public class r_asmcBL
    {
        public IEnumerable<r_asmc_Region> GetRegions()
        {
            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_Select_M_Region_for_Map");

            //var query = dt.AsEnumerable()
            //            .GroupBy(r => new { RegionCD = r.Field<string>("RegionCD"), RegionName = r.Field<string>("RegionName"), CssName = r.Field<string>("CssName") })
            //            .Select(r => new r_asmc_Region
            //            {
            //                RegionCD = r.Key.RegionCD,
            //                RegionName = r.Key.RegionName,
            //                CssName = r.Key.CssName,
            //                PrefName = string.Join("|", r.Select(rr => rr.Field<string>("PrefName"))),
            //            });

            var query = dt.AsEnumerable()
                        .Select(r => new r_asmc_Region
                        {
                            RegionCD = r.Field<string>("RegionCD"),
                            RegionName = r.Field<string>("RegionName"),
                            CssName = r.Field<string>("CssName"),
                            PrefName = r.Field<string>("PrefName").Replace("\r", "<br>")
                        });

            return query;
        }
    }
}
