namespace FORM_DKY
{
    partial class Form_TrangChu
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
            components = new System.ComponentModel.Container();
            panelsidebar = new Panel();
            btnDangNhap = new Button();
            btn_TrangChu = new Button();
            btnMenu = new Button();
            panelchildform = new Panel();
            sidebarTimer = new System.Windows.Forms.Timer(components);
            pictureBox1 = new PictureBox();
            panelsidebar.SuspendLayout();
            panelchildform.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelsidebar
            // 
            panelsidebar.BackColor = Color.SlateGray;
            panelsidebar.Controls.Add(btnDangNhap);
            panelsidebar.Controls.Add(btn_TrangChu);
            panelsidebar.Controls.Add(btnMenu);
            panelsidebar.Dock = DockStyle.Left;
            panelsidebar.Location = new Point(0, 0);
            panelsidebar.MaximumSize = new Size(200, 0);
            panelsidebar.MinimumSize = new Size(50, 0);
            panelsidebar.Name = "panelsidebar";
            panelsidebar.Size = new Size(50, 450);
            panelsidebar.TabIndex = 7;
            // 
            // btnDangNhap
            // 
            btnDangNhap.Dock = DockStyle.Top;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDangNhap.Location = new Point(0, 46);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Padding = new Padding(15, 0, 0, 0);
            btnDangNhap.Size = new Size(50, 23);
            btnDangNhap.TabIndex = 3;
            btnDangNhap.UseVisualStyleBackColor = true;
            btnDangNhap.Click += btnDangNhap_Click;
            // 
            // btn_TrangChu
            // 
            btn_TrangChu.Dock = DockStyle.Top;
            btn_TrangChu.FlatAppearance.BorderSize = 0;
            btn_TrangChu.FlatStyle = FlatStyle.Flat;
            btn_TrangChu.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_TrangChu.Location = new Point(0, 23);
            btn_TrangChu.Name = "btn_TrangChu";
            btn_TrangChu.Padding = new Padding(15, 0, 0, 0);
            btn_TrangChu.Size = new Size(50, 23);
            btn_TrangChu.TabIndex = 1;
            btn_TrangChu.UseVisualStyleBackColor = true;
            btn_TrangChu.Click += btn_TrangChu_Click;
            // 
            // btnMenu
            // 
            btnMenu.Dock = DockStyle.Top;
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatStyle = FlatStyle.Flat;
            btnMenu.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnMenu.Location = new Point(0, 0);
            btnMenu.Name = "btnMenu";
            btnMenu.Padding = new Padding(15, 0, 0, 0);
            btnMenu.Size = new Size(50, 23);
            btnMenu.TabIndex = 0;
            btnMenu.Text = "≡";
            btnMenu.UseVisualStyleBackColor = true;
            btnMenu.Click += btnMenu_Click;
            // 
            // panelchildform
            // 
            panelchildform.BackColor = SystemColors.ButtonFace;
            panelchildform.Controls.Add(pictureBox1);
            panelchildform.Dock = DockStyle.Fill;
            panelchildform.Location = new Point(50, 0);
            panelchildform.Name = "panelchildform";
            panelchildform.Size = new Size(750, 450);
            panelchildform.TabIndex = 8;
            // 
            // sidebarTimer
            // 
            sidebarTimer.Enabled = true;
            sidebarTimer.Interval = 10;
            sidebarTimer.Tick += sidebarTimer_Tick;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(239, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(256, 79);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // Form_TrangChu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelchildform);
            Controls.Add(panelsidebar);
            Name = "Form_TrangChu";
            Text = "Form_TrangChu";
            Load += Form_Menu_Load;
            panelsidebar.ResumeLayout(false);
            panelchildform.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel panelsidebar;
        private Button btnMenu;
        private Button btn_TrangChu;
        private Button btnDangNhap;
        private Panel panelchildform;
        private System.Windows.Forms.Timer sidebarTimer;
        private PictureBox pictureBox1;
    }
}