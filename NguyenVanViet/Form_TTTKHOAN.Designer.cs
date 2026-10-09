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
            btnAVT = new Button();
            picAVT = new PictureBox();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picAVT).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblMail
            // 
            lblMail.AutoSize = true;
            lblMail.BackColor = Color.Transparent;
            lblMail.Font = new Font("Segoe UI", 35F);
            lblMail.Location = new Point(487, 263);
            lblMail.Name = "lblMail";
            lblMail.Size = new Size(137, 62);
            lblMail.TabIndex = 0;
            lblMail.Text = "Email";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.BackColor = Color.Transparent;
            lblName.Font = new Font("Segoe UI", 35F);
            lblName.Location = new Point(487, 26);
            lblName.Name = "lblName";
            lblName.Size = new Size(99, 62);
            lblName.TabIndex = 1;
            lblName.Text = "Tên";
            // 
            // btnDangXuat
            // 
            btnDangXuat.Dock = DockStyle.Bottom;
            btnDangXuat.Location = new Point(0, 570);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(968, 48);
            btnDangXuat.TabIndex = 2;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = true;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.BackColor = Color.Transparent;
            lblSDT.Font = new Font("Segoe UI", 35F);
            lblSDT.Location = new Point(487, 151);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(108, 62);
            lblSDT.TabIndex = 3;
            lblSDT.Text = "SDT";
            // 
            // btnAVT
            // 
            btnAVT.Location = new Point(179, 483);
            btnAVT.Name = "btnAVT";
            btnAVT.Size = new Size(90, 39);
            btnAVT.TabIndex = 5;
            btnAVT.Text = "Đổi Avatar";
            btnAVT.UseVisualStyleBackColor = true;
            btnAVT.Click += btnAVT_Click;
            // 
            // picAVT
            // 
            picAVT.Location = new Point(47, 36);
            picAVT.Name = "picAVT";
            picAVT.Size = new Size(362, 441);
            picAVT.SizeMode = PictureBoxSizeMode.StretchImage;
            picAVT.TabIndex = 4;
            picAVT.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(btnAVT);
            panel1.Controls.Add(picAVT);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(445, 537);
            panel1.TabIndex = 8;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = Properties.Resources.NenTT;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(968, 618);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // Form_TTTKHOAN
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(968, 618);
            Controls.Add(panel1);
            Controls.Add(lblSDT);
            Controls.Add(lblName);
            Controls.Add(lblMail);
            Controls.Add(btnDangXuat);
            Controls.Add(pictureBox1);
            Name = "Form_TTTKHOAN";
            Text = "Form_TTTKHOAN";
            Load += Form_TTTKHOAN_Load;
            ((System.ComponentModel.ISupportInitialize)picAVT).EndInit();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMail;
        private Label lblName;
        private Button btnDangXuat;
        private Label lblSDT;
        private Button btnAVT;
        private PictureBox picAVT;
        private Panel panel1;
        private PictureBox pictureBox1;
    }
}