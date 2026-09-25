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
            SuspendLayout();
            // 
            // lblMail
            // 
            lblMail.AutoSize = true;
            lblMail.Location = new Point(133, 111);
            lblMail.Name = "lblMail";
            lblMail.Size = new Size(39, 15);
            lblMail.TabIndex = 0;
            lblMail.Text = "Email:";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(133, 65);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 15);
            lblName.TabIndex = 1;
            lblName.Text = "Name:";
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
            // Form_TTTKHOAN
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDangXuat);
            Controls.Add(lblName);
            Controls.Add(lblMail);
            Name = "Form_TTTKHOAN";
            Text = "Form_TTTKHOAN";
            Load += Form_TTTKHOAN_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMail;
        private Label lblName;
        private Button btnDangXuat;
    }
}