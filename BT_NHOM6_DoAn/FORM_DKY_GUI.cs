using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class FORM_DKY_GUI : Form
    {
        // Đã cập nhật kết nối sang Database chung: QuanLyCuaHangThoiTrang
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

        public FORM_DKY_GUI()
        {
            InitializeComponent();
        }

        private void Form_DKY_Resize(object sender, EventArgs e)
        {
            // Giữ Panel luôn ở chính giữa khi phóng to / thu nhỏ Form
            CanGiuaPanel();
        }

        private void CanGiuaPanel()
        {
            if (panel1 != null)
            {
                int x = (this.ClientSize.Width - panel1.Width) / 2;
                int y = (this.ClientSize.Height - panel1.Height) / 2;
                panel1.Location = new Point(x, y);
            }
        }

        // Hàm tự động sinh Mã Nhân Viên tiếp theo (VD: NV01, NV02,...)
        private string TaoMaNhanVienTuDong(SqlConnection conn, SqlTransaction transaction)
        {
            string maNVNew = "NV01";
            string query = "SELECT TOP 1 MaNV FROM NhanVien ORDER BY MaNV DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    string maCu = result.ToString(); // Ví dụ: "NV05"
                    if (maCu.Length >= 2 && int.TryParse(maCu.Substring(2), out int so))
                    {
                        so++;
                        maNVNew = "NV" + so.ToString("D2"); // Định dạng 2 chữ số (VD: NV06, NV07...)
                    }
                }
            }
            return maNVNew;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string fullName = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPW.Text.Trim();
            string confirmPassword = TxtCP.Text.Trim();
            string sdt = txtSDT.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập Tên, Mật khẩu và Xác nhận mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtCP.Focus();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Sử dụng Transaction để đảm bảo cả NhanVien và Users đều được tạo thành công
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Bước 1: Tự động sinh MaNV mới
                    string maNVNew = TaoMaNhanVienTuDong(conn, transaction);

                    // Bước 2: Thêm dữ liệu vào bảng NhanVien trước (mặc định Chức vụ là 'Nhân viên')
                    string insertNhanVienQuery = @"INSERT INTO NhanVien (MaNV, TenNV, SDT, ChucVu) 
                                                   VALUES (@MaNV, @TenNV, @SDT, N'Nhân viên')";
                    using (SqlCommand cmdNV = new SqlCommand(insertNhanVienQuery, conn, transaction))
                    {
                        cmdNV.Parameters.AddWithValue("@MaNV", maNVNew);
                        cmdNV.Parameters.AddWithValue("@TenNV", fullName);
                        cmdNV.Parameters.AddWithValue("@SDT", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt);
                        cmdNV.ExecuteNonQuery();
                    }

                    // Bước 3: Thêm dữ liệu vào bảng Users, đồng thời gắn khóa ngoại MaNV vừa sinh
                    string insertUserQuery = @"INSERT INTO Users (Username, PassW, Email, SDT, MaNV) 
                                               VALUES (@Username, @PassW, @Email, @SDT, @MaNV)";
                    using (SqlCommand cmdUser = new SqlCommand(insertUserQuery, conn, transaction))
                    {
                        cmdUser.Parameters.AddWithValue("@Username", fullName);
                        cmdUser.Parameters.AddWithValue("@PassW", password);
                        cmdUser.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                        cmdUser.Parameters.AddWithValue("@SDT", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt);
                        cmdUser.Parameters.AddWithValue("@MaNV", maNVNew);
                        cmdUser.ExecuteNonQuery();
                    }

                    // Nếu mọi thứ OK, commit transaction
                    transaction.Commit();

                    MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa trắng form sau khi đăng ký thành công
                    txtName.Clear();
                    txtEmail.Clear();
                    txtPW.Clear();
                    TxtCP.Clear();
                    txtSDT.Clear();

                    // Mở Form Đăng nhập
                    FormDangNhap frmLogin = new FormDangNhap();
                    frmLogin.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    // Lỗi xảy ra -> Rollback lại toàn bộ thao tác database
                    try
                    {
                        transaction.Rollback();
                    }
                    catch { }

                    MessageBox.Show("Lỗi lưu dữ liệu (Tài khoản có thể đã tồn tại): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FORM_DKY_GUI_Load(object sender, EventArgs e)
        {
            // 1. Gán ảnh nền trực tiếp cho Form từ thư mục Images
            this.BackgroundImage = ImageHelper.GetImage("NenDKY.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // 2. Chỉnh Panel thành trong suốt
            panel1.BackColor = Color.Transparent;

            // 3. Gọi hàm căn giữa Panel ngay khi mở Form
            CanGiuaPanel();
        }

        bool isShow1 = false, isShow2 = false;
        private void btnPW_Click(object sender, EventArgs e)
        {
            if (isShow1)
            {
                txtPW.PasswordChar = '*';
                btnPW.Text = "👁️";
                isShow1 = false;
            }
            else
            {
                txtPW.PasswordChar = '\0';
                btnPW.Text = "🔒";
                isShow1 = true;
            }
        }

        private void btnCP_Click(object sender, EventArgs e)
        {
            if (isShow2)
            {
                TxtCP.PasswordChar = '*';
                btnCP.Text = "👁️";
                isShow2 = false;
            }
            else
            {
                TxtCP.PasswordChar = '\0';
                btnCP.Text = "🔒";
                isShow2 = true;
            }
        }
    }
}