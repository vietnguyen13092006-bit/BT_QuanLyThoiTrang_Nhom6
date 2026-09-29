using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using FORM_DKY.GiaoDien;
using Microsoft.Data.SqlClient;

namespace FORM_DKY
{
    public partial class Form_TrangChu : Form
    {
        private Form activeForm = null;
        private bool isSidebarExpanded = true;
        public Form_TrangChu()
        {
            InitializeComponent();

            TaiDanhSachSP();
            sidebarTimer.Interval = 10;
            sidebarTimer.Tick += sidebarTimer_Tick;

            ApplyFlatButton(btnMenu);
            ApplyFlatButton(btn_TrangChu);
            ApplyFlatButton(btnDangNhap);
            ApplyFlatButton(btnQlyQAo);
        }
        private void ApplyFlatButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None; // Xóa thanh tiêu đề và nút X _
            childForm.Dock = DockStyle.Fill;                 // Tự giãn nở kín panel

            // Cập nhật form con
            childForm.FormClosed += ChildForm_FormClosed;

            // Thêm Form con vào panelChildForm và hiển thị
            panelchildform.Controls.Add(childForm);
            panelchildform.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CapNhatGiaoDienNutTaiKhoan();
        }

        private void CapNhatGiaoDienNutTaiKhoan()
        {
            if (isSidebarExpanded)
            {
                if (!string.IsNullOrEmpty(FormDangNhap.Ten))
                {
                    btnDangNhap.Text = "  " + FormDangNhap.Ten;
                }
                else
                {
                    btnDangNhap.Text = "Đăng nhập";
                }
            }
            else
            {
                btnDangNhap.Text = "👤";
            }
        }


        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                openChildForm(new FormDangNhap());
            }
            else
            {
                openChildForm(new Form_TTTKHOAN());
            }
        }
        private void btn_TrangChu_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }
        }
        private void btnMenu_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }


        // Khi di chuột RA KHỎI vùng Sidebar -> Cho Timer chạy để THU GỌN MENU IN
        private void Form_Menu_Load(object sender, EventArgs e)
        {

        }
        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
            if (isSidebarExpanded)
            {
                // Thu gọn
                panelsidebar.Width -= 15;
                if (panelsidebar.Width <= panelsidebar.MinimumSize.Width)
                {
                    isSidebarExpanded = false;
                    sidebarTimer.Stop();
                    btn_TrangChu.Text = "🏠";
                    btnDangNhap.Text = "👤";
                    btnQlyQAo.Text = "🛍️";
                    //btn_TrangChu.Text = "";
                    //btnDangNhap.Text = "";
                }
            }
            else
            {
                // Xòe ra
                panelsidebar.Width += 15;
                if (panelsidebar.Width >= panelsidebar.MaximumSize.Width)
                {
                    isSidebarExpanded = true;
                    sidebarTimer.Stop();
                    btn_TrangChu.Text = "Trang chủ";
                    btnQlyQAo.Text = "Quản lý";
                    CapNhatGiaoDienNutTaiKhoan();
                }
            }

        }

        private void btnQlyQAo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem quản lý cửa hàng", "Thông báo", MessageBoxButtons.OK); ;
            }
            else
            {
                openChildForm(new FrmSanPham());
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string TuKhoa = txtTkiem.Text.Trim();
            MessageBox.Show("Bạn vừa tìm kiếm: " + TuKhoa);
        }
        private void TaiDanhSachSP()
        {
            DANHMUC.Controls.Clear();
            string sql = "SELECT* FROM SanPham";
            DataTable dt = Database.GetData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ShowSP SP = new ShowSP();
                    string ma = row["MaSanPham"] != DBNull.Value ? row["MaSanPham"].ToString() : "";
                    string ten = row["TenSanPham"] != DBNull.Value ? row["TenSanPham"].ToString() : "";
                    decimal gia = row["GiaBan"] != DBNull.Value ? Convert.ToDecimal(row["GiaBan"]) : 0;
                    string mau = dt.Columns.Contains("MauSac") && row["MauSac"] != DBNull.Value ? row["MauSac"].ToString() : "";
                    string size = dt.Columns.Contains("Size") && row["Size"] != DBNull.Value ? row["Size"].ToString() : "";
                    string ghiChu = dt.Columns.Contains("GhiChu") && row["GhiChu"] != DBNull.Value ? row["GhiChu"].ToString() : "";
                    string hinh = row["HinhAnh"] != DBNull.Value ? row["HinhAnh"].ToString() : "";
                    SP.ThongTinSP(ma, ten, gia, mau, size, ghiChu,hinh);
                    DANHMUC.Controls.Add(SP);
                }
            }

        }

           private void Form_TrangChu_Load(object sender, EventArgs e)
        {
            // 1. Ép thanh tìm kiếm và nút bấm ra làm con trực tiếp của Form (không nằm trong panel/flow nào nữa)
            txtTkiem.Parent = this;
            btnTimKiem.Parent = this;

            // 2. Tắt Dock = Fill của DANHMUC
            DANHMUC.Dock = DockStyle.None;

            // 3. Chỉnh kích thước ô tìm kiếm cho rộng rãi
            txtTkiem.Size = new Size(250, 30);
            btnTimKiem.Size = new Size(80, 30);

            // 4. Tính toán căn ra CHÍNH GIỮA chiều ngang Form
            int totalWidth = txtTkiem.Width + btnTimKiem.Width + 10;
            int startX = (this.ClientSize.Width - totalWidth) / 2;

            txtTkiem.Location = new Point(startX, 15);
            btnTimKiem.Location = new Point(startX + txtTkiem.Width + 10, 14);

            // 5. Đặt DANHMUC bên dưới khoảng trống (Y = 60)
            DANHMUC.Location = new Point(10, 60);
            DANHMUC.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 70);
            DANHMUC.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // 6. Ép hiển thị lên trên cùng
            txtTkiem.Visible = true;
            btnTimKiem.Visible = true;
            txtTkiem.BringToFront();
            btnTimKiem.BringToFront();
        }

        private void panelchildform_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
