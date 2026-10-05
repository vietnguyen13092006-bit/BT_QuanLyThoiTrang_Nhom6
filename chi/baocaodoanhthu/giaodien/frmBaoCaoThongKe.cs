using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace baocaodoanhthu.giaodien
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
        private Label lblValDoanhThu = null!;
        private Label lblValGiaVon = null!;
        private Label lblValLoiNhuan = null!;

        private DataGridView dgvBaoCao = null!;
        private DataGridView dgvChiTiet = null!;

        private Panel pnlTop = null!;
        private Panel pnlCards = null!;
        private Panel pnlBottom = null!;
        private GroupBox grpChiTiet = null!;
        private Panel pnlMainContainer = null!;

        public frmBaoCaoThongKe()
        {
            InitializeComponentCustom();
            InitializeComponent();
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

                string query = @"SELECT 
                                    h.MaHD AS [Số Hóa Đơn],
                                    h.NgayLap AS [Ngày Lập],
                                    h.TongTien AS [Doanh Thu],
                                    ISNULL(SUM(ct.SoLuong * ISNULL(pn.GiaNhapGoc, ct.DonGia * 0.6)), 0) AS [Giá Vốn],
                                    (h.TongTien - ISNULL(SUM(ct.SoLuong * ISNULL(pn.GiaNhapGoc, ct.DonGia * 0.6)), 0)) AS [Lợi Nhuận],
                                    n.TenNV AS [Nhân Viên Lập]
                                FROM HoaDon h
                                LEFT JOIN ChiTietHoaDon ct ON h.MaHD = ct.MaHD
                                LEFT JOIN NhanVien n ON h.MaNV = n.MaNV
                                OUTER APPLY (
                                    SELECT TOP 1 GiaNhap AS GiaNhapGoc 
                                    FROM ChiTietPhieuNhap 
                                    WHERE MaSP = ct.MaSP AND Size = ct.Size 
                                    ORDER BY MaPN DESC
                                ) pn
                                WHERE h.NgayLap BETWEEN @TuNgay AND @DenNgay
                                GROUP BY h.MaHD, h.NgayLap, h.TongTien, n.TenNV";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay)
                };

                DataTable dt = DatabaseHelper.GetData(query, parameters);
                dgvBaoCao.DataSource = dt;

                // Định dạng hiển thị số cho DataGridView
                if (dgvBaoCao.Columns["Doanh Thu"] != null) dgvBaoCao.Columns["Doanh Thu"].DefaultCellStyle.Format = "N0";
                if (dgvBaoCao.Columns["Giá Vốn"] != null) dgvBaoCao.Columns["Giá Vốn"].DefaultCellStyle.Format = "N0";
                if (dgvBaoCao.Columns["Lợi Nhuận"] != null) dgvBaoCao.Columns["Lợi Nhuận"].DefaultCellStyle.Format = "N0";

                txtSoLuongHD.Text = dt.Rows.Count.ToString();

                cboMaHoaDon.Items.Clear();
                cboMaHoaDon.Items.Add("-- Chọn để định vị --");
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Số Hóa Đơn"] != DBNull.Value)
                    {
                        cboMaHoaDon.Items.Add(row["Số Hóa Đơn"].ToString() ?? "");
                    }
                }
                cboMaHoaDon.SelectedIndex = 0;

                decimal tongDoanhThu = 0, tongGiaVon = 0, tongLoiNhuan = 0;
                foreach (DataRow row in dt.Rows)
                {
                    tongDoanhThu += Convert.ToDecimal(row["Doanh Thu"]);
                    tongGiaVon += Convert.ToDecimal(row["Giá Vốn"]);
                    tongLoiNhuan += Convert.ToDecimal(row["Lợi Nhuận"]);
                }

                lblValDoanhThu.Text = tongDoanhThu.ToString("N0") + " VNĐ";
                lblValGiaVon.Text = tongGiaVon.ToString("N0") + " VNĐ";
                lblValLoiNhuan.Text = tongLoiNhuan.ToString("N0") + " VNĐ";

                // Xóa dữ liệu bảng chi tiết cũ
                dgvChiTiet.DataSource = null;

                // Tự động tính toán lại chiều cao bảng hóa đơn để thu gọn giao diện
                CapNhatChieuCaoGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatChieuCaoGridView()
        {
            int rowCount = dgvBaoCao.Rows.Count;
            int rowHeight = dgvBaoCao.RowTemplate.Height;
            int headerHeight = dgvBaoCao.ColumnHeadersHeight;

            // Tính chiều cao chuẩn cho DataGridView
            int calculatedHeight = headerHeight + (rowCount * rowHeight) + 10;

            // Giới hạn chiều cao tối thiểu 110px và tối đa 320px
            if (calculatedHeight < 110) calculatedHeight = 110;
            if (calculatedHeight > 320) calculatedHeight = 320;

            dgvBaoCao.Height = calculatedHeight;
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
                                    ct.Size AS [Kích Cỡ],
                                    ct.SoLuong AS [Số Lượng],
                                    ct.DonGia AS [Đơn Giá Bán],
                                    (ct.SoLuong * ct.DonGia) AS [Thành Tiền]
                                FROM ChiTietHoaDon ct
                                WHERE ct.MaHD = @MaHD";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaHD", maHD)
                };

                DataTable dtChiTiet = DatabaseHelper.GetData(query, parameters);
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
            if (cboMaHoaDon.SelectedIndex <= 0) return;
            string? selectedHD = cboMaHoaDon.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedHD)) return;

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
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xls)|*.xls",
                FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("ddMMyyyy") + ".xls"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.Unicode))
                    {
                        sw.WriteLine("Số Hóa Đơn\tNgày Lập\tDoanh Thu\tGiá Vốn\tLợi Nhuận\tNhân Viên Lập");
                        foreach (DataGridViewRow row in dgvBaoCao.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                sw.WriteLine($"{row.Cells["Số Hóa Đơn"].Value}\t{row.Cells["Ngày Lập"].Value}\t{row.Cells["Doanh Thu"].Value}\t{row.Cells["Giá Vốn"].Value}\t{row.Cells["Lợi Nhuận"].Value}\t{row.Cells["Nhân Viên Lập"].Value}");
                            }
                        }
                    }
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.MinimumSize = new Size(1100, 700);
            this.Text = "BÁO CÁO THỐNG KÊ DOANH THU & LỢI NHUẬN";

            pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(240, 244, 248) };
            pnlCards = new Panel { Dock = DockStyle.Top, Height = 85, BackColor = Color.White };

            // Panel cuộn tự động bao bọc bên dưới
            pnlMainContainer = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            // DataGridView Hóa Đơn (Xếp trên cùng của pnlMainContainer)
            dgvBaoCao = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 120,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };
            dgvBaoCao.CellClick += DgvBaoCao_CellClick;

            // GroupBox Chi tiết sản phẩm (Nằm ngay bên dưới dgvBaoCao)
            grpChiTiet = new GroupBox
            {
                Text = "📄 CHI TIẾT SẢN PHẨM TRONG HÓA ĐƠN ",
                Dock = DockStyle.Top,
                Height = 220,
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

            // Panel chức năng nút bấm (Xếp ngay dưới GroupBox Chi tiết)
            pnlBottom = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(240, 244, 248) };

            // Controls Lọc Ngày Tháng
            rdoHomNay = new RadioButton { Text = "Hôm nay", Location = new Point(15, 18), AutoSize = true };
            rdoThangNay = new RadioButton { Text = "Tháng ", Location = new Point(100, 18), AutoSize = true };
            rdoNamNay = new RadioButton { Text = "Năm ", Location = new Point(190, 18), AutoSize = true };
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

            lblValDoanhThu = CreateCard(pnlCards, "💵 TỔNG DOANH THU", Color.SeaGreen, 30);
            lblValGiaVon = CreateCard(pnlCards, "💸 TỔNG GIÁ VỐN", Color.IndianRed, 350);
            lblValLoiNhuan = CreateCard(pnlCards, "💰 TỔNG LỢI NHUẬN", Color.DarkBlue, 670);

            // Thanh chức năng bên dưới
            Label lblChonHD = new Label { Text = "🔍 Tìm HD:", Location = new Point(20, 18), AutoSize = true };

            // Kéo cboMaHoaDon sát lại gần label (đổi Location X từ 160 -> 105)
            cboMaHoaDon = new ComboBox { Location = new Point(85, 15), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboMaHoaDon.SelectedIndexChanged += CboMaHoaDon_SelectedIndexChanged;

            // Đẩy các nút Xuất Excel và Thoát sang gần ComboBox hơn
            btnExcel = new Button { Text = "🟢 XUẤT Excel", Location = new Point(275, 10), Size = new Size(110, 35), BackColor = Color.ForestGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnExcel.Click += BtnExcel_Click;

            btnThoat = new Button { Text = "🚪 Thoát", Location = new Point(395, 10), Size = new Size(110, 35), FlatStyle = FlatStyle.Flat };
            btnThoat.Click += BtnThoat_Click;

            pnlBottom.Controls.Clear();
            pnlBottom.Controls.AddRange(new Control[] { lblChonHD, cboMaHoaDon, btnExcel, btnThoat });

            // Thứ tự Add vào container rất quan trọng để xếp từ trên xuống dưới
            pnlMainContainer.Controls.Add(pnlBottom);
            pnlMainContainer.Controls.Add(grpChiTiet);
            pnlMainContainer.Controls.Add(dgvBaoCao);

            this.Controls.Add(pnlMainContainer);
            this.Controls.Add(pnlCards);
            this.Controls.Add(pnlTop);

            this.Load += FrmBaoCaoThongKe_Load;
        }

        private static Label CreateCard(Panel parent, string title, Color color, int x)
        {
            Panel p = new Panel { Location = new Point(x, 8), Size = new Size(280, 68), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.AliceBlue };
            Label lblTitle = new Label { Text = title, Location = new Point(10, 6), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = color };
            Label lblVal = new Label { Text = "0 VNĐ", Location = new Point(10, 30), AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            p.Controls.Add(lblTitle);
            p.Controls.Add(lblVal);
            parent.Controls.Add(p);
            return lblVal;
        }
    }
}