using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Seller;
namespace Seruichi.BL.Seller
{
    public class a_mypage_ahisBL
    {

        public DataTable GetD_AssReqProgressList(a_mypage_ahisModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_ahis_Select_D_AssReqProgress", sqlParams);
            return dt;


        }
    }


}
