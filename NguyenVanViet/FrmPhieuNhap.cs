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

            // Thiết lập cấu trúc cột cho dgvPN nếu chưa có (Có đầy đủ Mã SP, Tên, Màu, Size, SL, Giá, Thành tiền)
            if (dgvPN.Columns.Count == 0)
            {
                dgvPN.Columns.Add("MaSP", "Mã SP");
                dgvPN.Columns.Add("TenSP", "Tên sản phẩm");
                dgvPN.Columns.Add("MauSac", "Màu");
                dgvPN.Columns.Add("Size", "Size");
                dgvPN.Columns.Add("SoLuong", "Số lượng");
                dgvPN.Columns.Add("GiaNhap", "Giá nhập");
                dgvPN.Columns.Add("ThanhTien", "Thành tiền");
            }

            // Cấu hình FullRowSelect để click chọn cả dòng mượt mà
            dgvPN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPN.MultiSelect = false;

            // Đăng ký sự kiện CellClick để đẩy ngược dữ liệu lên ô nhập
            dgvPN.CellClick -= DgvPN_CellClick;
            dgvPN.CellClick += DgvPN_CellClick;

            // Định dạng hiển thị tiền tệ
            if (dgvPN.Columns["GiaNhap"] != null)
                dgvPN.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            if (dgvPN.Columns["ThanhTien"] != null)
                dgvPN.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

            LoadDanhSachSanPham();
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            if (string.IsNullOrEmpty(txtThue.Text))
            {
                txtThue.Text = "10";
            }
        }

        // Sự kiện click dòng trên lưới tạm để đẩy ngược lên các ô nhập liệu
        private void DgvPN_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPN.Rows[e.RowIndex].Cells["MaSP"].Value != null)
            {
                DataGridViewRow row = dgvPN.Rows[e.RowIndex];

                if (txtMaSP != null)
                    txtMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? "";

                if (txtSoluong != null)
                    txtSoluong.Text = row.Cells["SoLuong"].Value?.ToString() ?? "";

                if (txtGiaNhap != null)
                {
                    if (decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out decimal gia))
                        txtGiaNhap.Text = gia.ToString("N0");
                    else
                        txtGiaNhap.Text = row.Cells["GiaNhap"].Value?.ToString() ?? "";
                }

                if (txtMau != null)
                    txtMau.Text = row.Cells["MauSac"].Value?.ToString() ?? "";

                if (txtSize != null)
                    txtSize.Text = row.Cells["Size"].Value?.ToString() ?? "";

                string tenSPFull = row.Cells["TenSP"].Value?.ToString() ?? "";
                comboBox1.Text = tenSPFull;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedItem is DataRowView row)
            {
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

        // NÚT THÊM SP VÀO GIỎ TẠM (CHỈ LƯU TRÊN LƯỚI, KHÔNG ĐỤNG DATABASE)
        private void btnThemSp_Click(object sender, EventArgs e)
        {
            string tenSanPhamGoc = comboBox1.Text.Trim();
            string mauSac = txtMau != null ? txtMau.Text.Trim() : "";
            string size = txtSize != null ? txtSize.Text.Trim() : "";

            if (string.IsNullOrEmpty(tenSanPhamGoc) || string.IsNullOrEmpty(mauSac) || string.IsNullOrEmpty(size))
            {
                MessageBox.Show("Vui lòng chọn Tên sản phẩm và nhập đầy đủ Màu sắc, Kích thước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            // Gán mã tạm hoặc lấy mã cũ nếu sản phẩm đã có sẵn trong combobox
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP) || maSP.StartsWith("SP_TEMP"))
            {
                maSP = "SP_TEMP_" + Guid.NewGuid().ToString().Substring(0, 6);
            }

            decimal phanTramThue = 0;
            decimal.TryParse(txtThue.Text.Trim(), out phanTramThue);

            decimal tienHang = soLuong * giaNhap;
            decimal thanhTien = tienHang + (tienHang * (phanTramThue / 100));

            // Kiểm tra xem trên lưới tạm đã có sản phẩm này chưa -> nếu có thì cộng dồn số lượng
            bool daTonTai = false;
            foreach (DataGridViewRow row in dgvPN.Rows)
            {
                if (row.Cells["TenSP"]?.Value?.ToString() == tenSanPhamGoc &&
                    row.Cells["MauSac"]?.Value?.ToString() == mauSac &&
                    row.Cells["Size"]?.Value?.ToString() == size)
                {
                    int slCu = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    int slMoi = slCu + soLuong;
                    decimal tienHangMoi = slMoi * giaNhap;

                    row.Cells["SoLuong"].Value = slMoi;
                    row.Cells["ThanhTien"].Value = tienHangMoi + (tienHangMoi * (phanTramThue / 100));
                    daTonTai = true;
                    break;
                }
            }

            if (!daTonTai)
            {
                dgvPN.Rows.Add(maSP, tenSanPhamGoc, mauSac, size, soLuong, giaNhap, thanhTien);
            }

            TinhTongTien();

            // Reset ô nhập
            txtSoluong.Clear();
            txtGiaNhap.Clear();
            if (txtMau != null) txtMau.Clear();
            if (txtSize != null) txtSize.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox1.Focus();
        }

        // NÚT SỬA DÒNG TRÊN LƯỚI TẠM
        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (dgvPN.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPN.SelectedRows[0];

                if (!int.TryParse(txtSoluong.Text.Trim(), out int soLuongMoi) || soLuongMoi <= 0)
                {
                    MessageBox.Show("Số lượng mới không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoluong.Focus();
                    return;
                }

                if (!decimal.TryParse(txtGiaNhap.Text.Trim().Replace(",", ""), out decimal giaNhapMoi) || giaNhapMoi < 0)
                {
                    MessageBox.Show("Giá nhập mới không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaNhap.Focus();
                    return;
                }

                string tenSPMoi = comboBox1.Text.Trim();
                string mauMoi = txtMau != null ? txtMau.Text.Trim() : "";
                string sizeMoi = txtSize != null ? txtSize.Text.Trim() : "";

                if (string.IsNullOrEmpty(tenSPMoi) || string.IsNullOrEmpty(mauMoi) || string.IsNullOrEmpty(sizeMoi))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Tên, Màu và Size!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal phanTramThue = 0;
                decimal.TryParse(txtThue.Text.Trim(), out phanTramThue);

                decimal tienHangMoi = soLuongMoi * giaNhapMoi;
                decimal thanhTienMoi = tienHangMoi + (tienHangMoi * (phanTramThue / 100));

                // Cập nhật lại trực tiếp trên dòng của lưới tạm
                row.Cells["TenSP"].Value = tenSPMoi;
                row.Cells["MauSac"].Value = mauMoi;
                row.Cells["Size"].Value = sizeMoi;
                row.Cells["SoLuong"].Value = soLuongMoi;
                row.Cells["GiaNhap"].Value = giaNhapMoi;
                row.Cells["ThanhTien"].Value = thanhTienMoi;

                TinhTongTien();
                MessageBox.Show("Cập nhật dòng tạm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng sản phẩm cần sửa trên danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // NÚT XÓA DÒNG TRÊN LƯỚI TẠM
        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (dgvPN.SelectedRows.Count > 0)
            {
                dgvPN.Rows.RemoveAt(dgvPN.SelectedRows[0].Index);
                TinhTongTien();
                MessageBox.Show("Đã xóa sản phẩm khỏi danh sách tạm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng sản phẩm cần xóa trong danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TinhTongTien()
        {
            decimal tongTienHang = 0;
            foreach (DataGridViewRow row in dgvPN.Rows)
            {
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
        }

        private void txtThue_TextChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void txtGiaNhap_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtGiaNhap.Text.Replace(",", ""), out decimal number))
            {
                txtGiaNhap.Text = number.ToString("N0");
            }
        }

        private void txtGiaNhap_Enter(object sender, EventArgs e)
        {
            txtGiaNhap.Text = txtGiaNhap.Text.Replace(",", "");
        }

        // NÚT LƯU HÓA ĐƠN (ĐẨY TOÀN BỘ VÀO DATABASE BẰNG TRANSACTION)
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

            decimal tongTienHangChuaThue = 0;
            foreach (DataGridViewRow row in dgvPN.Rows)
            {
                if (row.Cells["SoLuong"].Value != null && row.Cells["GiaNhap"].Value != null)
                {
                    int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    decimal giaNhap = Convert.ToDecimal(row.Cells["GiaNhap"].Value);
                    tongTienHangChuaThue += soLuong * giaNhap;
                }
            }

            decimal phanTramThue = 0;
            decimal.TryParse(txtThue.Text, out phanTramThue);

            decimal tienThue = Math.Round(tongTienHangChuaThue * (phanTramThue / 100), 0);
            decimal tongTienThanhToan = tongTienHangChuaThue + tienThue;

            string maPN = txtPN.Text.Trim();
            DateTime ngayNhap = dateTimePicker1.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Kiểm tra nhân viên tồn tại
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

                    // 2. Lưu bảng PhieuNhap chính
                    string queryPN = "INSERT INTO PhieuNhap (MaPN, NgayNhap, MaNV, TongTienNhap, Thue) VALUES (@MaPN, @NgayNhap, @MaNV, @TongTienNhap, @Thue)";
                    using (SqlCommand cmd = new SqlCommand(queryPN, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaPN", maPN);
                        cmd.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@TongTienNhap", tongTienThanhToan);
                        cmd.Parameters.AddWithValue("@Thue", phanTramThue);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Duyệt qua từng dòng trên lưới tạm để xử lý Sản phẩm và Chi Tiết Phiếu Nhập
                    foreach (DataGridViewRow row in dgvPN.Rows)
                    {
                        if (row.Cells["TenSP"].Value == null) continue;

                        string tenSP = row.Cells["TenSP"].Value.ToString() ?? "";
                        string mauSac = row.Cells["MauSac"].Value?.ToString() ?? "";
                        string size = row.Cells["Size"].Value?.ToString() ?? "";
                        int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                        decimal giaNhap = Convert.ToDecimal(row.Cells["GiaNhap"].Value);
                        string maSPChinhThuc = "";

                        // Kiểm tra xem biến thể (Tên + Màu + Size) đã tồn tại trong CSDL chưa
                        string queryCheckSP = "SELECT MaSP FROM SanPham WHERE TenSP = @TenSP AND MauSac = @MauSac AND Size = @Size";
                        using (SqlCommand cmdCheckSP = new SqlCommand(queryCheckSP, conn, transaction))
                        {
                            cmdCheckSP.Parameters.AddWithValue("@TenSP", tenSP);
                            cmdCheckSP.Parameters.AddWithValue("@MauSac", mauSac);
                            cmdCheckSP.Parameters.AddWithValue("@Size", size);
                            object result = cmdCheckSP.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                maSPChinhThuc = result.ToString(); // Đã có sẵn -> Lấy mã cũ
                            }
                            else
                            {
                                // Chưa có -> Tạo mã chuẩn mới và Insert vào bảng SanPham đúng 1 lần duy nhất lúc Lưu
                                maSPChinhThuc = "SP" + DateTime.Now.ToString("yyMMddHHmmssfff");
                                string queryInsertSP = "INSERT INTO SanPham (MaSP, TenSP, MauSac, Size, SoLuongTon, GiaNhap) VALUES (@MaSP, @TenSP, @MauSac, @Size, 0, @GiaNhap)";
                                using (SqlCommand cmdInsSP = new SqlCommand(queryInsertSP, conn, transaction))
                                {
                                    cmdInsSP.Parameters.AddWithValue("@MaSP", maSPChinhThuc);
                                    cmdInsSP.Parameters.AddWithValue("@TenSP", tenSP);
                                    cmdInsSP.Parameters.AddWithValue("@MauSac", mauSac);
                                    cmdInsSP.Parameters.AddWithValue("@Size", size);
                                    cmdInsSP.Parameters.AddWithValue("@GiaNhap", giaNhap);
                                    cmdInsSP.ExecuteNonQuery();
                                }
                            }
                        }

                        // Lưu vào ChiTietPhieuNhap
                        string queryCT = "INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuong, GiaNhap) VALUES (@MaPN, @MaSP, @SoLuong, @GiaNhap)";
                        using (SqlCommand cmdCT = new SqlCommand(queryCT, conn, transaction))
                        {
                            cmdCT.Parameters.AddWithValue("@MaPN", maPN);
                            cmdCT.Parameters.AddWithValue("@MaSP", maSPChinhThuc);
                            cmdCT.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdCT.Parameters.AddWithValue("@GiaNhap", giaNhap);
                            cmdCT.ExecuteNonQuery();
                        }

                        // Cập nhật tồn kho và giá nhập
                        string queryUpdateKho = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @SoLuong, GiaNhap = @GiaNhap WHERE MaSP = @MaSP";
                        using (SqlCommand cmdKho = new SqlCommand(queryUpdateKho, conn, transaction))
                        {
                            cmdKho.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdKho.Parameters.AddWithValue("@GiaNhap", giaNhap);
                            cmdKho.Parameters.AddWithValue("@MaSP", maSPChinhThuc);
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
            string tenSP = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên sản phẩm mới:", "Thêm nhanh sản phẩm", "", -1, -1);

            if (string.IsNullOrWhiteSpace(tenSP))
            {
                return;
            }

            if (comboBox1.DataSource is DataTable dt)
            {
                DataRow newRow = dt.NewRow();
                newRow["MaSP"] = "SP_TEMP_" + DateTime.Now.ToString("mmssfff");
                newRow["TenSP"] = tenSP.Trim();
                dt.Rows.Add(newRow);

                comboBox1.SelectedValue = newRow["MaSP"];
            }
            else
            {
                comboBox1.Items.Add(tenSP.Trim());
                comboBox1.Text = tenSP.Trim();
            }

            MessageBox.Show("Đã thêm tên sản phẩm! Vui lòng nhập thêm Màu sắc và Kích thước rồi bấm Thêm vào phiếu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtMau.Focus();
        }

        private void btnXemLichSu_Click(object sender, EventArgs e)
        {
            FrmLichSuPhieuNhap popup = new FrmLichSuPhieuNhap();
            popup.ShowDialog();
        }
    }
}