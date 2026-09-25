using System;
using System.Windows.Forms;

namespace quanlycuahangthoitrang
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Dòng này giúp khởi chạy Form Quản lý nhập hàng khi nhấn F5
            Application.Run(new quanlynhaphang());
        }
    }
}