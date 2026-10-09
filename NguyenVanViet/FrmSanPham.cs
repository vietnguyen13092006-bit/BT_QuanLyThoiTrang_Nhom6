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
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
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
                RowCount = 5
            };
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            txtMaSP = new TextBox { Dock = DockStyle.Fill };
            txtTenSP = new TextBox { Dock = DockStyle.Fill };

            cboLoaiSP = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown };
            cboLoaiSP.Items.AddRange(new object[] { "Áo thun", "Áo sơ mi", "Quần jean", "Quần short", "Áo khoác", "Váy/Đầm" });

            cboMauSac = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown };
            cboMauSac.Items.AddRange(new object[] { "Đen", "Trắng", "Xanh", "Đỏ", "Vàng", "Xám", "Nâu" });

            cboSize = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown };
            cboSize.Items.AddRange(new object[] { "S", "M", "L", "XL", "XXL", "28", "29", "30", "31", "32" });

            nudGiaNhap = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 1000000000, Increment = 10000, ThousandsSeparator = true };
            nudGiaBan = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 1000000000, Increment = 10000, ThousandsSeparator = true };
            nudSoLuong = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 100000, Value = 1 };

            txtGhiChu = new TextBox { Dock = DockStyle.Fill };

            picHinhAnh = new PictureBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                Height = 120
            };

            btnChonAnh = new Button { Text = "Chọn Ảnh...", Dock = DockStyle.Bottom, Height = 30 };
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
            tlpMauSize.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tlpMauSize.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
            tlpMauSize.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tlpMauSize.Controls.Add(cboMauSac, 0, 0);
            tlpMauSize.Controls.Add(new Label { Text = "Size:", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            tlpMauSize.Controls.Add(cboSize, 2, 0);

            tlpInput.Controls.Add(new Label { Text = "Màu / Size:", Anchor = AnchorStyles.Left }, 0, 3);
            tlpInput.Controls.Add(tlpMauSize, 1, 3);
            tlpInput.Controls.Add(new Label { Text = "Ghi Chú:", Anchor = AnchorStyles.Left }, 2, 3);
            tlpInput.Controls.Add(txtGhiChu, 3, 3);

            // Dòng 4 (Chỉ giữ lại nút Cập Nhật và Làm Mới)
            flpButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            btnSua = new Button { Text = "Cập Nhật", Width = 110, Height = 32, BackColor = Color.LightSkyBlue };
            btnLamMoi = new Button { Text = "Làm Mới", Width = 110, Height = 32 };

            btnSua.Click += BtnSua_Click;
            btnLamMoi.Click += (s, e) => { TaiDanhSachSanPham(); XoaTrangForm(); };

            flpButtons.Controls.AddRange(new Control[] { btnSua, btnLamMoi });

            TableLayoutPanel tlpTimKiem = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            tlpTimKiem.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpTimKiem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            txtTimKiem = new TextBox { Dock = DockStyle.Fill };
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            tlpTimKiem.Controls.Add(new Label { Text = "Tìm kiếm:", Anchor = AnchorStyles.Left }, 0, 0);
            tlpTimKiem.Controls.Add(txtTimKiem, 1, 0);

            tlpInput.Controls.Add(flpButtons, 0, 4);
            tlpInput.SetColumnSpan(flpButtons, 2);
            tlpInput.Controls.Add(tlpTimKiem, 2, 4);
            tlpInput.SetColumnSpan(tlpTimKiem, 3);

            gbThongTin.Controls.Add(tlpInput);
            tlpMain.Controls.Add(gbThongTin, 0, 1);

            // DataGridView Hàng hóa
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

            tlpMain.Controls.Add(dgvSanPham, 0, 2);
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
            if (e.RowIndex >= 0 && dgvSanPham.CurrentRow != null)
            {
                DataGridViewRow row = dgvSanPham.CurrentRow;
                txtMaSP.Text = row.Cells["MaSP"].Value?.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value?.ToString();
                cboLoaiSP.Text = row.Cells["LoaiSP"].Value?.ToString();
                cboMauSac.Text = row.Cells["MauSac"].Value?.ToString();
                cboSize.Text = row.Cells["Size"].Value?.ToString();

                decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out decimal giaNhap);
                nudGiaNhap.Value = giaNhap;

                decimal.TryParse(row.Cells["GiaBan"].Value?.ToString(), out decimal giaBan);
                nudGiaBan.Value = giaBan;

                int.TryParse(row.Cells["SoLuongTon"].Value?.ToString(), out int soLuong);
                nudSoLuong.Value = soLuong;

                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                duongDanAnh = row.Cells["HinhAnh"].Value?.ToString() ?? "";

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
            txtMaSP.Focus();
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

            string query = @"UPDATE SanPham 
                            SET TenSP = @TenSP, LoaiSP = @LoaiSP, MauSac = @MauSac, Size = @Size, 
                                GiaNhap = @GiaNhap, GiaBan = @GiaBan, SoLuongTon = @SoLuong, 
                                GhiChu = @GhiChu, HinhAnh = @HinhAnh 
                            WHERE MaSP = @MaSP";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSP", txtMaSP.Text.Trim()),
                new SqlParameter("@TenSP", txtTenSP.Text.Trim()),
                new SqlParameter("@LoaiSP", (object?)cboLoaiSP.Text ?? DBNull.Value),
                new SqlParameter("@MauSac", (object?)cboMauSac.Text ?? DBNull.Value),
                new SqlParameter("@Size", (object?)cboSize.Text ?? DBNull.Value),
                new SqlParameter("@GiaNhap", nudGiaNhap.Value),
                new SqlParameter("@GiaBan", nudGiaBan.Value),
                new SqlParameter("@SoLuong", (int)nudSoLuong.Value),
                new SqlParameter("@GhiChu", (object?)txtGhiChu.Text ?? DBNull.Value),
                new SqlParameter("@HinhAnh", (object?)duongDanAnh ?? DBNull.Value)
            };

            if (Database.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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