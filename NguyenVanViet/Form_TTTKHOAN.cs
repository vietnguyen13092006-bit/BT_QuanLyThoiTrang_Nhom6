using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class Form_TTTKHOAN : Form
    {
        public Form_TTTKHOAN()
        {
            InitializeComponent();
        }

        private void Form_TTTKHOAN_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = ImageHelper.GetImage("NenTT.jpg");
            lblName.Text = "Tên: " + FormDangNhap.Ten;
            lblMail.Parent = pictureBox1;
            lblName.Parent = pictureBox1;
            lblSDT.Parent = pictureBox1;

            lblMail.BackColor = Color.Transparent;
            lblName.BackColor = Color.Transparent;
            lblSDT.BackColor = Color.Transparent;

            //string connectionString = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SDT, Email, AvatarPath FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", FormDangNhap.Ten);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                lblSDT.Text = "SĐT: " + (dr["SDT"] != DBNull.Value ? dr["SDT"].ToString() : "Chưa có");
                                lblMail.Text = "Email: " + (dr["Email"] != DBNull.Value ? dr["Email"].ToString() : "Chưa có");
                                if (dr["AvatarPath"] != DBNull.Value)
                                {
                                    FormDangNhap.AvatarPath = dr["AvatarPath"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi");
                }

            }
            if (!string.IsNullOrEmpty(FormDangNhap.AvatarPath) && File.Exists(FormDangNhap.AvatarPath))
            {
                picAVT.Image = Image.FromFile(FormDangNhap.AvatarPath);
            }
            else
            {
                picAVT.Image = null;
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            FormDangNhap.Ten = "";
            FormDangNhap.Mail = "";
            FormDangNhap.SDT = "";

            MessageBox.Show("Đã đăng xuất tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnAVT_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện";
                ofd.Filter = "File ảnh (*.jpg; *.png; *.jpeg)|*.jpg;*.png;*.jpeg";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string duongDanAnhMoi = ofd.FileName;

                    // 1. Hiển thị ngay lên PictureBox
                    picAVT.Image = Image.FromFile(duongDanAnhMoi);

                    // 2. Lưu đường dẫn mới vào biến toàn cục
                    FormDangNhap.AvatarPath = duongDanAnhMoi;

                    // 3. (Tùy chọn) Cập nhật đường dẫn này vào Database SQL Server
                    CapNhatAvatarVaoDatabase(FormDangNhap.Ten, duongDanAnhMoi);

                    MessageBox.Show("Đã cập nhật ảnh đại diện thành công!", "Thông báo");
                }
            }
        }

            private void CapNhatAvatarVaoDatabase(string username, string AvatarPath)
        {
            string query = "UPDATE Users SET AvatarPath = @AvatarPath WHERE Username = @Username";
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=TAIKHOAN;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AvatarPath", AvatarPath);
                        cmd.Parameters.AddWithValue("@Username", username);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Đã cập nhật ảnh đại diện thành công!", "Thông báo");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật ảnh vào CSDL: " + ex.Message, "Lỗi");
                }
            }
        }
    }
}
    

