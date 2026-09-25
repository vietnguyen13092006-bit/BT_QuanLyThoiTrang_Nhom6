using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class FORM_DKY_GUI : Form
    {
        private string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";

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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string fullName = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPW.Text.Trim();
            string confirmPassword = TxtCP.Text.Trim();

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
                    string insertQuery = "INSERT INTO Users (Username, PassW, Email) VALUES (@Username, @PassW, @email)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", fullName);

                        // Nếu người dùng không nhập email -> lưu NULL vào database
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);

                        cmd.Parameters.AddWithValue("@PassW", password);

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
    }
}
