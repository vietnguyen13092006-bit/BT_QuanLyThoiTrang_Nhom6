using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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

            // 🟢 Chỉ giữ lại thiết lập màu sắc và bo tròn cho các nút trong sidebar
            foreach (Control ctrl in panelsidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Cursor = Cursors.Hand;
                    btn.BackColor = Color.FromArgb(220, 228, 238);
                    btn.ForeColor = Color.FromArgb(40, 40, 40);

                    int radius = 12;
                    btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius));

                    btn.SizeChanged += (s, e) =>
                    {
                        if (btn.Width > 0 && btn.Height > 0)
                        {
                            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius));
                        }
                    };
                }
            }

            TaiDanhSachSP();
        }

        // Hàm API Windows để cắt bo tròn góc mượt mà, không giật lag trên máy ảo
        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        // Hàm làm nổi bật (highlight) nút tương ứng với form đang mở
        private void HighlightButton(Button activeBtn)
        {
            foreach (Control ctrl in panelsidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(220, 228, 238); // Trả về màu mặc định
                    btn.ForeColor = Color.FromArgb(40, 40, 40);
                }
            }

            if (activeBtn != null)
            {
                activeBtn.BackColor = Color.FromArgb(170, 195, 225); // Màu đậm hơn khi được chọn
                activeBtn.ForeColor = Color.Black;
            }
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
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            childForm.FormClosed += ChildForm_FormClosed;

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
                    btnDangNhap.Text = "  Đăng nhập";
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
            HighlightButton(btn_TrangChu);
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
                // Thu gọn sidebar
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
                // Mở rộng sidebar
                panelsidebar.Width = panelsidebar.MaximumSize.Width;
                isSidebarExpanded = true;

                btn_TrangChu.Text = "Trang chủ";
                btnQlyQAo.Text = "Qly quần áo";
                btnHdon.Text = "Qly hóa đơn";
                btnBaocao.Text = "Báo cáo";
                btnPhieuNhap.Text = "Phiếu nhập";
                CapNhatGiaoDienNutTaiKhoan();
            }

            // 🟢 Tự động ép tất cả các nút co giãn theo chiều rộng mới của sidebar và cập nhật lại góc bo tròn
            int targetWidth = panelsidebar.Width - 20; // Trừ khoảng lề 2 bên cho đẹp
            if (targetWidth < 40) targetWidth = panelsidebar.Width - 8; // Đảm bảo không bị âm khi thu cực nhỏ

            foreach (Control ctrl in panelsidebar.Controls)
            {
                if (ctrl is Button btn && btn != btnMenu)
                {
                    btn.Width = targetWidth;

                    // Cập nhật lại bo tròn theo kích thước mới
                    int radius = 12;
                    btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius));
                }
            }
        }

        private void btnQlyQAo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem quản lý quần áo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                HighlightButton(btnQlyQAo);
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
            HighlightButton(btn_TrangChu); // Mặc định sáng nút Trang chủ khi vừa mở app
            TaiDanhSachSP();
        }

        private void btnHdon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem quản lý hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                HighlightButton(btnHdon);
                openChildForm(new FrmQuanLyHoaDonBanHang());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem báo cáo thống kê", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                HighlightButton(btnBaocao);
                openChildForm(new frmBaoCaoThongKe());
            }
        }

        private void btnPhieuNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormDangNhap.Ten))
            {
                MessageBox.Show("Bạn cần đăng nhập để xem phiếu nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                HighlightButton(btnPhieuNhap);
                openChildForm(new FrmPhieuNhap());
            }
        }
    }
}