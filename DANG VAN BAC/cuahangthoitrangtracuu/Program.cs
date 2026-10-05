using cuahangthoitrang.GiaoDien;

namespace cuahangthoitrang
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmTimKiemTraCuuThongTin());
        }
    }
}