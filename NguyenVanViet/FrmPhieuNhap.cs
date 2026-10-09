using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class FrmPhieuNhap : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

        public FrmPhieuNhap()
        {
            InitializeComponent();
            this.Load += FrmPhieuNhap_Load;
        }

        private void FrmPhieuNhap_Load(object sender, EventArgs e)
        {
            txtPN.Text = "PN" + DateTime.Now.ToString("yyyyMMddHHmmss");
            dateTimePicker1.Value = DateTime.Now;

            // Kiểm tra và tạo cột cho DataGridView nếu chưa có
            if (dgvPN.Columns.Count == 0)
            {
                dgvPN.Columns.Add("MaSP", "Mã SP");
                dgvPN.Columns.Add("TenSP", "Tên sản phẩm");
                dgvPN.Columns.Add("SoLuong", "Số lượng");
                dgvPN.Columns.Add("GiaNhap", "Giá nhập");
                dgvPN.Columns.Add("ThanhTien", "Thành tiền");
            }

            // === ĐOẠN CODE NÀY ĐỂ ĐỊNH DẠNG DẤU PHẨY CHO BẢNG 1 (dgvPN) ===
            if (dgvPN.Columns["GiaNhap"] != null)
            {
                dgvPN.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            }
            if (dgvPN.Columns["ThanhTien"] != null)
            {
                dgvPN.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }
            // ==============================================================

            // Đổ dữ liệu sản phẩm vào ComboBox
            LoadDanhSachSanPham();
            LoadChiTietPhieuNhap();

            if (string.IsNullOrEmpty(txtThue.Text))
            {
                txtThue.Text = "10";
            }
        }
        private void LoadChiTietPhieuNhap()
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

                    dgvLoad.DataSource = dt;

                    // Định dạng hiển thị tiền tệ
                    if (dgvLoad.Columns["Giá Nhập"] != null)
                    {
                        dgvLoad.Columns["Giá Nhập"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvLoad.Columns["Thành Tiền"] != null)
                    {
                        dgvLoad.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải chi tiết phiếu nhập lên dgvLoad: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedItem is DataRowView row)
            {
                // Nếu có ô textbox hiển thị Mã SP, code sẽ tự điền vào đây:
                txtMaSP.Text = row["MaSP"].ToString();
            }
        }
        private void LoadDanhSachSanPham()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaSP, TenSP FROM SanPham";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "TenSP";
                    comboBox1.ValueMember = "MaSP";
                    comboBox1.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThemSp_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSP = comboBox1.SelectedValue.ToString();
            string tenSP = comboBox1.Text;

            if (!int.TryParse(txtSoluong.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng nhập phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoluong.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaNhap.Text.Trim().Replace(",", ""), out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return;
            }

            // Lấy phần trăm thuế từ txtThue (mặc định 0 nếu trống)
            decimal phanTramThue = 0;
            decimal.TryParse(txtThue.Text.Trim(), out phanTramThue);

            // --- BẮT ĐẦU ĐOẠN CODE TÍNH TOÁN VÀ THÊM VÀO DGVPN ---
            // Tính thành tiền có bao gồm thuế cho từng dòng sản phẩm
            decimal tienHang = soLuong * giaNhap;
            decimal thanhTien = tienHang + (tienHang * (phanTramThue / 100));

            bool daTonTai = false;
            foreach (DataGridViewRow row in dgvPN.Rows)
            {
                if (row.Cells["MaSP"].Value != null && row.Cells["MaSP"].Value.ToString() == maSP)
                {
                    int slCu = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    int slMoi = slCu + soLuong;
                    decimal tienHangMoi = slMoi * giaNhap;

                    row.Cells["SoLuong"].Value = slMoi;
                    // Cập nhật lại thành tiền có gộp thuế khi cộng dồn sản phẩm
                    row.Cells["ThanhTien"].Value = tienHangMoi + (tienHangMoi * (phanTramThue / 100));
                    daTonTai = true;
                    break;
                }
            }

            if (!daTonTai)
            {
                // Lúc add vào row:
                dgvPN.Rows.Add(maSP, tenSP, soLuong, giaNhap, thanhTien);
            }
            // ----------------------------------------------------

            TinhTongTien();

            txtSoluong.Clear();
            txtGiaNhap.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox1.Focus();
        }

        private void TinhTongTien()
        {
            decimal tongTienHang = 0;
            foreach (DataGridViewRow row in dgvPN.Rows)
            {
                // Lấy tiền hàng gốc (Số lượng * Giá nhập) để tính thuế chuẩn xác tổng phiếu
                if (row.Cells["SoLuong"].Value != null && row.Cells["GiaNhap"].Value != null)
                {
                    int sl = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    decimal gn = Convert.ToDecimal(row.Cells["GiaNhap"].Value);
                    tongTienHang += sl * gn;
                }
            }

            decimal phanTramThue = 0;
            decimal.TryParse(txtThue.Text, out phanTramThue);

            decimal tienThue = tongTienHang * (phanTramThue / 100);
            decimal tongThanhToan = tongTienHang + tienThue;

            // Hiển thị lên giao diện tương ứng (label7 là tiền hàng, lblThue là tiền thuế, v.v.)
            label7.Text = tongTienHang.ToString("N0") + " VNĐ";
            lblThue.Text = tienThue.ToString("N0") + " VNĐ";
        }

        private void txtThue_TextChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }
        // Khi rời khỏi ô (Sự kiện Leave):
        private void txtGiaNhap_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtGiaNhap.Text.Replace(",", ""), out decimal number))
            {
                txtGiaNhap.Text = number.ToString("N0"); // Tự động thêm dấu phẩy
            }
        }

        // Khi click vào lại để sửa (Sự kiện Enter):
        private void txtGiaNhap_Enter(object sender, EventArgs e)
        {
            // Bỏ dấu phẩy đi để người dùng sửa số cho dễ
            txtGiaNhap.Text = txtGiaNhap.Text.Replace(",", "");
        }
        private void btnLuuPhieu_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên lập phiếu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Focus();
                return;
            }

            if (dgvPN.Rows.Count == 0 || (dgvPN.Rows.Count == 1 && dgvPN.Rows[0].Cells["MaSP"].Value == null))
            {
                MessageBox.Show("Chưa có sản phẩm nào trong danh sách nhập hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tongTienThanhToan = 0;
            foreach (DataGridViewRow row in dgvPN.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    tongTienThanhToan += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
                }
            }

            decimal phanTramThue = 0;
            decimal.TryParse(txtThue.Text, out phanTramThue);
            tongTienThanhToan += tongTienThanhToan * (phanTramThue / 100);

            string maPN = txtPN.Text.Trim();
            DateTime ngayNhap = dateTimePicker1.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Kiểm tra xem mã nhân viên gõ vào có thực sự tồn tại trong bảng NhanVien không để tránh lỗi khóa ngoại
                    string checkNVQuery = "SELECT COUNT(1) FROM NhanVien WHERE MaNV = @MaNV";
                    using (SqlCommand cmdCheck = new SqlCommand(checkNVQuery, conn, transaction))
                    {
                        cmdCheck.Parameters.AddWithValue("@MaNV", maNV);
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count == 0)
                        {
                            throw new Exception("Mã nhân viên '" + maNV + "' không tồn tại trong hệ thống!");
                        }
                    }

                    // Thêm vào bảng PhieuNhap
                    string queryPN = "INSERT INTO PhieuNhap (MaPN, NgayNhap, MaNV, TongTienNhap, Thue) VALUES (@MaPN, @NgayNhap, @MaNV, @TongTienNhap, @Thue)";
                    using (SqlCommand cmd = new SqlCommand(queryPN, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaPN", maPN);
                        cmd.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@TongTienNhap", tongTienThanhToan);
                        cmd.Parameters.AddWithValue("@Thue", phanTramThue); // Lưu phần trăm thuế vào CSDL
                        cmd.ExecuteNonQuery();
                    }

                    // Thêm chi tiết phiếu nhập và cập nhật tồn kho
                    foreach (DataGridViewRow row in dgvPN.Rows)
                    {
                        if (row.Cells["MaSP"].Value == null) continue;

                        string maSP = row.Cells["MaSP"].Value.ToString();
                        int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                        decimal giaNhap = Convert.ToDecimal(row.Cells["GiaNhap"].Value);

                        string queryCT = "INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuong, GiaNhap) VALUES (@MaPN, @MaSP, @SoLuong, @GiaNhap)";
                        using (SqlCommand cmdCT = new SqlCommand(queryCT, conn, transaction))
                        {
                            cmdCT.Parameters.AddWithValue("@MaPN", maPN);
                            cmdCT.Parameters.AddWithValue("@MaSP", maSP);
                            cmdCT.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdCT.Parameters.AddWithValue("@GiaNhap", giaNhap);
                            cmdCT.ExecuteNonQuery();
                        }

                        // Vừa cộng dồn số lượng tồn, vừa cập nhật giá nhập mới nhất cho sản phẩm đó
                        string queryUpdateKho = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @SoLuong, GiaNhap = @GiaNhap WHERE MaSP = @MaSP";
                        using (SqlCommand cmdKho = new SqlCommand(queryUpdateKho, conn, transaction))
                        {
                            cmdKho.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdKho.Parameters.AddWithValue("@GiaNhap", giaNhap); // Cập nhật giá nhập mới nhất
                            cmdKho.Parameters.AddWithValue("@MaSP", maSP);
                            cmdKho.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Lập phiếu nhập thành công và đã cập nhật kho hàng!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dgvPN.Rows.Clear();
                    FrmPhieuNhap_Load(sender, e);
                    txtMaNV.Clear();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi lưu phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThemNhanhSP_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại nhỏ để nhập tên sản phẩm
            string tenSP = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên sản phẩm mới:", "Thêm nhanh sản phẩm", "", -1, -1);

            if (string.IsNullOrWhiteSpace(tenSP))
            {
                return; // Người dùng bấm Hủy hoặc không nhập gì thì bỏ qua
            }

            // Tự động sinh mã sản phẩm dựa theo thời gian để không bị trùng và đỡ mất công gõ
            string maSP = "SP" + DateTime.Now.ToString("yyMMddHHmmss");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Thêm nhanh vào bảng SanPham với số lượng tồn ban đầu là 0
                    string query = "INSERT INTO SanPham (MaSP, TenSP, SoLuongTon) VALUES (@MaSP, @TenSP, 0)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.Parameters.AddWithValue("@TenSP", tenSP.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại danh sách sản phẩm trong ComboBox
                    LoadDanhSachSanPham();

                    // Tự động chọn sản phẩm vừa thêm vào ComboBox để nhập kho
                    comboBox1.SelectedValue = maSP;
                    txtSoluong.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}