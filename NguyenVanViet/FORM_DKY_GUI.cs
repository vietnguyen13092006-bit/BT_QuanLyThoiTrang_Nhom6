using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class FORM_DKY_GUI : Form
    {
        //private string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
        public FORM_DKY_GUI()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

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
                // Tính toán vị trí X, Y để căn giữa Panel trong lòng Form
                int x = (this.ClientSize.Width - panel1.Width) / 2;
                int y = (this.ClientSize.Height - panel1.Height) / 2;

                panel1.Location = new Point(x, y);
            }
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
                try
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO Users (Username, PassW, Email, SDT) VALUES (@Username, @PassW, @email, @SDT)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", fullName);

                        // Nếu người dùng không nhập email -> lưu NULL vào database
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);

                        cmd.Parameters.AddWithValue("@PassW", password);

                        cmd.Parameters.AddWithValue("@SDT", sdt);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Xóa trắng form sau khi đăng ký thành công
                        txtName.Clear();
                        txtEmail.Clear();
                        txtPW.Clear();
                        TxtCP.Clear();

                        // Mở Form Đăng nhập
                        FormDangNhap frmLogin = new FormDangNhap();
                        frmLogin.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void FORM_DKY_GUI_Load(object sender, EventArgs e)
        {
            {
                // 1. Gán ảnh nền trực tiếp cho Form từ thư mục Images
                this.BackgroundImage = ImageHelper.GetImage("NenDKY.jpg"); // Đổi đúng tên file ảnh 
                this.BackgroundImageLayout = ImageLayout.Stretch; // Hoặc Zoom tùy nhu cầu

                // 2. Chỉnh Panel thành trong suốt
                panel1.BackColor = Color.Transparent;

                // 3. Gọi hàm căn giữa Panel ngay khi mở Form
                CanGiuaPanel();
            }
        }
    }
}
