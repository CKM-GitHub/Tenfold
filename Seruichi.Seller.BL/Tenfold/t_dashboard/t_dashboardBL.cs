using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_dashboard
{
    public class t_dashboardBL
    {
        public DataTable GetCustomerInformationWaitingCount()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_dashboard_Select_AssReqProgressForCustomerInfo", sqlParams);
            return dt;
        }
        public DataTable GetChatConfirmationWaitingCount()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_dashboard_Select_ConsultationForChatConfirmation", sqlParams);
            return dt;
        }
        public DataTable GetNewRequestCasesCount()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_dashboard_Select_AssReqProgressForNewRequest", sqlParams);
            return dt;
        }
        public DataTable GetDuringnegotiationsCasesCount()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_dashboard_Select_AssReqProgressForDuringnegotiations", sqlParams);
            return dt;
        }
        public DataTable GetContractCasesCount()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_dashboard_Select_AssReqProgressForContract", sqlParams);
            return dt;
        }
        public DataTable GetDeclineCasesCount()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_dashboard_Select_AssReqProgressForDecline", sqlParams);
            return dt;
        }
    }
}
