using System;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_asmc_address;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_asmc_railway
{
    public class r_asmc_railwayBL
    {
        public r_asmc_addressModel GetLinesByRegionCD(string regionCD, string realECD)
        {
            r_asmc_addressModel result = new r_asmc_addressModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RegionCD", SqlDbType.VarChar){ Value = regionCD },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_railway_Select_Lines_by_RegionCD", sqlParams);

            IEnumerable<r_asmc_address_City> e = dt.AsEnumerableEntity<r_asmc_address_City>();

            var query = e.GroupBy(r => new { r.PrefCD, r.PrefName, r.CityGroupCD, r.CityGroupName })
                        .Select(r => new r_asmc_address_CityGroup
                        {
                            PrefCD = r.Key.PrefCD,
                            PrefName = r.Key.PrefName,
                            CityGroupCD = r.Key.CityGroupCD,
                            CityGroupName = r.Key.CityGroupName,
                            Cities = r.Select(rr => rr),
                            CitiesCount = r.Count()
                        })
                        .GroupBy(r => new { r.PrefCD, r.PrefName })
                        .Select(r => new r_asmc_address_Pref
                        {
                            PrefCD = r.Key.PrefCD,
                            PrefName = r.Key.PrefName,
                            CityGroups = r.Select(rr => rr)
                        });

            result.Prefectures = query;
            return result;
        }

        public r_asmc_addressDetailModel GetStationsByLineCD(string linecdCsv, string realECD)
        {
            r_asmc_addressDetailModel result = new r_asmc_addressDetailModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@LinecdCsv", SqlDbType.VarChar){ Value = linecdCsv },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_railway_Select_Stations_by_Lines", sqlParams);

            IEnumerable<r_asmc_address_Town> e = dt.AsEnumerableEntity<r_asmc_address_Town>();

            var query = e.GroupBy(r => new { r.CityCD, r.CityName })
                        .Select(r => new r_asmc_address_City
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
