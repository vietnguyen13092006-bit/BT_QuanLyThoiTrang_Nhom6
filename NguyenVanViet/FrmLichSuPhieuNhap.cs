using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class FrmLichSuPhieuNhap : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

        private string selectedMaPN = "";
        private string selectedMaSP = "";
        private int oldSoLuong = 0;

        public FrmLichSuPhieuNhap()
        {
            InitializeComponent();
            this.Load += FrmLichSuPhieuNhap_Load;

            // Gắn sự kiện cho DataGridView và Nút sửa (giả sử tên nút trên Designer là btnSua và tên lưới là dgvLichSu)
            if (dgvLichSu != null) dgvLichSu.CellClick += DgvLichSu_CellClick;
            if (btnSua != null) btnSua.Click += BtnSua_Click;
        }

        private void FrmLichSuPhieuNhap_Load(object sender, EventArgs e)
        {
            TaiDuLieuLichSu();
        }

        private void TaiDuLieuLichSu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Bổ sung sp.MauSac và sp.Size vào câu SELECT
                    string query = @"SELECT ct.MaPN AS [Mã Phiếu Nhập], 
                                    ct.MaSP AS [Mã SP], 
                                    sp.TenSP AS [Tên Sản Phẩm], 
                                    sp.MauSac AS [Màu], 
                                    sp.Size AS [Size], 
                                    ct.SoLuong AS [Số Lượng], 
                                    ct.GiaNhap AS [Giá Nhập], 
                                    (ct.SoLuong * ct.GiaNhap * (1 + ISNULL(pn.Thue, 0) / 100.0)) AS [Thành Tiền] 
                             FROM ChiTietPhieuNhap ct 
                             JOIN SanPham sp ON ct.MaSP = sp.MaSP
                             JOIN PhieuNhap pn ON ct.MaPN = pn.MaPN
                             ORDER BY pn.NgayNhap DESC, pn.MaPN DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLichSu.DataSource = dt;

                    // Định dạng hiển thị các cột tiền tệ cho đẹp mắt
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

        private void DgvLichSu_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLichSu.Rows[e.RowIndex].Cells["Mã Phiếu Nhập"].Value != null)
            {
                DataGridViewRow row = dgvLichSu.Rows[e.RowIndex];
                selectedMaPN = row.Cells["Mã Phiếu Nhập"].Value.ToString() ?? "";
                selectedMaSP = row.Cells["Mã SP"].Value.ToString() ?? "";

                int.TryParse(row.Cells["Số Lượng"].Value?.ToString(), out oldSoLuong);
                decimal.TryParse(row.Cells["Giá Nhập"].Value?.ToString(), out decimal oldGiaNhap);

                // Nếu bạn có các ô textbox trên Designer để người dùng nhập giá trị mới, hãy gán vào đây:
                // txtSoLuongMoi.Text = oldSoLuong.ToString();
                // txtGiaNhapMoi.Text = oldGiaNhap.ToString("0");
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (dgvLichSu.SelectedRows.Count == 0 && dgvLichSu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng phiếu nhập cần sửa trên lưới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvLichSu.CurrentRow;
            string maPN = row.Cells["Mã Phiếu Nhập"].Value?.ToString() ?? "";
            string maSP = row.Cells["Mã SP"].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(maPN) || string.IsNullOrEmpty(maSP))
            {
                MessageBox.Show("Dòng được chọn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy số lượng và giá cũ từ lúc click
            int soLuongCu = oldSoLuong;

            // Lấy giá trị mới trực tiếp từ các cột tương ứng trên lưới (hoặc bạn có thể cho phép người dùng gõ sửa)
            if (!int.TryParse(row.Cells["Số Lượng"].Value?.ToString(), out int soLuongMoi) || soLuongMoi <= 0)
            {
                MessageBox.Show("Số lượng mới trên lưới không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(row.Cells["Giá Nhập"].Value?.ToString().Replace(",", ""), out decimal giaNhapMoi) || giaNhapMoi < 0)
            {
                MessageBox.Show("Giá nhập mới trên lưới không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Cập nhật lại ChiTietPhieuNhap
                    string queryUpdateCT = "UPDATE ChiTietPhieuNhap SET SoLuong = @SoLuong, GiaNhap = @GiaNhap WHERE MaPN = @MaPN AND MaSP = @MaSP";
                    using (SqlCommand cmd = new SqlCommand(queryUpdateCT, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SoLuong", soLuongMoi);
                        cmd.Parameters.AddWithValue("@GiaNhap", giaNhapMoi);
                        cmd.Parameters.AddWithValue("@MaPN", maPN);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Tính chênh lệch số lượng để bù trừ vào tồn kho (SoLuongTon) của bảng SanPham
                    int chenhLech = soLuongMoi - soLuongCu;
                    if (chenhLech != 0)
                    {
                        string queryUpdateKho = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @ChenhLech WHERE MaSP = @MaSP";
                        using (SqlCommand cmdKho = new SqlCommand(queryUpdateKho, conn, transaction))
                        {
                            cmdKho.Parameters.AddWithValue("@ChenhLech", chenhLech);
                            cmdKho.Parameters.AddWithValue("@MaSP", maSP);
                            cmdKho.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Cập nhật phiếu nhập và đồng bộ kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tải lại dữ liệu để lưới cập nhật mới
                    TaiDuLieuLichSu();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi cập nhật phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}