using cuahangthoitrang;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace cuahangthoitrang.GiaoDien
{
    public partial class frmTimKiemTraCuuThongTin : Form
    {
        // Biến lưu trữ chế độ đang tra cứu hiện tại (1: Hóa đơn, 2: Phiếu nhập, 3: Sản phẩm)
        private int loaiTraCuuHienTai = 1;

        public frmTimKiemTraCuuThongTin()
        {
            InitializeComponent();
        }

        private void frmTimKiemTraCuuThongTin_Load_1(object sender, EventArgs e)
        {
            // Mặc định chọn khoảng thời gian bao quát
            DateTime now = DateTime.Now;
            dtpTuNgay.Value = new DateTime(now.Year, 1, 1);
            dtpDenNgay.Value = now;

            // Load mặc định danh sách Hóa đơn
            LoadDanhSachHoaDon();
        }

        // ==========================================
        // 1. TÌM KIẾM THEO SẢN PHẨM
        // ==========================================
        private void btnTimSanPham_Click(object sender, EventArgs e)
        {
            LoadDanhSachSanPham();
        }

        private void LoadDanhSachSanPham()
        {
            try
            {
                loaiTraCuuHienTai = 3;
                string tuKhoaSP = txtMaSP.Text.Trim();

                string query = @"SELECT s.MaSP AS [Mã Sản Phẩm], 
                                       s.TenSP AS [Tên Sản Phẩm], 
                                       s.MauSac AS [Màu Sắc], 
                                       s.GiaBan AS [Giá Bán]
                                FROM SanPham s
                                WHERE s.MaSP LIKE @TuKhoa OR s.TenSP LIKE @TuKhoa OR s.MauSac LIKE @TuKhoa";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TuKhoa", "%" + tuKhoaSP + "%")
                };

                lblBangChinh.Text = "KẾT QUẢ TÌM KIẾM SẢN PHẨM:";
                lblBangChiTiet.Text = "CHI TIẾT TỒN KHO THEO SIZE:";

                DataTable dt = DatabaseHelper.GetData(query, parameters);
                dgvKetQua.DataSource = dt;
                dgvChiTiet.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 2. TÌM KIẾM THEO HÓA ĐƠN
        // ==========================================
        private void btnTimHoaDon_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        private void LoadDanhSachHoaDon()
        {
            try
            {
                loaiTraCuuHienTai = 1;
                string tuKhoaHD = txtMaHD.Text.Trim();
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

                string query = @"SELECT DISTINCT h.MaHD AS [Mã Hóa Đơn], 
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
                    new SqlParameter("@TuKhoa", "%" + tuKhoaHD + "%")
                };

                lblBangChinh.Text = "KẾT QUẢ TÌM KIẾM HÓA ĐƠN:";
                lblBangChiTiet.Text = "CHI TIẾT HÓA ĐƠN ĐƯỢC CHỌN:";

                DataTable dt = DatabaseHelper.GetData(query, parameters);
                dgvKetQua.DataSource = dt;
                dgvChiTiet.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 3. TÌM KIẾM THEO PHIẾU NHẬP
        // ==========================================
        private void btnTimPhieuNhap_Click(object sender, EventArgs e)
        {
            LoadDanhSachPhieuNhap();
        }

        private void LoadDanhSachPhieuNhap()
        {
            try
            {
                loaiTraCuuHienTai = 2;
                string tuKhoaPN = txtMaPN.Text.Trim();
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

                string query = @"SELECT DISTINCT p.MaPN AS [Mã Phiếu Nhập], 
                                       p.NgayNhap AS [Ngày Nhập], 
                                       ncc.TenNCC AS [Nhà Cung Cấp], 
                                       nv.TenNV AS [Nhân Viên Nhập], 
                                       p.TongTienNhap AS [Tổng Tiền Nhập]
                                FROM PhieuNhap p
                                LEFT JOIN NhaCungCap ncc ON p.MaNCC = ncc.MaNCC
                                LEFT JOIN NhanVien nv ON p.MaNV = nv.MaNV
                                WHERE p.NgayNhap BETWEEN @TuNgay AND @DenNgay
                                  AND (p.MaPN LIKE @TuKhoa OR ncc.TenNCC LIKE @TuKhoa OR nv.TenNV LIKE @TuKhoa)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@TuKhoa", "%" + tuKhoaPN + "%")
                };

                lblBangChinh.Text = "KẾT QUẢ TÌM KIẾM PHIẾU NHẬP:";
                lblBangChiTiet.Text = "CHI TIẾT PHIẾU NHẬP ĐƯỢC CHỌN:";

                DataTable dt = DatabaseHelper.GetData(query, parameters);
                dgvKetQua.DataSource = dt;
                dgvChiTiet.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // CLICK HÀNG Ở BẢNG TRÊN ĐỂ HỢP NHẤT HIỂN THỊ BẢNG DƯỚI
        // ==========================================
        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (loaiTraCuuHienTai == 1) // Hóa đơn
            {
                if (dgvKetQua.Rows[e.RowIndex].Cells["Mã Hóa Đơn"].Value != null)
                {
                    string maHD = dgvKetQua.Rows[e.RowIndex].Cells["Mã Hóa Đơn"].Value.ToString();
                    LoadChiTietHoaDon(maHD);
                }
            }
            else if (loaiTraCuuHienTai == 2) // Phiếu nhập
            {
                if (dgvKetQua.Rows[e.RowIndex].Cells["Mã Phiếu Nhập"].Value != null)
                {
                    string maPN = dgvKetQua.Rows[e.RowIndex].Cells["Mã Phiếu Nhập"].Value.ToString();
                    LoadChiTietPhieuNhap(maPN);
                }
            }
            else if (loaiTraCuuHienTai == 3) // Sản phẩm
            {
                if (dgvKetQua.Rows[e.RowIndex].Cells["Mã Sản Phẩm"].Value != null)
                {
                    string maSP = dgvKetQua.Rows[e.RowIndex].Cells["Mã Sản Phẩm"].Value.ToString();
                    LoadChiTietTonKho(maSP);
                }
            }
        }

        private void LoadChiTietHoaDon(string maHD)
        {
            string query = @"SELECT c.MaSP AS [Mã SP], s.TenSP AS [Tên Sản Phẩm], c.Size AS [Kích Thước], 
                                    c.SoLuong AS [Số Lượng], c.DonGia AS [Đơn Giá], c.ThanhTien AS [Thành Tiền]
                             FROM ChiTietHoaDon c
                             JOIN SanPham s ON c.MaSP = s.MaSP
                             WHERE c.MaHD = @MaHD";
            dgvChiTiet.DataSource = DatabaseHelper.GetData(query, new SqlParameter[] { new SqlParameter("@MaHD", maHD) });
        }

        private void LoadChiTietPhieuNhap(string maPN)
        {
            string query = @"SELECT c.MaSP AS [Mã SP], s.TenSP AS [Tên Sản Phẩm], c.Size AS [Kích Thước], 
                                    c.SoLuongNhap AS [Số Lượng Nhập], c.GiaNhap AS [Giá Nhập], c.ThanhTien AS [Thành Tiền]
                             FROM ChiTietPhieuNhap c
                             JOIN SanPham s ON c.MaSP = s.MaSP
                             WHERE c.MaPN = @MaPN";
            dgvChiTiet.DataSource = DatabaseHelper.GetData(query, new SqlParameter[] { new SqlParameter("@MaPN", maPN) });
        }

        private void LoadChiTietTonKho(string maSP)
        {
            string query = @"SELECT Size AS [Kích Thước], SoLuongTon AS [Số Lượng Tồn Kho]
                             FROM SizeTonKho
                             WHERE MaSP = @MaSP";
            dgvChiTiet.DataSource = DatabaseHelper.GetData(query, new SqlParameter[] { new SqlParameter("@MaSP", maSP) });
        }

        // Nút Làm Mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSP.Clear();
            txtMaHD.Clear();
            txtMaPN.Clear();
            DateTime now = DateTime.Now;
            dtpTuNgay.Value = new DateTime(now.Year, 1, 1);
            dtpDenNgay.Value = now;
            LoadDanhSachHoaDon();
        }

        // Nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Component Designer generated code
        private TableLayoutPanel tblMainLayout;
        private Panel pnlHeader;
        private Label lblTitle;

        private Label lblMaSP;
        private TextBox txtMaSP;
        private Button btnTimSanPham;

        private Label lblMaHD;
        private TextBox txtMaHD;
        private Button btnTimHoaDon;

        private Label lblMaPN;
        private TextBox txtMaPN;
        private Button btnTimPhieuNhap;

        private Label lblTuNgay;
        private DateTimePicker dtpTuNgay;
        private Label lblDenNgay;
        private DateTimePicker dtpDenNgay;

        private Button btnLamMoi;
        private Label lblBangChinh;
        private DataGridView dgvKetQua;
        private Label lblBangChiTiet;
        private DataGridView dgvChiTiet;
        private Panel pnlFooter;
        private Button btnThoat;

        private void InitializeComponent()
        {
            tblMainLayout = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblTitle = new Label();

            lblMaSP = new Label();
            txtMaSP = new TextBox();
            btnTimSanPham = new Button();

            lblMaHD = new Label();
            txtMaHD = new TextBox();
            btnTimHoaDon = new Button();

            lblMaPN = new Label();
            txtMaPN = new TextBox();
            btnTimPhieuNhap = new Button();

            lblTuNgay = new Label();
            dtpTuNgay = new DateTimePicker();
            lblDenNgay = new Label();
            dtpDenNgay = new DateTimePicker();
            btnLamMoi = new Button();

            lblBangChinh = new Label();
            dgvKetQua = new DataGridView();
            lblBangChiTiet = new Label();
            dgvChiTiet = new DataGridView();

            pnlFooter = new Panel();
            btnThoat = new Button();

            tblMainLayout.SuspendLayout();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();

            // 
            // tblMainLayout
            // 
            tblMainLayout.ColumnCount = 1;
            tblMainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMainLayout.Controls.Add(pnlHeader, 0, 0);
            tblMainLayout.Controls.Add(lblBangChinh, 0, 1);
            tblMainLayout.Controls.Add(dgvKetQua, 0, 2);
            tblMainLayout.Controls.Add(lblBangChiTiet, 0, 3);
            tblMainLayout.Controls.Add(dgvChiTiet, 0, 4);
            tblMainLayout.Controls.Add(pnlFooter, 0, 5);
            tblMainLayout.Dock = DockStyle.Fill;
            tblMainLayout.Location = new Point(0, 0);
            tblMainLayout.Name = "tblMainLayout";
            tblMainLayout.RowCount = 6;
            tblMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 135F));
            tblMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tblMainLayout.Size = new Size(1020, 700);

            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblTitle);

            pnlHeader.Controls.Add(lblMaSP);
            pnlHeader.Controls.Add(txtMaSP);
            pnlHeader.Controls.Add(btnTimSanPham);

            pnlHeader.Controls.Add(lblMaHD);
            pnlHeader.Controls.Add(txtMaHD);
            pnlHeader.Controls.Add(btnTimHoaDon);

            pnlHeader.Controls.Add(lblMaPN);
            pnlHeader.Controls.Add(txtMaPN);
            pnlHeader.Controls.Add(btnTimPhieuNhap);

            pnlHeader.Controls.Add(lblTuNgay);
            pnlHeader.Controls.Add(dtpTuNgay);
            pnlHeader.Controls.Add(lblDenNgay);
            pnlHeader.Controls.Add(dtpDenNgay);
            pnlHeader.Controls.Add(btnLamMoi);

            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(3, 3);
            pnlHeader.Name = "pnlHeader";

            // lblTitle
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkBlue;
            lblTitle.Location = new Point(270, 5);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(519, 31);
            lblTitle.Text = "TRA CỨU VÀ TÌM KIẾM THÔNG TIN CỬA HÀNG";

            // --- 1. SẢN PHẨM ---
            lblMaSP.AutoSize = true;
            lblMaSP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMaSP.Location = new Point(20, 48);
            lblMaSP.Text = "Sản phẩm:";

            txtMaSP.Location = new Point(110, 45);
            txtMaSP.Size = new Size(155, 27);
            txtMaSP.PlaceholderText = "Nhập mã/tên SP...";

            btnTimSanPham.BackColor = Color.FromArgb(0, 122, 204);
            btnTimSanPham.FlatStyle = FlatStyle.Flat;
            btnTimSanPham.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnTimSanPham.ForeColor = Color.White;
            btnTimSanPham.Location = new Point(272, 44);
            btnTimSanPham.Size = new Size(100, 29);
            btnTimSanPham.Text = "🔍 Tìm SP";
            btnTimSanPham.UseVisualStyleBackColor = false;
            btnTimSanPham.Click += btnTimSanPham_Click;

            // --- 2. HÓA ĐƠN ---
            lblMaHD.AutoSize = true;
            lblMaHD.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMaHD.Location = new Point(20, 90);
            lblMaHD.Text = "Hóa đơn:";

            txtMaHD.Location = new Point(110, 87);
            txtMaHD.Size = new Size(155, 27);
            txtMaHD.PlaceholderText = "Nhập mã HĐ/Tên KH...";

            btnTimHoaDon.BackColor = Color.FromArgb(0, 122, 204);
            btnTimHoaDon.FlatStyle = FlatStyle.Flat;
            btnTimHoaDon.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnTimHoaDon.ForeColor = Color.White;
            btnTimHoaDon.Location = new Point(272, 86);
            btnTimHoaDon.Size = new Size(100, 29);
            btnTimHoaDon.Text = "🔍 Tìm HĐ";
            btnTimHoaDon.UseVisualStyleBackColor = false;
            btnTimHoaDon.Click += btnTimHoaDon_Click;

            // --- 3. PHIẾU NHẬP ---
            lblMaPN.AutoSize = true;
            lblMaPN.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMaPN.Location = new Point(390, 48);
            lblMaPN.Text = "Phiếu nhập:";

            txtMaPN.Location = new Point(485, 45);
            txtMaPN.Size = new Size(175, 27);
            txtMaPN.PlaceholderText = "Nhập mã PN/Nhà CC...";

            btnTimPhieuNhap.BackColor = Color.FromArgb(0, 122, 204);
            btnTimPhieuNhap.FlatStyle = FlatStyle.Flat;
            btnTimPhieuNhap.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnTimPhieuNhap.ForeColor = Color.White;
            btnTimPhieuNhap.Location = new Point(667, 44);
            btnTimPhieuNhap.Size = new Size(100, 29);
            btnTimPhieuNhap.Text = "🔍 Tìm PN";
            btnTimPhieuNhap.UseVisualStyleBackColor = false;
            btnTimPhieuNhap.Click += btnTimPhieuNhap_Click;

            // --- 4. KHOẢNG NGÀY & LÀM MỚI ---
            lblTuNgay.AutoSize = true;
            lblTuNgay.Location = new Point(390, 90);
            lblTuNgay.Text = "Từ ngày:";

            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Location = new Point(450, 87);
            dtpTuNgay.Size = new Size(110, 27);

            lblDenNgay.AutoSize = true;
            lblDenNgay.Location = new Point(570, 90);
            lblDenNgay.Text = "Đến:";

            dtpDenNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Location = new Point(605, 87);
            dtpDenNgay.Size = new Size(110, 27);

            btnLamMoi.BackColor = Color.Gray;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(780, 84);
            btnLamMoi.Size = new Size(100, 33);
            btnLamMoi.Text = "🔄 Làm Mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;

            // --- BẢNG DỮ LIỆU ---
            lblBangChinh.Dock = DockStyle.Fill;
            lblBangChinh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBangChinh.Margin = new Padding(10, 0, 0, 0);

            dgvKetQua.AllowUserToAddRows = false;
            dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKetQua.Dock = DockStyle.Fill;
            dgvKetQua.Margin = new Padding(10, 3, 10, 3);
            dgvKetQua.ReadOnly = true;
            dgvKetQua.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKetQua.CellClick += dgvKetQua_CellClick;

            lblBangChiTiet.Dock = DockStyle.Fill;
            lblBangChiTiet.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBangChiTiet.Margin = new Padding(10, 0, 0, 0);

            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.Dock = DockStyle.Fill;
            dgvChiTiet.Margin = new Padding(10, 3, 10, 3);
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            pnlFooter.Controls.Add(btnThoat);
            pnlFooter.Dock = DockStyle.Fill;

            btnThoat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnThoat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThoat.Location = new Point(885, 5);
            btnThoat.Size = new Size(120, 35);
            btnThoat.Text = "🚪 Thoát";
            btnThoat.Click += btnThoat_Click;

            Controls.Add(tblMainLayout);
            MinimumSize = new Size(980, 680);
            Name = "frmTimKiemTraCuuThongTin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TRA CỨU VÀ TÌM KIẾM THÔNG TIN CỬA HÀNG";
            Load += frmTimKiemTraCuuThongTin_Load_1;

            tblMainLayout.ResumeLayout(false);
            tblMainLayout.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}