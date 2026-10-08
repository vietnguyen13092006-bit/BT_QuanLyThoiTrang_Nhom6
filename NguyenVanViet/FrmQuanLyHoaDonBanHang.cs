using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace FORM_DKY
{
    public partial class FrmQuanLyHoaDonBanHang : Form
    {
        // Khai báo các Control
        private Label lblTitle;
        private GroupBox gbThongTinHD, gbThongTinSP, gbGioHang, gbDanhSachHD;
        private TextBox txtMaHD, txtKhachHang, txtSDT, txtDonGia, txtTongTien, txtNhanVien; // Đổi cboNhanVien thành txtNhanVien
        private DateTimePicker dtpNgayBan;
        private ComboBox cboMaSP, cboSanPham, cboMau, cboKichThuoc;
        private NumericUpDown nudSoLuong;
        private Button btnThemSP, btnSua, btnXoa, btnLamMoi, btnThanhToan, btnXuatHoaDon, btnTaiDanhSachHD;

        // DataGridView
        private DataGridView dgvGioHang;
        private DataGridView dgvDanhSachHD;

        // Bảng tạm giỏ hàng
        private DataTable dtGioHang;

        // Cờ chống lặp sự kiện giữa cboMaSP và cboSanPham
        private bool isBindingSP = false;

        public FrmQuanLyHoaDonBanHang()
        {
            InitializeComponent();
            InitializeComponentCustom();
            InitGioHang();
            LoadDataToComboBoxes();
            TaoMaHoaDonMoi();
            LoadDanhSachHoaDonDaLuu();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "QUẢN LÝ HÓA ĐƠN BÁN HÀNG";
            this.Size = new Size(1150, 820);
            this.MinimumSize = new Size(1050, 720);
            this.StartPosition = FormStartPosition.CenterScreen;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                RowCount = 6,
                ColumnCount = 1
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // 0. Tiêu đề
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 95F));  // 1. Thông tin HD
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F)); // 2. Thông tin SP
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));   // 3. Giỏ hàng tạm
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));  // 4. Thanh toán & Nút bấm
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));   // 5. Danh sách Hóa đơn đã lưu

            // 0. Tiêu đề
            lblTitle = new Label
            {
                Text = "QUẢN LÝ HÓA ĐƠN BÁN HÀNG",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 1. GroupBox THÔNG TIN HÓA ĐƠN
            gbThongTinHD = new GroupBox { Text = "THÔNG TIN HÓA ĐƠN", Font = new Font("Arial", 9, FontStyle.Bold), Dock = DockStyle.Fill };
            TableLayoutPanel tlpHD = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 2,
                Padding = new Padding(5)
            };
            tlpHD.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tlpHD.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpHD.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tlpHD.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpHD.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpHD.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            txtMaHD = new TextBox { ReadOnly = true, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            dtpNgayBan = new DateTimePicker { Format = DateTimePickerFormat.Short, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            txtNhanVien = new TextBox { Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) }; // Ô nhập tự do tên nhân viên
            txtKhachHang = new TextBox { Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            txtSDT = new TextBox { Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };

            tlpHD.Controls.Add(new Label { Text = "Mã hóa đơn:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            tlpHD.Controls.Add(txtMaHD, 1, 0);
            tlpHD.Controls.Add(new Label { Text = "Ngày bán:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            tlpHD.Controls.Add(dtpNgayBan, 3, 0);
            tlpHD.Controls.Add(new Label { Text = "Nhân viên bán:", Anchor = AnchorStyles.Left, AutoSize = true }, 4, 0);
            tlpHD.Controls.Add(txtNhanVien, 5, 0);

            tlpHD.Controls.Add(new Label { Text = "Khách hàng:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            tlpHD.Controls.Add(txtKhachHang, 1, 1);
            tlpHD.Controls.Add(new Label { Text = "SĐT:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 1);
            tlpHD.Controls.Add(txtSDT, 3, 1);

            gbThongTinHD.Controls.Add(tlpHD);

            // 2. GroupBox THÔNG TIN SẢN PHẨM
            gbThongTinSP = new GroupBox { Text = "THÔNG TIN SẢN PHẨM", Font = new Font("Arial", 9, FontStyle.Bold), Dock = DockStyle.Fill };
            TableLayoutPanel tlpSP = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 10,
                RowCount = 2,
                Padding = new Padding(5)
            };
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 65F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 65F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            cboMaSP = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            cboSanPham = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            cboMau = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            cboKichThuoc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            nudSoLuong = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 1, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            txtDonGia = new TextBox { ReadOnly = true, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };

            btnThemSP = new Button
            {
                Text = "THÊM SẢN PHẨM",
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Height = 32
            };

            cboMaSP.SelectedIndexChanged += CboMaSP_SelectedIndexChanged;
            cboSanPham.SelectedIndexChanged += CboSanPham_SelectedIndexChanged;
            btnThemSP.Click += BtnThemSP_Click;

            tlpSP.Controls.Add(new Label { Text = "Mã SP:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            tlpSP.Controls.Add(cboMaSP, 1, 0);
            tlpSP.Controls.Add(new Label { Text = "Tên SP:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            tlpSP.Controls.Add(cboSanPham, 3, 0);
            tlpSP.Controls.Add(new Label { Text = "Màu:", Anchor = AnchorStyles.Left, AutoSize = true }, 4, 0);
            tlpSP.Controls.Add(cboMau, 5, 0);
            tlpSP.Controls.Add(new Label { Text = "Size:", Anchor = AnchorStyles.Left, AutoSize = true }, 6, 0);
            tlpSP.Controls.Add(cboKichThuoc, 7, 0);
            tlpSP.Controls.Add(new Label { Text = "Số lượng:", Anchor = AnchorStyles.Left, AutoSize = true }, 8, 0);
            tlpSP.Controls.Add(nudSoLuong, 9, 0);

            tlpSP.Controls.Add(new Label { Text = "Đơn giá:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            tlpSP.Controls.Add(txtDonGia, 1, 1);
            tlpSP.Controls.Add(btnThemSP, 3, 1);
            tlpSP.SetColumnSpan(btnThemSP, 2);

            gbThongTinSP.Controls.Add(tlpSP);

            // 3. GroupBox GIỎ HÀNG
            gbGioHang = new GroupBox { Text = "GIỎ HÀNG ĐANG TẠO", Font = new Font("Arial", 9, FontStyle.Bold), Dock = DockStyle.Fill };
            dgvGioHang = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            gbGioHang.Controls.Add(dgvGioHang);

            // 4. Layout Bottom
            TableLayoutPanel tlpBottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            FlowLayoutPanel flpButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            btnSua = new Button { Text = "SỬA SP", Width = 80, Height = 35 };
            btnXoa = new Button { Text = "XÓA SP", Width = 80, Height = 35 };
            btnLamMoi = new Button { Text = "LÀM MỚI", Width = 90, Height = 35 };

            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            flpButtons.Controls.AddRange(new Control[] { btnSua, btnXoa, btnLamMoi });

            TableLayoutPanel tlpPay = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            tlpPay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpPay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));

            txtTongTien = new TextBox { ReadOnly = true, Text = "0", Font = new Font("Arial", 11, FontStyle.Bold), TextAlign = HorizontalAlignment.Right, Dock = DockStyle.Fill };

            FlowLayoutPanel flpPayBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            btnThanhToan = new Button { Text = "THANH TOÁN HD DƯỚI", BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold), Width = 165, Height = 38 };
            btnXuatHoaDon = new Button { Text = "XUẤT HÓA ĐƠN", BackColor = Color.Orange, ForeColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold), Width = 125, Height = 38 };

            btnThanhToan.Click += BtnThanhToanHDDuoi_Click;
            btnXuatHoaDon.Click += BtnXuatHoaDon_Click;

            flpPayBtns.Controls.Add(btnThanhToan);
            flpPayBtns.Controls.Add(btnXuatHoaDon);

            tlpPay.Controls.Add(new Label { Text = "TỔNG TIỀN:", Font = new Font("Arial", 9, FontStyle.Bold), Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            tlpPay.Controls.Add(txtTongTien, 1, 0);
            tlpPay.Controls.Add(flpPayBtns, 1, 1);

            tlpBottom.Controls.Add(flpButtons, 0, 0);
            tlpBottom.Controls.Add(tlpPay, 1, 0);

            // 5. GroupBox DANH SÁCH HÓA ĐƠN ĐÃ LƯU
            gbDanhSachHD = new GroupBox { Text = "DANH SÁCH HÓA ĐƠN TRONG DATABASE (BẤM ĐỂ THANH TOÁN / XEM CHI TIẾT)", Font = new Font("Arial", 9, FontStyle.Bold), Dock = DockStyle.Fill };

            TableLayoutPanel tlpDS = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            btnTaiDanhSachHD = new Button { Text = "🔄 Tải lại danh sách Hóa đơn", Width = 180, Height = 28, Anchor = AnchorStyles.Left };
            btnTaiDanhSachHD.Click += (s, e) => LoadDanhSachHoaDonDaLuu();

            dgvDanhSachHD = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvDanhSachHD.CellDoubleClick += DgvDanhSachHD_CellDoubleClick;

            tlpDS.Controls.Add(btnTaiDanhSachHD, 0, 0);
            tlpDS.Controls.Add(dgvDanhSachHD, 0, 1);
            gbDanhSachHD.Controls.Add(tlpDS);

            mainLayout.Controls.Add(lblTitle, 0, 0);
            mainLayout.Controls.Add(gbThongTinHD, 0, 1);
            mainLayout.Controls.Add(gbThongTinSP, 0, 2);
            mainLayout.Controls.Add(gbGioHang, 0, 3);
            mainLayout.Controls.Add(tlpBottom, 0, 4);
            mainLayout.Controls.Add(gbDanhSachHD, 0, 5);

            this.Controls.Add(mainLayout);
        }

        private void InitGioHang()
        {
            dtGioHang = new DataTable();
            dtGioHang.Columns.Add("MaHD", typeof(string));
            dtGioHang.Columns.Add("MaSP", typeof(string));
            dtGioHang.Columns.Add("TenSP", typeof(string));
            dtGioHang.Columns.Add("Mau", typeof(string));
            dtGioHang.Columns.Add("KichThuoc", typeof(string));
            dtGioHang.Columns.Add("SoLuong", typeof(int));
            dtGioHang.Columns.Add("DonGia", typeof(decimal));
            dtGioHang.Columns.Add("ThanhTien", typeof(decimal));

            dgvGioHang.DataSource = dtGioHang;

            dgvGioHang.Columns["MaHD"].HeaderText = "Mã Hóa Đơn";
            dgvGioHang.Columns["MaSP"].HeaderText = "Mã SP";
            dgvGioHang.Columns["TenSP"].HeaderText = "Tên sản phẩm";
            dgvGioHang.Columns["Mau"].HeaderText = "Màu";
            dgvGioHang.Columns["KichThuoc"].HeaderText = "Kích thước";
            dgvGioHang.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvGioHang.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvGioHang.Columns["ThanhTien"].HeaderText = "Thành tiền";

            dgvGioHang.Columns["MaSP"].Visible = true;
            dgvGioHang.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
        }

        private void TaoMaHoaDonMoi()
        {
            txtMaHD.Text = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void LoadDataToComboBoxes()
        {
            try
            {
                // Chỉ nạp danh mục sản phẩm lên ComboBox
                DataTable dtSP = DatabaseHelper.ExecuteQuery("SELECT MaSP, TenSP, MauSac, Size, GiaBan FROM SanPham");

                isBindingSP = true;

                cboMaSP.DataSource = dtSP.Copy();
                cboMaSP.DisplayMember = "MaSP";
                cboMaSP.ValueMember = "MaSP";

                cboSanPham.DataSource = dtSP;
                cboSanPham.DisplayMember = "TenSP";
                cboSanPham.ValueMember = "MaSP";

                isBindingSP = false;

                if (cboSanPham.Items.Count > 0)
                {
                    CapNhatChiTietSPSelected((DataRowView)cboSanPham.SelectedItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isBindingSP || cboMaSP.SelectedValue == null) return;
            isBindingSP = true;
            cboSanPham.SelectedValue = cboMaSP.SelectedValue;
            if (cboSanPham.SelectedItem is DataRowView drv)
            {
                CapNhatChiTietSPSelected(drv);
            }
            isBindingSP = false;
        }

        private void CboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isBindingSP || cboSanPham.SelectedValue == null) return;
            isBindingSP = true;
            cboMaSP.SelectedValue = cboSanPham.SelectedValue;
            if (cboSanPham.SelectedItem is DataRowView drv)
            {
                CapNhatChiTietSPSelected(drv);
            }
            isBindingSP = false;
        }

        private void CapNhatChiTietSPSelected(DataRowView drv)
        {
            txtDonGia.Text = Convert.ToDecimal(drv["GiaBan"]).ToString("N0");

            cboMau.Items.Clear();
            cboMau.Items.Add(drv["MauSac"] != DBNull.Value ? drv["MauSac"].ToString() : "");
            cboMau.SelectedIndex = 0;

            cboKichThuoc.Items.Clear();
            cboKichThuoc.Items.Add(drv["Size"] != DBNull.Value ? drv["Size"].ToString() : "");
            cboKichThuoc.SelectedIndex = 0;
        }

        private void LoadDanhSachHoaDonDaLuu()
        {
            try
            {
                string sql = @"SELECT hd.MaHD AS [Mã Hóa Đơn], 
                                      hd.NgayBan AS [Ngày Bán], 
                                      nv.TenNV AS [Nhân Viên], 
                                      ISNULL(hd.TenKhachHang, N'Khách lẻ') AS [Khách Hàng], 
                                      ISNULL(hd.SDT, '') AS [SĐT], 
                                      hd.TongTien AS [Tổng Tiền],
                                      ISNULL(hd.TrangThai, N'Chưa thanh toán') AS [Trạng Thái]
                               FROM HoaDon hd
                               LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                               ORDER BY hd.NgayBan DESC";

                DataTable dtHD = DatabaseHelper.ExecuteQuery(sql);
                dgvDanhSachHD.DataSource = dtHD;

                if (dgvDanhSachHD.Columns["Tổng Tiền"] != null)
                {
                    dgvDanhSachHD.Columns["Tổng Tiền"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hóa đơn từ SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDanhSachHD_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSachHD.Rows[e.RowIndex];
                string maHD = row.Cells["Mã Hóa Đơn"].Value.ToString();
                string ngayBan = Convert.ToDateTime(row.Cells["Ngày Bán"].Value).ToString("dd/MM/yyyy");
                string nhanVien = row.Cells["Nhân Viên"].Value.ToString();
                string khachHang = row.Cells["Khách Hàng"].Value.ToString();
                string sdt = row.Cells["SĐT"].Value.ToString();
                string tongTien = Convert.ToDecimal(row.Cells["Tổng Tiền"].Value).ToString("N0");

                string sqlChiTiet = @"SELECT sp.MaSP, sp.TenSP, sp.MauSac AS Mau, sp.Size AS KichThuoc, ct.SoLuong, ct.DonGia, (ct.SoLuong * ct.DonGia) AS ThanhTien
                                      FROM ChiTietHoaDon ct
                                      JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                      WHERE ct.MaHD = @MaHD";

                SqlParameter[] p = { new SqlParameter("@MaHD", maHD) };
                DataTable dtCT = DatabaseHelper.ExecuteQuery(sqlChiTiet, p);

                FrmXuatHoaDonPopup frm = new FrmXuatHoaDonPopup(maHD, ngayBan, nhanVien, khachHang, sdt, tongTien, dtCT);
                frm.ShowDialog();
            }
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedItem == null) return;

            DataRowView drv = (DataRowView)cboSanPham.SelectedItem;
            string maSP = drv["MaSP"].ToString();
            string tenSP = drv["TenSP"].ToString();
            string mau = cboMau.Text;
            string kichThuoc = cboKichThuoc.Text;
            int soLuong = (int)nudSoLuong.Value;
            decimal donGia = Convert.ToDecimal(drv["GiaBan"]);

            foreach (DataRow row in dtGioHang.Rows)
            {
                if (row["MaSP"].ToString() == maSP)
                {
                    row["SoLuong"] = (int)row["SoLuong"] + soLuong;
                    row["ThanhTien"] = (int)row["SoLuong"] * donGia;
                    TinhTongTien();
                    return;
                }
            }

            dtGioHang.Rows.Add(txtMaHD.Text, maSP, tenSP, mau, kichThuoc, soLuong, donGia, soLuong * donGia);
            TinhTongTien();
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;
            foreach (DataRow row in dtGioHang.Rows)
            {
                tongTien += Convert.ToDecimal(row["ThanhTien"]);
            }
            txtTongTien.Text = tongTien.ToString("N0");
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvGioHang.SelectedRows[0];
                row.Cells["SoLuong"].Value = (int)nudSoLuong.Value;
                row.Cells["ThanhTien"].Value = (int)nudSoLuong.Value * Convert.ToDecimal(row.Cells["DonGia"].Value);
                TinhTongTien();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.SelectedRows.Count > 0)
            {
                dgvGioHang.Rows.RemoveAt(dgvGioHang.SelectedRows[0].Index);
                TinhTongTien();
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            dtGioHang.Rows.Clear();
            txtKhachHang.Clear();
            txtSDT.Clear();
            txtNhanVien.Clear();
            nudSoLuong.Value = 1;
            TaoMaHoaDonMoi();
            TinhTongTien();
        }

        // Hàm kiểm tra và tự động thêm Nhân viên nếu chưa có trong bảng NhanVien (để tránh lỗi khóa ngoại SQL)
        private string XuLyVaLayMaNhanVien(SqlConnection conn, SqlTransaction transaction, string tenNV)
        {
            // Kiểm tra xem tên nhân viên đã tồn tại chưa
            string checkQuery = "SELECT MaNV FROM NhanVien WHERE TenNV = @TenNV";
            using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn, transaction))
            {
                cmdCheck.Parameters.AddWithValue("@TenNV", tenNV);
                object result = cmdCheck.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
            }

            // Nếu chưa có, tự động sinh MaNV mới (VD: NV + ticks) và thêm vào bảng NhanVien
            string newMaNV = "NV" + DateTime.Now.ToString("mmssff");
            string insertNV = "INSERT INTO NhanVien (MaNV, TenNV) VALUES (@MaNV, @TenNV)";
            using (SqlCommand cmdInsert = new SqlCommand(insertNV, conn, transaction))
            {
                cmdInsert.Parameters.AddWithValue("@MaNV", newMaNV);
                cmdInsert.Parameters.AddWithValue("@TenNV", tenNV);
                cmdInsert.ExecuteNonQuery();
            }

            return newMaNV;
        }

        private bool LuuHoaDonBanDau()
        {
            if (dtGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào giỏ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên bán hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhanVien.Focus();
                return false;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Tự động kiểm tra/thêm nhân viên vừa gõ vào Database
                    string maNV = XuLyVaLayMaNhanVien(conn, transaction, txtNhanVien.Text.Trim());

                    string insertHD = @"INSERT INTO HoaDon (MaHD, NgayBan, MaNV, TenKhachHang, SDT, TongTien, TrangThai) 
                                        VALUES (@MaHD, @NgayBan, @MaNV, @TenKhachHang, @SDT, @TongTien, N'Chưa thanh toán')";

                    SqlParameter[] pHD = {
                        new SqlParameter("@MaHD", txtMaHD.Text),
                        new SqlParameter("@NgayBan", dtpNgayBan.Value),
                        new SqlParameter("@MaNV", maNV),
                        new SqlParameter("@TenKhachHang", string.IsNullOrWhiteSpace(txtKhachHang.Text) ? (object)DBNull.Value : txtKhachHang.Text),
                        new SqlParameter("@SDT", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text),
                        new SqlParameter("@TongTien", decimal.Parse(txtTongTien.Text.Replace(".", "").Replace(",", "")))
                    };
                    DatabaseHelper.ExecuteNonQuery(insertHD, pHD, transaction);

                    foreach (DataRow row in dtGioHang.Rows)
                    {
                        string insertCT = @"INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGia) 
                                            VALUES (@MaHD, @MaSP, @SoLuong, @DonGia)";

                        SqlParameter[] pCT = {
                            new SqlParameter("@MaHD", txtMaHD.Text),
                            new SqlParameter("@MaSP", row["MaSP"].ToString()),
                            new SqlParameter("@SoLuong", Convert.ToInt32(row["SoLuong"])),
                            new SqlParameter("@DonGia", Convert.ToDecimal(row["DonGia"]))
                        };
                        DatabaseHelper.ExecuteNonQuery(insertCT, pCT, transaction);
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lưu hóa đơn thất bại: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        // NÚT XUẤT HÓA ĐƠN
        private void BtnXuatHoaDon_Click(object sender, EventArgs e)
        {
            if (LuuHoaDonBanDau())
            {
                string maHD = txtMaHD.Text;
                string ngayBan = dtpNgayBan.Value.ToString("dd/MM/yyyy");
                string nhanVien = txtNhanVien.Text;
                string khachHang = txtKhachHang.Text;
                string sdt = txtSDT.Text;
                string tongTien = txtTongTien.Text;

                LoadDanhSachHoaDonDaLuu();

                FrmXuatHoaDonPopup frm = new FrmXuatHoaDonPopup(maHD, ngayBan, nhanVien, khachHang, sdt, tongTien, dtGioHang.Copy());
                frm.ShowDialog();

                BtnLamMoi_Click(null, null);
            }
        }

        // NÚT THANH TOÁN HÓA ĐƠN ĐƯỢC CHỌN Ở BẢNG 2
        private void BtnThanhToanHDDuoi_Click(object sender, EventArgs e)
        {
            if (dgvDanhSachHD.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn 1 hóa đơn cần thanh toán từ danh sách bên dưới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvDanhSachHD.SelectedRows[0];
            string maHD = row.Cells["Mã Hóa Đơn"].Value.ToString();
            string trangThai = row.Cells["Trạng Thái"].Value.ToString();

            if (trangThai == "Đã thanh toán")
            {
                MessageBox.Show("Hóa đơn này đã được thanh toán rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show($"Xác nhận thanh toán cho hóa đơn [{maHD}]?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // 1. Đổi trạng thái Hóa đơn
                        string updateHD = "UPDATE HoaDon SET TrangThai = N'Đã thanh toán' WHERE MaHD = @MaHD";
                        SqlParameter[] p1 = { new SqlParameter("@MaHD", maHD) };
                        DatabaseHelper.ExecuteNonQuery(updateHD, p1, transaction);

                        // 2. Trừ tồn kho chính xác theo MaSP
                        string updateKho = @"UPDATE SanPham 
                                             SET SoLuongTon = SoLuongTon - ct.SoLuong 
                                             FROM SanPham sp
                                             INNER JOIN ChiTietHoaDon ct ON sp.MaSP = ct.MaSP
                                             WHERE ct.MaHD = @MaHD";
                        SqlParameter[] p2 = { new SqlParameter("@MaHD", maHD) };
                        DatabaseHelper.ExecuteNonQuery(updateKho, p2, transaction);

                        transaction.Commit();
                        MessageBox.Show($"Thanh toán thành công hóa đơn [{maHD}]!\nĐã tự động trừ số lượng tồn kho theo Mã SP.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDanhSachHoaDonDaLuu();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

// =========================================================================
// POPUP XUẤT HÓA ĐƠN
// =========================================================================
public class FrmXuatHoaDonPopup : Form
{
    private string maHD, ngayBan, nhanVien, khachHang, sdt, tongTien;
    private DataTable dtChiTiet;

    public FrmXuatHoaDonPopup(string maHD, string ngayBan, string nhanVien, string khachHang, string sdt, string tongTien, DataTable dtChiTiet)
    {
        this.maHD = maHD;
        this.ngayBan = ngayBan;
        this.nhanVien = nhanVien;
        this.khachHang = khachHang;
        this.sdt = sdt;
        this.tongTien = tongTien;
        this.dtChiTiet = dtChiTiet;

        TaoGiaoDienHoaDon();
    }

    private void TaoGiaoDienHoaDon()
    {
        this.Text = "HÓA ĐƠN BÁN HÀNG";
        this.Size = new Size(720, 580);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            RowCount = 5,
            ColumnCount = 1
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

        Label lblHeader = new Label
        {
            Text = "HÓA ĐƠN BÁN HÀNG",
            Font = new Font("Arial", 16, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };

        GroupBox gbTTKH = new GroupBox
        {
            Text = "Thông Tin Khách Hàng & Hóa Đơn",
            Font = new Font("Arial", 9, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(5)
        };

        Label lblTTKH = new Label
        {
            Text = $"Mã HD: {maHD}    |    Ngày bán: {ngayBan}\n" +
                   $"Khách hàng: {(string.IsNullOrWhiteSpace(khachHang) ? "Khách lẻ" : khachHang)}    |    SĐT: {(string.IsNullOrWhiteSpace(sdt) ? "Không có" : sdt)}\n" +
                   $"Nhân viên bán: {nhanVien}",
            Font = new Font("Arial", 9, FontStyle.Regular),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        gbTTKH.Controls.Add(lblTTKH);

        DataGridView dgvSanPham = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.Fixed3D
        };

        DataTable dtPrint = new DataTable();
        dtPrint.Columns.Add("Mã SP", typeof(string));
        dtPrint.Columns.Add("Tên sản phẩm", typeof(string));
        dtPrint.Columns.Add("Màu", typeof(string));
        dtPrint.Columns.Add("Kích thước", typeof(string));
        dtPrint.Columns.Add("Số lượng", typeof(int));
        dtPrint.Columns.Add("Đơn giá", typeof(decimal));
        dtPrint.Columns.Add("Thành tiền", typeof(decimal));

        if (dtChiTiet != null)
        {
            foreach (DataRow row in dtChiTiet.Rows)
            {
                string maSP = row.Table.Columns.Contains("MaSP") ? row["MaSP"].ToString() : "";
                string tenSP = row.Table.Columns.Contains("TenSP") ? row["TenSP"].ToString() : "";
                string mau = row.Table.Columns.Contains("Mau") ? row["Mau"].ToString() : (row.Table.Columns.Contains("MauSac") ? row["MauSac"].ToString() : "");
                string size = row.Table.Columns.Contains("KichThuoc") ? row["KichThuoc"].ToString() : (row.Table.Columns.Contains("Size") ? row["Size"].ToString() : "");
                int soLuong = row.Table.Columns.Contains("SoLuong") && row["SoLuong"] != DBNull.Value ? Convert.ToInt32(row["SoLuong"]) : 0;
                decimal donGia = row.Table.Columns.Contains("DonGia") && row["DonGia"] != DBNull.Value ? Convert.ToDecimal(row["DonGia"]) : 0;
                decimal thanhTien = row.Table.Columns.Contains("ThanhTien") && row["ThanhTien"] != DBNull.Value ? Convert.ToDecimal(row["ThanhTien"]) : (soLuong * donGia);

                dtPrint.Rows.Add(maSP, tenSP, mau, size, soLuong, donGia, thanhTien);
            }
        }

        dgvSanPham.DataSource = dtPrint;

        if (dgvSanPham.Columns["Đơn giá"] != null) dgvSanPham.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
        if (dgvSanPham.Columns["Thành tiền"] != null) dgvSanPham.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";

        Label lblTongTien = new Label
        {
            Text = $"TỔNG TIỀN THANH TOÁN: {tongTien} VNĐ",
            Font = new Font("Arial", 11, FontStyle.Bold),
            ForeColor = Color.Red,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight
        };

        FlowLayoutPanel flpButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 5, 0, 0)
        };

        Button btnThoat = new Button { Text = "THOÁT", Width = 100, Height = 35, BackColor = Color.LightGray, Font = new Font("Arial", 9, FontStyle.Bold) };
        Button btnIn = new Button { Text = "IN HÓA ĐƠN", Width = 120, Height = 35, BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold) };

        btnThoat.Click += (s, e) => this.Close();
        btnIn.Click += (s, e) => MessageBox.Show("Đã gửi lệnh in hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        flpButtons.Controls.Add(btnThoat);
        flpButtons.Controls.Add(btnIn);

        mainLayout.Controls.Add(lblHeader, 0, 0);
        mainLayout.Controls.Add(gbTTKH, 0, 1);
        mainLayout.Controls.Add(dgvSanPham, 0, 2);
        mainLayout.Controls.Add(lblTongTien, 0, 3);
        mainLayout.Controls.Add(flpButtons, 0, 4);

        this.Controls.Add(mainLayout);
    }
}