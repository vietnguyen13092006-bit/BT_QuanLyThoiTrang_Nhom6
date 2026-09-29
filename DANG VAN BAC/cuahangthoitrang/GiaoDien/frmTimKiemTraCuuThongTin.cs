using cuahangthoitrang;
using Microsoft.Data.SqlClient; // Nếu dự án báo lỗi dòng này, hãy đổi thành: using System.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangThoiTrang.GiaoDien
{
    public partial class frmTimKiemTraCuuThongTin : Form
    {
        public frmTimKiemTraCuuThongTin()
        {
            InitializeComponent();
        }

        private void frmTimKiemTraCuuThongTin_Load_1(object sender, EventArgs e)
        {
            // Thiết lập ngày mặc định (từ đầu tháng đến hôm nay)
            DateTime now = DateTime.Now;
            dtpTuNgay.Value = new DateTime(now.Year, now.Month, 1);
            dtpDenNgay.Value = now;

            // Load danh sách hóa đơn ban đầu
            LoadDanhSachHoaDon();
        }

        // --- HÀM TẢI DANH SÁCH HÓA ĐƠN ---
        private void LoadDanhSachHoaDon()
        {
            try
            {
                string tuKhoa = txtTuKhoa.Text.Trim();
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

                string query = @"SELECT h.MaHD AS [Mã Hóa Đơn], 
                                       h.NgayLap AS [Ngày Lập], 
                                       k.TenKH AS [Khách Hàng], 
                                       n.TenNV AS [Nhân Viên Lập], 
                                       h.TongTien AS [Tổng Tiền]
                                FROM HoaDon h
                                LEFT JOIN KhachHang k ON h.MaKH = k.MaKH
                                LEFT JOIN NhanVien n ON h.MaNV = n.MaNV
                                WHERE h.NgayLap BETWEEN @TuNgay AND @DenNgay
                                  AND (h.MaHD LIKE @TuKhoa OR k.TenKH LIKE @TuKhoa OR n.TenNV LIKE @TuKhoa)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@TuKhoa", "%" + tuKhoa + "%")
                };

                DataTable dt = DatabaseHelper.GetData(query, parameters);
                dgvKetQua.DataSource = dt;

                // Xóa dữ liệu bảng chi tiết
                dgvChiTiet.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- HÀM TẢI CHI TIẾT HÓA ĐƠN ---
        private void LoadChiTietHoaDon(string maHD)
        {
            try
            {
                string query = @"SELECT c.MaSP AS [Mã SP], 
                                       s.TenSP AS [Tên Sản Phẩm], 
                                       c.Size AS [Kích Thước], 
                                       c.SoLuong AS [Số Lượng], 
                                       c.DonGia AS [Đơn Giá], 
                                       c.ThanhTien AS [Thành Tiền]
                                FROM ChiTietHoaDon c
                                JOIN SanPham s ON c.MaSP = s.MaSP
                                WHERE c.MaHD = @MaHD";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaHD", maHD)
                };

                DataTable dtChiTiet = DatabaseHelper.GetData(query, parameters);
                dgvChiTiet.DataSource = dtChiTiet;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút Tìm Kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        // Nút Làm Mới
        private void button2_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            DateTime now = DateTime.Now;
            dtpTuNgay.Value = new DateTime(now.Year, now.Month, 1);
            dtpDenNgay.Value = now;

            LoadDanhSachHoaDon();
        }

        // Sự kiện click vào 1 dòng ở Bảng Hóa Đơn
        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvKetQua.Rows[e.RowIndex].Cells["Mã Hóa Đơn"].Value != null)
            {
                string maHD = dgvKetQua.Rows[e.RowIndex].Cells["Mã Hóa Đơn"].Value.ToString();
                LoadChiTietHoaDon(maHD);
            }
        }

        // Nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn thoát giao diện tra cứu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dtpTuNgay_ValueChanged(object sender, EventArgs e) { }
        private void dtpDenNgay_ValueChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblTuKhoa = new Label();
            txtTuKhoa = new TextBox();
            dtpTuNgay = new DateTimePicker();
            lblTuNgay = new Label();
            dtpDenNgay = new DateTimePicker();
            lblDenNgay = new Label();
            btnTimKiem = new Button();
            btnLamMoi = new Button();
            dgvKetQua = new DataGridView();
            lblBangChinh = new Label();
            pnlHeader = new Panel();
            lblBangChiTiet = new Label();
            dgvChiTiet = new DataGridView();
            btnThoat = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(131, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(519, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "TRA CỨU VÀ TÌM KIẾM THÔNG TIN CỬA HÀNG";
            // 
            // lblTuKhoa
            // 
            lblTuKhoa.AutoSize = true;
            lblTuKhoa.Location = new Point(3, 50);
            lblTuKhoa.Name = "lblTuKhoa";
            lblTuKhoa.Size = new Size(69, 20);
            lblTuKhoa.TabIndex = 1;
            lblTuKhoa.Text = "Từ khóa :";
            lblTuKhoa.Click += label2_Click;
            // 
            // txtTuKhoa
            // 
            txtTuKhoa.Location = new Point(75, 48);
            txtTuKhoa.Name = "txtTuKhoa";
            txtTuKhoa.Size = new Size(125, 27);
            txtTuKhoa.TabIndex = 2;
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Location = new Point(277, 50);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(140, 27);
            dtpTuNgay.TabIndex = 3;
            dtpTuNgay.ValueChanged += dtpTuNgay_ValueChanged;
            // 
            // lblTuNgay
            // 
            lblTuNgay.AutoSize = true;
            lblTuNgay.Location = new Point(206, 51);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.Size = new Size(65, 20);
            lblTuNgay.TabIndex = 4;
            lblTuNgay.Text = "Từ ngày:";
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Location = new Point(504, 50);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(140, 27);
            dtpDenNgay.TabIndex = 5;
            dtpDenNgay.ValueChanged += dtpDenNgay_ValueChanged;
            // 
            // lblDenNgay
            // 
            lblDenNgay.AutoSize = true;
            lblDenNgay.Location = new Point(423, 55);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.Size = new Size(75, 20);
            lblDenNgay.TabIndex = 6;
            lblDenNgay.Text = "Đến ngày:";
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.FromArgb(0, 122, 204);
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(54, 118);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(130, 37);
            btnTimKiem.TabIndex = 7;
            btnTimKiem.Text = "🔍 Tìm Kiếm";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.Olive;
            btnLamMoi.ForeColor = SystemColors.Info;
            btnLamMoi.Location = new Point(206, 118);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(122, 37);
            btnLamMoi.TabIndex = 8;
            btnLamMoi.Text = "🔄 Làm Mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += button2_Click;
            // 
            // dgvKetQua
            // 
            dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKetQua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKetQua.Location = new Point(0, 186);
            dgvKetQua.Name = "dgvKetQua";
            dgvKetQua.ReadOnly = true;
            dgvKetQua.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKetQua.RowHeadersWidth = 51;
            dgvKetQua.Size = new Size(746, 188);
            dgvKetQua.TabIndex = 9;
            dgvKetQua.CellClick += dgvKetQua_CellClick; // Đã thêm đăng ký sự kiện click dòng
            // 
            // lblBangChinh
            // 
            lblBangChinh.AutoSize = true;
            lblBangChinh.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBangChinh.Location = new Point(12, 161);
            lblBangChinh.Name = "lblBangChinh";
            lblBangChinh.Size = new Size(147, 20);
            lblBangChinh.TabIndex = 11;
            lblBangChinh.Text = "KẾT QUẢ TRA CỨU:";
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(btnTimKiem);
            pnlHeader.Controls.Add(btnLamMoi);
            pnlHeader.Controls.Add(lblTuKhoa);
            pnlHeader.Controls.Add(dtpDenNgay);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblDenNgay);
            pnlHeader.Controls.Add(txtTuKhoa);
            pnlHeader.Controls.Add(lblTuNgay);
            pnlHeader.Controls.Add(dtpTuNgay);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(756, 158);
            pnlHeader.TabIndex = 12;
            // 
            // lblBangChiTiet
            // 
            lblBangChiTiet.AutoSize = true;
            lblBangChiTiet.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBangChiTiet.Location = new Point(12, 377);
            lblBangChiTiet.Name = "lblBangChiTiet";
            lblBangChiTiet.Size = new Size(165, 20);
            lblBangChiTiet.TabIndex = 13;
            lblBangChiTiet.Text = "DANH SÁCH CHI TIẾT:";
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Location = new Point(0, 400);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.Size = new Size(746, 188);
            dgvChiTiet.TabIndex = 14;
            // 
            // btnThoat
            // 
            btnThoat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnThoat.Location = new Point(586, 609);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(130, 41);
            btnThoat.TabIndex = 15;
            btnThoat.Text = "🚪 Thoát";
            btnThoat.UseVisualStyleBackColor = true;
            btnThoat.Click += btnThoat_Click;
            // 
            // frmTimKiemTraCuuThongTin
            // 
            ClientSize = new Size(756, 662);
            Controls.Add(btnThoat);
            Controls.Add(dgvChiTiet);
            Controls.Add(lblBangChiTiet);
            Controls.Add(lblBangChinh);
            Controls.Add(dgvKetQua);
            Controls.Add(pnlHeader);
            Name = "frmTimKiemTraCuuThongTin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TRA CỨU VÀ TÌM KIẾM THÔNG TIN CỬA HÀNG";
            Load += frmTimKiemTraCuuThongTin_Load_1;
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private Label lblTitle;
        private Label lblTuKhoa;
        private TextBox txtTuKhoa;
        private Label lblTuNgay;
        private DateTimePicker dtpDenNgay;
        private Label lblDenNgay;
        private Button btnTimKiem;
        private Button btnLamMoi;
        private DataGridView dgvKetQua;
        private Label lblBangChinh;
        private Panel pnlHeader;
        private Label lblBangChiTiet;
        private DataGridView dgvChiTiet;
        private Button btnThoat;
        private DateTimePicker dtpTuNgay;
        #endregion
    }
}