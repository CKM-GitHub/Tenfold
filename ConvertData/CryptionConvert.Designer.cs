namespace CryptionConvert
{
    partial class CryptionConvert
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMSeller = new System.Windows.Forms.Button();
            this.btnCryptionTest = new System.Windows.Forms.Button();
            this.txtResult1 = new System.Windows.Forms.TextBox();
            this.txtResult2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnMSeller
            // 
            this.btnMSeller.Font = new System.Drawing.Font("メイリオ", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMSeller.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMSeller.Location = new System.Drawing.Point(88, 85);
            this.btnMSeller.Name = "btnMSeller";
            this.btnMSeller.Size = new System.Drawing.Size(332, 60);
            this.btnMSeller.TabIndex = 6;
            this.btnMSeller.Text = "売主マスタ";
            this.btnMSeller.UseVisualStyleBackColor = true;
            this.btnMSeller.Click += new System.EventHandler(this.btnMSeller_Click);
            // 
            // btnCryptionTest
            // 
            this.btnCryptionTest.Font = new System.Drawing.Font("メイリオ", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCryptionTest.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCryptionTest.Location = new System.Drawing.Point(88, 205);
            this.btnCryptionTest.Name = "btnCryptionTest";
            this.btnCryptionTest.Size = new System.Drawing.Size(332, 60);
            this.btnCryptionTest.TabIndex = 7;
            this.btnCryptionTest.Text = "Cryptionテスト";
            this.btnCryptionTest.UseVisualStyleBackColor = true;
            this.btnCryptionTest.Click += new System.EventHandler(this.btnCryptionTest_Click);
            // 
            // txtResult1
            // 
            this.txtResult1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtResult1.Font = new System.Drawing.Font("メイリオ", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtResult1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtResult1.Location = new System.Drawing.Point(435, 91);
            this.txtResult1.Name = "txtResult1";
            this.txtResult1.ReadOnly = true;
            this.txtResult1.Size = new System.Drawing.Size(221, 41);
            this.txtResult1.TabIndex = 8;
            // 
            // txtResult2
            // 
            this.txtResult2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtResult2.Font = new System.Drawing.Font("メイリオ", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtResult2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtResult2.Location = new System.Drawing.Point(435, 211);
            this.txtResult2.Name = "txtResult2";
            this.txtResult2.ReadOnly = true;
            this.txtResult2.Size = new System.Drawing.Size(221, 41);
            this.txtResult2.TabIndex = 9;
            // 
            // CryptionConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtResult2);
            this.Controls.Add(this.txtResult1);
            this.Controls.Add(this.btnCryptionTest);
            this.Controls.Add(this.btnMSeller);
            this.Name = "CryptionConvert";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMSeller;
        private System.Windows.Forms.Button btnCryptionTest;
        private System.Windows.Forms.TextBox txtResult1;
        private System.Windows.Forms.TextBox txtResult2;
    }
}

