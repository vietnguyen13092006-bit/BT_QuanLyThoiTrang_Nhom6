using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
namespace FORM_DKY
{
    public partial class FormDangNhap : Form
    {
        public static string Ten { get; set; } = "";
        public static string Mail { get; set; } = "";
        public static string SDT { get; set; } = "";
        public static string AvatarPath { get; set; } = "";
        private string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ten = txtName.Text.Trim();
            string password = txtPW.Text.Trim();

            // 2. Kiểm tra ô trống
            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên và Mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Kiểm tra thông tin trong Database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Tìm tài khoản khớp cả tên và Password
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND PassW = @PassW";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", ten);
                        cmd.Parameters.AddWithValue("@PassW", password);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Ten = txtName.Text;
                            MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Email hoặc Mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FORM_DKY_GUI frmLogin = new FORM_DKY_GUI();
            frmLogin.Show();
            this.Hide();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}
