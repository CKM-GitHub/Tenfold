using Models.RealEstate.r_asmc_set_area_check_tab;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_asmc_set_area_check_tab
{
    public class r_asmc_set_area_check_tabBL
    {
        public M_RECondAreaSecModel GetM_RECondAreaSec(string realECD, string townCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
                new SqlParameter("@TownCD", SqlDbType.VarChar){ Value = townCD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_area_check_tab_Select_M_RECondAreaSec_by_TownCD", sqlParams);

            if (dt.Rows.Count == 0)
            {
                return new M_RECondAreaSecModel();
            }
            else
            {
                var model = dt.Rows[0].ToEntity<M_RECondAreaSecModel>();
                model = GetV_RECondAreaSec(model);
                return model;
            }
        }

        public M_RECondAreaRateModel GetM_RECondAreaRate(M_RECondAreaSecModel model)
        {
            M_RECondAreaRateModel result = new M_RECondAreaRateModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@ConditionSEQ", SqlDbType.Int){ Value = model.ConditionSEQ },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_area_check_tab_Select_M_RECondAreaRate_by_ConditionSEQ", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return result;
            }

            var e = dt.AsEnumerable();

            var max = (from r in e
                       group r by new { dummy = 1 } into g
                       select new
                       {
                           ColNo = g.Max(rr => rr.Field<int>("ColNo")),
                           RowNo = g.Max(rr => rr.Field<int>("RowNo"))
                       }).FirstOrDefault();

            for (int i = 0; i <= max.RowNo; i++)
            {
                var queryRow = e.Where(r => r.Field<int>("RowNo") == i);
                if (!queryRow.Any()) continue;

                string[] rowHeader = null;
                List<string> rowData = new List<string>();

                foreach (DataRow dr in queryRow)
                {
                    //ColHeader
                    if (i == max.RowNo)
                    {
                        if (dr["ColNo"].ToInt32(0) == max.ColNo)
                        {
                            result.ColHeader.Add(new string[] { dr["DistanceFrom"].ToInt32(0).ToString(), "～" });
                        }
                        else
                        {
                            result.ColHeader.Add(new string[] { dr["DistanceTo"].ToInt32(0).ToString(), "以内" });
                        }
                    }

                    //RowHeader
                    if (rowHeader == null)
                    {
                        rowHeader = new string[2];
                        if (i == max.RowNo)
                        {
                            rowHeader = new string[] { dr["AgeFrom"].ToInt32(0).ToString(), "～" };
                        }
                        else
                        {
                            rowHeader = new string[] { dr["AgeTo"].ToInt32(0).ToString(), "以内" };
                        }
                    }

                    //Rate
                    rowData.Add(dr["Rate"].ToDecimal(0).ToStringOrEmpty());
                }

                result.RowHeader.Add(rowHeader);
                result.RowData.Add(rowData);
            }

            return result;
        }

        public M_RECondAreaRentModel GetM_RECondAreaRent(M_RECondAreaSecModel model)
        {
            M_RECondAreaRentModel result = new M_RECondAreaRentModel();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@ConditionSEQ", SqlDbType.Int){ Value = model.ConditionSEQ },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_area_check_tab_Select_M_RECondAreaRent_by_ConditionSEQ", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return result;
            }

            var e = dt.AsEnumerable();

            var max = (from r in e
                       group r by new { dummy = 1 } into g
                       select new
                       {
                           ColNo = g.Max(rr => rr.Field<int>("ColNo")),
                           RowNo = g.Max(rr => rr.Field<int>("RowNo"))
                       }).FirstOrDefault();

            for (int i = 0; i <= max.RowNo; i++)
            {
                var queryRow = e.Where(r => r.Field<int>("RowNo") == i);
                if (!queryRow.Any()) continue;

                string[] rowHeader = null;
                List<string[]> rowData = new List<string[]>();

                foreach (DataRow dr in queryRow)
                {
                    //ColHeader
                    if (i == max.RowNo)
                    {
                        if (dr["ColNo"].ToInt32(0) == max.ColNo)
                        {
                            result.ColHeader.Add(new string[] { dr["DistanceFrom"].ToInt32(0).ToString(), "～" });
                        }
                        else
                        {
                            result.ColHeader.Add(new string[] { dr["DistanceTo"].ToInt32(0).ToString(), "以内" });
                        }
                    }

                    //RowHeader
                    if (rowHeader == null)
                    {
                        rowHeader = new string[2];
                        if (i == max.RowNo)
                        {
                            rowHeader = new string[] { dr["AgeFrom"].ToInt32(0).ToString(), "～" };
                        }
                        else
                        {
                            rowHeader = new string[] { dr["AgeTo"].ToInt32(0).ToString(), "以内" };
                        }
                    }

                    //Rate
                    decimal rentHigh = dr["RentHigh"].ToDecimal(0);
                    decimal rentLow = dr["RentLow"].ToDecimal(0);
                    rowData.Add(new string[] 
                    {
                        rentHigh == 0 ? "未設定" : rentHigh.ToString("###,###,##0"),
                        rentLow == 0 ? "未設定" : rentLow.ToString("###,###,##0")
                    });
                }

                result.RowHeader.Add(rowHeader);
                result.RowData.Add(rowData);
            }

            return result;
        }

        public List<M_RECondAreaOptRow> GetM_RECondAreaOpt(M_RECondAreaSecModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@ConditionSEQ", SqlDbType.Int){ Value = model.ConditionSEQ },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_area_check_tab_Select_M_RECondAreaOpt_by_ConditionSEQ", sqlParams);

            return dt.AsEnumerableEntity<M_RECondAreaOptRow>().ToList();
        }


        public M_RECondAreaSecModel GetV_RECondAreaSec(M_RECondAreaSecModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@TownCD", SqlDbType.VarChar){ Value = model.TownCD },
                new SqlParameter("@ConditionSEQ", SqlDbType.Int){ Value = model.ConditionSEQ },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_area_check_tab_Select_V_RECondAreaSec", sqlParams);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                model.REStaffName = dr["REStaffName"].ToStringOrEmpty();
                model.ExpDate = dr["ExpDate"].ToStringOrEmpty();
                model.Priority = dr["Priority"].ToInt32(0);
                model.Remark = dr["Remark"].ToStringOrEmpty();
            }

            return model;
        }
    }
}
