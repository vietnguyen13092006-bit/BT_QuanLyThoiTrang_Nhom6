using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
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

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            TaiDanhSachSP();
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
            TaiDanhSachSP();
        }
        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (isSidebarExpanded)
            {
                // Thu gọn ngay lập tức về kích thước nhỏ
                panelsidebar.Width = panelsidebar.MinimumSize.Width;
                isSidebarExpanded = false;
                btn_TrangChu.Text = "🏠";
                btnDangNhap.Text = "👤";
                btnQlyQAo.Text = "🛍️";
                btnHdon.Text = "🧾";
                btnBaocao.Text = "📈";
                btnPhieuNhap.Text = "📋";
            }
            else
            {
                // Mở rộng ngay lập tức
                panelsidebar.Width = panelsidebar.MaximumSize.Width;
                isSidebarExpanded = true;
                btn_TrangChu.Text = "Trang chủ";
                btnQlyQAo.Text = "Qly quần áo";
                btnHdon.Text = "Qly hóa đơn";
                btnBaocao.Text = "Báo cáo thống kê";
                btnPhieuNhap.Text = "Phiếu nhập";
                CapNhatGiaoDienNutTaiKhoan();
            }
        }
        // Bạn có thể xóa hẳn hoặc để trống sự kiện sidebarTimer_Tick đi vì không dùng nữa.

        private void btnQlyQAo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem quản lý quần áo", "Thông báo", MessageBoxButtons.OK); ;
            }
            else
            {
                openChildForm(new FrmSanPham());
            }
        }

        private void TaiDanhSachSP()
        {
            panelchildform.Controls.Clear();
            ShowSP showtt = new ShowSP();
            showtt.Dock = DockStyle.Fill;
            panelchildform.Controls.Add(showtt);
            showtt.BringToFront();
            showtt.Show();
        }

        private void Form_TrangChu_Load(object sender, EventArgs e)
        {
            TaiDanhSachSP();
        }


        private void btnHdon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem quản lý hóa đơn", "Thông báo", MessageBoxButtons.OK); ;
            }
            else
            {
                openChildForm(new FrmQuanLyHoaDonBanHang());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem quản lý hóa đơn", "Thông báo", MessageBoxButtons.OK); ;
            }
            else
            {
                openChildForm(new frmBaoCaoThongKe());
            }
        }

        private void btnPhieuNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem quản lý hóa đơn", "Thông báo", MessageBoxButtons.OK); ;
            }
            else
            {
                openChildForm(new FrmPhieuNhap());
            }
        }
    }
}
