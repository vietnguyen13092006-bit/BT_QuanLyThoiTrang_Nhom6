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
            {
                activeForm.Close();
                activeForm.Dispose();
                activeForm = null;
            }
            txtTkiem.Visible = false;
            btnTimKiem.Visible = false;
            DANHMUC.Visible = false;

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None; // Xóa thanh tiêu đề và nút X _
            childForm.Dock = DockStyle.Fill;                 // Tự giãn nở kín panel

            // Cập nhật form con
            childForm.FormClosed += ChildForm_FormClosed;

            // Thêm Form con vào panelChildForm và hiển thị
            panelchildform.Controls.Add(childForm);
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
            // 1. Nếu đang mở Form con khác (Đăng nhập / Quản lý / TTKHoan), hãy ĐÓNG NÓ LẠI
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm.Dispose();
                activeForm = null;
            }

            txtTkiem.Visible = true;
            btnTimKiem.Visible = true;
            DANHMUC.Visible = true;
            TaiDanhSachSP();
        }
        private void btnMenu_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }


        // Khi di chuột RA KHỎI vùng Sidebar -> Cho Timer chạy để THU GỌN MENU IN
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
            // 1. Đưa các control Tìm kiếm và DANHMUC vào BÊN TRONG panelchildform (để không bị panel che)
            panelchildform.Controls.Add(txtTkiem);
            panelchildform.Controls.Add(btnTimKiem);
            panelchildform.Controls.Add(DANHMUC);

            // 2. Vị trí TextBox & Nút Tìm kiếm
            txtTkiem.Size = new Size(250, 30);
            txtTkiem.Location = new Point(20, 12);

            btnTimKiem.Size = new Size(90, 30);
            btnTimKiem.Location = new Point(280, 11);

            // 3. Định vị FlowLayoutPanel DANHMUC nằm ngay dưới thanh tìm kiếm
            DANHMUC.Dock = DockStyle.None;
            DANHMUC.Location = new Point(10, 50);
            DANHMUC.Size = new Size(panelchildform.ClientSize.Width - 20, panelchildform.ClientSize.Height - 60);
            DANHMUC.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            txtTkiem.BringToFront();
            btnTimKiem.BringToFront();
            DANHMUC.BringToFront();

            // 4. Tải danh sách sản phẩm
            TaiDanhSachSP();
        }


        private void panelchildform_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
