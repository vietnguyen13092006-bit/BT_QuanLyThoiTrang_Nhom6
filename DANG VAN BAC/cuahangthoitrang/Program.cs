using System;
using System.Windows.Forms;
using QuanLyCuaHangThoiTrang.GiaoDien; // Gọi namespace chứa Form trong folder GiaoDien

namespace cuahangthoitrang
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi chạy chính của ứng dụng cuahangthoitrang
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Chạy trực tiếp Form Tra Cứu & Tìm Kiếm
            Application.Run(new frmTimKiemTraCuuThongTin());
        }
    }
}