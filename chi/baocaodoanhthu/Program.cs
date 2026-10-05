using System;
using System.Windows.Forms;

namespace baocaodoanhthu
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Chạy Form báo cáo thống kê
            Application.Run(new giaodien.frmBaoCaoThongKe());
        }
    }
}