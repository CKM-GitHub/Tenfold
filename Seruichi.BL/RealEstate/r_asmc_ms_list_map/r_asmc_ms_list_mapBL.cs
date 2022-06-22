using Models.RealEstate.r_asmc_ms_list_map;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_asmc_ms_list_map
{
    public class r_asmc_ms_list_mapBL
    {
        public List<M_Pref_CityGP_City_Town> Get_Pref_CityGP_City_Town(string selected_cities, string selected_towns)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CityCDCsv", SqlDbType.VarChar){ Value = selected_cities.ToStringOrNull() },
                new SqlParameter("@TownCDCsv", SqlDbType.VarChar){ Value =  selected_towns.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_list_map_Select_Pref_CityGP_City_Town", sqlParams);

            return dt.AsEnumerableEntity<M_Pref_CityGP_City_Town>().ToList();
        }

        public Dictionary<string, string> ValidateAll(r_asmc_ms_list_mapModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckIsDoubleByte("MansionName", model.MansionName, 100);
            //E104,E105
            validator.CheckIsNumeric("StartYear", model.StartYear, 2, 0);
            validator.CheckIsNumeric("EndYear", model.EndYear, 2, 0);
            validator.CheckIsNumeric("StartDistance", model.StartDistance, 2, 0);
            validator.CheckIsNumeric("EndDistance", model.EndDistance, 2, 0);
            validator.CheckIsNumeric("StartRooms", model.StartRooms, 3, 0);
            validator.CheckIsNumeric("EndRooms", model.EndRooms, 3, 0);
            //E113
            validator.CheckCompareNum("EndYear", model.StartYear, model.EndYear);
            validator.CheckCompareNum("EndDistance", model.StartDistance, model.EndDistance);
            validator.CheckCompareNum("EndRooms", model.StartRooms, model.EndRooms);

            if (string.IsNullOrEmpty(model.MansionName) 
                && string.IsNullOrEmpty(model.CityCDList) && string.IsNullOrEmpty(model.TownCDList)
                && string.IsNullOrEmpty(model.StartYear) && string.IsNullOrEmpty(model.EndYear)
                && string.IsNullOrEmpty(model.StartDistance) && string.IsNullOrEmpty(model.EndDistance)
                && string.IsNullOrEmpty(model.StartRooms) && string.IsNullOrEmpty(model.EndRooms)
                && string.IsNullOrEmpty(model.Priority))
            {
                validator.AddValidationResult("btnDisplay", "E303");
            }
            return validator.GetValidationResult();
        }

        public DataTable GetMansionData(r_asmc_ms_list_mapModel model)
        {

            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() },
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value =  model.MansionName.ToStringOrNull() },
                new SqlParameter("@CityCDCsv", SqlDbType.VarChar){ Value =  model.CityCDList.ToStringOrNull() },
                new SqlParameter("@TownCDCsv", SqlDbType.VarChar){ Value =  model.TownCDList.ToStringOrNull() },
                new SqlParameter("@YearFrom", SqlDbType.Int){ Value =  model.StartYear },
                new SqlParameter("@YearTo", SqlDbType.Int){ Value = model.EndYear },
                new SqlParameter("@DistanceFrom", SqlDbType.Int){ Value = model.StartDistance },
                new SqlParameter("@DistanceTo", SqlDbType.Int){ Value = model.EndDistance },
                new SqlParameter("@RoomsFrom", SqlDbType.Int){ Value = model.StartRooms },
                new SqlParameter("@RoomsTo", SqlDbType.Int){ Value = model.EndRooms },
                new SqlParameter("@Unregistered", SqlDbType.TinyInt){ Value = model.Unregistered.ToByte(0) },
                new SqlParameter("@Priority", SqlDbType.TinyInt){ Value = model.Priority.ToByte(0) },
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_list_map_Select_MansionData", sqlParams);

            return dt;
        }

        public void Insert_L_Log(Models.RealEstate.RealEstate_L_Log_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0)},
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_RealEstate_Insert_L_Log", false, sqlParams);
        }

    }
}
