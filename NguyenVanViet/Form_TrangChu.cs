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
    public partial class Form_TrangChu : Form
    {
        private Form activeForm = null;
        private bool isSidebarExpanded = true;
        public Form_TrangChu()
        {
            InitializeComponent();
            sidebarTimer.Interval = 10;
            sidebarTimer.Tick += sidebarTimer_Tick;

            ApplyFlatButton(btnMenu);
            ApplyFlatButton(btn_TrangChu);
            ApplyFlatButton(btnDangNhap);
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

        private void  CapNhatGiaoDienNutTaiKhoan()
        {
            if(isSidebarExpanded)
            {
                if(!string.IsNullOrEmpty(FormDangNhap.Ten))
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
            if(activeForm != null)
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
                    CapNhatGiaoDienNutTaiKhoan();
                }
            }

        }
    }
}
