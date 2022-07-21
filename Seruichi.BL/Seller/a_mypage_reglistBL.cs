using Models.Seller;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Seller
{
   public class a_mypage_reglistBL
    {
        public DataTable get_displaydata(string sellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_reglist_Select_M_SellerMansion", sqlParams);


            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var dr = dt.Rows[0];
            return dt;
        }


        public void InsertD_AssReq(a_mypage_reglistModel model)
        {
            var sqlParams = new SqlParameter[]
             {
               // new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = null },
               // new SqlParameter("@ReqDateTime", SqlDbType.VarChar){ Value = model.ReqDateTime.ToStringOrNull() },
               // new SqlParameter("@LoginFLG", SqlDbType.VarChar){ Value = model.LoginFLG.ToStringOrNull() },
               // new SqlParameter("@AssKBN", SqlDbType.TinyInt){ Value = model.AssKBN.ToByte(0) },
               // new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
               // new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
               // new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID.ToStringOrNull() },
               // new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = model.MansionCD.ToStringOrNull() },
               // new SqlParameter("@MansionName", SqlDbType.VarChar){ Value = model.MansionName.ToStringOrNull() },
               // new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
               // new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() },
               // new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = model.PrefCD.ToStringOrNull() },
               // new SqlParameter("@PrefName", SqlDbType.VarChar){ Value = model.PrefName.ToStringOrNull() },
               // new SqlParameter("@CityCD", SqlDbType.VarChar){ Value = model.CityCD.ToStringOrNull() },
               // new SqlParameter("@CityName", SqlDbType.VarChar){ Value = model.CityName.ToStringOrNull() },
               // new SqlParameter("@TownCD", SqlDbType.VarChar){ Value = model.TownCD.ToStringOrNull() },
               // new SqlParameter("@TownName", SqlDbType.VarChar){ Value = model.TownName.ToStringOrNull() },
               // new SqlParameter("@Address", SqlDbType.VarChar){ Value = model.Address.ToStringOrNull() },
               // new SqlParameter("@StructuralKBN", SqlDbType.TinyInt){ Value = model.StructuralKBN.ToByte(0) },
               // new SqlParameter("@Floors", SqlDbType.Int){ Value = model.Floors.ToInt32(0) },
               // new SqlParameter("@ConstYYYYMM", SqlDbType.Int){ Value = model.ConstYYYYMM.Replace("/", "").ToInt32(0) },
               // new SqlParameter("@Rooms", SqlDbType.Int){ Value = model.Rooms.ToInt32(0) },
               // new SqlParameter("@LocationFloor", SqlDbType.Int){ Value = model.LocationFloor.ToInt32(0) },
               // new SqlParameter("@RoomNumber", SqlDbType.VarChar){ Value = model.RoomNumber.ToStringOrNull() },
               // new SqlParameter("@RoomArea", SqlDbType.Decimal){ Value = model.RoomArea.ToDecimal(0) },
               // new SqlParameter("@BalconyKBN", SqlDbType.TinyInt){ Value = model.BalconyKBN.ToByte(0) },
               // new SqlParameter("@BalconyArea", SqlDbType.Decimal){ Value = model.BalconyArea.ToDecimal(0) },
               // new SqlParameter("@Direction", SqlDbType.TinyInt){ Value = model.Direction.ToByte(0) },
               // new SqlParameter("@FloorType", SqlDbType.Int){ Value = model.FloorType.ToInt32(0) },
               
               // new SqlParameter("@BathKBN", SqlDbType.TinyInt){ Value = model.BathKBN.ToByte(0) },
               // new SqlParameter("@RightKBN", SqlDbType.TinyInt){ Value = model.RightKBN.ToByte(0) },
               // new SqlParameter("@CurrentKBN", SqlDbType.TinyInt){ Value = model.CurrentKBN.ToByte(0) },
               // new SqlParameter("@ManagementKBN", SqlDbType.TinyInt){ Value = model.ManagementKBN.ToByte(0) },
               // new SqlParameter("@RentFee", SqlDbType.Money){ Value = model.RentFee.ToDecimal(0) },
               // new SqlParameter("@ManagementFee", SqlDbType.Money){ Value = model.ManagementFee.ToDecimal(0) },
               // new SqlParameter("@RepairFee", SqlDbType.Money){ Value = model.RepairFee.ToDecimal(0) },
               // new SqlParameter("@ExtraFee", SqlDbType.Money){ Value = model.ExtraFee.ToDecimal(0) },
               // new SqlParameter("@PropertyTax", SqlDbType.Money){ Value = model.PropertyTax.ToDecimal(0) },
               // new SqlParameter("@DesiredTime", SqlDbType.TinyInt){ Value = model.DesiredTime.ToByte(0) },
               //new SqlParameter("@AuthenticateNo", SqlDbType.TinyInt){ Value = model.AuthenticateNo.ToStringOrEmpty()},
               // new SqlParameter("@AuthCIndexDateTime", SqlDbType.DateTime){ Value = model.AuthCIndexDateTime},
               // new SqlParameter("@AuthenticateDateTime", SqlDbType.DateTime){ Value = model.AuthenticateDateTime },
               // new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
               // new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
               // new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
               
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("", false, sqlParams);
        }



        public void InsertD_AssReqProgress(a_mypage_reglistModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID.ToStringOrNull() },
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },

             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("", false, sqlParams);
        }
        public void Insert_L_Log(a_mypage_reglistModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LogDateTime", SqlDbType.VarChar){ Value = model.LogDateTime },
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0) },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_mansion_list_Insert_L_Log", false, sqlParams);
        }
    }
}
