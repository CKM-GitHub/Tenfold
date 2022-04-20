using Seruichi.Common;
using System;
using System.Windows.Forms;

namespace Cryption
{
    public partial class Cryption : Form
    {
        private AESCryption crypt = new AESCryption();

        public Cryption()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtKey.Text = "";
            txtValue.Text = "";
            txtResult.Text = "";
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Text = crypt.EncryptToBase64(txtValue.Text, txtKey.Text);
            }
            catch (Exception ex)
            {
                txtResult.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Text = crypt.DecryptFromBase64(txtValue.Text, txtKey.Text);
            }
            catch (Exception ex)
            {
                txtResult.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtResult.Text);
        }

        private void btnKey1_Click(object sender, EventArgs e)
        {
            txtKey.Text = AESCryption.DefaultKey;
        }

        private void btnKey2_Click(object sender, EventArgs e)
        {
            txtKey.Text = AESCryption.DefaultKey2;
        }

        private void btnKey3_Click(object sender, EventArgs e)
        {
            ReadIni ini = new ReadIni();
            string encryptedDataCryptionKey = ini.GetDataCryptionKey();
            string cryptionKey = crypt.DecryptFromBase64(encryptedDataCryptionKey, AESCryption.DefaultKey);
            txtKey.Text = cryptionKey;
        }

        private void btnPasswordHash_Click(object sender, EventArgs e)
        {
            PasswordHash pwhash = new PasswordHash();
            txtResultPassword.Text = pwhash.GeneratePasswordHash(txtMailAddress.Text, txtPassword.Text);
        }

        private void btnCopyPassword_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtResultPassword.Text);
        }
    }
}
