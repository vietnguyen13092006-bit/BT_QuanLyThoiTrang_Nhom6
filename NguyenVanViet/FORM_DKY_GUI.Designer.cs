namespace FORM_DKY
{
    partial class FORM_DKY_GUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            btnSend = new Button();
            label3 = new Label();
            txtName = new TextBox();
            txtPW = new TextBox();
            label5 = new Label();
            txtEmail = new TextBox();
            TxtCP = new TextBox();
            label4 = new Label();
            lblSDT = new Label();
            txtSDT = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(151, 83);
            label1.Name = "label1";
            label1.Size = new Size(28, 15);
            label1.TabIndex = 0;
            label1.Text = "Tên:";
            label1.Click += label1_Click;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(355, 328);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 1;
            btnSend.Text = "SEND";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(151, 147);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 3;
            label3.Text = "Mật khẩu:";
            // 
            // txtName
            // 
            txtName.Location = new Point(293, 80);
            txtName.Name = "txtName";
            txtName.Size = new Size(100, 23);
            txtName.TabIndex = 5;
            // 
            // txtPW
            // 
            txtPW.Location = new Point(293, 147);
            txtPW.Name = "txtPW";
            txtPW.Size = new Size(100, 23);
            txtPW.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(532, 147);
            label5.Name = "label5";
            label5.Size = new Size(39, 15);
            label5.TabIndex = 10;
            label5.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(597, 139);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(100, 23);
            txtEmail.TabIndex = 11;
            // 
            // TxtCP
            // 
            TxtCP.Location = new Point(293, 219);
            TxtCP.Name = "TxtCP";
            TxtCP.Size = new Size(100, 23);
            TxtCP.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(151, 227);
            label4.Name = "label4";
            label4.Size = new Size(112, 15);
            label4.TabIndex = 9;
            label4.Text = "Xác nhận mật khẩu:";
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Location = new Point(532, 80);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(29, 15);
            lblSDT.TabIndex = 12;
            lblSDT.Text = "SDT:";
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(597, 75);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(100, 23);
            txtSDT.TabIndex = 13;
            // 
            // FORM_DKY_GUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtSDT);
            Controls.Add(lblSDT);
            Controls.Add(txtEmail);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(TxtCP);
            Controls.Add(txtPW);
            Controls.Add(txtName);
            Controls.Add(label3);
            Controls.Add(btnSend);
            Controls.Add(label1);
            Name = "FORM_DKY_GUI";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnSend;
        private Label label3;
        private TextBox txtName;
        private TextBox txtPW;
        private Label label5;
        private TextBox txtEmail;
        private TextBox TxtCP;
        private Label label4;
        private Label label2;
        private Label lblSDT;
        private TextBox txtSDT;
    }
}
