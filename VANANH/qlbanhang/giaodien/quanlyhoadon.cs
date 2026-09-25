using System;
using System.Drawing;
using System.Windows.Forms;

namespace qlbanhang
{
    public partial class quanlyhoadon : Form
    {
        // =====================================================
        // THÔNG TIN HÓA ĐƠN
        // =====================================================
        TextBox txtMaHoaDon;
        DateTimePicker dtpNgayBan;
        ComboBox cboNhanVien;

        // =====================================================
        // THÔNG TIN KHÁCH HÀNG
        // =====================================================
        TextBox txtTenKhachHang;
        TextBox txtSDT;

        // =====================================================
        // THÔNG TIN SẢN PHẨM
        // =====================================================
        ComboBox cboSanPham;
        ComboBox cboMau;
        ComboBox cboKichThuoc;
        NumericUpDown nudSoLuong;
        TextBox txtDonGia;

        // =====================================================
        // DANH MỤC SẢN PHẨM
        // =====================================================
        DataGridView dgvSanPham;

        // =====================================================
        // TỔNG TIỀN
        // =====================================================
        TextBox txtTongTien;

        // =====================================================
        // BUTTON
        // =====================================================
        Button btnThem;
        Button btnSua;
        Button btnXoa;
        Button btnLamMoi;
        Button btnThanhToan;


        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public quanlyhoadon()
        {
            InitializeComponent();

            TaoGiaoDien();

            LoadDuLieuMau();
        }


