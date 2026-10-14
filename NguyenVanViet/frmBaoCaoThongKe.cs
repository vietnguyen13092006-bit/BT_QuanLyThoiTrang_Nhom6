using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class frmBaoCaoThongKe : Form
    {
        private RadioButton rdoHomNay = null!;
        private RadioButton rdoThangNay = null!;
        private RadioButton rdoNamNay = null!;
        private RadioButton rdoTuChon = null!;

        private DateTimePicker dtpTuNgay = null!;
        private DateTimePicker dtpDenNgay = null!;

        private Button btnTraCuu = null!;
        private Button btnExcel = null!;
        private Button btnThoat = null!;
        private TextBox txtSoLuongHD = null!;
        private ComboBox cboMaHoaDon = null!;

        // 5 Thẻ Card Thống Kê (Bao gồm cả Thuế Đầu Vào)
        private Label lblValDoanhThu = null!;
        private Label lblValGiaVon = null!;
        private Label lblThueDauVao = null!;  // Thuế đầu vào từ PhieuNhap
        private Label lblTienThue = null!;     // Thuế đầu ra (VAT) từ HoaDon
        private Label lblValLoiNhuan = null!;

        private DataGridView dgvBaoCao = null!;
        private DataGridView dgvChiTiet = null!;

        private Panel pnlTop = null!;
        private Panel pnlCards = null!;
        private Panel pnlBottom = null!;
        private GroupBox grpBaoCao = null!;
        private GroupBox grpChiTiet = null!;
        private Panel pnlMainContainer = null!;

        public frmBaoCaoThongKe()
        {
            InitializeComponentCustom();
        }

        private void FrmBaoCaoThongKe_Load(object? sender, EventArgs e)
        {
            rdoHomNay.Checked = true;
            ToggleDatePickers(false);
            ThucHienTraCuu();
        }

        private void RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            ToggleDatePickers(rdoTuChon.Checked);
        }

        private void ToggleDatePickers(bool enable)
        {
            dtpTuNgay.Enabled = enable;
            dtpDenNgay.Enabled = enable;
        }

        private void BtnTraCuu_Click(object? sender, EventArgs e)
        {
            ThucHienTraCuu();
        }

        private void ThucHienTraCuu()
        {
            try
            {
                DateTime tuNgay = DateTime.Now.Date;
                DateTime denNgay = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

                if (rdoThangNay.Checked)
                {
                    DateTime now = DateTime.Now;
                    tuNgay = new DateTime(now.Year, now.Month, 1);
                    denNgay = tuNgay.AddMonths(1).AddSeconds(-1);
                }
                else if (rdoNamNay.Checked)
                {
                    int year = DateTime.Now.Year;
                    tuNgay = new DateTime(year, 1, 1);
                    denNgay = new DateTime(year, 12, 31, 23, 59, 59);
                }
                else if (rdoTuChon.Checked)
                {
                    tuNgay = dtpTuNgay.Value.Date;
                    denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
                }

                // 1. Truy vấn báo cáo hóa đơn (Đầu ra)
                string query = @"SELECT 
                                    h.MaHD AS [Số Hóa Đơn],
                                    h.NgayBan AS [Ngày Lập],
                                    h.TongTien AS [Doanh Thu],
                                    ISNULL(h.TienThue, 0) AS [Tổng Tiền Thuế],
                                    ISNULL(SUM(ct.SoLuong * ISNULL(pn.GiaNhapGoc, ct.DonGia * 0.6)), 0) AS [Giá Vốn],
                                    (h.TongTien - ISNULL(SUM(ct.SoLuong * ISNULL(pn.GiaNhapGoc, ct.DonGia * 0.6)), 0)) AS [Lợi Nhuận],
                                    n.TenNV AS [Nhân Viên Lập]
                                FROM HoaDon h
                                LEFT JOIN ChiTietHoaDon ct ON h.MaHD = ct.MaHD
                                LEFT JOIN NhanVien n ON h.MaNV = n.MaNV
                                OUTER APPLY (
                                    SELECT TOP 1 GiaNhap AS GiaNhapGoc 
                                    FROM ChiTietPhieuNhap 
                                    WHERE MaSP = ct.MaSP 
                                    ORDER BY NgayBan DESC, MaPN DESC
                                ) pn
                                WHERE h.NgayBan BETWEEN @TuNgay AND @DenNgay
                                GROUP BY h.MaHD, h.NgayBan, h.TongTien, h.TienThue, n.TenNV
                                ORDER BY h.NgayBan DESC, h.MaHD DESC";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay)
                };

                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
                dgvBaoCao.DataSource = dt;

                if (dgvBaoCao.Columns["Doanh Thu"] != null) dgvBaoCao.Columns["Doanh Thu"].DefaultCellStyle.Format = "N0";
                if (dgvBaoCao.Columns["Giá Vốn"] != null) dgvBaoCao.Columns["Giá Vốn"].DefaultCellStyle.Format = "N0";
                if (dgvBaoCao.Columns["Lợi Nhuận"] != null) dgvBaoCao.Columns["Lợi Nhuận"].DefaultCellStyle.Format = "N0";
                if (dgvBaoCao.Columns["Tổng Tiền Thuế"] != null) dgvBaoCao.Columns["Tổng Tiền Thuế"].DefaultCellStyle.Format = "N0";

                txtSoLuongHD.Text = dt.Rows.Count.ToString();

                cboMaHoaDon.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Số Hóa Đơn"] != DBNull.Value)
                    {
                        string maHD = row["Số Hóa Đơn"].ToString() ?? "";
                        if (!cboMaHoaDon.Items.Contains(maHD))
                        {
                            cboMaHoaDon.Items.Add(maHD);
                        }
                    }
                }
                cboMaHoaDon.SelectedIndex = -1;

                decimal tongDoanhThu = 0, tongGiaVon = 0, tongLoiNhuan = 0, tongThueVATRa = 0;
                foreach (DataRow row in dt.Rows)
                {
                    tongDoanhThu += Convert.ToDecimal(row["Doanh Thu"]);
                    tongGiaVon += Convert.ToDecimal(row["Giá Vốn"]);
                    tongLoiNhuan += Convert.ToDecimal(row["Lợi Nhuận"]);
                    tongThueVATRa += Convert.ToDecimal(row["Tổng Tiền Thuế"]);
                }

                // 2. Truy vấn Tổng Thuế Đầu Vào từ bảng PhieuNhap trong khoảng thời gian chọn
                string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

                decimal tongThueDauVao = 0;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string queryThueNhap = @"SELECT ISNULL(SUM(TongTienNhap -(TongTienNhap / (1.0 + Thue / 100.0))), 0) 
                                     FROM PhieuNhap 
                                     WHERE NgayNhap BETWEEN @TuNgay AND @DenNgay";
                    using (SqlCommand cmd = new SqlCommand(queryThueNhap, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                        cmd.Parameters.AddWithValue("@DenNgay", denNgay);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            tongThueDauVao = Convert.ToDecimal(result);
                        }
                    }
                }

                // Gán giá trị lên 5 thẻ Card
                lblValDoanhThu.Text = tongDoanhThu.ToString("N0") + " VNĐ";
                lblValGiaVon.Text = tongGiaVon.ToString("N0") + " VNĐ";
                lblThueDauVao.Text = tongThueDauVao.ToString("N0") + " VNĐ";
                lblTienThue.Text = tongThueVATRa.ToString("N0") + " VNĐ";
                lblValLoiNhuan.Text = tongLoiNhuan.ToString("N0") + " VNĐ";

                // Tự động load chi tiết của hóa đơn đầu tiên nếu có dữ liệu
                if (dt.Rows.Count > 0)
                {
                    dgvBaoCao.Rows[0].Selected = true;
                    string maHDDauTien = dt.Rows[0]["Số Hóa Đơn"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(maHDDauTien))
                    {
                        HienThiChiTietHoaDon(maHDDauTien);
                    }
                }
                else
                {
                    dgvChiTiet.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvBaoCao_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvBaoCao.Rows[e.RowIndex].Cells["Số Hóa Đơn"].Value != null)
            {
                string maHD = dgvBaoCao.Rows[e.RowIndex].Cells["Số Hóa Đơn"].Value.ToString() ?? "";
                HienThiChiTietHoaDon(maHD);
            }
        }

        private void HienThiChiTietHoaDon(string maHD)
        {
            try
            {
                string query = @"SELECT 
                                    ct.MaHD AS [Mã HD],
                                    ct.MaSP AS [Mã Sản Phẩm],
                                    sp.TenSP AS [Tên Sản Phẩm],
                                    ct.SoLuong AS [Số Lượng],
                                    ct.DonGia AS [Đơn Giá Bán],
                                    (ct.SoLuong * ct.DonGia) AS [Thành Tiền]
                                FROM ChiTietHoaDon ct
                                INNER JOIN HoaDon h ON ct.MaHD = h.MaHD
                                LEFT JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                WHERE ct.MaHD = @MaHD";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaHD", maHD)
                };

                DataTable dtChiTiet = DatabaseHelper.ExecuteQuery(query, parameters);
                dgvChiTiet.DataSource = dtChiTiet;

                if (dgvChiTiet.Columns["Đơn Giá Bán"] != null) dgvChiTiet.Columns["Đơn Giá Bán"].DefaultCellStyle.Format = "N0";
                if (dgvChiTiet.Columns["Thành Tiền"] != null) dgvChiTiet.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboMaHoaDon_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboMaHoaDon.Text)) return;
            string selectedHD = cboMaHoaDon.Text.Trim();

            foreach (DataGridViewRow row in dgvBaoCao.Rows)
            {
                if (row.Cells["Số Hóa Đơn"].Value?.ToString() == selectedHD)
                {
                    row.Selected = true;
                    dgvBaoCao.FirstDisplayedScrollingRowIndex = row.Index;
                    HienThiChiTietHoaDon(selectedHD);
                    break;
                }
            }
        }

        private void BtnExcel_Click(object? sender, EventArgs e)
        {
            if (dgvBaoCao.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xls)|*.xls",
                FileName = "BaoCaoDoanhThuVaLoiNhuan_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.Unicode))
                    {
                        // Tiêu đề báo cáo tổng quan
                        sw.WriteLine("BÁO CÁO DOANH THU, THUẾ & LỢI NHUẬN CỬA HÀNG");
                        sw.WriteLine($"Thời gian xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                        sw.WriteLine("");

                        // Header bảng danh sách hóa đơn
                        sw.WriteLine("DANH SÁCH HÓA ĐƠN TRONG KỲ");
                        sw.WriteLine("Số Hóa Đơn\tNgày Lập\tDoanh Thu (VNĐ)\tTổng Tiền Thuế (VNĐ)\tGiá Vốn (VNĐ)\tLợi Nhuận (VNĐ)\tNhân Viên Lập");

                        foreach (DataGridViewRow row in dgvBaoCao.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                sw.WriteLine($"{row.Cells["Số Hóa Đơn"].Value}\t" +
                                             $"{row.Cells["Ngày Lập"].Value}\t" +
                                             $"{row.Cells["Doanh Thu"].Value}\t" +
                                             $"{row.Cells["Tổng Tiền Thuế"].Value}\t" +
                                             $"{row.Cells["Giá Vốn"].Value}\t" +
                                             $"{row.Cells["Lợi Nhuận"].Value}\t" +
                                             $"{row.Cells["Nhân Viên Lập"].Value}");
                            }
                        }

                        // Thêm phần tổng kết thông số các thẻ card thống kê lên file Excel cho chuyên nghiệp
                        sw.WriteLine("");
                        sw.WriteLine("TỔNG KẾT CHỈ SỐ KINH DOANH:");
                        sw.WriteLine($"- Tổng Doanh Thu:\t{lblValDoanhThu.Text}");
                        sw.WriteLine($"- Tổng Giá Vốn:\t{lblValGiaVon.Text}");
                        sw.WriteLine($"- Tổng Thuế Đầu Vào:\t{lblThueDauVao.Text}");
                        sw.WriteLine($"- Tổng Thuế Đầu Ra (VAT):\t{lblTienThue.Text}");
                        sw.WriteLine($"- Tổng Lợi Nhuận:\t{lblValLoiNhuan.Text}");
                    }
                    MessageBox.Show("Xuất báo cáo Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnThoat_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponentCustom()
        {
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1350, 700);
            this.Text = "BÁO CÁO THỐNG KÊ DOANH THU & LỢI NHUẬN";

            pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(240, 244, 248) };
            pnlCards = new Panel { Dock = DockStyle.Top, Height = 85, BackColor = Color.White };
            pnlBottom = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(240, 244, 248) };

            pnlMainContainer = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            // GroupBox cố định cho bảng Danh sách hóa đơn (Bảng trên)
            grpBaoCao = new GroupBox
            {
                Text = "📋 DANH SÁCH HÓA ĐƠN TRONG KỲ",
                Dock = DockStyle.Top,
                Height = 260,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            dgvBaoCao = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            dgvBaoCao.CellClick += DgvBaoCao_CellClick;
            grpBaoCao.Controls.Add(dgvBaoCao);

            // GroupBox cố định cho bảng Chi tiết sản phẩm (Bảng dưới)
            grpChiTiet = new GroupBox
            {
                Text = "📄 CHI TIẾT SẢN PHẨM TRONG HÓA ĐƠN ĐƯỢC CHỌN",
                Dock = DockStyle.Top,
                Height = 280,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            dgvChiTiet = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AllowUserToAddRows = false
            };
            grpChiTiet.Controls.Add(dgvChiTiet);

            // Các Control trên thanh lọc (pnlTop)
            rdoHomNay = new RadioButton { Text = "Hôm nay", Location = new Point(15, 18), AutoSize = true };
            rdoThangNay = new RadioButton { Text = "Tháng này", Location = new Point(100, 18), AutoSize = true };
            rdoNamNay = new RadioButton { Text = "Năm nay", Location = new Point(190, 18), AutoSize = true };
            rdoTuChon = new RadioButton { Text = "Tùy chọn:", Location = new Point(275, 18), AutoSize = true };

            rdoHomNay.CheckedChanged += RadioButton_CheckedChanged;
            rdoThangNay.CheckedChanged += RadioButton_CheckedChanged;
            rdoNamNay.CheckedChanged += RadioButton_CheckedChanged;
            rdoTuChon.CheckedChanged += RadioButton_CheckedChanged;

            Label lblTu = new Label { Text = "Từ:", Location = new Point(360, 20), AutoSize = true };
            dtpTuNgay = new DateTimePicker { Location = new Point(390, 16), Format = DateTimePickerFormat.Short, Width = 110 };

            Label lblDen = new Label { Text = "Đến:", Location = new Point(510, 20), AutoSize = true };
            dtpDenNgay = new DateTimePicker { Location = new Point(550, 16), Format = DateTimePickerFormat.Short, Width = 110 };

            btnTraCuu = new Button { Text = "🔍 Tra Cứu", Location = new Point(675, 12), Size = new Size(100, 32), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTraCuu.Click += BtnTraCuu_Click;

            Label lblSL = new Label { Text = "Tổng số HD:", Location = new Point(790, 20), AutoSize = true };
            txtSoLuongHD = new TextBox { Location = new Point(870, 17), Width = 50, ReadOnly = true, TextAlign = HorizontalAlignment.Center };

            pnlTop.Controls.AddRange(new Control[] { rdoHomNay, rdoThangNay, rdoNamNay, rdoTuChon, lblTu, dtpTuNgay, lblDen, dtpDenNgay, btnTraCuu, lblSL, txtSoLuongHD });

            // 5 Thẻ Card Thống Kê
            lblValDoanhThu = CreateCard(pnlCards, "💵 TỔNG DOANH THU", Color.SeaGreen, 15, 200);
            lblValGiaVon = CreateCard(pnlCards, "💸 TỔNG GIÁ VỐN", Color.IndianRed, 275, 200);
            lblThueDauVao = CreateCard(pnlCards, "📥 THUẾ ĐẦU VÀO", Color.Teal, 535, 200);
            lblTienThue = CreateCard(pnlCards, "🧾 THUẾ ĐẦU RA", Color.DarkOrange, 795, 200);
            lblValLoiNhuan = CreateCard(pnlCards, "💰 TỔNG LỢI NHUẬN", Color.DarkBlue, 1055, 200);

            // Các nút chức năng dưới đáy (pnlBottom)
            Label lblChonHD = new Label { Text = "🔍 Tìm HD:", Location = new Point(20, 18), AutoSize = true };

            cboMaHoaDon = new ComboBox
            {
                Location = new Point(85, 15),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            cboMaHoaDon.SelectedIndexChanged += CboMaHoaDon_SelectedIndexChanged;

            btnExcel = new Button { Text = "🟢 XUẤT Excel", Location = new Point(275, 10), Size = new Size(110, 35), BackColor = Color.ForestGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnExcel.Click += BtnExcel_Click;

            btnThoat = new Button { Text = "🚪 Thoát", Location = new Point(395, 10), Size = new Size(110, 35), FlatStyle = FlatStyle.Flat };
            btnThoat.Click += BtnThoat_Click;

            pnlBottom.Controls.Clear();
            pnlBottom.Controls.AddRange(new Control[] { lblChonHD, cboMaHoaDon, btnExcel, btnThoat });

            // Thêm thứ tự các Control vào container chính (thứ tự ngược vì Dock = Top)
            pnlMainContainer.Controls.Add(grpChiTiet);
            pnlMainContainer.Controls.Add(grpBaoCao);

            this.Controls.Add(pnlMainContainer);
            this.Controls.Add(pnlBottom);
            this.Controls.Add(pnlCards);
            this.Controls.Add(pnlTop);

            this.Load += FrmBaoCaoThongKe_Load;
        }

        private static Label CreateCard(Panel parent, string title, Color color, int x, int width)
        {
            Panel p = new Panel { Location = new Point(x, 8), Size = new Size(width, 68), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.AliceBlue };
            Label lblTitle = new Label { Text = title, Location = new Point(10, 6), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = color };
            Label lblVal = new Label { Text = "0 VNĐ", Location = new Point(10, 30), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            p.Controls.Add(lblTitle);
            p.Controls.Add(lblVal);
            parent.Controls.Add(p);
            return lblVal;
        }
    }
}