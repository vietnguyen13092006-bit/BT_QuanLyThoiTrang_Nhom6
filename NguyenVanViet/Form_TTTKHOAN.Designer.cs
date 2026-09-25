namespace FORM_DKY
{
    partial class Form_TTTKHOAN
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblMail = new Label();
            lblName = new Label();
            btnDangXuat = new Button();
            lblSDT = new Label();
            picAVT = new PictureBox();
            btnAVT = new Button();
            ((System.ComponentModel.ISupportInitialize)picAVT).BeginInit();
            SuspendLayout();
            // 
            // lblMail
            // 
            lblMail.AutoSize = true;
            lblMail.Location = new Point(136, 179);
            lblMail.Name = "lblMail";
            lblMail.Size = new Size(36, 15);
            lblMail.TabIndex = 0;
            lblMail.Text = "Email";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(133, 65);
            lblName.Name = "lblName";
            lblName.Size = new Size(25, 15);
            lblName.TabIndex = 1;
            lblName.Text = "Tên";
            // 
            // btnDangXuat
            // 
            btnDangXuat.Location = new Point(349, 286);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(75, 23);
            btnDangXuat.TabIndex = 2;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = true;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Location = new Point(133, 118);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(26, 15);
            lblSDT.TabIndex = 3;
            lblSDT.Text = "SDT";
            // 
            // picAVT
            // 
            picAVT.Location = new Point(508, 12);
            picAVT.Name = "picAVT";
            picAVT.Size = new Size(250, 250);
            picAVT.SizeMode = PictureBoxSizeMode.StretchImage;
            picAVT.TabIndex = 4;
            picAVT.TabStop = false;
            // 
            // btnAVT
            // 
            btnAVT.Location = new Point(604, 299);
            btnAVT.Name = "btnAVT";
            btnAVT.Size = new Size(75, 23);
            btnAVT.TabIndex = 5;
            btnAVT.Text = "Đổi Avatar";
            btnAVT.UseVisualStyleBackColor = true;
            btnAVT.Click += btnAVT_Click;
            // 
            // Form_TTTKHOAN
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnAVT);
            Controls.Add(picAVT);
            Controls.Add(lblSDT);
            Controls.Add(btnDangXuat);
            Controls.Add(lblName);
            Controls.Add(lblMail);
            Name = "Form_TTTKHOAN";
            Text = "Form_TTTKHOAN";
            Load += Form_TTTKHOAN_Load;
            ((System.ComponentModel.ISupportInitialize)picAVT).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMail;
        private Label lblName;
        private Button btnDangXuat;
        private Label lblSDT;
        private PictureBox picAVT;
        private Button btnAVT;
    }
}