        // =====================================================
        // TẠO GIAO DIỆN
        // =====================================================
        private void TaoGiaoDien()
        {
            // =================================================
            // FORM
            // =================================================
            this.Text = "QUẢN LÝ HÓA ĐƠN";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;


            // =================================================
            // TIÊU ĐỀ
            // =================================================
            Label lblTieuDe = new Label();

            lblTieuDe.Text = "QUẢN LÝ HÓA ĐƠN";
            lblTieuDe.Font = new Font(
                "Arial",
                20,
                FontStyle.Bold
            );

            lblTieuDe.ForeColor = Color.DarkBlue;
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(390, 20);

            this.Controls.Add(lblTieuDe);


            // =================================================
            // THÔNG TIN HÓA ĐƠN
            // =================================================
            GroupBox grpHoaDon = new GroupBox();

            grpHoaDon.Text = "THÔNG TIN HÓA ĐƠN";
            grpHoaDon.Font = new Font(
                "Arial",
                10,
                FontStyle.Bold
            );

            grpHoaDon.Location = new Point(20, 60);
            grpHoaDon.Size = new Size(1040, 110);

            this.Controls.Add(grpHoaDon);


            // =================================================
            // MÃ HÓA ĐƠN
            // =================================================
            Label lblMaHD = new Label();

            lblMaHD.Text = "Mã hóa đơn:";
            lblMaHD.Location = new Point(20, 30);
            lblMaHD.AutoSize = true;

            txtMaHoaDon = new TextBox();

            txtMaHoaDon.Location = new Point(110, 27);
            txtMaHoaDon.Size = new Size(180, 25);


            // =================================================
            // NGÀY BÁN
            // =================================================
            Label lblNgayBan = new Label();

            lblNgayBan.Text = "Ngày bán:";
            lblNgayBan.Location = new Point(330, 30);
            lblNgayBan.AutoSize = true;

            dtpNgayBan = new DateTimePicker();

            dtpNgayBan.Location = new Point(405, 27);
            dtpNgayBan.Size = new Size(180, 25);
            dtpNgayBan.Format = DateTimePickerFormat.Short;


            // =================================================
            // NHÂN VIÊN
            // =================================================
            Label lblNhanVien = new Label();

            lblNhanVien.Text = "Nhân viên bán:";
            lblNhanVien.Location = new Point(620, 30);
            lblNhanVien.AutoSize = true;

            cboNhanVien = new ComboBox();

            cboNhanVien.Location = new Point(720, 27);
            cboNhanVien.Size = new Size(250, 25);

            cboNhanVien.DropDownStyle =
                ComboBoxStyle.DropDownList;


            // =================================================
            // KHÁCH HÀNG
            // =================================================
            Label lblKhachHang = new Label();

            lblKhachHang.Text = "Khách hàng:";
            lblKhachHang.Location = new Point(20, 70);
            lblKhachHang.AutoSize = true;

            txtTenKhachHang = new TextBox();

            txtTenKhachHang.Location = new Point(110, 67);
            txtTenKhachHang.Size = new Size(250, 25);


            // =================================================
            // SĐT
            // =================================================
            Label lblSDT = new Label();

            lblSDT.Text = "SĐT:";
            lblSDT.Location = new Point(400, 70);
            lblSDT.AutoSize = true;

            txtSDT = new TextBox();

            txtSDT.Location = new Point(440, 67);
            txtSDT.Size = new Size(180, 25);


            // Thêm control vào GroupBox
            grpHoaDon.Controls.Add(lblMaHD);
            grpHoaDon.Controls.Add(txtMaHoaDon);

            grpHoaDon.Controls.Add(lblNgayBan);
            grpHoaDon.Controls.Add(dtpNgayBan);

            grpHoaDon.Controls.Add(lblNhanVien);
            grpHoaDon.Controls.Add(cboNhanVien);

            grpHoaDon.Controls.Add(lblKhachHang);
            grpHoaDon.Controls.Add(txtTenKhachHang);

            grpHoaDon.Controls.Add(lblSDT);
            grpHoaDon.Controls.Add(txtSDT);


            // =================================================
            // THÔNG TIN SẢN PHẨM
            // =================================================
            GroupBox grpSanPham = new GroupBox();

            grpSanPham.Text = "THÔNG TIN SẢN PHẨM";
            grpSanPham.Font = new Font(
                "Arial",
                10,
                FontStyle.Bold
            );

            grpSanPham.Location = new Point(20, 180);
            grpSanPham.Size = new Size(1040, 130);

            this.Controls.Add(grpSanPham);


            // =================================================
            // SẢN PHẨM
            // =================================================
            Label lblSanPham = new Label();

            lblSanPham.Text = "Sản phẩm:";
            lblSanPham.Location = new Point(20, 35);
            lblSanPham.AutoSize = true;

            cboSanPham = new ComboBox();

            cboSanPham.Location = new Point(100, 32);
            cboSanPham.Size = new Size(200, 25);

            cboSanPham.DropDownStyle =
                ComboBoxStyle.DropDownList;


            // =================================================
            // MÀU
            // =================================================
            Label lblMau = new Label();

            lblMau.Text = "Màu:";
            lblMau.Location = new Point(330, 35);
            lblMau.AutoSize = true;

            cboMau = new ComboBox();

            cboMau.Location = new Point(370, 32);
            cboMau.Size = new Size(120, 25);

            cboMau.DropDownStyle =
                ComboBoxStyle.DropDownList;


            // =================================================
            // KÍCH THƯỚC
            // =================================================
            Label lblKichThuoc = new Label();

            lblKichThuoc.Text = "Kích thước:";
            lblKichThuoc.Location = new Point(520, 35);
            lblKichThuoc.AutoSize = true;

            cboKichThuoc = new ComboBox();

            cboKichThuoc.Location = new Point(605, 32);
            cboKichThuoc.Size = new Size(100, 25);

            cboKichThuoc.DropDownStyle =
                ComboBoxStyle.DropDownList;


            // =================================================
            // SỐ LƯỢNG
            // =================================================
            Label lblSoLuong = new Label();

            lblSoLuong.Text = "Số lượng:";
            lblSoLuong.Location = new Point(730, 35);
            lblSoLuong.AutoSize = true;

            nudSoLuong = new NumericUpDown();

            nudSoLuong.Location = new Point(800, 32);
            nudSoLuong.Size = new Size(80, 25);

            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 1000;
            nudSoLuong.Value = 1;


            // =================================================
            // ĐƠN GIÁ
            // =================================================
            Label lblDonGia = new Label();

            lblDonGia.Text = "Đơn giá:";
            lblDonGia.Location = new Point(20, 80);
            lblDonGia.AutoSize = true;

            txtDonGia = new TextBox();

            txtDonGia.Location = new Point(100, 77);
            txtDonGia.Size = new Size(200, 25);


            // =================================================
            // NÚT THÊM
            // =================================================
            btnThem = new Button();

            btnThem.Text = "THÊM SẢN PHẨM";
            btnThem.Location = new Point(330, 75);
            btnThem.Size = new Size(150, 35);

            btnThem.BackColor = Color.LightGreen;

            btnThem.Click += btnThem_Click;


            // Sự kiện chọn sản phẩm
            cboSanPham.SelectedIndexChanged +=
                cboSanPham_SelectedIndexChanged;


            // Thêm control vào GroupBox
            grpSanPham.Controls.Add(lblSanPham);
            grpSanPham.Controls.Add(cboSanPham);

            grpSanPham.Controls.Add(lblMau);
            grpSanPham.Controls.Add(cboMau);

            grpSanPham.Controls.Add(lblKichThuoc);
            grpSanPham.Controls.Add(cboKichThuoc);

            grpSanPham.Controls.Add(lblSoLuong);
            grpSanPham.Controls.Add(nudSoLuong);

            grpSanPham.Controls.Add(lblDonGia);
            grpSanPham.Controls.Add(txtDonGia);

            grpSanPham.Controls.Add(btnThem);


            // =================================================
            // DANH MỤC SẢN PHẨM
            // =================================================
            GroupBox grpDanhMuc = new GroupBox();

            grpDanhMuc.Text = "DANH MỤC SẢN PHẨM";

            grpDanhMuc.Font = new Font(
                "Arial",
                10,
                FontStyle.Bold
            );

            grpDanhMuc.Location = new Point(20, 320);
            grpDanhMuc.Size = new Size(1040, 210);

            this.Controls.Add(grpDanhMuc);


            // =================================================
            // DATAGRIDVIEW
            // =================================================
            dgvSanPham = new DataGridView();

            dgvSanPham.Location = new Point(15, 25);
            dgvSanPham.Size = new Size(1010, 170);

            dgvSanPham.AllowUserToAddRows = false;

            dgvSanPham.ReadOnly = false;

            dgvSanPham.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvSanPham.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvSanPham.MultiSelect = false;

            dgvSanPham.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Arial",
                    9,
                    FontStyle.Bold
                );


