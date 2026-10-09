using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class FrmLichSuPhieuNhap : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

        public FrmLichSuPhieuNhap()
        {
            InitializeComponent();
            this.Load += FrmLichSuPhieuNhap_Load;
        }

        private void FrmLichSuPhieuNhap_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT ct.MaPN AS [Mã Phiếu Nhập], 
                                            ct.MaSP AS [Mã SP], 
                                            sp.TenSP AS [Tên Sản Phẩm], 
                                            ct.SoLuong AS [Số Lượng], 
                                            ct.GiaNhap AS [Giá Nhập], 
                                            (ct.SoLuong * ct.GiaNhap * (1 + ISNULL(pn.Thue, 0) / 100.0)) AS [Thành Tiền] 
                                     FROM ChiTietPhieuNhap ct 
                                     JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                     JOIN PhieuNhap pn ON ct.MaPN = pn.MaPN";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLichSu.DataSource = dt;

                    if (dgvLichSu.Columns["Giá Nhập"] != null)
                        dgvLichSu.Columns["Giá Nhập"].DefaultCellStyle.Format = "N0";
                    if (dgvLichSu.Columns["Thành Tiền"] != null)
                        dgvLichSu.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải lịch sử chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}