namespace FORM_DKY
{
    partial class FrmPhieuNhap
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
            txtPN = new TextBox();
            label1 = new Label();
            panelTong = new Panel();
            label4 = new Label();
            btnXemLichSu = new Button();
            btnThemNhanhSP = new Button();
            label8 = new Label();
            txtMaNV = new TextBox();
            dgvPN = new DataGridView();
            btnLuuPhieu = new Button();
            btnThemSp = new Button();
            label7 = new Label();
            comboBox1 = new ComboBox();
            label6 = new Label();
            dateTimePicker1 = new DateTimePicker();
            txtThue = new TextBox();
            lblThue = new Label();
            txtSoluong = new TextBox();
            txtGiaNhap = new TextBox();
            txtMaSP = new TextBox();
            label5 = new Label();
            label3 = new Label();
            label2 = new Label();
            panelTong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPN).BeginInit();
            SuspendLayout();
            // 
            // txtPN
            // 
            txtPN.Anchor = AnchorStyles.Top;
            txtPN.Enabled = false;
            txtPN.Location = new Point(117, 167);
            txtPN.Name = "txtPN";
            txtPN.ReadOnly = true;
            txtPN.Size = new Size(193, 23);
            txtPN.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(68, 167);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 1;
            label1.Text = "Mã PN";
            // 
            // panelTong
            // 
            panelTong.AutoScroll = true;
            panelTong.AutoSize = true;
            panelTong.Controls.Add(label4);
            panelTong.Controls.Add(btnXemLichSu);
            panelTong.Controls.Add(btnThemNhanhSP);
            panelTong.Controls.Add(label8);
            panelTong.Controls.Add(txtMaNV);
            panelTong.Controls.Add(dgvPN);
            panelTong.Controls.Add(btnLuuPhieu);
            panelTong.Controls.Add(btnThemSp);
            panelTong.Controls.Add(label7);
            panelTong.Controls.Add(comboBox1);
            panelTong.Controls.Add(label6);
            panelTong.Controls.Add(dateTimePicker1);
            panelTong.Controls.Add(txtThue);
            panelTong.Controls.Add(lblThue);
            panelTong.Controls.Add(txtSoluong);
            panelTong.Controls.Add(txtGiaNhap);
            panelTong.Controls.Add(txtMaSP);
            panelTong.Controls.Add(label5);
            panelTong.Controls.Add(label3);
            panelTong.Controls.Add(label2);
            panelTong.Controls.Add(label1);
            panelTong.Controls.Add(txtPN);
            panelTong.Dock = DockStyle.Fill;
            panelTong.Location = new Point(0, 0);
            panelTong.Name = "panelTong";
            panelTong.Size = new Size(1021, 639);
            panelTong.TabIndex = 2;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 50F);
            label4.Location = new Point(321, 9);
            label4.Name = "label4";
            label4.Size = new Size(370, 89);
            label4.TabIndex = 25;
            label4.Text = "Phiếu nhập";
            // 
            // btnXemLichSu
            // 
            btnXemLichSu.Anchor = AnchorStyles.Top;
            btnXemLichSu.Location = new Point(786, 578);
            btnXemLichSu.Name = "btnXemLichSu";
            btnXemLichSu.Size = new Size(146, 46);
            btnXemLichSu.TabIndex = 24;
            btnXemLichSu.Text = "Xem phiếu nhập đã tạo";
            btnXemLichSu.UseVisualStyleBackColor = true;
            btnXemLichSu.Click += btnXemLichSu_Click;
            // 
            // btnThemNhanhSP
            // 
            btnThemNhanhSP.Anchor = AnchorStyles.Top;
            btnThemNhanhSP.Location = new Point(832, 298);
            btnThemNhanhSP.Name = "btnThemNhanhSP";
            btnThemNhanhSP.Size = new Size(100, 39);
            btnThemNhanhSP.TabIndex = 23;
            btnThemNhanhSP.Text = "Thêm SP mới";
            btnThemNhanhSP.UseVisualStyleBackColor = true;
            btnThemNhanhSP.Click += btnThemNhanhSP_Click;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top;
            label8.AutoSize = true;
            label8.Location = new Point(530, 310);
            label8.Name = "label8";
            label8.Size = new Size(79, 15);
            label8.TabIndex = 20;
            label8.Text = "Mã nhân viên";
            // 
            // txtMaNV
            // 
            txtMaNV.Anchor = AnchorStyles.Top;
            txtMaNV.Location = new Point(618, 307);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new Size(193, 23);
            txtMaNV.TabIndex = 19;
            // 
            // dgvPN
            // 
            dgvPN.Anchor = AnchorStyles.Top;
            dgvPN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPN.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvPN.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPN.Location = new Point(77, 343);
            dgvPN.Name = "dgvPN";
            dgvPN.Size = new Size(855, 229);
            dgvPN.TabIndex = 18;
            // 
            // btnLuuPhieu
            // 
            btnLuuPhieu.Anchor = AnchorStyles.Top;
            btnLuuPhieu.Location = new Point(627, 578);
            btnLuuPhieu.Name = "btnLuuPhieu";
            btnLuuPhieu.Size = new Size(119, 46);
            btnLuuPhieu.TabIndex = 17;
            btnLuuPhieu.Text = "Lưu hóa đơn";
            btnLuuPhieu.UseVisualStyleBackColor = true;
            btnLuuPhieu.Click += btnLuuPhieu_Click;
            // 
            // btnThemSp
            // 
            btnThemSp.Anchor = AnchorStyles.Top;
            btnThemSp.Location = new Point(82, 578);
            btnThemSp.Name = "btnThemSp";
            btnThemSp.Size = new Size(97, 46);
            btnThemSp.TabIndex = 16;
            btnThemSp.Text = "Thêm SP";
            btnThemSp.UseVisualStyleBackColor = true;
            btnThemSp.Click += btnThemSp_Click;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top;
            label7.AutoSize = true;
            label7.Location = new Point(344, 174);
            label7.Name = "label7";
            label7.Size = new Size(84, 15);
            label7.TabIndex = 15;
            label7.Text = "Loại sản phẩm";
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Top;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(434, 171);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(145, 23);
            comboBox1.TabIndex = 14;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top;
            label6.AutoSize = true;
            label6.Location = new Point(651, 232);
            label6.Name = "label6";
            label6.Size = new Size(65, 15);
            label6.TabIndex = 13;
            label6.Text = "Ngày nhập";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Anchor = AnchorStyles.Top;
            dateTimePicker1.Location = new Point(722, 228);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(210, 23);
            dateTimePicker1.TabIndex = 12;
            // 
            // txtThue
            // 
            txtThue.Anchor = AnchorStyles.Top;
            txtThue.Location = new Point(279, 307);
            txtThue.Name = "txtThue";
            txtThue.Size = new Size(179, 23);
            txtThue.TabIndex = 11;
            txtThue.TextChanged += txtThue_TextChanged;
            // 
            // lblThue
            // 
            lblThue.Anchor = AnchorStyles.Top;
            lblThue.AutoSize = true;
            lblThue.Location = new Point(219, 310);
            lblThue.Name = "lblThue";
            lblThue.Size = new Size(44, 15);
            lblThue.TabIndex = 10;
            lblThue.Text = "%Thuế";
            // 
            // txtSoluong
            // 
            txtSoluong.Anchor = AnchorStyles.Top;
            txtSoluong.Location = new Point(403, 228);
            txtSoluong.Name = "txtSoluong";
            txtSoluong.Size = new Size(179, 23);
            txtSoluong.TabIndex = 8;
            // 
            // txtGiaNhap
            // 
            txtGiaNhap.Anchor = AnchorStyles.Top;
            txtGiaNhap.Location = new Point(131, 228);
            txtGiaNhap.Name = "txtGiaNhap";
            txtGiaNhap.Size = new Size(179, 23);
            txtGiaNhap.TabIndex = 7;
            txtGiaNhap.Enter += txtGiaNhap_Enter;
            txtGiaNhap.Leave += txtGiaNhap_Leave;
            // 
            // txtMaSP
            // 
            txtMaSP.Anchor = AnchorStyles.Top;
            txtMaSP.Location = new Point(697, 168);
            txtMaSP.Name = "txtMaSP";
            txtMaSP.Size = new Size(235, 23);
            txtMaSP.TabIndex = 6;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Location = new Point(651, 170);
            label5.Name = "label5";
            label5.Size = new Size(40, 15);
            label5.TabIndex = 5;
            label5.Text = "Mã SP";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(71, 231);
            label3.Name = "label3";
            label3.Size = new Size(54, 15);
            label3.TabIndex = 3;
            label3.Text = "Giá nhập";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(343, 231);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 2;
            label2.Text = "Số lượng";
            // 
            // FrmPhieuNhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1021, 639);
            Controls.Add(panelTong);
            Name = "FrmPhieuNhap";
            Text = "FrmPhieuNhap";
            Load += FrmPhieuNhap_Load;
            panelTong.ResumeLayout(false);
            panelTong.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPN).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPN;
        private Label label1;
        private Panel panelTong;
        private Label label3;
        private Label label2;
        private Label label5;
        private TextBox txtSoluong;
        private TextBox txtGiaNhap;
        private TextBox txtMaSP;
        private Label lblThue;
        private TextBox txtThue;
        private Label label6;
        private DateTimePicker dateTimePicker1;
        private Label label7;
        private ComboBox comboBox1;
        private Button btnLuuPhieu;
        private Button btnThemSp;
        private DataGridView dgvPN;
        private Label label8;
        private TextBox txtMaNV;
        private Button btnThemNhanhSP;
        private Button btnXemLichSu;
        private Label label4;
    }
}