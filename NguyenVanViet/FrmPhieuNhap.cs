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

            // Kiểm tra và tạo cột cho DataGridView lập phiếu (dgvPN) nếu chưa có
            if (dgvPN.Columns.Count == 0)
            {
                dgvPN.Columns.Add("MaSP", "Mã SP");
                dgvPN.Columns.Add("TenSP", "Tên sản phẩm");
                dgvPN.Columns.Add("SoLuong", "Số lượng");
                dgvPN.Columns.Add("GiaNhap", "Giá nhập");
                dgvPN.Columns.Add("ThanhTien", "Thành tiền");
            }

            // Định dạng hiển thị dấu phẩy
            if (dgvPN.Columns["GiaNhap"] != null)
            {
                dgvPN.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            }
            if (dgvPN.Columns["ThanhTien"] != null)
            {
                dgvPN.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }

            // Chỉ tải danh sách sản phẩm vào ComboBox, loại bỏ dgvLoad nặng nề
            LoadDanhSachSanPham();
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            if (string.IsNullOrEmpty(txtThue.Text))
            {
                txtThue.Text = "10";
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

            decimal phanTramThue = 0;
            decimal.TryParse(txtThue.Text.Trim(), out phanTramThue);

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
                    row.Cells["ThanhTien"].Value = tienHangMoi + (tienHangMoi * (phanTramThue / 100));
                    daTonTai = true;
                    break;
                }
            }

            if (!daTonTai)
            {
                dgvPN.Rows.Add(maSP, tenSP, soLuong, giaNhap, thanhTien);
            }

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

                        string queryUpdateKho = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @SoLuong, GiaNhap = @GiaNhap WHERE MaSP = @MaSP";
                        using (SqlCommand cmdKho = new SqlCommand(queryUpdateKho, conn, transaction))
                        {
                            cmdKho.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdKho.Parameters.AddWithValue("@GiaNhap", giaNhap);
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
            string tenSP = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên sản phẩm mới:", "Thêm nhanh sản phẩm", "", -1, -1);

            if (string.IsNullOrWhiteSpace(tenSP))
            {
                return;
            }

            string maSP = "SP" + DateTime.Now.ToString("yyMMddHHmmss");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO SanPham (MaSP, TenSP, SoLuongTon) VALUES (@MaSP, @TenSP, 0)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.Parameters.AddWithValue("@TenSP", tenSP.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSanPham();
                    comboBox1.SelectedValue = maSP;
                    txtSoluong.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Sự kiện nút mở popup xem lịch sử chi tiết nhập hàng
        private void btnXemLichSu_Click(object sender, EventArgs e)
        {
            FrmLichSuPhieuNhap popup = new FrmLichSuPhieuNhap();
            popup.ShowDialog();
        }
    }
}