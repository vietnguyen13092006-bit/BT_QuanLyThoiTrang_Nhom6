using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangThoiTrang.GiaoDien
{
    public partial class FrmSanPham : Form
    {
        // ==============================
        // KHAI BÁO CÁC CONTROL
        // ==============================

        private Label lblTieuDe;
        private Label lblThongTin;

        private Label lblMaSanPham;
        private Label lblTenSanPham;
        private Label lblLoaiSanPham;
        private Label lblMauSac;
        private Label lblSize;
        private Label lblGiaNhap;
        private Label lblGiaBan;
        private Label lblGhiChu;
        private Label lblTimKiem;

        private TextBox txtMaSanPham;
        private TextBox txtTenSanPham;
        private TextBox txtMauSac;
        private TextBox txtGiaNhap;
        private TextBox txtGiaBan;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;

        private ComboBox cboLoaiSanPham;
        private ComboBox cboSize;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;

        private DataGridView dgvSanPham;

        // Lưu dữ liệu sản phẩm tạm thời
        private DataTable dtSanPham;

        // ==============================
        // CONSTRUCTOR
        // ==============================

        public FrmSanPham()
        {
            KhoiTaoForm();
            KhoiTaoGiaoDien();
            KhoiTaoDuLieu();
        }

        // ==============================
        // CẤU HÌNH FORM
        // ==============================

        private void KhoiTaoForm()
        {
            this.Text = "Quản lý sản phẩm";
            this.Name = "FrmSanPham";

            this.StartPosition = FormStartPosition.CenterScreen;

            this.Size = new Size(1200, 700);

            this.MinimumSize = new Size(1000, 600);

            this.BackColor = Color.White;

            this.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Regular
            );
        }

        // ==============================
        // TẠO GIAO DIỆN
        // ==============================

        private void KhoiTaoGiaoDien()
        {
            // ------------------------------
            // TIÊU ĐỀ
            // ------------------------------

            lblTieuDe = new Label();

            lblTieuDe.Text = "QUẢN LÝ SẢN PHẨM";

            lblTieuDe.Font = new Font(
                "Segoe UI",
                20,
                FontStyle.Bold
            );

            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;

            lblTieuDe.Dock = DockStyle.Top;

            lblTieuDe.Height = 55;

            this.Controls.Add(lblTieuDe);


            // ------------------------------
            // TIÊU ĐỀ THÔNG TIN
            // ------------------------------

            lblThongTin = TaoLabel(
                "THÔNG TIN SẢN PHẨM",
                30,
                70,
                300,
                30
            );

            lblThongTin.Font = new Font(
                "Segoe UI",
                12,
                FontStyle.Bold
            );

            this.Controls.Add(lblThongTin);


            // ------------------------------
            // MÃ SẢN PHẨM
            // ------------------------------

            lblMaSanPham = TaoLabel(
                "Mã sản phẩm:",
                30,
                115,
                120,
                30
            );

            txtMaSanPham = TaoTextBox(
                150,
                115,
                220,
                30
            );


            // ------------------------------
            // TÊN SẢN PHẨM
            // ------------------------------

            lblTenSanPham = TaoLabel(
                "Tên sản phẩm:",
                410,
                115,
                120,
                30
            );

            txtTenSanPham = TaoTextBox(
                530,
                115,
                250,
                30
            );


            // ------------------------------
            // LOẠI SẢN PHẨM
            // ------------------------------

            lblLoaiSanPham = TaoLabel(
                "Loại sản phẩm:",
                30,
                160,
                120,
                30
            );

            cboLoaiSanPham = new ComboBox();

            cboLoaiSanPham.Name = "cboLoaiSanPham";

            cboLoaiSanPham.Location =
                new Point(150, 160);

            cboLoaiSanPham.Size =
                new Size(220, 30);

            cboLoaiSanPham.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboLoaiSanPham.Items.AddRange(
                new object[]
                {
                    "Áo thun",
                    "Áo sơ mi",
                    "Áo polo",
                    "Áo kiểu",
                    "Quần jean",
                    "Quần kaki",
                    "Chân váy"
                }
            );

            this.Controls.Add(cboLoaiSanPham);


            // ------------------------------
            // MÀU SẮC
            // ------------------------------

            lblMauSac = TaoLabel(
                "Màu sắc:",
                410,
                160,
                120,
                30
            );

            txtMauSac = TaoTextBox(
                530,
                160,
                250,
                30
            );


            // ------------------------------
            // SIZE
            // ------------------------------

            lblSize = TaoLabel(
                "Size:",
                30,
                205,
                120,
                30
            );

            cboSize = new ComboBox();

            cboSize.Name = "cboSize";

            cboSize.Location =
                new Point(150, 205);

            cboSize.Size =
                new Size(220, 30);

            cboSize.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboSize.Items.AddRange(
                new object[]
                {
                    "S",
                    "M",
                    "L",
                    "XL",
                    "XXL"
                }
            );

            this.Controls.Add(cboSize);


            // ------------------------------
            // GIÁ NHẬP
            // ------------------------------

            lblGiaNhap = TaoLabel(
                "Giá nhập:",
                410,
                205,
                120,
                30
            );

            txtGiaNhap = TaoTextBox(
                530,
                205,
                250,
                30
            );


            // ------------------------------
            // GIÁ BÁN
            // ------------------------------

            lblGiaBan = TaoLabel(
                "Giá bán:",
                30,
                250,
                120,
                30
            );

            txtGiaBan = TaoTextBox(
                150,
                250,
                220,
                30
            );


            // ------------------------------
            // GHI CHÚ
            // ------------------------------

            lblGhiChu = TaoLabel(
                "Ghi chú:",
                410,
                250,
                120,
                30
            );

            txtGhiChu = new TextBox();

            txtGhiChu.Name = "txtGhiChu";

            txtGhiChu.Location =
                new Point(530, 250);

            txtGhiChu.Size =
                new Size(250, 55);

            txtGhiChu.Multiline = true;

            this.Controls.Add(txtGhiChu);


            // ------------------------------
            // NÚT THÊM
            // ------------------------------

            btnThem = TaoButton(
                "THÊM",
                30,
                320,
                100,
                40
            );

            btnThem.Click += BtnThem_Click;


            // ------------------------------
            // NÚT SỬA
            // ------------------------------

            btnSua = TaoButton(
                "SỬA",
                145,
                320,
                100,
                40
            );

            btnSua.Click += BtnSua_Click;


            // ------------------------------
            // NÚT XÓA
            // ------------------------------

            btnXoa = TaoButton(
                "XÓA",
                260,
                320,
                100,
                40
            );

            btnXoa.Click += BtnXoa_Click;


            // ------------------------------
            // NÚT LÀM MỚI
            // ------------------------------

            btnLamMoi = TaoButton(
                "LÀM MỚI",
                375,
                320,
                110,
                40
            );

            btnLamMoi.Click += BtnLamMoi_Click;


            // ------------------------------
            // TÌM KIẾM
            // ------------------------------

            lblTimKiem = TaoLabel(
                "Tìm kiếm:",
                520,
                325,
                80,
                30
            );

            txtTimKiem = TaoTextBox(
                605,
                320,
                200,
                30
            );

            btnTimKiem = TaoButton(
                "TÌM KIẾM",
                820,
                320,
                110,
                40
            );

            btnTimKiem.Click += BtnTimKiem_Click;


            // ------------------------------
            // DATAGRIDVIEW
            // ------------------------------

            dgvSanPham = new DataGridView();

            dgvSanPham.Name = "dgvSanPham";

            dgvSanPham.Location =
                new Point(30, 385);

            dgvSanPham.Size =
                new Size(1120, 230);

            dgvSanPham.ReadOnly = true;

            dgvSanPham.AllowUserToAddRows = false;

            dgvSanPham.AllowUserToDeleteRows = false;

            dgvSanPham.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvSanPham.MultiSelect = false;

            dgvSanPham.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvSanPham.CellClick +=
                DgvSanPham_CellClick;

            this.Controls.Add(dgvSanPham);


            // Thêm các Label/TextBox còn thiếu vào Form
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

        // ==============================
        // TẠO LABEL
        // ==============================

        private Label TaoLabel(
            string text,
            int x,
            int y,
            int width,
            int height)
        {
            Label label = new Label();

            label.Text = text;

            label.Location =
                new Point(x, y);

            label.Size =
                new Size(width, height);

            label.TextAlign =
                ContentAlignment.MiddleLeft;

            return label;
        }

        // ==============================
        // TẠO TEXTBOX
        // ==============================

        private TextBox TaoTextBox(
            int x,
            int y,
            int width,
            int height)
        {
            TextBox textBox = new TextBox();

            textBox.Location =
                new Point(x, y);

            textBox.Size =
                new Size(width, height);

            return textBox;
        }

        // ==============================
        // TẠO BUTTON
        // ==============================

        private Button TaoButton(
            string text,
            int x,
            int y,
            int width,
            int height)
        {
            Button button = new Button();

            button.Text = text;

            button.Location =
                new Point(x, y);

            button.Size =
                new Size(width, height);

            button.Cursor =
                Cursors.Hand;

            return button;
        }

        // ==============================
        // TẠO DỮ LIỆU
        // ==============================

        private void KhoiTaoDuLieu()
        {
            dtSanPham = new DataTable();

            dtSanPham.Columns.Add(
                "Mã",
                typeof(string)
            );

            dtSanPham.Columns.Add(
                "Tên",
                typeof(string)
            );

            dtSanPham.Columns.Add(
                "Loại",
                typeof(string)
            );

            dtSanPham.Columns.Add(
                "Màu",
                typeof(string)
            );

            dtSanPham.Columns.Add(
                "Size",
                typeof(string)
            );

            dtSanPham.Columns.Add(
                "Giá nhập",
                typeof(decimal)
            );

            dtSanPham.Columns.Add(
                "Giá bán",
                typeof(decimal)
            );

            dtSanPham.Columns.Add(
                "Tồn",
                typeof(int)
            );


            // Dữ liệu mẫu

            dtSanPham.Rows.Add(
                "SP001",
                "Áo thun nam Basic",
                "Áo thun",
                "Đen",
                "M",
                100000,
                180000,
                50
            );

            dtSanPham.Rows.Add(
                "SP002",
                "Áo polo nam",
                "Áo polo",
                "Trắng",
                "L",
                150000,
                250000,
                30
            );

            dtSanPham.Rows.Add(
                "SP003",
                "Quần jean nam",
                "Quần jean",
                "Xanh",
                "32",
                250000,
                400000,
                20
            );

            dgvSanPham.DataSource =
                dtSanPham;
        }

        // ==============================
        // NÚT THÊM
        // ==============================

        private void BtnThem_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtMaSanPham.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập mã sản phẩm!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtMaSanPham.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtTenSanPham.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên sản phẩm!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtTenSanPham.Focus();

                return;
            }

            foreach (DataRow row in dtSanPham.Rows)
            {
                if (row["Mã"].ToString()
                    .Equals(
                        txtMaSanPham.Text.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Mã sản phẩm đã tồn tại!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }
            }

            decimal.TryParse(
                txtGiaNhap.Text,
                out decimal giaNhap
            );

            decimal.TryParse(
                txtGiaBan.Text,
                out decimal giaBan
            );

            dtSanPham.Rows.Add(
                txtMaSanPham.Text.Trim(),
                txtTenSanPham.Text.Trim(),
                cboLoaiSanPham.Text,
                txtMauSac.Text.Trim(),
                cboSize.Text,
                giaNhap,
                giaBan,
                0
            );

            MessageBox.Show(
                "Thêm sản phẩm thành công!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            LamMoi();
        }

        // ==============================
        // NÚT SỬA
        // ==============================

        private void BtnSua_Click(
            object sender,
            EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm cần sửa!"
                );

                return;
            }

            int index =
                dgvSanPham.CurrentRow.Index;

            DataRow row =
                dtSanPham.Rows[index];

            row["Mã"] =
                txtMaSanPham.Text.Trim();

            row["Tên"] =
                txtTenSanPham.Text.Trim();

            row["Loại"] =
                cboLoaiSanPham.Text;

            row["Màu"] =
                txtMauSac.Text.Trim();

            row["Size"] =
                cboSize.Text;

            decimal.TryParse(
                txtGiaNhap.Text,
                out decimal giaNhap
            );

            decimal.TryParse(
                txtGiaBan.Text,
                out decimal giaBan
            );

            row["Giá nhập"] = giaNhap;

            row["Giá bán"] = giaBan;

            MessageBox.Show(
                "Sửa sản phẩm thành công!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            LamMoi();
        }

        // ==============================
        // NÚT XÓA
        // ==============================

        private void BtnXoa_Click(
            object sender,
            EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm cần xóa!"
                );

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Bạn có chắc muốn xóa sản phẩm này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                int index =
                    dgvSanPham.CurrentRow.Index;

                dtSanPham.Rows.RemoveAt(index);

                LamMoi();
            }
        }

        // ==============================
        // NÚT LÀM MỚI
        // ==============================

        private void BtnLamMoi_Click(
            object sender,
            EventArgs e)
        {
            LamMoi();
        }

        private void LamMoi()
        {
            txtMaSanPham.Clear();

            txtTenSanPham.Clear();

            txtMauSac.Clear();

            txtGiaNhap.Clear();

            txtGiaBan.Clear();

            txtGhiChu.Clear();

            txtTimKiem.Clear();

            cboLoaiSanPham.SelectedIndex = -1;

            cboSize.SelectedIndex = -1;

            dgvSanPham.ClearSelection();

            txtMaSanPham.Focus();
        }

        // ==============================
        // TÌM KIẾM
        // ==============================

        private void BtnTimKiem_Click(
            object sender,
            EventArgs e)
        {
            string tuKhoa =
                txtTimKiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                dgvSanPham.DataSource =
                    dtSanPham;

                return;
            }

            DataView view =
                dtSanPham.DefaultView;

            view.RowFilter =
                $"Mã LIKE '%{tuKhoa.Replace("'", "''")}%' " +
                $"OR Tên LIKE '%{tuKhoa.Replace("'", "''")}%' " +
                $"OR Loại LIKE '%{tuKhoa.Replace("'", "''")}%' " +
                $"OR Màu LIKE '%{tuKhoa.Replace("'", "''")}%'";

            dgvSanPham.DataSource = view;
        }

        // ==============================
        // CLICK VÀO DÒNG
        // ==============================

        private void DgvSanPham_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row =
                dgvSanPham.Rows[e.RowIndex];

            txtMaSanPham.Text =
                row.Cells["Mã"].Value?.ToString();

            txtTenSanPham.Text =
                row.Cells["Tên"].Value?.ToString();

            cboLoaiSanPham.Text =
                row.Cells["Loại"].Value?.ToString();

            txtMauSac.Text =
                row.Cells["Màu"].Value?.ToString();

            cboSize.Text =
                row.Cells["Size"].Value?.ToString();

            txtGiaNhap.Text =
                row.Cells["Giá nhập"].Value?.ToString();

            txtGiaBan.Text =
                row.Cells["Giá bán"].Value?.ToString();
        }
    }
}