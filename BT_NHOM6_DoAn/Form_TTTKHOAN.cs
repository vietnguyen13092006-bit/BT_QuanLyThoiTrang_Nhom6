using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class Form_TTTKHOAN : Form
    {
        // Sử dụng chung chuỗi kết nối đến Database QuanLyCuaHangThoiTrang
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

        public Form_TTTKHOAN()
        {
            InitializeComponent();
        }

        private void Form_TTTKHOAN_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = ImageHelper.GetImage("NenTT.jpg");

            // Hiển thị Tên tài khoản
            lblName.Text = "Tên: " + FormDangNhap.Ten;

            // Hiển thị Mã Nhân Viên từ UserSession đã được lưu lúc đăng nhập thành công
            lblManv.Text = "Mã NV: " + (!string.IsNullOrEmpty(UserSession.MaNV) ? UserSession.MaNV : "Chưa có");

            // Truy vấn thông tin SĐT, Email, Avatar của tài khoản từ CSDL
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
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Hiển thị ảnh đại diện nếu đường dẫn tồn tại
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
            // Reset toàn bộ thông tin tài khoản và UserSession để bảo mật phiên làm việc
            FormDangNhap.Ten = "";
            FormDangNhap.Mail = "";
            FormDangNhap.SDT = "";
            FormDangNhap.AvatarPath = "";

            UserSession.MaNV = "";
            UserSession.TenNV = "";

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

                    // 3. Cập nhật đường dẫn này vào Database SQL Server
                    CapNhatAvatarVaoDatabase(FormDangNhap.Ten, duongDanAnhMoi);
                }
            }
        }

        private void CapNhatAvatarVaoDatabase(string username, string avatarPath)
        {
            string query = "UPDATE Users SET AvatarPath = @AvatarPath WHERE Username = @Username";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AvatarPath", avatarPath);
                        cmd.Parameters.AddWithValue("@Username", username);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Đã cập nhật ảnh đại diện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật ảnh vào CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}