using System;
using System.Windows.Forms;

namespace QUANLYBANHANGVAHOADON
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Khởi chạy Form Quản Lý Hóa Đơn
            Application.Run(new FrmQuanLyHoaDonBanHang());
        }
    }
}