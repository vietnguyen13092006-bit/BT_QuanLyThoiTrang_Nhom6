using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangThoiTrang.GiaoDien
{
    public partial class FrmSanPham : Form
    {
        // Khai báo các Control với dấu ? (Nullable) để không bị báo warning Nullability
        private Label? lblTieuDe, lblThongTin, lblMaSanPham, lblTenSanPham, lblLoaiSanPham, lblMauSac, lblSize, lblGiaNhap, lblGiaBan, lblGhiChu, lblTimKiem;
        private TextBox? txtMaSanPham, txtTenSanPham, txtMauSac, txtGiaNhap, txtGiaBan, txtGhiChu, txtTimKiem;
        private ComboBox? cboLoaiSanPham, cboSize;
        private Button? btnThem, btnSua, btnXoa, btnLamMoi, btnTimKiem;
        private DataGridView? dgvSanPham;

        public FrmSanPham()
        {
            KhoiTaoForm();
            KhoiTaoGiaoDien();
            TaiDanhSachSanPham();
        }

        private void KhoiTaoForm()
        {
            this.Text = "Quản lý sản phẩm";
            this.Name = "FrmSanPham";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 700);
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }

        private void KhoiTaoGiaoDien()
        {
            // Tiêu đề
            lblTieuDe = new Label { Text = "QUẢN LÝ SẢN PHẨM", Font = new Font("Segoe UI", 20, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Top, Height = 55 };
            this.Controls.Add(lblTieuDe);

            lblThongTin = TaoLabel("THÔNG TIN SẢN PHẨM", 30, 70, 300, 30);
            lblThongTin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.Controls.Add(lblThongTin);

            // Hàng 1
            lblMaSanPham = TaoLabel("Mã sản phẩm:", 30, 115, 120, 30);
            txtMaSanPham = TaoTextBox(160, 115, 210, 30);

            lblTenSanPham = TaoLabel("Tên sản phẩm:", 410, 115, 120, 30);
            txtTenSanPham = TaoTextBox(540, 115, 250, 30);

            // Hàng 2 (Loại SP: Rộng 130px, ComboBox dạng DropDown gõ chữ thoải mái)
            lblLoaiSanPham = TaoLabel("Loại sản phẩm:", 30, 160, 130, 30);
            cboLoaiSanPham = new ComboBox
            {
                Name = "cboLoaiSanPham",
                Location = new Point(160, 160),
                Size = new Size(210, 30),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cboLoaiSanPham.Items.AddRange(new object[] { "Áo thun", "Áo sơ mi", "Áo polo", "Áo khoác", "Áo kiểu", "Quần jean", "Quần kaki", "Chân váy" });
            this.Controls.Add(cboLoaiSanPham);

            lblMauSac = TaoLabel("Màu sắc:", 410, 160, 120, 30);
            txtMauSac = TaoTextBox(540, 160, 250, 30);

            // Hàng 3
            lblSize = TaoLabel("Size:", 30, 205, 120, 30);
            cboSize = new ComboBox
            {
                Name = "cboSize",
                Location = new Point(160, 205),
                Size = new Size(210, 30),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cboSize.Items.AddRange(new object[] { "S", "M", "L", "XL", "XXL", "FreeSize" });
            this.Controls.Add(cboSize);

            lblGiaNhap = TaoLabel("Giá nhập:", 410, 205, 120, 30);
            txtGiaNhap = TaoTextBox(540, 205, 250, 30);

            // Hàng 4
            lblGiaBan = TaoLabel("Giá bán:", 30, 250, 120, 30);
            txtGiaBan = TaoTextBox(160, 250, 210, 30);

            lblGhiChu = TaoLabel("Ghi chú:", 410, 250, 120, 30);
            txtGhiChu = new TextBox { Name = "txtGhiChu", Location = new Point(540, 250), Size = new Size(250, 55), Multiline = true };
            this.Controls.Add(txtGhiChu);

            // Các nút
            btnThem = TaoButton("THÊM", 30, 320, 100, 40);
            btnThem.Click += BtnThem_Click;

            btnSua = TaoButton("SỬA", 145, 320, 100, 40);
            btnSua.Click += BtnSua_Click;

            btnXoa = TaoButton("XÓA", 260, 320, 100, 40);
            btnXoa.Click += BtnXoa_Click;

            btnLamMoi = TaoButton("LÀM MỚI", 375, 320, 110, 40);
            btnLamMoi.Click += BtnLamMoi_Click;

            lblTimKiem = TaoLabel("Tìm kiếm:", 520, 325, 80, 30);
            txtTimKiem = TaoTextBox(605, 320, 200, 30);
            btnTimKiem = TaoButton("TÌM KIẾM", 820, 320, 110, 40);
            btnTimKiem.Click += BtnTimKiem_Click;

            // Bảng DataGridView
            dgvSanPham = new DataGridView
            {
                Name = "dgvSanPham",
                Location = new Point(30, 385),
                Size = new Size(1120, 230),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvSanPham.CellClick += DgvSanPham_CellClick;
            this.Controls.Add(dgvSanPham);

            // Add các control lên Form
            this.Controls.Add(lblMaSanPham);
            this.Controls.Add(txtMaSanPham);
            this.Controls.Add(lblTenSanPham);
            this.Controls.Add(txtTenSanPham);
            this.Controls.Add(lblLoaiSanPham);
            this.Controls.Add(lblMauSac);
            this.Controls.Add(txtMauSac);
            this.Controls.Add(lblSize);
            this.Controls.Add(lblGiaNhap);
            this.Controls.Add(txtGiaNhap);
            this.Controls.Add(lblGiaBan);
            this.Controls.Add(txtGiaBan);
            this.Controls.Add(lblGhiChu);
            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);
            this.Controls.Add(lblTimKiem);
            this.Controls.Add(txtTimKiem);
            this.Controls.Add(btnTimKiem);
        }

        private Label TaoLabel(string text, int x, int y, int width, int height) => new Label { Text = text, Location = new Point(x, y), Size = new Size(width, height), TextAlign = ContentAlignment.MiddleLeft };
        private TextBox TaoTextBox(int x, int y, int width, int height) => new TextBox { Location = new Point(x, y), Size = new Size(width, height) };
        private Button TaoButton(string text, int x, int y, int width, int height) => new Button { Text = text, Location = new Point(x, y), Size = new Size(width, height), Cursor = Cursors.Hand };

        private void TaiDanhSachSanPham()
        {
            string sql = @"SELECT 
                            MaSanPham AS [Mã], 
                            TenSanPham AS [Tên], 
                            LoaiSanPham AS [Loại], 
                            MauSac AS [Màu], 
                            Size AS [Size], 
                            GiaNhap AS [Giá nhập], 
                            GiaBan AS [Giá bán], 
                            SoLuongTon AS [Tồn], 
                            GhiChu AS [Ghi chú] 
                          FROM SanPham";
            DataTable dt = Database.GetData(sql);
            if (dgvSanPham != null) dgvSanPham.DataSource = dt;
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (txtMaSanPham == null || txtTenSanPham == null || cboLoaiSanPham == null || txtMauSac == null || cboSize == null || txtGiaNhap == null || txtGiaBan == null || txtGhiChu == null) return;

            if (string.IsNullOrWhiteSpace(txtMaSanPham.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSanPham.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenSanPham.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanPham.Focus();
                return;
            }

            string checkQuery = "SELECT COUNT(*) FROM SanPham WHERE MaSanPham = @MaSanPham";
            SqlParameter[] checkParams = { new SqlParameter("@MaSanPham", txtMaSanPham.Text.Trim()) };
            DataTable checkDt = Database.GetData(checkQuery, checkParams);

            if (checkDt != null && checkDt.Rows.Count > 0 && Convert.ToInt32(checkDt.Rows[0][0]) > 0)
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap);
            decimal.TryParse(txtGiaBan.Text, out decimal giaBan);

            string insertSql = @"INSERT INTO SanPham (MaSanPham, TenSanPham, LoaiSanPham, MauSac, Size, GiaNhap, GiaBan, SoLuongTon, GhiChu)
                                VALUES (@Ma, @Ten, @Loai, @Mau, @Size, @GiaNhap, @GiaBan, 0, @GhiChu)";

            SqlParameter[] sqlParams =
            {
                new SqlParameter("@Ma", txtMaSanPham.Text.Trim()),
                new SqlParameter("@Ten", txtTenSanPham.Text.Trim()),
                new SqlParameter("@Loai", cboLoaiSanPham.Text.Trim()),
                new SqlParameter("@Mau", txtMauSac.Text.Trim()),
                new SqlParameter("@Size", cboSize.Text.Trim()),
                new SqlParameter("@GiaNhap", giaNhap),
                new SqlParameter("@GiaBan", giaBan),
                new SqlParameter("@GhiChu", txtGhiChu.Text.Trim())
            };

            if (Database.ExecuteNonQuery(insertSql, sqlParams) > 0)
            {
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                LamMoi();
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (dgvSanPham == null || dgvSanPham.CurrentRow == null || txtMaSanPham == null || txtTenSanPham == null || cboLoaiSanPham == null || txtMauSac == null || cboSize == null || txtGiaNhap == null || txtGiaBan == null || txtGhiChu == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!");
                return;
            }

            decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap);
            decimal.TryParse(txtGiaBan.Text, out decimal giaBan);

            string updateSql = @"UPDATE SanPham 
                                SET TenSanPham = @Ten, 
                                    LoaiSanPham = @Loai, 
                                    MauSac = @Mau, 
                                    Size = @Size, 
                                    GiaNhap = @GiaNhap, 
                                    GiaBan = @GiaBan, 
                                    GhiChu = @GhiChu 
                                WHERE MaSanPham = @Ma";

            SqlParameter[] sqlParams =
            {
                new SqlParameter("@Ma", txtMaSanPham.Text.Trim()),
                new SqlParameter("@Ten", txtTenSanPham.Text.Trim()),
                new SqlParameter("@Loai", cboLoaiSanPham.Text.Trim()),
                new SqlParameter("@Mau", txtMauSac.Text.Trim()),
                new SqlParameter("@Size", cboSize.Text.Trim()),
                new SqlParameter("@GiaNhap", giaNhap),
                new SqlParameter("@GiaBan", giaBan),
                new SqlParameter("@GhiChu", txtGhiChu.Text.Trim())
            };

            if (Database.ExecuteNonQuery(updateSql, sqlParams) > 0)
            {
                MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                LamMoi();
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (dgvSanPham == null || dgvSanPham.CurrentRow == null || txtMaSanPham == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
                return;
            }

            string maSP = txtMaSanPham.Text.Trim();

            DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm {maSP}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string deleteSql = "DELETE FROM SanPham WHERE MaSanPham = @Ma";
                SqlParameter[] sqlParams = { new SqlParameter("@Ma", maSP) };

                if (Database.ExecuteNonQuery(deleteSql, sqlParams) > 0)
                {
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachSanPham();
                    LamMoi();
                }
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            LamMoi();
            TaiDanhSachSanPham();
        }

        private void LamMoi()
        {
            txtMaSanPham?.Clear();
            txtTenSanPham?.Clear();
            txtMauSac?.Clear();
            txtGiaNhap?.Clear();
            txtGiaBan?.Clear();
            txtGhiChu?.Clear();
            txtTimKiem?.Clear();

            if (txtMaSanPham != null) txtMaSanPham.ReadOnly = false;
            if (cboLoaiSanPham != null) cboLoaiSanPham.Text = "";
            if (cboSize != null) cboSize.Text = "";
            dgvSanPham?.ClearSelection();
            txtMaSanPham?.Focus();
        }

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            if (txtTimKiem == null) return;
            string tuKhoa = txtTimKiem.Text.Trim();

            string sql = @"SELECT 
                            MaSanPham AS [Mã], 
                            TenSanPham AS [Tên], 
                            LoaiSanPham AS [Loại], 
                            MauSac AS [Màu], 
                            Size AS [Size], 
                            GiaNhap AS [Giá nhập], 
                            GiaBan AS [Giá bán], 
                            SoLuongTon AS [Tồn], 
                            GhiChu AS [Ghi chú] 
                          FROM SanPham
                          WHERE MaSanPham LIKE @TuKhoa 
                             OR TenSanPham LIKE @TuKhoa 
                             OR LoaiSanPham LIKE @TuKhoa 
                             OR MauSac LIKE @TuKhoa";

            SqlParameter[] sqlParams = { new SqlParameter("@TuKhoa", "%" + tuKhoa + "%") };
            DataTable dt = Database.GetData(sql, sqlParams);
            if (dgvSanPham != null) dgvSanPham.DataSource = dt;
        }

        private void DgvSanPham_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dgvSanPham == null || e.RowIndex < 0) return;

            DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

            if (txtMaSanPham != null) txtMaSanPham.Text = row.Cells["Mã"].Value?.ToString();
            if (txtTenSanPham != null) txtTenSanPham.Text = row.Cells["Tên"].Value?.ToString();
            if (cboLoaiSanPham != null) cboLoaiSanPham.Text = row.Cells["Loại"].Value?.ToString();
            if (txtMauSac != null) txtMauSac.Text = row.Cells["Màu"].Value?.ToString();
            if (cboSize != null) cboSize.Text = row.Cells["Size"].Value?.ToString();
            if (txtGiaNhap != null) txtGiaNhap.Text = row.Cells["Giá nhập"].Value?.ToString();
            if (txtGiaBan != null) txtGiaBan.Text = row.Cells["Giá bán"].Value?.ToString();
            if (txtGhiChu != null) txtGhiChu.Text = row.Cells["Ghi chú"].Value?.ToString();

            if (txtMaSanPham != null) txtMaSanPham.ReadOnly = true;
        }
    }
}