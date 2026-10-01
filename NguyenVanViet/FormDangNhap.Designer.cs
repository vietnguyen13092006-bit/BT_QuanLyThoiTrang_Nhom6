namespace FORM_DKY
{
    partial class FormDangNhap
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
            txtPW = new TextBox();
            txtName = new TextBox();
            label3 = new Label();
            label1 = new Label();
            button1 = new Button();
            label2 = new Label();
            button2 = new Button();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // txtPW
            // 
            txtPW.Location = new Point(490, 243);
            txtPW.Name = "txtPW";
            txtPW.Size = new Size(275, 23);
            txtPW.TabIndex = 15;
            // 
            // txtName
            // 
            txtName.Location = new Point(401, 141);
            txtName.Name = "txtName";
            txtName.Size = new Size(364, 23);
            txtName.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 25F);
            label3.Location = new Point(316, 222);
            label3.Name = "label3";
            label3.Size = new Size(168, 46);
            label3.TabIndex = 13;
            label3.Text = "Mật khẩu:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 25F);
            label1.Location = new Point(316, 120);
            label1.Name = "label1";
            label1.Size = new Size(79, 46);
            label1.TabIndex = 12;
            label1.Text = "Tên:";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15F);
            button1.Location = new Point(634, 348);
            button1.Name = "button1";
            button1.Size = new Size(131, 49);
            button1.TabIndex = 18;
            button1.Text = "Đăng nhập";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 25F);
            label2.Location = new Point(32, 447);
            label2.Name = "label2";
            label2.Size = new Size(363, 46);
            label2.TabIndex = 19;
            label2.Text = "Bạn chưa có tài khoản?";
            // 
            // button2
            // 
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 20F);
            button2.ForeColor = SystemColors.MenuHighlight;
            button2.Location = new Point(401, 447);
            button2.Name = "button2";
            button2.Size = new Size(194, 45);
            button2.TabIndex = 20;
            button2.Text = "Đăng ký ngay";
            button2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtName);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(txtPW);
            panel1.Location = new Point(29, 12);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 180, 0, 0);
            panel1.Size = new Size(892, 562);
            panel1.TabIndex = 21;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = Properties.Resources.NenDangNhap;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1059, 641);
            pictureBox1.TabIndex = 22;
            pictureBox1.TabStop = false;
            // 
            // FormDangNhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1059, 641);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Name = "FormDangNhap";
            Text = "FormDangNhap";
            Load += FormDangNhap_Load;
            Resize += FormDangNhap_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TextBox txtPW;
        private TextBox txtName;
        private Label label3;
        private Label label1;
        private Button button1;
        private Label label2;
        private Button button2;
        private Panel panel1;
        private PictureBox pictureBox1;
    }
}