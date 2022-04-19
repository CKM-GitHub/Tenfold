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
            txtKey.Text = AESCryption.DefaultKey;
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
    }
}
