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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FORM_DKY_GUI));
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
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20F);
            label1.Location = new Point(11, 22);
            label1.Name = "label1";
            label1.Size = new Size(63, 37);
            label1.TabIndex = 0;
            label1.Text = "Tên:";
            label1.Click += label1_Click;
            // 
            // btnSend
            // 
            btnSend.Font = new Font("Segoe UI", 20F);
            btnSend.Location = new Point(455, 244);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(107, 49);
            btnSend.TabIndex = 1;
            btnSend.Text = "SEND";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 20F);
            label3.Location = new Point(11, 86);
            label3.Name = "label3";
            label3.Size = new Size(134, 37);
            label3.TabIndex = 3;
            label3.Text = "Mật khẩu:";
            // 
            // txtName
            // 
            txtName.Location = new Point(80, 34);
            txtName.Name = "txtName";
            txtName.Size = new Size(363, 23);
            txtName.TabIndex = 5;
            // 
            // txtPW
            // 
            txtPW.Location = new Point(151, 100);
            txtPW.Name = "txtPW";
            txtPW.Size = new Size(292, 23);
            txtPW.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 20F);
            label5.Location = new Point(474, 86);
            label5.Name = "label5";
            label5.Size = new Size(88, 37);
            label5.TabIndex = 10;
            label5.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(568, 100);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(151, 23);
            txtEmail.TabIndex = 11;
            // 
            // TxtCP
            // 
            TxtCP.Location = new Point(259, 171);
            TxtCP.Name = "TxtCP";
            TxtCP.Size = new Size(184, 23);
            TxtCP.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 20F);
            label4.Location = new Point(11, 157);
            label4.Name = "label4";
            label4.Size = new Size(248, 37);
            label4.TabIndex = 9;
            label4.Text = "Xác nhận mật khẩu:";
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Font = new Font("Segoe UI", 20F);
            lblSDT.Location = new Point(478, 22);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(69, 37);
            lblSDT.TabIndex = 12;
            lblSDT.Text = "SDT:";
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(568, 33);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(151, 23);
            txtSDT.TabIndex = 13;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(txtSDT);
            panel1.Controls.Add(txtName);
            panel1.Controls.Add(lblSDT);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(btnSend);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtPW);
            panel1.Controls.Add(TxtCP);
            panel1.Location = new Point(12, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(758, 388);
            panel1.TabIndex = 14;
            // 
            // FORM_DKY_GUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Name = "FORM_DKY_GUI";
            Text = "Form1";
            Load += FORM_DKY_GUI_Load;
            Resize += Form_DKY_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
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
        private Panel panel1;
    }
}
