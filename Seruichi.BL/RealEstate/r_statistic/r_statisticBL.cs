﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Seruichi.Common;
using Models.RealEstate.r_statistic;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Seruichi.BL.RealEstate.r_statistic
{
    public class r_statisticBL
    {
        public Dictionary<string, string> ValidateAll(r_statisticModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();
            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            CheckDateDifference("StartDate", model.StartDate, model.EndDate, model.ryudo);

            validator.CheckCheckboxLenght("dac_route", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public void CheckDateDifference(string elementId, string fromdate, string todate, byte ryudo)
        {
            DateTime fdate = (DateTime)fromdate.ToDateTime();
            DateTime tdate = (DateTime)todate.ToDateTime();

        }


        public async Task<DataTable> get_r_statistic_displayData(r_statisticModel model)
        {
            return await Task.Run(() =>
            {
                AESCryption crypt = new AESCryption();
                string cryptionKey = StaticCache.GetDataCryptionKey();

                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@dac_route", SqlDbType.TinyInt){ Value = model.dac_route.ToByte(0) },
                    new SqlParameter("@dac_apartment", SqlDbType.TinyInt){ Value = model.dac_apartment.ToByte(0) },
                    new SqlParameter("@top5_route", SqlDbType.TinyInt){ Value = model.top5_route.ToByte(0) },
                    new SqlParameter("@top5_apartment", SqlDbType.TinyInt){ Value = model.top5_apartment.ToByte(0) },
                    new SqlParameter("@contracts_route", SqlDbType.TinyInt){ Value = model.contracts_route.ToByte(0) },
                    new SqlParameter("@contracts_apartment", SqlDbType.TinyInt){ Value = model.contracts_apartment.ToByte(0) },
                    new SqlParameter("@bd_route", SqlDbType.TinyInt){ Value = model.bd_route.ToByte(0) },
                    new SqlParameter("@bd_apartment", SqlDbType.TinyInt){ Value = model.bd_apartment.ToByte(0) },
                    new SqlParameter("@sd_route", SqlDbType.TinyInt){ Value = model.sd_route.ToByte(0) },
                    new SqlParameter("@sd_apartment", SqlDbType.TinyInt){ Value = model.sd_apartment.ToByte(0) },
                    new SqlParameter("@ryudo", SqlDbType.TinyInt){ Value = model.ryudo.ToByte(0) },
                    new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                    new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
                };

                DBAccess db = new DBAccess();
                var dt = db.SelectDatatable("pr_r_statistic_getDisplayData", sqlParams);
                return dt;
            });
        }
    }
}
