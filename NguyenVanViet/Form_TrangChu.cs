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
            
        }

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None; // Xóa thanh tiêu đề và nút X _
            childForm.Dock = DockStyle.Fill;                 // Tự giãn nở kín panel

            // Thêm Form con vào panelChildForm và hiển thị
            panelchildform.Controls.Add(childForm);
            panelchildform.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            openChildForm(new FormDangNhap());
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
                    btnDangNhap.Text = "Đăng nhập";
                }
            }

        }
    }
}
