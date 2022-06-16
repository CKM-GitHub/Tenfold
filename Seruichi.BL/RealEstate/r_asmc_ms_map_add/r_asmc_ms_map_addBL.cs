using Models.RealEstate.r_asmc_ms_map_add;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_asmc_ms_map_add
{
    public class r_asmc_ms_map_addBL
    {
        public r_asmc_ms_map_addModel GetCitiesByRegionCD(string regionCD, string realECD)
        {
            r_asmc_ms_map_addModel result = new r_asmc_ms_map_addModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RegionCD", SqlDbType.VarChar){ Value = regionCD },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_ms_map_add_Select_Cities_by_RegionCD", sqlParams);

            IEnumerable<r_asmc_ms_map_add_City> e = dt.AsEnumerableEntity<r_asmc_ms_map_add_City>();

            var query = e.GroupBy(r => new { r.PrefCD, r.PrefName, r.CityGroupCD, r.CityGroupName })
                        .Select(r => new r_asmc_ms_map_add_CityGroup
                        {
                            PrefCD = r.Key.PrefCD,
                            PrefName = r.Key.PrefName,
                            CityGroupCD = r.Key.CityGroupCD,
                            CityGroupName = r.Key.CityGroupName,
                            Cities = r.Select(rr => rr),
                            CitiesCount = r.Count()
                        })
                        .GroupBy(r => new { r.PrefCD, r.PrefName })
                        .Select(r => new r_asmc_ms_map_add_Pref
                        {
                            PrefCD = r.Key.PrefCD,
                            PrefName = r.Key.PrefName,
                            CityGroups = r.Select(rr => rr)
                        });

            result.Prefectures = query;
            return result;
        }

        public r_asmc_ms_map_addDetailModel GetTownsByCityCD(string citycdCsv, string realECD)
        {
            r_asmc_ms_map_addDetailModel result = new r_asmc_ms_map_addDetailModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CitycdCsv", SqlDbType.VarChar){ Value = citycdCsv },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_ms_map_add_Select_Towns_by_Cities", sqlParams);

            IEnumerable<r_asmc_ms_map_add_Town> e = dt.AsEnumerableEntity<r_asmc_ms_map_add_Town>();

            var query = e.GroupBy(r => new { r.CityCD, r.CityName })
                        .Select(r => new r_asmc_ms_map_add_City
                        {
                            CityCD = r.Key.CityCD,
                            CityName = r.Key.CityName,
                            Towns = r.Select(rr => rr),
                            TownsCount = r.Count()
                        });

            result.Cities = query;
            return result;
        }
    }
}
