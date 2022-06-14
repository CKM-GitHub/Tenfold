using Models.RealEstate.r_asmc_railway;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_asmc_railway
{
    public class r_asmc_railwayBL
    {
        public r_asmc_railwayModel GetLinesByRegionCD(string regionCD, string realECD)
        {
            r_asmc_railwayModel result = new r_asmc_railwayModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RegionCD", SqlDbType.VarChar){ Value = regionCD },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_railway_Select_Lines_by_RegionCD", sqlParams);

            IEnumerable<r_asmc_railway_Line> e = dt.AsEnumerableEntity<r_asmc_railway_Line>();

            var query = e.GroupBy(r => new { r.PrefCD, r.PrefName, r.CityGroupCD, r.CityGroupName })
                        .Select(r => new r_asmc_railway_LineCompany
                        {
                            PrefCD = r.Key.PrefCD,
                            PrefName = r.Key.PrefName,
                            CityGroupCD = r.Key.CityGroupCD,        //CompanyCD
                            CityGroupName = r.Key.CityGroupName,    //CompanyName
                            Cities = r.Select(rr => rr),            //Line
                            CitiesCount = r.Count()
                        })
                        .GroupBy(r => new { r.PrefCD, r.PrefName })
                        .Select(r => new r_asmc_railway_Pref
                        {
                            PrefCD = r.Key.PrefCD,
                            PrefName = r.Key.PrefName,
                            CityGroups = r.Select(rr => rr)         //Company
                        });

            result.Prefectures = query;
            return result;
        }

        public r_asmc_railwayDetailModel GetStationsByLineCD(string linecdCsv, string realECD)
        {
            r_asmc_railwayDetailModel result = new r_asmc_railwayDetailModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@LinecdCsv", SqlDbType.VarChar){ Value = linecdCsv },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_railway_Select_Stations_by_Lines", sqlParams);

            IEnumerable<r_asmc_railway_Station> e = dt.AsEnumerableEntity<r_asmc_railway_Station>();

            var query = e.GroupBy(r => new { r.CityCD, r.CityName })
                        .Select(r => new r_asmc_railway_Line
                        {
                            CityCD = r.Key.CityCD,                  //LineCD
                            CityName = r.Key.CityName,              //LineName
                            Towns = r.Select(rr => rr),             //Station
                            TownsCount = r.Count()
                        });

            result.Cities = query;
            return result;
        }
    }
}
