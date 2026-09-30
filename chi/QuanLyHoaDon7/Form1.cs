using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyHoaDon7
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối đến SQL Server của bạn
        string connStr = @"Data Source=.\SQLEXPRESS02;Initial Catalog=QuanLyBanHangDB;Integrated Security=True";

        DataTable dtChiTiet = new DataTable();

        // Khai báo các điều khiển giao diện
        private TextBox txtMaHD, txtNhanVien, txtKhachHang, txtTenSP, txtMauSac, txtKichThuoc, txtSoLuong, txtDonGia, txtTuKhoa;
        private DateTimePicker dtpNgayLap, dtpTuNgay, dtpDenNgay;
        private ComboBox cboPTTT;
        private DataGridView dgvChiTiet, dgvDanhSach;
        private Button btnThemSP, btnLuuHD, btnTimKiem, btnThongKe;
        private Label lblTongDoanhThu;

        public Form1()
        {
            // KHÔNG gọi InitializeComponent() ở đây để tránh lỗi
            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            this.Text = "Phần Mềm Quản Lý Hóa Đơn";
            this.Width = 1120;
            this.Height = 720;
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- THÔNG TIN HÓA ĐƠN ---
            Label l1 = new Label() { Text = "Mã HĐ:", Left = 20, Top = 20, Width = 60 };
            txtMaHD = new TextBox() { Left = 90, Top = 20, Width = 150 };

            Label l2 = new Label() { Text = "Ngày lập:", Left = 260, Top = 20, Width = 60 };
            dtpNgayLap = new DateTimePicker() { Left = 330, Top = 20, Width = 150, Format = DateTimePickerFormat.Short };

            Label l3 = new Label() { Text = "Nhân viên:", Left = 20, Top = 60, Width = 70 };
            txtNhanVien = new TextBox() { Left = 95, Top = 58, Width = 115 }; // Thu ngắn Width lại còn 115

            Label l4 = new Label() { Text = "Khách:", Left = 220, Top = 60, Width = 50 };
            txtKhachHang = new TextBox() { Left = 275, Top = 58, Width = 150 };

            Label l5 = new Label() { Text = "Thanh toán:", Left = 500, Top = 20, Width = 70 };
            cboPTTT = new ComboBox() { Left = 580, Top = 20, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPTTT.Items.AddRange(new string[] { "Tiền mặt", "Chuyển khoản", "Thẻ" });
            cboPTTT.SelectedIndex = 0;

            // --- CHI TIẾT SẢN PHẨM ---
            GroupBox grpSP = new GroupBox() { Text = " Danh sách sản phẩm mua ", Left = 20, Top = 100, Width = 710, Height = 130 };

            grpSP.Controls.Add(new Label() { Text = "Tên SP:", Left = 10, Top = 25, Width = 50 });
            txtTenSP = new TextBox() { Left = 60, Top = 22, Width = 120 };

            grpSP.Controls.Add(new Label() { Text = "Màu:", Left = 190, Top = 25, Width = 35 });
            txtMauSac = new TextBox() { Left = 225, Top = 22, Width = 70 };

            grpSP.Controls.Add(new Label() { Text = "Size:", Left = 305, Top = 25, Width = 30 });
            txtKichThuoc = new TextBox() { Left = 340, Top = 22, Width = 50 };

            grpSP.Controls.Add(new Label() { Text = "SL:", Left = 400, Top = 25, Width = 25 });
            txtSoLuong = new TextBox() { Left = 425, Top = 22, Width = 40 };

            grpSP.Controls.Add(new Label() { Text = "Đơn giá:", Left = 475, Top = 25, Width = 50 });
            txtDonGia = new TextBox() { Left = 525, Top = 22, Width = 80 };

            btnThemSP = new Button() { Text = "Thêm SP", Left = 615, Top = 20, Width = 80, Height = 25 };
            btnThemSP.Click += BtnThemSP_Click;
            grpSP.Controls.Add(btnThemSP);

            dgvChiTiet = new DataGridView() { Left = 10, Top = 55, Width = 685, Height = 62 };
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grpSP.Controls.Add(dgvChiTiet);
            this.Controls.Add(grpSP);

            btnLuuHD = new Button() { Text = "LƯU HÓA ĐƠN", Left = 580, Top = 60, Width = 150, Height = 30, BackColor = Color.LightGreen, Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold) };
            btnLuuHD.Click += BtnLuuHD_Click;

            // --- TRA CỨU & THỐNG KÊ ---
            GroupBox grpTimKiem = new GroupBox() { Text = " Tra cứu & Thống kê doanh thu ", Left = 740, Top = 20, Width = 345, Height = 210 };

            grpTimKiem.Controls.Add(new Label() { Text = "Từ khóa:", Left = 10, Top = 25, Width = 60 });
            txtTuKhoa = new TextBox() { Left = 80, Top = 22, Width = 150 };

            btnTimKiem = new Button() { Text = "Tìm", Left = 240, Top = 20, Width = 90, Height = 25 };
            btnTimKiem.Click += BtnTimKiem_Click;
            grpTimKiem.Controls.Add(btnTimKiem);

            grpTimKiem.Controls.Add(new Label() { Text = "Từ ngày:", Left = 10, Top = 65, Width = 60 });
            dtpTuNgay = new DateTimePicker() { Left = 80, Top = 63, Width = 150, Format = DateTimePickerFormat.Short };

            grpTimKiem.Controls.Add(new Label() { Text = "Đến ngày:", Left = 10, Top = 100, Width = 60 });
            dtpDenNgay = new DateTimePicker() { Left = 80, Top = 98, Width = 150, Format = DateTimePickerFormat.Short };

            btnThongKe = new Button() { Text = "Thống kê Doanh Thu", Left = 10, Top = 135, Width = 150, Height = 30 };
            btnThongKe.Click += BtnThongKe_Click;
            grpTimKiem.Controls.Add(btnThongKe);

            lblTongDoanhThu = new Label() { Text = "Doanh thu: 0 VNĐ", Left = 10, Top = 178, Width = 325, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold), ForeColor = Color.DarkBlue };
            grpTimKiem.Controls.Add(lblTongDoanhThu);

            this.Controls.Add(grpTimKiem);

            // --- BẢNG DANH SÁCH HÓA ĐƠN ---
            dgvDanhSach = new DataGridView() { Left = 20, Top = 245, Width = 1065, Height = 420 };
            dgvDanhSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Controls.Add(dgvDanhSach);

            // Đưa các control còn lại lên Form
            this.Controls.Add(l1); this.Controls.Add(txtMaHD);
            this.Controls.Add(l2); this.Controls.Add(dtpNgayLap);
            this.Controls.Add(l3); this.Controls.Add(txtNhanVien);
            this.Controls.Add(l4); this.Controls.Add(txtKhachHang);
            this.Controls.Add(l5); this.Controls.Add(cboPTTT);
            this.Controls.Add(btnLuuHD);

            // Khởi tạo dữ liệu
            KhoiTaoBangTam();
            LoadDanhSachHoaDon();
        }

        private void KhoiTaoBangTam()
        {
            dtChiTiet.Columns.Add("TenSP", typeof(string));
            dtChiTiet.Columns.Add("MauSac", typeof(string));
            dtChiTiet.Columns.Add("KichThuoc", typeof(string));
            dtChiTiet.Columns.Add("SoLuong", typeof(int));
            dtChiTiet.Columns.Add("DonGia", typeof(decimal));
            dtChiTiet.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");
            dgvChiTiet.DataSource = dtChiTiet;
        }

        private void LoadDanhSachHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HoaDon", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDanhSach.DataSource = dt;
            }
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text) || string.IsNullOrWhiteSpace(txtSoLuong.Text) || string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên SP, Số lượng và Đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRow row = dtChiTiet.NewRow();
                row["TenSP"] = txtTenSP.Text;
                row["MauSac"] = txtMauSac.Text;
                row["KichThuoc"] = txtKichThuoc.Text;
                row["SoLuong"] = int.Parse(txtSoLuong.Text);
                row["DonGia"] = decimal.Parse(txtDonGia.Text);
                dtChiTiet.Rows.Add(row);

                txtTenSP.Clear(); txtMauSac.Clear(); txtKichThuoc.Clear(); txtSoLuong.Clear(); txtDonGia.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Số lượng và Đơn giá phải là định dạng số!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuuHD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text) || dtChiTiet.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng nhập Mã Hóa Đơn và thêm ít nhất một sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    decimal tongTien = 0;
                    foreach (DataRow r in dtChiTiet.Rows)
                    {
                        tongTien += Convert.ToDecimal(r["ThanhTien"]);
                    }

                    // 1. Lưu hóa đơn chính
                    string q1 = "INSERT INTO HoaDon (MaHoaDon, NgayLap, NhanVien, KhachHang, PhuongThucTT, TongTien) VALUES (@MaHD, @Ngay, @NV, @KH, @PT, @Tong)";
                    SqlCommand cmd1 = new SqlCommand(q1, conn, trans);
                    cmd1.Parameters.AddWithValue("@MaHD", txtMaHD.Text.Trim());
                    cmd1.Parameters.AddWithValue("@Ngay", dtpNgayLap.Value);
                    cmd1.Parameters.AddWithValue("@NV", txtNhanVien.Text.Trim());
                    cmd1.Parameters.AddWithValue("@KH", txtKhachHang.Text.Trim());
                    cmd1.Parameters.AddWithValue("@PT", cboPTTT.SelectedItem.ToString());
                    cmd1.Parameters.AddWithValue("@Tong", tongTien);
                    cmd1.ExecuteNonQuery();

                    // 2. Lưu chi tiết sản phẩm
                    foreach (DataRow r in dtChiTiet.Rows)
                    {
                        string q2 = "INSERT INTO ChiTietHoaDon (MaHoaDon, TenSP, MauSac, KichThuoc, SoLuong, DonGia) VALUES (@MaHD, @Ten, @Mau, @Size, @SL, @DG)";
                        SqlCommand cmd2 = new SqlCommand(q2, conn, trans);
                        cmd2.Parameters.AddWithValue("@MaHD", txtMaHD.Text.Trim());
                        cmd2.Parameters.AddWithValue("@Ten", r["TenSP"]);
                        cmd2.Parameters.AddWithValue("@Mau", r["MauSac"]);
                        cmd2.Parameters.AddWithValue("@Size", r["KichThuoc"]);
                        cmd2.Parameters.AddWithValue("@SL", r["SoLuong"]);
                        cmd2.Parameters.AddWithValue("@DG", r["DonGia"]);
                        cmd2.ExecuteNonQuery();
                    }

                    trans.Commit();
                    MessageBox.Show("Lưu hóa đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset form sau khi lưu
                    dtChiTiet.Clear();
                    txtMaHD.Clear();
                    txtNhanVien.Clear();
                    txtKhachHang.Clear();
                    LoadDanhSachHoaDon();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Lỗi khi lưu (Có thể mã hóa đơn bị trùng): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string kw = "%" + txtTuKhoa.Text.Trim() + "%";
                string query = "SELECT * FROM HoaDon WHERE MaHoaDon LIKE @kw OR NhanVien LIKE @kw OR KhachHang LIKE @kw";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@kw", kw);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDanhSach.DataSource = dt;
            }
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT SUM(TongTien) FROM HoaDon WHERE NgayLap BETWEEN @tu AND @den";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tu", dtpTuNgay.Value.Date);
                cmd.Parameters.AddWithValue("@den", dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1));

                object val = cmd.ExecuteScalar();
                decimal doanhThu = (val != DBNull.Value && val != null) ? Convert.ToDecimal(val) : 0;
                lblTongDoanhThu.Text = "Doanh thu: " + doanhThu.ToString("N0") + " VNĐ";
            }
        }
    }
}