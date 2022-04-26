using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace CryptionConvert
{
    public partial class CryptionConvert : Form
    {
        public CryptionConvert()
        {
            InitializeComponent();
            StaticCache.SetIniInfo();
        }

        private void btnMSeller_Click(object sender, EventArgs e)
        {
            txtResult1.Text = "処理中...";
            txtResult1.Update();

            var dt = GetSellerCDList().AsEnumerable();

            foreach(var row in dt)
            {
                var model = GetSellerData(row["SellerCD"].ToStringOrEmpty());
                model.Password = "password";
                var result = UpdateSellerData(model, out string msgid);
            }
            txtResult1.Text = "";
            MessageBox.Show("完了");
        }

        private DataTable GetSellerCDList()
        {
            DBAccess db = new DBAccess();
            return db.SelectDatatable("pr_CryptionConvert_M_Seller_Select_SellerCD_All");
        }

        private a_mypage_uinfoModel GetSellerData(string sellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_CryptionConvert_M_Seller_Select_by_SellerCD", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return new a_mypage_uinfoModel() { SellerCD = sellerCD };
            }

            AESCryption_Old crypt = new AESCryption_Old();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            var dr = dt.Rows[0];
            var model = new a_mypage_uinfoModel()
            {
                SellerCD = sellerCD,
                SellerName = crypt.DecryptFromBase64(dr["SellerName"].ToStringOrEmpty(), decryptionKey),
                SellerKana = crypt.DecryptFromBase64(dr["SellerKana"].ToStringOrEmpty(), decryptionKey),
                Birthday = crypt.DecryptFromBase64(dr["Birthday"].ToStringOrEmpty(), decryptionKey),
                ZipCode1 = dr["ZipCode1"].ToStringOrEmpty(),
                ZipCode2 = dr["ZipCode2"].ToStringOrEmpty(),
                PrefCD = dr["PrefCD"].ToStringOrEmpty(),
                PrefName = dr["PrefName"].ToStringOrEmpty(),
                CityName = crypt.DecryptFromBase64(dr["CityName"].ToStringOrEmpty(), decryptionKey),
                TownName = crypt.DecryptFromBase64(dr["TownName"].ToStringOrEmpty(), decryptionKey),
                Address1 = crypt.DecryptFromBase64(dr["Address1"].ToStringOrEmpty(), decryptionKey),
                Address2 = crypt.DecryptFromBase64(dr["Address2"].ToStringOrEmpty(), decryptionKey),
                HandyPhone = crypt.DecryptFromBase64(dr["HandyPhone"].ToStringOrEmpty(), decryptionKey),
                HousePhone = crypt.DecryptFromBase64(dr["HousePhone"].ToStringOrEmpty(), decryptionKey),
                Fax = crypt.DecryptFromBase64(dr["Fax"].ToStringOrEmpty(), decryptionKey),
                MailAddress = crypt.DecryptFromBase64(dr["MailAddress"].ToStringOrEmpty(), decryptionKey),
            };

            return model;
        }

        private bool UpdateSellerData(a_mypage_uinfoModel model, out string msgid)
        {
            msgid = "";

            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();

            PasswordHash pwhash = new PasswordHash();
            string hashedPassword = pwhash.GeneratePasswordHash(model.MailAddress, model.Password);

            //yyyy-MM-dd
            model.Birthday = model.Birthday.ToDateTime().ToDateString(DateTimeFormat.yyyy_MM_dd);

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.MailAddress, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = hashedPassword.ToStringOrNull() },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.SellerName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@SellerKana", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.SellerKana, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Birthday", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Birthday, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() },
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = model.PrefCD.ToStringOrNull() },
                new SqlParameter("@PrefName", SqlDbType.VarChar){ Value = model.PrefName.ToStringOrNull() },
                new SqlParameter("@CityName", SqlDbType.VarChar){ Value = model.CityName.ToStringOrNull() },
                new SqlParameter("@TownName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.TownName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Address1", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Address1, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Address2", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Address2, cryptionKey).ToStringOrNull() },
                new SqlParameter("@HandyPhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.HandyPhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@HousePhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.HousePhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Fax", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Fax, cryptionKey).ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                //return db.InsertUpdateDeleteData("pr_M_Seller_Update_CryptionConvert", false, sqlParams);
                return true;
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        private void btnCryptionTest_Click(object sender, EventArgs e)
        {
            txtResult2.Text = "処理中...";
            txtResult2.Update();

            AESCryption_Old crypt = new AESCryption_Old();
            PasswordHash pwhash = new PasswordHash();
            string key = StaticCache.GetDataCryptionKey();

            var data = crypt.EncryptToBase64("赤尾　早恵", key);
            var count = 1000;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            List<string> lst = new List<string>();
            for (int i = 0; i < count; i++) { lst.Add(data); }
            string sss = "";

            foreach (var item in lst)
            {
                sss = crypt.DecryptFromBase64(item, key);
            }

            sw.Stop();
            var sec1 = sw.ElapsedMilliseconds;

            sw.Restart();

            Parallel.ForEach(lst, item =>
            {
                sss = crypt.DecryptFromBase64(item, key);
                sss = crypt.DecryptFromBase64(item, key);
            });

            sw.Stop();
            var sec2 = sw.ElapsedMilliseconds;

            txtResult2.Text = "";
            MessageBox.Show("完了（" + count.ToString() + "件）\r\n" + sec1.ToString() + "ミリ秒\r\n" + sec2.ToString() + "ミリ秒", "", MessageBoxButtons.OK, MessageBoxIcon.Information);





            //var salt = crypt.GenerateRandomDataBase64(25);
            //string sha256;
            //var saltAndPwd = String.Concat(key, salt);
            //var buffer = System.Text.Encoding.UTF8.GetBytes(saltAndPwd);
            //using (var csp = new SHA256CryptoServiceProvider())
            //{
            //    var hash = csp.ComputeHash(buffer);
            //    sha256 = Convert.ToBase64String(hash);
            //}

            //string Rfc2898;
            //using (var deriveBytes = new Rfc2898DeriveBytes(
            //    key, System.Text.Encoding.UTF8.GetBytes(salt), 1, HashAlgorithmName.SHA256))
            //{
            //    var hash = deriveBytes.GetBytes(256 / 8);
            //    Rfc2898 = Convert.ToBase64String(hash);
            //}
            //MessageBox.Show("SHA256:" + sha256 + " RFC2898:" + Rfc2898);
        }
    }
}
