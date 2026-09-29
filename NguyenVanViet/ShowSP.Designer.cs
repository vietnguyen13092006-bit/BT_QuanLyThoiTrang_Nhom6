namespace FORM_DKY
{
    partial class ShowSP
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picAnh = new PictureBox();
            lblTenSP = new Label();
            lblGiaBan = new Label();
            btnChiTiet = new Button();
            ((System.ComponentModel.ISupportInitialize)picAnh).BeginInit();
            SuspendLayout();
            // 
            // picAnh
            // 
            picAnh.BackColor = Color.FromArgb(224, 224, 224);
            picAnh.Location = new Point(22, 16);
            picAnh.Name = "picAnh";
            picAnh.Size = new Size(150, 150);
            picAnh.TabIndex = 0;
            picAnh.TabStop = false;
            // 
            // lblTenSP
            // 
            lblTenSP.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTenSP.AutoSize = true;
            lblTenSP.Location = new Point(22, 180);
            lblTenSP.Name = "lblTenSP";
            lblTenSP.Size = new Size(38, 15);
            lblTenSP.TabIndex = 1;
            lblTenSP.Text = "label1";
            // 
            // lblGiaBan
            // 
            lblGiaBan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblGiaBan.AutoSize = true;
            lblGiaBan.Location = new Point(22, 206);
            lblGiaBan.Name = "lblGiaBan";
            lblGiaBan.Size = new Size(38, 15);
            lblGiaBan.TabIndex = 2;
            lblGiaBan.Text = "label1";
            lblGiaBan.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnChiTiet
            // 
            btnChiTiet.Location = new Point(66, 232);
            btnChiTiet.Name = "btnChiTiet";
            btnChiTiet.Size = new Size(75, 23);
            btnChiTiet.TabIndex = 3;
            btnChiTiet.Text = "Chi tiết";
            btnChiTiet.UseVisualStyleBackColor = true;
            btnChiTiet.Click += btnChiTiet_Click;
            // 
            // ShowSP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(btnChiTiet);
            Controls.Add(lblGiaBan);
            Controls.Add(lblTenSP);
            Controls.Add(picAnh);
            Name = "ShowSP";
            Size = new Size(198, 258);
            ((System.ComponentModel.ISupportInitialize)picAnh).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picAnh;
        private Label lblTenSP;
        private Label lblGiaBan;
        private Button btnChiTiet;
    }
}
