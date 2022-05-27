using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.RealEstate.r_staff;
using Models;

namespace Seruichi.BL.RealEstate.r_staff
{
   public class r_staffBL
    {
        public DataTable Get_M_REStaff_By_RealECD_IsAdmin(string RealECD,string REStaffCD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){Value = REStaffCD },
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_staff_Select_M_REStaff_By_RealECD_IsAdmin", sqlParams);
            return dt;
        }
        
       public bool Get_select_M_REStaff(string RealECD, string REStaffCD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){Value = REStaffCD },
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_login_Select_M_REStaff_by_RealECD_and_REStaffCD", sqlParams);
            if(dt.Rows.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
