using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace quanlythongkevabaocao7
{
    public partial class FormDoiTra : Form
    {
        string connStr = @"Data Source=.\SQLEXPRESS03;Initial Catalog=QuanLyBanHangDB;Integrated Security=True";

        // Khai báo các Controls cho giao diện Đổi Trả Hàng
        private TextBox txtMaHDLQ, txtThongTinKH, txtTenSP, txtMauSac, txtKichThuoc, txtSoLuong, txtTienThem;
        private DateTimePicker dtpNgayDoiTra;
        private ComboBox cboHinhThuc;
        private Button btnThucHienDoiTra, btnTimHD;
        private DataGridView dgvLichSuDoiTra;

        public FormDoiTra()
        {
            InitializeComponentCustom();
            LoadLichSuDoiTra();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Quản Lý Đổi Trả Hàng";
            this.Width = 1050;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;

            // GroupBox Thông tin đổi trả (Đã tăng chiều cao và giãn cách không bị đè chữ)
            GroupBox grpInfo = new GroupBox() { Text = " Thông Tin Đổi Trả Hàng ", Left = 20, Top = 20, Width = 990, Height = 280 };

            // Dòng 1: Mã HĐ, Ngày, Hình thức
            grpInfo.Controls.Add(new Label() { Text = "Mã HĐ Liên Quan:", Left = 20, Top = 30, Width = 110 });
            txtMaHDLQ = new TextBox() { Left = 135, Top = 27, Width = 140 };
            grpInfo.Controls.Add(txtMaHDLQ);

            btnTimHD = new Button() { Text = "Kiểm tra HĐ", Left = 285, Top = 25, Width = 95, Height = 26 };
            btnTimHD.Click += BtnTimHD_Click;
            grpInfo.Controls.Add(btnTimHD);

            grpInfo.Controls.Add(new Label() { Text = "Ngày Đổi Trả:", Left = 410, Top = 30, Width = 90 });
            dtpNgayDoiTra = new DateTimePicker() { Left = 505, Top = 27, Width = 160, Format = DateTimePickerFormat.Short };
            grpInfo.Controls.Add(dtpNgayDoiTra);

            grpInfo.Controls.Add(new Label() { Text = "Hình thức:", Left = 690, Top = 30, Width = 70 });
            cboHinhThuc = new ComboBox() { Left = 765, Top = 27, Width = 195, DropDownStyle = ComboBoxStyle.DropDownList };
            cboHinhThuc.Items.AddRange(new string[] { "Đổi sản phẩm", "Trả hàng hoàn tiền" });
            cboHinhThuc.SelectedIndex = 0;
            grpInfo.Controls.Add(cboHinhThuc);

            // Dòng 2: Khách hàng
            grpInfo.Controls.Add(new Label() { Text = "TT Khách Hàng:", Left = 20, Top = 75, Width = 110 });
            txtThongTinKH = new TextBox() { Left = 135, Top = 72, Width = 345 };
            grpInfo.Controls.Add(txtThongTinKH);

            // Dòng 3: Sản phẩm, Màu, Size, Số lượng
            grpInfo.Controls.Add(new Label() { Text = "Sản phẩm đổi/trả:", Left = 20, Top = 120, Width = 110 });
            txtTenSP = new TextBox() { Left = 135, Top = 117, Width = 210 };
            grpInfo.Controls.Add(txtTenSP);

            grpInfo.Controls.Add(new Label() { Text = "Màu sắc:", Left = 360, Top = 120, Width = 60 });
            txtMauSac = new TextBox() { Left = 425, Top = 117, Width = 80 };
            grpInfo.Controls.Add(txtMauSac);

            grpInfo.Controls.Add(new Label() { Text = "Size:", Left = 520, Top = 120, Width = 35 });
            txtKichThuoc = new TextBox() { Left = 560, Top = 117, Width = 55 };
            grpInfo.Controls.Add(txtKichThuoc);

            grpInfo.Controls.Add(new Label() { Text = "Số lượng:", Left = 635, Top = 120, Width = 60 });
            txtSoLuong = new TextBox() { Left = 700, Top = 117, Width = 60 };
            grpInfo.Controls.Add(txtSoLuong);

            // Dòng 4: Tiền thanh toán thêm
            grpInfo.Controls.Add(new Label() { Text = "Tiền trả thêm:", Left = 20, Top = 165, Width = 110 });
            txtTienThem = new TextBox() { Left = 135, Top = 162, Width = 210, Text = "0" };
            grpInfo.Controls.Add(txtTienThem);

            // Nút xác nhận đổi trả
            btnThucHienDoiTra = new Button() { Text = "XÁC NHẬN ĐỔI TRẢ", Left = 765, Top = 215, Width = 195, Height = 40, BackColor = Color.Orange, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold) };
            btnThucHienDoiTra.Click += BtnThucHienDoiTra_Click;
            grpInfo.Controls.Add(btnThucHienDoiTra);

            this.Controls.Add(grpInfo);

            // DataGridView hiển thị lịch sử đổi trả dịch xuống phía dưới
            GroupBox grpList = new GroupBox() { Text = " Lịch Sử Đổi Trả Hàng ", Left = 20, Top = 310, Width = 990, Height = 330 };
            dgvLichSuDoiTra = new DataGridView() { Left = 15, Top = 25, Width = 960, Height = 290 };
            dgvLichSuDoiTra.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grpList.Controls.Add(dgvLichSuDoiTra);
            this.Controls.Add(grpList);
        }

        private void BtnTimHD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHDLQ.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã Hóa Đơn liên quan cần kiểm tra!", "Thông báo");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT KhachHang FROM HoaDon WHERE MaHoaDon = @MaHD";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHD", txtMaHDLQ.Text.Trim());
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    txtThongTinKH.Text = result.ToString();
                    MessageBox.Show("Đã tìm thấy hóa đơn liên quan!", "Thành công");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Mã Hóa Đơn này trong hệ thống!", "Lỗi");
                    txtThongTinKH.Clear();
                }
            }
        }

        private void BtnThucHienDoiTra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHDLQ.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text) || string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ Mã HĐ, Tên sản phẩm và Số lượng!", "Cảnh báo");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    string query = @"INSERT INTO DoiTraHang (MaHoaDonLienQuan, NgayDoiTra, ThongTinKhachHang, HinhThucDoiTra, TenSanPham, MauSac, KichThuoc, SoLuong, TienKhachThanhToanThem) 
                                     VALUES (@MaHD, @Ngay, @KH, @HinhThuc, @TenSP, @Mau, @Size, @SL, @TienThem)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHD", txtMaHDLQ.Text.Trim());
                    cmd.Parameters.AddWithValue("@Ngay", dtpNgayDoiTra.Value);
                    cmd.Parameters.AddWithValue("@KH", txtThongTinKH.Text.Trim());
                    cmd.Parameters.AddWithValue("@HinhThuc", cboHinhThuc.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TenSP", txtTenSP.Text.Trim());
                    cmd.Parameters.AddWithValue("@Mau", txtMauSac.Text.Trim());
                    cmd.Parameters.AddWithValue("@Size", txtKichThuoc.Text.Trim());
                    cmd.Parameters.AddWithValue("@SL", int.Parse(txtSoLuong.Text));
                    cmd.Parameters.AddWithValue("@TienThem", decimal.Parse(txtTienThem.Text));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ghi nhận đổi trả hàng thành công!", "Thông báo");

                    LoadLichSuDoiTra();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thực hiện đổi trả: " + ex.Message + "\n(Hãy đảm bảo bạn đã tạo bảng DoiTraHang trong CSDL SQL Server)", "Lỗi");
                }
            }
        }

        private void LoadLichSuDoiTra()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DoiTraHang", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLichSuDoiTra.DataSource = dt;
                }
                catch
                {
                    // Bỏ qua nếu bảng chưa được khởi tạo lần đầu
                }
            }
        }

        private void ClearForm()
        {
            txtMaHDLQ.Clear();
            txtThongTinKH.Clear();
            txtTenSP.Clear();
            txtMauSac.Clear();
            txtKichThuoc.Clear();
            txtSoLuong.Clear();
            txtTienThem.Text = "0";
        }
    }
}