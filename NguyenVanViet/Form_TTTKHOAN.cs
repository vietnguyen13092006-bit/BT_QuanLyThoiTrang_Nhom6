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
            lblName.Text = "Tên: " + FormDangNhap.Ten;
            lblMail.Text = "Email: " + FormDangNhap.Mail;
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            FormDangNhap.Ten = "";
            FormDangNhap.Mail = "";

            MessageBox.Show("Đã đăng xuất tài khoản!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
        }
    }
}
