using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_dashboard;

namespace Seruichi.BL.RealEstate.r_dashboard
{
    public class r_dashboardBL
    {
        public DataTable GetREFaceImg(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() },
                new SqlParameter("@restaffCD ", SqlDbType.VarChar){ Value = model.REStaffCD.ToString() }
              
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectREFaceImage", sqlParams);
            return dt;
        }

        public DataTable GetREStaffName(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() },
                new SqlParameter("@restaffCD ", SqlDbType.VarChar){ Value = model.REStaffCD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectForREStaffName", sqlParams);
            return dt;
        }

        public DataTable GetREName(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }
                
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectForREName", sqlParams);
            return dt;
        }

        public DataTable GetCustomerInfo(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectForSendCustomerInfo", sqlParams);
            return dt;
        }

        public DataTable GetSpecificplan(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectForSpecificPlan", sqlParams);
            return dt;
        }

        public DataTable GetArea(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectCountFromV_RECondAreaSec", sqlParams);
            return dt;
        }

        public DataTable GetWay(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectCountFromV_RECondLineSta", sqlParams);
            return dt;
        }

        public DataTable GetApartment(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectCountFromV_RECondMan", sqlParams);
            return dt;
        }

        public DataTable GetOldestDate(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectForOldestDate", sqlParams);
            return dt;
        }

        public DataTable GetOldestDatecount(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@ConfDateTime", SqlDbType.DateTime){ Value = model.ConfDateTime }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectForOldestDatecount", sqlParams);
            return dt;
        }

        public DataTable GetNewRequestData(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectNumForNewRequest", sqlParams);
            return dt;
        }

        public DataTable GetNegotiationsData(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectNumForNegotiations", sqlParams);
            return dt;
        }

        public DataTable GetNumberOfCompletedData(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectNumForNumberOfCompleted", sqlParams);
            return dt;
        }

        public DataTable GetNumberOfDeclineData(r_dashboardModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@realECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_dashboard_SelectNumForNumberOfDecline", sqlParams);
            return dt;
        }
    }
}
