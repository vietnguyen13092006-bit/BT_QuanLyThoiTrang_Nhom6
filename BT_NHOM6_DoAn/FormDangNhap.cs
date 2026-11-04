using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace FORM_DKY
{
    public partial class FormDangNhap : Form
    {
        public static string Ten { get; set; } = "";
        public static string Mail { get; set; } = "";
        public static string SDT { get; set; } = "";
        public static string AvatarPath { get; set; } = "";
        bool isShow = false;

        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ten = txtName.Text.Trim();
            string password = txtPW.Text.Trim();

            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên và Mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Truy vấn lấy cả MaNV và Username từ bảng Users (có liên kết sang NhanVien nếu có)
                    string query = "SELECT Username, MaNV FROM Users WHERE Username = @Username AND PassW = @PassW";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", ten);
                        cmd.Parameters.AddWithValue("@PassW", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Ten = reader["Username"].ToString();

                                // GÁN THÔNG TIN VÀO USERSESSION ĐỂ DÙNG CHUNG TOÀN HỆ THỐNG
                                UserSession.MaNV = reader["MaNV"] != DBNull.Value ? reader["MaNV"].ToString() : "";
                                UserSession.TenNV = Ten;

                                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Mở form chính hoặc form quản lý (ví dụ mở Form bán hàng hoặc menu chính)
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Tên đăng nhập hoặc Mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
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
            this.BackgroundImage = ImageHelper.GetImage("NenDangNhap.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackColor = Color.Transparent;
            CanGiuaPanel();
        }

        private void FormDangNhap_Resize(object sender, EventArgs e)
        {
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

        private void button2_Click(object sender, EventArgs e)
        {
            FORM_DKY_GUI frmSignin = new FORM_DKY_GUI();
            frmSignin.Show();
            this.Hide();
        }

        private void btnMat_Click(object sender, EventArgs e)
        {
            if (isShow)
            {
                txtPW.PasswordChar = '*';
                btnMat.Text = "👁️";
                isShow = false;
            }
            else
            {
                txtPW.PasswordChar = '\0';
                btnMat.Text = "🔒";
                isShow = true;
            }
        }
    }
}