using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangThoiTrang.GiaoDien
{
    public partial class frmTimKiemTraCuuThongTin : Form
    {
        // Khai báo các Control giao diện (Kèm kiểu Nullable ? để tránh lỗi CS0115 và Nullable Warning)
        private Label? lblTitle, lblTuKhoa, lblTuNgay, lblDenNgay, lblBangChinh, lblBangChiTiet;
        private TextBox? txtTuKhoa;
        private DateTimePicker? dtpTuNgay, dtpDenNgay;
        private Button? btnTimKiem, btnLamMoi, btnThoat;
        private DataGridView? dgvKetQua, dgvChiTiet;
        private Panel? pnlHeader, pnlBottom;

        public frmTimKiemTraCuuThongTin()
        {
            this.Text = "Hệ Thống Tra Cứu & Tìm Kiếm Thông Tin (Chế Độ Test Demo)";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            // Tự dựng giao diện bằng Pure Code C#
            InitializePureCodeComponents();
        }

        private void InitializePureCodeComponents()
        {
            // 1. Tiêu đề
            lblTitle = new Label
            {
                Text = "TRA CỨU VÀ TÌM KIẾM THÔNG TIN CỬA HÀNG THỜI TRANG",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.Navy,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 2. Khung Tìm Kiếm Top Panel
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = Color.FromArgb(240, 243, 246),
                Padding = new Padding(10)
            };

            lblTuKhoa = new Label { Text = "Từ khóa tìm kiếm:", Location = new Point(20, 20), AutoSize = true };
            txtTuKhoa = new TextBox { Location = new Point(140, 16), Width = 200 };

            lblTuNgay = new Label { Text = "Từ ngày:", Location = new Point(360, 20), AutoSize = true };
            dtpTuNgay = new DateTimePicker { Location = new Point(430, 16), Width = 130, Format = DateTimePickerFormat.Short };

            lblDenNgay = new Label { Text = "Đến ngày:", Location = new Point(580, 20), AutoSize = true };
            dtpDenNgay = new DateTimePicker { Location = new Point(660, 16), Width = 130, Format = DateTimePickerFormat.Short };

            btnTimKiem = new Button
            {
                Text = "🔍 Tìm Kiếm",
                Location = new Point(140, 58),
                Width = 120,
                Height = 35,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTimKiem.Click += BtnTimKiem_Click;

            btnLamMoi = new Button
            {
                Text = "🔄 Làm Mới",
                Location = new Point(270, 58),
                Width = 100,
                Height = 35,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLamMoi.Click += BtnLamMoi_Click;

            pnlHeader.Controls.AddRange(new Control[] {
                lblTuKhoa, txtTuKhoa, lblTuNgay, dtpTuNgay, lblDenNgay, dtpDenNgay, btnTimKiem, btnLamMoi
            });

            // 3. Bảng Kết Quả Chính (dgvKetQua)
            lblBangChinh = new Label
            {
                Text = "KẾT QUẢ TRA CỨU CHÍNH:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(20, 160),
                AutoSize = true
            };

            dgvKetQua = new DataGridView
            {
                Location = new Point(20, 185),
                Size = new Size(1040, 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvKetQua.CellClick += DgvKetQua_CellClick;
            dgvKetQua.CellValueChanged += DgvKetQua_CellValueChanged;

            // 4. Bảng Chi Tiết Mặt Hàng (dgvChiTiet)
            lblBangChiTiet = new Label
            {
                Text = "CHI TIẾT MẶT HÀNG (HÓA ĐƠN / PHIẾU NHẬP):",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(20, 415),
                AutoSize = true,
                Visible = false
            };

            dgvChiTiet = new DataGridView
            {
                Location = new Point(20, 440),
                Size = new Size(1040, 170),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Visible = false
            };

            // 5. Bottom Panel Nút Thoát
            pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 45 };
            btnThoat = new Button
            {
                Text = "🚪 Thoát",
                Size = new Size(90, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(970, 5)
            };
            btnThoat.Click += (s, e) => this.Close();
            pnlBottom.Controls.Add(btnThoat);

            // Add các Control vào Form
            this.Controls.Add(dgvChiTiet);
            this.Controls.Add(lblBangChiTiet);
            this.Controls.Add(dgvKetQua);
            this.Controls.Add(lblBangChinh);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblTitle);
            this.Controls.Add(pnlBottom);

            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;
        }

        // ==========================================
        // LOGIC TRA CỨU & DỮ LIỆU GIẢ (MOCK DATA)
        // ==========================================

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa?.Text.Trim().ToLower() ?? "";

            // 1. Nhận diện Tra cứu Hóa Đơn (Hỗ trợ gõ: hd, hoa don, hóa đơn, hoá đơn)
            if (tuKhoa.StartsWith("hd") ||
                tuKhoa.Contains("hoa don") ||
                tuKhoa.Contains("hóa đơn") ||
                tuKhoa.Contains("hoá đơn"))
            {
                HienThiBangChiTiet(true);
                LoadMockDataHoaDon();
            }
            // 2. Nhận diện Tra cứu Phiếu Nhập (Hỗ trợ gõ: pn, phieu nhap, phiếu nhập, phIếu nhập)
            else if (tuKhoa.StartsWith("pn") ||
                     tuKhoa.Contains("phieu nhap") ||
                     tuKhoa.Contains("phiếu nhập") ||
                     tuKhoa.Contains("phiếu nhập"))
            {
                HienThiBangChiTiet(true);
                LoadMockDataPhieuNhap();
            }
            // 3. Tra cứu Sản Phẩm / Tồn Kho (Mặc định)
            else
            {
                HienThiBangChiTiet(false);
                LoadMockDataSanPham();
            }
        }

        private void HienThiBangChiTiet(bool visible)
        {
            if (lblBangChiTiet != null) lblBangChiTiet.Visible = visible;
            if (dgvChiTiet != null) dgvChiTiet.Visible = visible;
            if (dgvKetQua != null) dgvKetQua.Height = visible ? 220 : 390;
        }

        // --- 1. MOCK DATA HÓA ĐƠN ---
        private void LoadMockDataHoaDon()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã Hóa Đơn");
            dt.Columns.Add("Ngày Lập");
            dt.Columns.Add("Khách Hàng");
            dt.Columns.Add("Nhân Viên Bán");
            dt.Columns.Add("Tổng Tiền");

            dt.Rows.Add("HD001", "25/09/2026", "Nguyễn Văn A", "Trần Thị B", "850,000 VNĐ");
            dt.Rows.Add("HD002", "26/09/2026", "Lê Thị C", "Trần Thị B", "1,200,000 VNĐ");

            if (dgvKetQua != null)
            {
                dgvKetQua.DataSource = null;
                dgvKetQua.Columns.Clear();
                dgvKetQua.DataSource = dt;
            }
        }

        // --- 2. MOCK DATA PHIẾU NHẬP ---
        private void LoadMockDataPhieuNhap()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã Phiếu Nhập");
            dt.Columns.Add("Ngày Nhập");
            dt.Columns.Add("Nhà Cung Cấp");
            dt.Columns.Add("Nhân Viên Nhập");
            dt.Columns.Add("Tổng Tiền Nhập");

            dt.Rows.Add("PN001", "20/09/2026", "Công ty May Mặc Việt Tiến", "Nguyễn Văn A", "15,000,000 VNĐ");
            dt.Rows.Add("PN002", "22/09/2026", "Xưởng Thời Trang PT2000", "Trần Thị B", "8,500,000 VNĐ");

            if (dgvKetQua != null)
            {
                dgvKetQua.DataSource = null;
                dgvKetQua.Columns.Clear();
                dgvKetQua.DataSource = dt;
            }
        }

        // --- 3. MOCK DATA SẢN PHẨM & TỒN KHO ---
        private void LoadMockDataSanPham()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã SP");
            dt.Columns.Add("Tên Sản Phẩm");
            dt.Columns.Add("Màu Sắc");
            dt.Columns.Add("Giá Bán");

            dt.Rows.Add("SP01", "Áo sơ mi Nam", "Trắng", "250,000 VNĐ");
            dt.Rows.Add("SP02", "Quần Jeans Nữ", "Xanh", "450,000 VNĐ");

            if (dgvKetQua != null)
            {
                dgvKetQua.DataSource = null;
                dgvKetQua.Columns.Clear();
                dgvKetQua.DataSource = dt;

                // CỘT Ô SỔ XUỐNG CHỌN SIZE
                DataGridViewComboBoxColumn cboSize = new DataGridViewComboBoxColumn();
                cboSize.HeaderText = "Chọn Size";
                cboSize.Name = "colSize";
                cboSize.Items.AddRange("S", "M", "L", "XL");
                dgvKetQua.Columns.Add(cboSize);

                // CỘT HIỂN THỊ SỐ LƯỢNG TỒN
                dgvKetQua.Columns.Add("colTonKho", "Số Lượng Tồn");
            }
        }

        // --- SỰ KIỆN CLICK VÀO DÒNG ĐỂ XEM CHI TIẾT ---
        private void DgvKetQua_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dgvKetQua == null || e.RowIndex < 0) return;

            // Click dòng Hóa Đơn
            if (dgvKetQua.Columns.Contains("Mã Hóa Đơn"))
            {
                string? maHD = dgvKetQua.Rows[e.RowIndex].Cells["Mã Hóa Đơn"].Value?.ToString();

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã SP");
                dt.Columns.Add("Tên Sản Phẩm");
                dt.Columns.Add("Màu");
                dt.Columns.Add("Size");
                dt.Columns.Add("Số Lượng");
                dt.Columns.Add("Đơn Giá Bán");
                dt.Columns.Add("Thành Tiền");

                if (maHD == "HD001")
                {
                    dt.Rows.Add("SP01", "Áo sơ mi Nam", "Trắng", "M", "2", "250,000", "500,000");
                    dt.Rows.Add("SP02", "Quần Tây Nam", "Đen", "L", "1", "350,000", "350,000");
                }
                else
                {
                    dt.Rows.Add("SP03", "Váy Nữ Công Sở", "Đỏ", "S", "2", "600,000", "1,200,000");
                }

                if (dgvChiTiet != null) dgvChiTiet.DataSource = dt;
            }
            // Click dòng Phiếu Nhập
            else if (dgvKetQua.Columns.Contains("Mã Phiếu Nhập"))
            {
                string? maPN = dgvKetQua.Rows[e.RowIndex].Cells["Mã Phiếu Nhập"].Value?.ToString();

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã SP");
                dt.Columns.Add("Tên Sản Phẩm");
                dt.Columns.Add("Màu");
                dt.Columns.Add("Size");
                dt.Columns.Add("Số Lượng Nhập");
                dt.Columns.Add("Giá Nhập Kho");
                dt.Columns.Add("Thành Tiền");

                if (maPN == "PN001")
                {
                    dt.Rows.Add("SP01", "Áo sơ mi Nam", "Trắng", "L", "50", "150,000", "7,500,000");
                    dt.Rows.Add("SP01", "Áo sơ mi Nam", "Trắng", "M", "50", "150,000", "7,500,000");
                }
                else
                {
                    dt.Rows.Add("SP02", "Quần Jeans Nữ", "Xanh", "S", "30", "283,333", "8,500,000");
                }

                if (dgvChiTiet != null) dgvChiTiet.DataSource = dt;
            }
        }

        // --- SỰ KIỆN CHỌN SIZE TRÊN BẢNG SẢN PHẨM -> NHẢY SỐ TỒN KHO ---
        private void DgvKetQua_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (dgvKetQua != null && e.RowIndex >= 0 && dgvKetQua.Columns.Contains("colSize") && e.ColumnIndex == dgvKetQua.Columns["colSize"].Index)
            {
                string? size = dgvKetQua.Rows[e.RowIndex].Cells["colSize"].Value?.ToString();

                int tonKhoGia = 0;
                if (size == "S") tonKhoGia = 5;
                else if (size == "M") tonKhoGia = 15;
                else if (size == "L") tonKhoGia = 8;
                else if (size == "XL") tonKhoGia = 2;

                dgvKetQua.Rows[e.RowIndex].Cells["colTonKho"].Value = tonKhoGia + " cái";
            }
        }

        // --- NÚT LÀM MỚI ---
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            if (txtTuKhoa != null) txtTuKhoa.Clear();
            if (dgvKetQua != null) dgvKetQua.DataSource = null;
            if (dgvChiTiet != null) dgvChiTiet.DataSource = null;
            HienThiBangChiTiet(false);
            txtTuKhoa?.Focus();
        }
    }
}