            // Các cột
            dgvSanPham.Columns.Add(
                "MaHD",
                "Mã Hóa Đơn"
            );

            dgvSanPham.Columns.Add(
                "TenSP",
                "Tên sản phẩm"
            );

            dgvSanPham.Columns.Add(
                "Mau",
                "Màu"
            );

            dgvSanPham.Columns.Add(
                "KichThuoc",
                "Kích thước"
            );

            dgvSanPham.Columns.Add(
                "SoLuong",
                "Số lượng"
            );

            dgvSanPham.Columns.Add(
                "DonGia",
                "Đơn giá"
            );

            dgvSanPham.Columns.Add(
                "ThanhTien",
                "Thành tiền"
            );


            grpDanhMuc.Controls.Add(dgvSanPham);


            // =================================================
            // NÚT SỬA
            // =================================================
            btnSua = new Button();

            btnSua.Text = "SỬA";
            btnSua.Location = new Point(20, 545);
            btnSua.Size = new Size(100, 35);

            btnSua.Click += btnSua_Click;


            // =================================================
            // NÚT XÓA
            // =================================================
            btnXoa = new Button();

            btnXoa.Text = "XÓA";
            btnXoa.Location = new Point(130, 545);
            btnXoa.Size = new Size(100, 35);

            btnXoa.Click += btnXoa_Click;


            // =================================================
            // NÚT LÀM MỚI
            // =================================================
            btnLamMoi = new Button();

            btnLamMoi.Text = "LÀM MỚI";
            btnLamMoi.Location = new Point(240, 545);
            btnLamMoi.Size = new Size(100, 35);

            btnLamMoi.Click += btnLamMoi_Click;


            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);


            // =================================================
            // TỔNG TIỀN
            // =================================================
            Label lblTongTien = new Label();

            lblTongTien.Text =
                "TỔNG TIỀN THANH TOÁN:";

            lblTongTien.Font = new Font(
                "Arial",
                11,
                FontStyle.Bold
            );

            lblTongTien.Location =
                new Point(620, 550);

            lblTongTien.AutoSize = true;


            txtTongTien = new TextBox();

            txtTongTien.Location =
                new Point(810, 547);

            txtTongTien.Size =
                new Size(180, 30);

            txtTongTien.Font =
                new Font(
                    "Arial",
                    11,
                    FontStyle.Bold
                );

            txtTongTien.TextAlign =
                HorizontalAlignment.Right;

            txtTongTien.ReadOnly = true;

            txtTongTien.Text = "0";


            this.Controls.Add(lblTongTien);
            this.Controls.Add(txtTongTien);


            // =================================================
            // THANH TOÁN
            // =================================================
            btnThanhToan = new Button();

            btnThanhToan.Text =
                "THANH TOÁN";

