using FORM_DKY;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class FrmSanPham : Form
    {
        private string duongDanAnh = "";

        public FrmSanPham()
        {
            InitializeComponent();
            TaoGiaoDienResponsive();
            TaiDanhSachSanPham();
        }

        #region 1. TỰ ĐỘNG TẠO GIAO DIỆN RESPONSIVE (DOCK & TABLELAYOUTPANEL)

        private TableLayoutPanel tlpMain = null!;
        private TableLayoutPanel tlpInput = null!;
        private FlowLayoutPanel flpButtons = null!;
        private DataGridView dgvSanPham = null!;

        private TextBox txtMaSP = null!;
        private TextBox txtTenSP = null!;
        private ComboBox cboLoaiSP = null!;
        private ComboBox cboMauSac = null!;
        private ComboBox cboSize = null!;
        private NumericUpDown nudGiaNhap = null!;
        private NumericUpDown nudGiaBan = null!;
        private NumericUpDown nudSoLuong = null!;
        private TextBox txtGhiChu = null!;
        private TextBox txtTimKiem = null!;

        private PictureBox picHinhAnh = null!;
        private Button btnChonAnh = null!;
        private Button btnSua = null!;
        private Button btnLamMoi = null!;

        private void TaoGiaoDienResponsive()
        {
            this.Text = "QUẢN LÝ SẢN PHẨM PHẦN MỀM THỜI TRANG";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ DANH MỤC SẢN PHẨM",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40
            };
            tlpMain.Controls.Add(lblTitle, 0, 0);

            // GroupBox chứa khung nhập thông tin
            GroupBox gbThongTin = new GroupBox
            {
                Text = "Thông tin sản phẩm",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };

            tlpInput = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 5,
                RowCount = 4
            };
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));

            for (int i = 0; i < 4; i++)
            {
                tlpInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            }

            // Các trường khóa lại không cho sửa trực tiếp (gắn liền với phiếu nhập)
            txtMaSP = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.WhiteSmoke };
            txtTenSP = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.WhiteSmoke };

            cboLoaiSP = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown };
            cboLoaiSP.Items.AddRange(new object[] { "Áo thun", "Áo sơ mi", "Quần jean", "Quần short", "Áo khoác", "Váy/Đầm" });

            cboMauSac = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown, Enabled = false };
            cboMauSac.Items.AddRange(new object[] { "Đen", "Trắng", "Xanh", "Đỏ", "Vàng", "Xám", "Nâu" });

            cboSize = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown, Enabled = false };
            cboSize.Items.AddRange(new object[] { "S", "M", "L", "XL", "XXL", "28", "29", "30", "31", "32" });

            nudGiaNhap = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 1000000000, Increment = 10000, ThousandsSeparator = true, Enabled = false };

            // Các trường cho phép chỉnh sửa
            nudGiaBan = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 1000000000, Increment = 10000, ThousandsSeparator = true };
            nudSoLuong = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 100000, Value = 1, Enabled = false };

            txtGhiChu = new TextBox { Dock = DockStyle.Fill };

            picHinhAnh = new PictureBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                Height = 110
            };

            btnChonAnh = new Button { Text = "Chọn Ảnh...", Dock = DockStyle.Bottom, Height = 28 };
            btnChonAnh.Click += BtnChonAnh_Click;

            Panel pnlAnh = new Panel { Dock = DockStyle.Fill };
            pnlAnh.Controls.Add(picHinhAnh);
            pnlAnh.Controls.Add(btnChonAnh);

            // Dòng 0
            tlpInput.Controls.Add(new Label { Text = "Mã SP:", Anchor = AnchorStyles.Left }, 0, 0);
            tlpInput.Controls.Add(txtMaSP, 1, 0);
            tlpInput.Controls.Add(new Label { Text = "Giá Nhập:", Anchor = AnchorStyles.Left }, 2, 0);
            tlpInput.Controls.Add(nudGiaNhap, 3, 0);
            tlpInput.Controls.Add(pnlAnh, 4, 0);
            tlpInput.SetRowSpan(pnlAnh, 4);

            // Dòng 1
            tlpInput.Controls.Add(new Label { Text = "Tên SP:", Anchor = AnchorStyles.Left }, 0, 1);
            tlpInput.Controls.Add(txtTenSP, 1, 1);
            tlpInput.Controls.Add(new Label { Text = "Giá Bán:", Anchor = AnchorStyles.Left }, 2, 1);
            tlpInput.Controls.Add(nudGiaBan, 3, 1);

            // Dòng 2
            tlpInput.Controls.Add(new Label { Text = "Loại SP:", Anchor = AnchorStyles.Left }, 0, 2);
            tlpInput.Controls.Add(cboLoaiSP, 1, 2);
            tlpInput.Controls.Add(new Label { Text = "Số Lượng:", Anchor = AnchorStyles.Left }, 2, 2);
            tlpInput.Controls.Add(nudSoLuong, 3, 2);

            // Dòng 3
            TableLayoutPanel tlpMauSize = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, Margin = new Padding(0) };
            tlpMauSize.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tlpMauSize.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 35F));
            tlpMauSize.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));

            cboMauSac.Margin = new Padding(0, 0, 2, 0);
            cboSize.Margin = new Padding(2, 0, 0, 0);

            tlpMauSize.Controls.Add(cboMauSac, 0, 0);
            tlpMauSize.Controls.Add(cboSize, 2, 0);

            tlpInput.Controls.Add(new Label { Text = "Màu / Size:", Anchor = AnchorStyles.Left }, 0, 3);
            tlpInput.Controls.Add(tlpMauSize, 1, 3);
            tlpInput.Controls.Add(new Label { Text = "Ghi Chú:", Anchor = AnchorStyles.Left }, 2, 3);
            tlpInput.Controls.Add(txtGhiChu, 3, 3);

            gbThongTin.Controls.Add(tlpInput);
            tlpMain.Controls.Add(gbThongTin, 0, 1);

            // Panel chứa nút và ô tìm kiếm
            FlowLayoutPanel flpBottomControl = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 5, 0, 5)
            };

            flpButtons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0, 0, 20, 0) };
            btnSua = new Button { Text = "Cập Nhật", Width = 100, Height = 32, BackColor = Color.LightSkyBlue };
            btnLamMoi = new Button { Text = "Làm Mới", Width = 100, Height = 32 };

            btnSua.Click += BtnSua_Click;
            btnLamMoi.Click += (s, e) => { TaiDanhSachSanPham(); XoaTrangForm(); };
            flpButtons.Controls.AddRange(new Control[] { btnSua, btnLamMoi });

            FlowLayoutPanel flpSearch = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            Label lblTim = new Label { Text = "Tìm kiếm:", AutoSize = true, Margin = new Padding(0, 8, 5, 0) };
            txtTimKiem = new TextBox { Width = 250, Height = 25 };
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            flpSearch.Controls.AddRange(new Control[] { lblTim, txtTimKiem });

            flpBottomControl.Controls.Add(flpButtons);
            flpBottomControl.Controls.Add(flpSearch);

            Panel pnlWrapper = new Panel { Dock = DockStyle.Top, AutoSize = true };
            pnlWrapper.Controls.Add(flpBottomControl);
            tlpMain.Controls.Add(pnlWrapper, 0, 2);

            // DataGridView
            dgvSanPham = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White
            };
            dgvSanPham.CellClick += DgvSanPham_CellClick;
            dgvSanPham.CellFormatting += DgvSanPham_CellFormatting;

            tlpMain.Controls.Add(dgvSanPham, 0, 3);
            this.Controls.Add(tlpMain);
        }

        #endregion

        #region 2. TẢI VÀ HIỂN THỊ DỮ LIỆU

        private void TaiDanhSachSanPham(string tuKhoa = "")
        {
            string query = "SELECT MaSP, TenSP, LoaiSP, MauSac, Size, GiaNhap, GiaBan, SoLuongTon, GhiChu, HinhAnh FROM SanPham";
            SqlParameter[]? parameters = null;

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query += " WHERE MaSP LIKE @Keyword OR TenSP LIKE @Keyword OR LoaiSP LIKE @Keyword";
                parameters = new SqlParameter[] { new SqlParameter("@Keyword", "%" + tuKhoa + "%") };
            }

            DataTable dt = Database.GetData(query, parameters);
            dgvSanPham.DataSource = dt;

            dgvSanPham.CellClick -= DgvSanPham_CellClick;
            dgvSanPham.CellClick += DgvSanPham_CellClick;

            if (dgvSanPham.Columns["MaSP"] != null) dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
            if (dgvSanPham.Columns["TenSP"] != null) dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
            if (dgvSanPham.Columns["LoaiSP"] != null) dgvSanPham.Columns["LoaiSP"].HeaderText = "Loại";
            if (dgvSanPham.Columns["MauSac"] != null) dgvSanPham.Columns["MauSac"].HeaderText = "Màu";
            if (dgvSanPham.Columns["Size"] != null) dgvSanPham.Columns["Size"].HeaderText = "Size";

            if (dgvSanPham.Columns["GiaNhap"] != null)
            {
                dgvSanPham.Columns["GiaNhap"].HeaderText = "Giá Nhập";
                dgvSanPham.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            }
            if (dgvSanPham.Columns["GiaBan"] != null)
            {
                dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán";
                dgvSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            }

            if (dgvSanPham.Columns["SoLuongTon"] != null) dgvSanPham.Columns["SoLuongTon"].HeaderText = "Số Lượng";
            if (dgvSanPham.Columns["GhiChu"] != null) dgvSanPham.Columns["GhiChu"].HeaderText = "Ghi Chú";
            if (dgvSanPham.Columns["HinhAnh"] != null) dgvSanPham.Columns["HinhAnh"].Visible = false;
        }

        private void DgvSanPham_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "SoLuongTon" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int soLuong) && soLuong <= 5)
                {
                    dgvSanPham.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                    dgvSanPham.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void DgvSanPham_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSanPham.Rows[e.RowIndex].DataBoundItem is DataRowView drv)
            {
                txtMaSP.Text = drv["MaSP"]?.ToString() ?? "";
                txtTenSP.Text = drv["TenSP"]?.ToString() ?? "";

                string loai = drv["LoaiSP"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(loai))
                {
                    if (!cboLoaiSP.Items.Contains(loai)) cboLoaiSP.Items.Add(loai);
                    cboLoaiSP.Text = loai;
                }
                else { cboLoaiSP.SelectedIndex = -1; }

                string mau = drv["MauSac"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(mau))
                {
                    if (!cboMauSac.Items.Contains(mau)) cboMauSac.Items.Add(mau);
                    cboMauSac.Text = mau;
                }
                else { cboMauSac.SelectedIndex = -1; }

                string size = drv["Size"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(size))
                {
                    if (!cboSize.Items.Contains(size)) cboSize.Items.Add(size);
                    cboSize.Text = size;
                }
                else { cboSize.SelectedIndex = -1; }

                decimal.TryParse(drv["GiaNhap"]?.ToString(), out decimal giaNhap);
                nudGiaNhap.Value = giaNhap;

                decimal.TryParse(drv["GiaBan"]?.ToString(), out decimal giaBan);
                nudGiaBan.Value = giaBan;

                int.TryParse(drv["SoLuongTon"]?.ToString(), out int soLuong);
                nudSoLuong.Value = soLuong;

                txtGhiChu.Text = drv["GhiChu"]?.ToString() ?? "";

                duongDanAnh = drv["HinhAnh"]?.ToString() ?? "";
                HienThiAnh(duongDanAnh);
            }
        }

        private void HienThiAnh(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        picHinhAnh.Image = Image.FromStream(fs);
                    }
                }
                catch
                {
                    picHinhAnh.Image = null;
                }
            }
            else
            {
                picHinhAnh.Image = null;
            }
        }

        private void XoaTrangForm()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = -1;
            cboMauSac.SelectedIndex = -1;
            cboSize.SelectedIndex = -1;
            nudGiaNhap.Value = 0;
            nudGiaBan.Value = 0;
            nudSoLuong.Value = 1;
            txtGhiChu.Clear();
            duongDanAnh = "";
            picHinhAnh.Image = null;
        }

        #endregion

        #region 3. THAO TÁC CẬP NHẬT, ẢNH, TÌM KIẾM

        private void BtnChonAnh_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "File Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    duongDanAnh = ofd.FileName;
                    HienThiAnh(duongDanAnh);
                }
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chỉ cho phép cập nhật các thuộc tính linh hoạt: Loại SP, Giá Bán, Ghi Chú, Hình Ảnh
            string query = @"UPDATE SanPham 
                            SET LoaiSP = @LoaiSP, 
                                GiaBan = @GiaBan, 
                                GhiChu = @GhiChu, 
                                HinhAnh = @HinhAnh 
                            WHERE MaSP = @MaSP";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSP", txtMaSP.Text.Trim()),
                new SqlParameter("@LoaiSP", (object?)cboLoaiSP.Text ?? DBNull.Value),
                new SqlParameter("@GiaBan", nudGiaBan.Value),
                new SqlParameter("@GhiChu", (object?)txtGhiChu.Text ?? DBNull.Value),
                new SqlParameter("@HinhAnh", (object?)duongDanAnh ?? DBNull.Value)
            };

            if (Database.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                XoaTrangForm();
            }
        }

        private void TxtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            TaiDanhSachSanPham(txtTimKiem.Text.Trim());
        }

        #endregion
    }
}