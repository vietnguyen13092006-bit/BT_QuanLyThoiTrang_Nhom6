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
        //private string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
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

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            // 1. Gán ảnh nền trực tiếp cho Form từ thư mục Images
            this.BackgroundImage = ImageHelper.GetImage("NenDangNhap.jpg"); // Đổi đúng tên file ảnh 
            this.BackgroundImageLayout = ImageLayout.Stretch; // Hoặc Zoom tùy nhu cầu

            // 2. Chỉnh Panel thành trong suốt
            panel1.BackColor = Color.Transparent;

            // 3. Gọi hàm căn giữa Panel ngay khi mở Form
            CanGiuaPanel();
        }

        private void FormDangNhap_Resize(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            FORM_DKY_GUI frmSignin = new FORM_DKY_GUI();
            frmSignin.Show();
            this.Hide();
        }
    }
}
