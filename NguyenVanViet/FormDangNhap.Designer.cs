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
            SuspendLayout();
            // 
            // txtPW
            // 
            txtPW.Location = new Point(444, 165);
            txtPW.Name = "txtPW";
            txtPW.Size = new Size(100, 23);
            txtPW.TabIndex = 15;
            // 
            // txtName
            // 
            txtName.Location = new Point(444, 100);
            txtName.Name = "txtName";
            txtName.Size = new Size(100, 23);
            txtName.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(302, 165);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 13;
            label3.Text = "Mật khẩu:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(302, 103);
            label1.Name = "label1";
            label1.Size = new Size(28, 15);
            label1.TabIndex = 12;
            label1.Text = "Tên:";
            // 
            // button1
            // 
            button1.Location = new Point(469, 247);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 18;
            button1.Text = "Đăng nhập";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(160, 247);
            label2.Name = "label2";
            label2.Size = new Size(129, 15);
            label2.TabIndex = 19;
            label2.Text = "Bạn chưa có tài khoản?";
            // 
            // button2
            // 
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = SystemColors.MenuHighlight;
            button2.Location = new Point(295, 243);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 20;
            button2.Text = "Đăng ký ngay";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // FormDangNhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(txtPW);
            Controls.Add(txtName);
            Controls.Add(label3);
            Controls.Add(label1);
            Name = "FormDangNhap";
            Text = "FormDangNhap";
            Load += FormDangNhap_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtPW;
        private TextBox txtName;
        private Label label3;
        private Label label1;
        private Button button1;
        private Label label2;
        private Button button2;
    }
}