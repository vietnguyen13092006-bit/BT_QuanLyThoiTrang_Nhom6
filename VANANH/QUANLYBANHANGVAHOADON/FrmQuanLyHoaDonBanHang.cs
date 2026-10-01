using QuanLyHoaDonApp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QUANLYBANHANGVAHOADON
{
    public partial class FrmQuanLyHoaDonBanHang : Form
    {
        // Khai báo các Control
        private Label lblTitle;
        private GroupBox gbThongTinHD, gbThongTinSP, gbDanhMucSP;
        private TextBox txtMaHD, txtKhachHang, txtSDT, txtDonGia, txtTongTien;
        private DateTimePicker dtpNgayBan;
        private ComboBox cboNhanVien, cboSanPham, cboMau, cboKichThuoc;
        private NumericUpDown nudSoLuong;
        private Button btnThemSP, btnSua, btnXoa, btnLamMoi, btnThanhToan, btnXuatHoaDon;
        private DataGridView dgvSanPham;

        // Bảng tạm giỏ hàng
        private DataTable dtGioHang;

        public FrmQuanLyHoaDonBanHang()
        {
            InitializeComponent();
            InitializeComponentCustom();
            InitGioHang();
            LoadDataToComboBoxes();
            TaoMaHoaDonMoi();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "QUẢN LÝ HÓA ĐƠN";
            this.Size = new Size(1000, 700);
            this.MinimumSize = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Layout chính co giãn tự động
            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Padding = new Padding(10);
            mainLayout.RowCount = 5;
            mainLayout.ColumnCount = 1;

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));  // Tiêu đề
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Thông tin HD
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F)); // Thông tin SP
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Danh mục SP (co giãn)
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));  // Thanh toán & Nút bấm

            // 1. Tiêu đề
            lblTitle = new Label
            {
                Text = "QUẢN LÝ HÓA ĐƠN",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 2. GroupBox THÔNG TIN HÓA ĐƠN
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
            cboNhanVien = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            txtKhachHang = new TextBox { Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };
            txtSDT = new TextBox { Dock = DockStyle.Fill, Font = new Font("Arial", 9, FontStyle.Regular) };

            tlpHD.Controls.Add(new Label { Text = "Mã hóa đơn:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            tlpHD.Controls.Add(txtMaHD, 1, 0);
            tlpHD.Controls.Add(new Label { Text = "Ngày bán:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            tlpHD.Controls.Add(dtpNgayBan, 3, 0);
            tlpHD.Controls.Add(new Label { Text = "Nhân viên bán:", Anchor = AnchorStyles.Left, AutoSize = true }, 4, 0);
            tlpHD.Controls.Add(cboNhanVien, 5, 0);

            tlpHD.Controls.Add(new Label { Text = "Khách hàng:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            tlpHD.Controls.Add(txtKhachHang, 1, 1);
            tlpHD.Controls.Add(new Label { Text = "SĐT:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 1);
            tlpHD.Controls.Add(txtSDT, 3, 1);

            gbThongTinHD.Controls.Add(tlpHD);

            // 3. GroupBox THÔNG TIN SẢN PHẨM
            gbThongTinSP = new GroupBox { Text = "THÔNG TIN SẢN PHẨM", Font = new Font("Arial", 9, FontStyle.Bold), Dock = DockStyle.Fill };
            TableLayoutPanel tlpSP = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 8,
                RowCount = 2,
                Padding = new Padding(5)
            };
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

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
                Height = 35
            };
            btnThemSP.Click += BtnThemSP_Click;
            cboSanPham.SelectedIndexChanged += CboSanPham_SelectedIndexChanged;

            tlpSP.Controls.Add(new Label { Text = "Sản phẩm:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            tlpSP.Controls.Add(cboSanPham, 1, 0);
            tlpSP.Controls.Add(new Label { Text = "Màu:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            tlpSP.Controls.Add(cboMau, 3, 0);
            tlpSP.Controls.Add(new Label { Text = "Kích thước:", Anchor = AnchorStyles.Left, AutoSize = true }, 4, 0);
            tlpSP.Controls.Add(cboKichThuoc, 5, 0);
            tlpSP.Controls.Add(new Label { Text = "Số lượng:", Anchor = AnchorStyles.Left, AutoSize = true }, 6, 0);
            tlpSP.Controls.Add(nudSoLuong, 7, 0);

            tlpSP.Controls.Add(new Label { Text = "Đơn giá:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            tlpSP.Controls.Add(txtDonGia, 1, 1);
            tlpSP.Controls.Add(btnThemSP, 3, 1);
            tlpSP.SetColumnSpan(btnThemSP, 2);

            gbThongTinSP.Controls.Add(tlpSP);

            // 4. GroupBox DANH MỤC SẢN PHẨM
            gbDanhMucSP = new GroupBox { Text = "DANH MỤC SẢN PHẨM", Font = new Font("Arial", 9, FontStyle.Bold), Dock = DockStyle.Fill };
            dgvSanPham = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            gbDanhMucSP.Controls.Add(dgvSanPham);

            // 5. Layout Bottom (Chức năng & Thanh toán)
            TableLayoutPanel tlpBottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));

            // Nút bấm bên trái
            FlowLayoutPanel flpButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            btnSua = new Button { Text = "SỬA", Width = 80, Height = 35 };
            btnXoa = new Button { Text = "XÓA", Width = 80, Height = 35 };
            btnLamMoi = new Button { Text = "LÀM MỚI", Width = 90, Height = 35 };

            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            flpButtons.Controls.AddRange(new Control[] { btnSua, btnXoa, btnLamMoi });

            // Tổng tiền & Thanh toán & Xuất hóa đơn bên phải
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
            btnThanhToan = new Button { Text = "THANH TOÁN", BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold), Width = 120, Height = 40 };
            btnXuatHoaDon = new Button { Text = "XUẤT HÓA ĐƠN", BackColor = Color.Orange, ForeColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold), Width = 130, Height = 40 };

            btnThanhToan.Click += BtnThanhToan_Click;
            btnXuatHoaDon.Click += BtnXuatHoaDon_Click;

            flpPayBtns.Controls.Add(btnThanhToan);
            flpPayBtns.Controls.Add(btnXuatHoaDon);

            tlpPay.Controls.Add(new Label { Text = "TỔNG TIỀN THANH TOÁN:", Font = new Font("Arial", 9, FontStyle.Bold), Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            tlpPay.Controls.Add(txtTongTien, 1, 0);
            tlpPay.Controls.Add(flpPayBtns, 1, 1);

            tlpBottom.Controls.Add(flpButtons, 0, 0);
            tlpBottom.Controls.Add(tlpPay, 1, 0);

            // Thêm các phần vào Layout chính
            mainLayout.Controls.Add(lblTitle, 0, 0);
            mainLayout.Controls.Add(gbThongTinHD, 0, 1);
            mainLayout.Controls.Add(gbThongTinSP, 0, 2);
            mainLayout.Controls.Add(gbDanhMucSP, 0, 3);
            mainLayout.Controls.Add(tlpBottom, 0, 4);

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

            dgvSanPham.DataSource = dtGioHang;

            dgvSanPham.Columns["MaHD"].HeaderText = "Mã Hóa Đơn";
            dgvSanPham.Columns["TenSP"].HeaderText = "Tên sản phẩm";
            dgvSanPham.Columns["Mau"].HeaderText = "Màu";
            dgvSanPham.Columns["KichThuoc"].HeaderText = "Kích thước";
            dgvSanPham.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvSanPham.Columns["ThanhTien"].HeaderText = "Thành tiền";
            dgvSanPham.Columns["MaSP"].Visible = false;
        }

        private void TaoMaHoaDonMoi()
        {
            txtMaHD.Text = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void LoadDataToComboBoxes()
        {
            try
            {
                DataTable dtNV = DatabaseHelper.ExecuteQuery("SELECT MaNV, TenNV FROM NhanVien");
                cboNhanVien.DataSource = dtNV;
                cboNhanVien.DisplayMember = "TenNV";
                cboNhanVien.ValueMember = "MaNV";

                DataTable dtSP = DatabaseHelper.ExecuteQuery("SELECT MaSP, TenSP, Mau, KichThuoc, DonGia FROM SanPham");
                cboSanPham.DataSource = dtSP;
                cboSanPham.DisplayMember = "TenSP";
                cboSanPham.ValueMember = "MaSP";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)cboSanPham.SelectedItem;
                txtDonGia.Text = Convert.ToDecimal(drv["DonGia"]).ToString("N0");

                cboMau.Items.Clear();
                cboMau.Items.Add(drv["Mau"].ToString());
                cboMau.SelectedIndex = 0;

                cboKichThuoc.Items.Clear();
                cboKichThuoc.Items.Add(drv["KichThuoc"].ToString());
                cboKichThuoc.SelectedIndex = 0;
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
            decimal donGia = Convert.ToDecimal(drv["DonGia"]);

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
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvSanPham.SelectedRows[0];
                row.Cells["SoLuong"].Value = (int)nudSoLuong.Value;
                row.Cells["ThanhTien"].Value = (int)nudSoLuong.Value * Convert.ToDecimal(row.Cells["DonGia"].Value);
                TinhTongTien();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                dgvSanPham.Rows.RemoveAt(dgvSanPham.SelectedRows[0].Index);
                TinhTongTien();
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            dtGioHang.Rows.Clear();
            txtKhachHang.Clear();
            txtSDT.Clear();
            nudSoLuong.Value = 1;
            TaoMaHoaDonMoi();
            TinhTongTien();
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (dtGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Lưu Hóa Đơn
                    string insertHD = @"INSERT INTO HoaDon (MaHD, NgayBan, MaNV, TenKhachHang, SDT, TongTien) 
                                        VALUES (@MaHD, @NgayBan, @MaNV, @TenKhachHang, @SDT, @TongTien)";

                    SqlParameter[] pHD = {
                        new SqlParameter("@MaHD", txtMaHD.Text),
                        new SqlParameter("@NgayBan", dtpNgayBan.Value),
                        new SqlParameter("@MaNV", cboNhanVien.SelectedValue.ToString()),
                        new SqlParameter("@TenKhachHang", txtKhachHang.Text),
                        new SqlParameter("@SDT", txtSDT.Text),
                        new SqlParameter("@TongTien", decimal.Parse(txtTongTien.Text.Replace(".", "").Replace(",", "")))
                    };
                    DatabaseHelper.ExecuteNonQuery(insertHD, pHD, transaction);

                    // Lưu Chi Tiết Hóa Đơn
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
                    MessageBox.Show("Thanh toán thành công! Hóa đơn đã được lưu vào SQL.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BtnLamMoi_Click(null, null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Thanh toán thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnXuatHoaDon_Click(object sender, EventArgs e)
        {
            if (dtGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào để xuất hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin từ giao diện chính
            string maHD = txtMaHD.Text;
            string ngayBan = dtpNgayBan.Value.ToString("dd/MM/yyyy");
            string nhanVien = cboNhanVien.Text;
            string khachHang = txtKhachHang.Text;
            string sdt = txtSDT.Text;
            string tongTien = txtTongTien.Text;

            // Gọi Form Popup nhỏ đã lồng bên dưới (Chỉ xem, có nút IN HÓA ĐƠN và THOÁT)
            FrmXuatHoaDonPopup frm = new FrmXuatHoaDonPopup(maHD, ngayBan, nhanVien, khachHang, sdt, tongTien, dtGioHang);
            frm.ShowDialog();
        }
    }
}

// =========================================================================
// LỒNG FORM HÓA ĐƠN NHỎ VÀO CÙNG FILE FrmQuanLyHoaDonBanHang.cs
// =========================================================================
public class FrmXuatHoaDonPopup : Form
{
    private string maHD, ngayBan, nhanVien, khachHang, sdt, tongTien;
    private DataTable dtGioHang;

    public FrmXuatHoaDonPopup(string maHD, string ngayBan, string nhanVien, string khachHang, string sdt, string tongTien, DataTable dtGioHang)
    {
        this.maHD = maHD;
        this.ngayBan = ngayBan;
        this.nhanVien = nhanVien;
        this.khachHang = khachHang;
        this.sdt = sdt;
        this.tongTien = tongTien;
        this.dtGioHang = dtGioHang;

        TaoGiaoDienHoaDon();
    }

    private void TaoGiaoDienHoaDon()
    {
        this.Text = "HÓA ĐƠN BÁN HÀNG";
        this.Size = new Size(680, 580);
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
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));  // 1. Tiêu đề HÓA ĐƠN
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));  // 2. Thông tin khách hàng (TTKH)
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // 3. Toàn bộ bảng Sản Phẩm
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));  // 4. TỔNG TIỀN
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));  // 5. NÚT (IN HÓA ĐƠN, THOÁT)

        // --- 1. ĐẦU BẢNG: CHỮ HÓA ĐƠN ---
        Label lblHeader = new Label
        {
            Text = "HÓA ĐƠN BÁN HÀNG",
            Font = new Font("Arial", 16, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };

        // --- 2. TTKH (THÔNG TIN KHÁCH HÀNG & HÓA ĐƠN) ---
        GroupBox gbTTKH = new GroupBox
        {
            Text = "TTKH & Hóa Đơn",
            Font = new Font("Arial", 9, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(5)
        };

        Label lblTTKH = new Label
        {
            Text = $"Mã HD: {maHD}   |   Ngày bán: {ngayBan}\n" +
                   $"Khách hàng: {(string.IsNullOrWhiteSpace(khachHang) ? "Khách lẻ" : khachHang)}   |   SĐT: {(string.IsNullOrWhiteSpace(sdt) ? "Không có" : sdt)}\n" +
                   $"Nhân viên bán: {nhanVien}",
            Font = new Font("Arial", 9, FontStyle.Regular),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        gbTTKH.Controls.Add(lblTTKH);

        // --- 3. SẢN PHẨM (CHỈ XEM, KHÔNG CHO SỬA) ---
        DataGridView dgvSanPham = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true, // Khóa không cho sửa
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.Fixed3D
        };

        DataTable dtPrint = new DataTable();
        dtPrint.Columns.Add("Tên sản phẩm", typeof(string));
        dtPrint.Columns.Add("Màu", typeof(string));
        dtPrint.Columns.Add("Kích thước", typeof(string));
        dtPrint.Columns.Add("Số lượng", typeof(int));
        dtPrint.Columns.Add("Đơn giá", typeof(decimal));
        dtPrint.Columns.Add("Thành tiền", typeof(decimal));

        foreach (DataRow row in dtGioHang.Rows)
        {
            dtPrint.Rows.Add(
                row["TenSP"],
                row["Mau"],
                row["KichThuoc"],
                row["SoLuong"],
                row["DonGia"],
                row["ThanhTien"]
            );
        }
        dgvSanPham.DataSource = dtPrint;

        // Định dạng tiền tệ và độ rộng cột (kiểm tra null tránh lỗi văng app)
        if (dgvSanPham.Columns["Đơn giá"] != null)
        {
            dgvSanPham.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
            dgvSanPham.Columns["Đơn giá"].FillWeight = 100;
        }

        if (dgvSanPham.Columns["Thành tiền"] != null)
        {
            dgvSanPham.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
            dgvSanPham.Columns["Thành tiền"].FillWeight = 100;
        }

        if (dgvSanPham.Columns["Tên sản phẩm"] != null) dgvSanPham.Columns["Tên sản phẩm"].FillWeight = 160;
        if (dgvSanPham.Columns["Màu"] != null) dgvSanPham.Columns["Màu"].FillWeight = 70;
        if (dgvSanPham.Columns["Kích thước"] != null) dgvSanPham.Columns["Kích thước"].FillWeight = 70;
        if (dgvSanPham.Columns["Số lượng"] != null) dgvSanPham.Columns["Số lượng"].FillWeight = 60;

        // --- 4. TỔNG TIỀN ---
        Label lblTongTien = new Label
        {
            Text = $"TỔNG TIỀN THANH TOÁN: {tongTien} VNĐ",
            Font = new Font("Arial", 11, FontStyle.Bold),
            ForeColor = Color.Red,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight
        };

        // --- 5. CHỈ CÓ 2 NÚT: IN HÓA ĐƠN VÀ THOÁT ---
        FlowLayoutPanel flpButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 5, 0, 0)
        };

        Button btnThoat = new Button
        {
            Text = "THOÁT",
            Width = 100,
            Height = 35,
            BackColor = Color.LightGray,
            Font = new Font("Arial", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        Button btnIn = new Button
        {
            Text = "IN HÓA ĐƠN",
            Width = 120,
            Height = 35,
            BackColor = Color.DodgerBlue,
            ForeColor = Color.White,
            Font = new Font("Arial", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        btnThoat.Click += (s, e) => this.Close();
        btnIn.Click += (s, e) =>
        {
            MessageBox.Show("Đã gửi lệnh in hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

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