            btnThanhToan.Font =
                new Font(
                    "Arial",
                    11,
                    FontStyle.Bold
                );

            btnThanhToan.ForeColor =
                Color.White;

            btnThanhToan.BackColor =
                Color.DodgerBlue;

            btnThanhToan.Location =
                new Point(810, 590);

            btnThanhToan.Size =
                new Size(180, 45);

            btnThanhToan.Click +=
                btnThanhToan_Click;

            this.Controls.Add(btnThanhToan);
        }


        // =====================================================
        // DỮ LIỆU MẪU
        // =====================================================
        private void LoadDuLieuMau()
        {
            // -------------------------
            // NHÂN VIÊN
            // -------------------------
            cboNhanVien.Items.Clear();

            cboNhanVien.Items.Add(
                "NV01 - Nguyễn Văn An");

            cboNhanVien.Items.Add(
                "NV02 - Trần Thị Bình");

            cboNhanVien.Items.Add(
                "NV03 - Lê Văn Cường");

            cboNhanVien.Items.Add(
                "NV04 - Phạm Thị Dung");


            // -------------------------
            // SẢN PHẨM
            // -------------------------
            cboSanPham.Items.Clear();

            cboSanPham.Items.Add(
                "Áo sơ mi");

            cboSanPham.Items.Add(
                "Quần jean");

            cboSanPham.Items.Add(
                "Áo phông");

            cboSanPham.Items.Add(
                "Váy");

            cboSanPham.Items.Add(
                "Áo khoác");

            cboSanPham.Items.Add(
                "Quần kaki");

            cboSanPham.Items.Add(
                "Giày thể thao");

            cboSanPham.Items.Add(
                "Áo len");


            // -------------------------
            // MÀU
            // -------------------------
            cboMau.Items.Clear();

            cboMau.Items.Add("Đen");
            cboMau.Items.Add("Trắng");
            cboMau.Items.Add("Đỏ");
            cboMau.Items.Add("Xanh");
            cboMau.Items.Add("Vàng");
            cboMau.Items.Add("Hồng");
            cboMau.Items.Add("Nâu");


            // -------------------------
            // KÍCH THƯỚC
            // -------------------------
            cboKichThuoc.Items.Clear();

            cboKichThuoc.Items.Add("S");
            cboKichThuoc.Items.Add("M");
            cboKichThuoc.Items.Add("L");
            cboKichThuoc.Items.Add("XL");
            cboKichThuoc.Items.Add("XXL");


            // -------------------------
            // THÔNG TIN HÓA ĐƠN DEMO
            // -------------------------
            txtMaHoaDon.Text = "HD001";

            txtTenKhachHang.Text =
                "Nguyễn Minh Anh";

            txtSDT.Text =
                "0987654321";

            dtpNgayBan.Value =
                DateTime.Now;


            // -------------------------
            // CHỌN MẶC ĐỊNH
            // -------------------------
            cboNhanVien.SelectedIndex = 0;

            cboSanPham.SelectedIndex = 0;

            cboMau.SelectedIndex = 0;

            cboKichThuoc.SelectedIndex = 1;

            nudSoLuong.Value = 1;


            // Đơn giá
            txtDonGia.Text = "250000";
        }


        // =====================================================
        // LẤY ĐƠN GIÁ THEO SẢN PHẨM
        // =====================================================
        private decimal LayDonGia(
            string tenSanPham)
        {
            switch (tenSanPham)
            {
                case "Áo sơ mi":
                    return 250000;

                case "Quần jean":
                    return 350000;

                case "Áo phông":
                    return 150000;

                case "Váy":
                    return 300000;

                case "Áo khoác":
                    return 450000;

                case "Quần kaki":
                    return 300000;

                case "Giày thể thao":
                    return 500000;

                case "Áo len":
                    return 280000;

                default:
                    return 0;
            }
        }


        // =====================================================
        // CHỌN SẢN PHẨM
        // =====================================================
        private void cboSanPham_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cboSanPham.SelectedIndex == -1)
                return;

            string tenSanPham =
                cboSanPham.Text;

            decimal donGia =
                LayDonGia(tenSanPham);

            txtDonGia.Text =
                donGia.ToString("N0");
        }


        // =====================================================
        // THÊM SẢN PHẨM
        // =====================================================
        private void btnThem_Click(
            object sender,
            EventArgs e)
        {
            // Kiểm tra mã hóa đơn
            if (string.IsNullOrWhiteSpace(
                txtMaHoaDon.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập mã hóa đơn!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaHoaDon.Focus();
                return;
            }


            // Kiểm tra nhân viên
            if (cboNhanVien.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn nhân viên!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // Kiểm tra khách hàng
            if (string.IsNullOrWhiteSpace(
                txtTenKhachHang.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên khách hàng!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenKhachHang.Focus();
                return;
            }


            // Kiểm tra sản phẩm
            if (cboSanPham.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // Kiểm tra màu
            if (cboMau.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn màu!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // Kiểm tra kích thước
            if (cboKichThuoc.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn kích thước!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // Lấy đơn giá
            decimal donGia;

            string giaText =
                txtDonGia.Text.Replace(",", "");

            if (!decimal.TryParse(
                giaText,
                out donGia))
            {
                MessageBox.Show(
                    "Đơn giá không hợp lệ!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDonGia.Focus();
                return;
            }


            // Số lượng
            int soLuong =
                (int)nudSoLuong.Value;


            if (soLuong <= 0)
            {
                MessageBox.Show(
                    "Số lượng phải lớn hơn 0!");

                return;
            }


            // Thành tiền
            decimal thanhTien =
                donGia * soLuong;


            // Thêm vào bảng
            dgvSanPham.Rows.Add(
                txtMaHoaDon.Text,
                cboSanPham.Text,
                cboMau.Text,
                cboKichThuoc.Text,
                soLuong,
                donGia.ToString("N0"),
                thanhTien.ToString("N0")
            );


            // Tính tổng
            TinhTongTien();


            MessageBox.Show(
                "Đã thêm sản phẩm!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }


        // =====================================================
        // TÍNH TỔNG TIỀN
        // =====================================================
        private void TinhTongTien()
        {
            decimal tongTien = 0;


            foreach (
                DataGridViewRow row
                in dgvSanPham.Rows)
            {
                if (row.Cells["ThanhTien"].Value == null)
                    continue;


                string value =
                    row.Cells["ThanhTien"]
                    .Value
                    .ToString()
                    .Replace(",", "")
                    .Replace(".", "");


                decimal thanhTien;


                if (decimal.TryParse(
                    value,
                    out thanhTien))
                {
                    tongTien += thanhTien;
                }
            }


            txtTongTien.Text =
                tongTien.ToString("N0");
        }


        // =====================================================
        // CHỌN DÒNG TRÊN BẢNG
        // =====================================================
        private void dgvSanPham_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            DataGridViewRow row =
                dgvSanPham.Rows[e.RowIndex];


            // Mã hóa đơn
            if (row.Cells["MaHD"].Value != null)
            {
                txtMaHoaDon.Text =
                    row.Cells["MaHD"].Value.ToString();
            }


            // Sản phẩm
            if (row.Cells["TenSP"].Value != null)
            {
                string tenSP =
                    row.Cells["TenSP"].Value.ToString();

                int index =
                    cboSanPham.Items.IndexOf(tenSP);

                if (index >= 0)
                    cboSanPham.SelectedIndex = index;
            }


            // Màu
            if (row.Cells["Mau"].Value != null)
            {
                string mau =
                    row.Cells["Mau"].Value.ToString();

                int index =
                    cboMau.Items.IndexOf(mau);

                if (index >= 0)
                    cboMau.SelectedIndex = index;
            }


            // Kích thước
            if (row.Cells["KichThuoc"].Value != null)
            {
                string kichThuoc =
                    row.Cells["KichThuoc"]
                    .Value
                    .ToString();

                int index =
                    cboKichThuoc.Items.IndexOf(
                        kichThuoc);

                if (index >= 0)
                    cboKichThuoc.SelectedIndex =
                        index;
            }


            // Số lượng
            if (row.Cells["SoLuong"].Value != null)
            {
                int soLuong;

                if (int.TryParse(
                    row.Cells["SoLuong"]
                    .Value
                    .ToString(),
                    out soLuong))
                {
                    if (soLuong >= 1 &&
                        soLuong <= 1000)
                    {
                        nudSoLuong.Value =
                            soLuong;
                    }
                }
            }


            // Đơn giá
            if (row.Cells["DonGia"].Value != null)
            {
                txtDonGia.Text =
                    row.Cells["DonGia"]
                    .Value
                    .ToString();
            }
        }


        // =====================================================
        // SỬA SẢN PHẨM
        // =====================================================
        private void btnSua_Click(
            object sender,
            EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm cần sửa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // Kiểm tra sản phẩm
            if (cboSanPham.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm!");

                return;
            }


            // Lấy đơn giá
            decimal donGia;

            string giaText =
                txtDonGia.Text
                .Replace(",", "")
                .Replace(".", "");


            if (!decimal.TryParse(
                giaText,
                out donGia))
            {
                MessageBox.Show(
                    "Đơn giá không hợp lệ!");

                return;
            }


            int soLuong =
                (int)nudSoLuong.Value;


            decimal thanhTien =
                donGia * soLuong;


            int index =
                dgvSanPham.SelectedRows[0].Index;


            DataGridViewRow row =
                dgvSanPham.Rows[index];


            row.Cells["MaHD"].Value =
                txtMaHoaDon.Text;

            row.Cells["TenSP"].Value =
                cboSanPham.Text;

            row.Cells["Mau"].Value =
                cboMau.Text;

            row.Cells["KichThuoc"].Value =
                cboKichThuoc.Text;

            row.Cells["SoLuong"].Value =
                soLuong;

            row.Cells["DonGia"].Value =
                donGia.ToString("N0");

            row.Cells["ThanhTien"].Value =
                thanhTien.ToString("N0");


            TinhTongTien();


            MessageBox.Show(
                "Đã sửa sản phẩm!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }


        // =====================================================
        // XÓA SẢN PHẨM
        // =====================================================
        private void btnXoa_Click(
            object sender,
            EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm cần xóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            DialogResult result =
                MessageBox.Show(
                    "Bạn có chắc muốn xóa sản phẩm này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {
                int index =
                    dgvSanPham.SelectedRows[0].Index;


                dgvSanPham.Rows.RemoveAt(index);


                TinhTongTien();


                MessageBox.Show(
                    "Đã xóa sản phẩm!");
            }
        }


        // =====================================================
        // LÀM MỚI
        // =====================================================
        private void btnLamMoi_Click(
            object sender,
            EventArgs e)
        {
            DialogResult result =
                MessageBox.Show(
                    "Bạn có muốn tạo hóa đơn mới?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


            if (result != DialogResult.Yes)
                return;


            // Xóa bảng
            dgvSanPham.Rows.Clear();


            // Mã hóa đơn mới
            txtMaHoaDon.Text =
                TaoMaHoaDonMoi();


            // Ngày hiện tại
            dtpNgayBan.Value =
                DateTime.Now;


            // Khách hàng
            txtTenKhachHang.Clear();

            txtSDT.Clear();


            // Sản phẩm
            cboNhanVien.SelectedIndex = 0;

            cboSanPham.SelectedIndex = 0;

            cboMau.SelectedIndex = 0;

            cboKichThuoc.SelectedIndex = 1;


            // Số lượng
            nudSoLuong.Value = 1;


            // Tổng tiền
            txtTongTien.Text = "0";


            MessageBox.Show(
                "Đã tạo hóa đơn mới!");
        }


        // =====================================================
        // TẠO MÃ HÓA ĐƠN MỚI
        // =====================================================
        private string TaoMaHoaDonMoi()
        {
            Random random =
                new Random();


            int so =
                random.Next(1, 999);


            return "HD" +
                so.ToString("000");
        }


        // =====================================================
        // THANH TOÁN
        // =====================================================
        private void btnThanhToan_Click(
            object sender,
            EventArgs e)
        {
            // Kiểm tra sản phẩm
            if (dgvSanPham.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Hóa đơn chưa có sản phẩm!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // Kiểm tra khách hàng
            if (string.IsNullOrWhiteSpace(
                txtTenKhachHang.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên khách hàng!");

                return;
            }


            // Hiển thị hóa đơn
            string thongBao =
                "THANH TOÁN THÀNH CÔNG!\n\n" +

                "Mã hóa đơn: " +
                txtMaHoaDon.Text +

                "\nNgày bán: " +
                dtpNgayBan.Value.ToString(
                    "dd/MM/yyyy") +

                "\nNhân viên: " +
                cboNhanVien.Text +

                "\nKhách hàng: " +
                txtTenKhachHang.Text +

                "\nSĐT: " +
                txtSDT.Text +

                "\n\nTổng tiền: " +
                txtTongTien.Text +
                " VNĐ";


            MessageBox.Show(
                thongBao,
                "THANH TOÁN",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
