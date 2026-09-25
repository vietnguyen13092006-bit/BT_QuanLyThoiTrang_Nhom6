using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace quanlycuahangthoitrang
{
    public class PhieuNhapModel
    {
        public string MaPhieu { get; set; }
        public DateTime NgayNhap { get; set; }
        public string MaNCC { get; set; }
        public string NhanVien { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietPhieuModel> ChiTiet { get; set; } = new List<ChiTietPhieuModel>();
    }

    public class ChiTietPhieuModel
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string MauSac { get; set; }
        public string KichThuoc { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }

    public partial class quanlynhaphang : Form
    {
        public static List<PhieuNhapModel> DanhSachPhieuNhap = new List<PhieuNhapModel>();

        private TextBox txtMaPhieu;
        private DateTimePicker dtpNgayNhap;
        private TextBox txtMaNhaCungCap;
        private TextBox txtNhanVien;

        private TextBox txtMaSP;
        private TextBox txtTenSP;
        private ComboBox cboMauSac;
        private ComboBox cboKichThuoc;
        private NumericUpDown nudSoLuong;
        private NumericUpDown nudDonGia;
        private Button btnThemSP;
        private Button btnXoaSP;

        private DataGridView dgvDanhSachSP;
        private Label lblTongTienVal;
        private Button btnLuuPhieu;
        private Button btnXemDanhSach;

        public quanlynhaphang()
        {
            InitializeComponentCustom();
            TaiDuLieuBanDau();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "QUẢN LÝ NHẬP HÀNG";
            this.Size = new Size(1020, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            // 1. Tiêu đề
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ NHẬP HÀNG",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Top,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            // 2. Group 1: Thông tin phiếu nhập (Căn chỉnh tọa độ không bị tràn)
            GroupBox gbThongTin = new GroupBox
            {
                Text = "Thông tin chung phiếu nhập",
                Location = new Point(20, 50),
                Size = new Size(960, 95)
            };

            // Hàng 1
            gbThongTin.Controls.Add(new Label { Text = "Mã phiếu nhập:", Location = new Point(15, 25), AutoSize = true });
            txtMaPhieu = new TextBox { Location = new Point(135, 22), Size = new Size(200, 25), ReadOnly = true };
            gbThongTin.Controls.Add(txtMaPhieu);

            gbThongTin.Controls.Add(new Label { Text = "Ngày nhập:", Location = new Point(480, 25), AutoSize = true });
            dtpNgayNhap = new DateTimePicker { Location = new Point(620, 22), Size = new Size(220, 25), Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
            gbThongTin.Controls.Add(dtpNgayNhap);

            // Hàng 2
            gbThongTin.Controls.Add(new Label { Text = "Mã nhà cung cấp:", Location = new Point(15, 58), AutoSize = true });
            txtMaNhaCungCap = new TextBox { Location = new Point(135, 55), Size = new Size(200, 25) };
            gbThongTin.Controls.Add(txtMaNhaCungCap);

            gbThongTin.Controls.Add(new Label { Text = "Nhân viên thực hiện:", Location = new Point(480, 58), AutoSize = true });
            txtNhanVien = new TextBox { Location = new Point(620, 55), Size = new Size(220, 25) };
            gbThongTin.Controls.Add(txtNhanVien);

            this.Controls.Add(gbThongTin);

            // 3. Group 2: Thêm sản phẩm (Căn khoảng cách rộng rãi)
            GroupBox gbChiTiet = new GroupBox
            {
                Text = "Thêm sản phẩm vào phiếu nhập",
                Location = new Point(20, 155),
                Size = new Size(960, 100)
            };

            // Hàng 1
            gbChiTiet.Controls.Add(new Label { Text = "Mã SP:", Location = new Point(15, 25), AutoSize = true });
            txtMaSP = new TextBox { Location = new Point(75, 22), Size = new Size(110, 25) };
            gbChiTiet.Controls.Add(txtMaSP);

            gbChiTiet.Controls.Add(new Label { Text = "Tên SP:", Location = new Point(205, 25), AutoSize = true });
            txtTenSP = new TextBox { Location = new Point(265, 22), Size = new Size(160, 25) };
            gbChiTiet.Controls.Add(txtTenSP);

            gbChiTiet.Controls.Add(new Label { Text = "Màu sắc:", Location = new Point(445, 25), AutoSize = true });
            cboMauSac = new ComboBox { Location = new Point(515, 22), Size = new Size(110, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboMauSac.Items.AddRange(new string[] { "Trắng", "Đen", "Đỏ", "Xanh Dương", "Vàng", "Xám" });
            gbChiTiet.Controls.Add(cboMauSac);

            gbChiTiet.Controls.Add(new Label { Text = "Kích thước:", Location = new Point(645, 25), AutoSize = true });
            cboKichThuoc = new ComboBox { Location = new Point(725, 22), Size = new Size(90, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboKichThuoc.Items.AddRange(new string[] { "S", "M", "L", "XL", "XXL", "FreeSize" });
            gbChiTiet.Controls.Add(cboKichThuoc);

            // Hàng 2
            gbChiTiet.Controls.Add(new Label { Text = "Số lượng:", Location = new Point(15, 60), AutoSize = true });
            nudSoLuong = new NumericUpDown { Location = new Point(75, 58), Size = new Size(110, 25), Minimum = 1, Maximum = 100000, Value = 1 };
            gbChiTiet.Controls.Add(nudSoLuong);

            gbChiTiet.Controls.Add(new Label { Text = "Đơn giá:", Location = new Point(205, 60), AutoSize = true });
            nudDonGia = new NumericUpDown { Location = new Point(265, 58), Size = new Size(160, 25), Minimum = 0, Maximum = 1000000000, Increment = 10000, ThousandsSeparator = true };
            gbChiTiet.Controls.Add(nudDonGia);

            btnThemSP = new Button { Text = "Thêm vào danh sách", Location = new Point(445, 56), Size = new Size(180, 30), BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat };
            btnThemSP.Click += BtnThemSP_Click;
            gbChiTiet.Controls.Add(btnThemSP);

            btnXoaSP = new Button { Text = "Xóa dòng chọn", Location = new Point(635, 56), Size = new Size(120, 30), BackColor = Color.MistyRose, FlatStyle = FlatStyle.Flat };
            btnXoaSP.Click += BtnXoaSP_Click;
            gbChiTiet.Controls.Add(btnXoaSP);

            this.Controls.Add(gbChiTiet);

            // 4. DataGridView Bảng Danh Sách
            dgvDanhSachSP = new DataGridView
            {
                Location = new Point(20, 265),
                Size = new Size(960, 320),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            dgvDanhSachSP.Columns.Add("MaSP", "Mã SP");
            dgvDanhSachSP.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvDanhSachSP.Columns.Add("MauSac", "Màu Sắc");
            dgvDanhSachSP.Columns.Add("KichThuoc", "Kích Thước");
            dgvDanhSachSP.Columns.Add("SoLuong", "Số Lượng Nhập");
            dgvDanhSachSP.Columns.Add("DonGia", "Đơn Giá Nhập (VNĐ)");
            dgvDanhSachSP.Columns.Add("ThanhTien", "Thành Tiền (VNĐ)");
            this.Controls.Add(dgvDanhSachSP);

            // 5. Tổng tiền nhập hàng & Nút thao tác
            Label lblTongTien = new Label { Text = "TỔNG TIỀN NHẬP HÀNG:", Font = new Font("Segoe UI", 12F, FontStyle.Bold), Location = new Point(450, 602), AutoSize = true };
            this.Controls.Add(lblTongTien);

            lblTongTienVal = new Label { Text = "0 VNĐ", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.Red, Location = new Point(670, 600), AutoSize = true };
            this.Controls.Add(lblTongTienVal);

            btnLuuPhieu = new Button { Text = "LƯU PHIẾU NHẬP", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(20, 595), Size = new Size(160, 40), BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLuuPhieu.Click += BtnLuuPhieu_Click;
            this.Controls.Add(btnLuuPhieu);

            btnXemDanhSach = new Button { Text = "XEM PHIẾU ĐÃ LƯU", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(190, 595), Size = new Size(170, 40), BackColor = Color.Orange, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnXemDanhSach.Click += BtnXemDanhSach_Click;
            this.Controls.Add(btnXemDanhSach);
        }

        private void TaiDuLieuBanDau()
        {
            txtMaPhieu.Text = "PN" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (cboMauSac.Items.Count > 0) cboMauSac.SelectedIndex = 0;
            if (cboKichThuoc.Items.Count > 0) cboKichThuoc.SelectedIndex = 0;
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong = (int)nudSoLuong.Value;
            decimal donGia = nudDonGia.Value;
            decimal thanhTien = soLuong * donGia;

            dgvDanhSachSP.Rows.Add(
                txtMaSP.Text.Trim(),
                txtTenSP.Text.Trim(),
                cboMauSac.SelectedItem.ToString(),
                cboKichThuoc.SelectedItem.ToString(),
                soLuong,
                donGia.ToString("#,##0", CultureInfo.GetCultureInfo("vi-VN")),
                thanhTien.ToString("#,##0", CultureInfo.GetCultureInfo("vi-VN"))
            );

            TinhTongTien();
            txtMaSP.Clear();
            txtTenSP.Clear();
            nudSoLuong.Value = 1;
            nudDonGia.Value = 0;
            txtMaSP.Focus();
        }

        private void BtnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvDanhSachSP.SelectedRows.Count > 0)
            {
                dgvDanhSachSP.Rows.RemoveAt(dgvDanhSachSP.SelectedRows[0].Index);
                TinhTongTien();
            }
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvDanhSachSP.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    string valStr = row.Cells["ThanhTien"].Value.ToString().Replace(".", "").Replace(",", "");
                    if (decimal.TryParse(valStr, out decimal thanhTien))
                        tongTien += thanhTien;
                }
            }
            lblTongTienVal.Text = tongTien.ToString("#,##0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }

        private void BtnLuuPhieu_Click(object sender, EventArgs e)
        {
            if (dgvDanhSachSP.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PhieuNhapModel phieu = new PhieuNhapModel
            {
                MaPhieu = txtMaPhieu.Text,
                NgayNhap = dtpNgayNhap.Value,
                MaNCC = txtMaNhaCungCap.Text,
                NhanVien = txtNhanVien.Text
            };

            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvDanhSachSP.Rows)
            {
                int sl = Convert.ToInt32(row.Cells["SoLuong"].Value);
                decimal dg = Convert.ToDecimal(row.Cells["DonGia"].Value.ToString().Replace(".", "").Replace(",", ""));

                phieu.ChiTiet.Add(new ChiTietPhieuModel
                {
                    MaSP = row.Cells["MaSP"].Value.ToString(),
                    TenSP = row.Cells["TenSP"].Value.ToString(),
                    MauSac = row.Cells["MauSac"].Value.ToString(),
                    KichThuoc = row.Cells["KichThuoc"].Value.ToString(),
                    SoLuong = sl,
                    DonGia = dg
                });
                tongTien += (sl * dg);
            }
            phieu.TongTien = tongTien;

            DanhSachPhieuNhap.Add(phieu);

            MessageBox.Show($"Đã lưu thành công Phiếu nhập [{phieu.MaPhieu}]!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dgvDanhSachSP.Rows.Clear();
            txtMaNhaCungCap.Clear();
            txtNhanVien.Clear();
            TaiDuLieuBanDau();
        }

        private void BtnXemDanhSach_Click(object sender, EventArgs e)
        {
            FormDanhSachPhieuNhap frm = new FormDanhSachPhieuNhap();
            frm.ShowDialog();
        }
    }

    public class FormDanhSachPhieuNhap : Form
    {
        private DataGridView dgvPhieu;
        private DataGridView dgvChiTiet;

        public FormDanhSachPhieuNhap()
        {
            this.Text = "DANH SÁCH PHIẾU NHẬP ĐÃ LƯU";
            this.Size = new Size(920, 550);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lbl1 = new Label { Text = "Danh sách Phiếu nhập:", Location = new Point(15, 10), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            this.Controls.Add(lbl1);

            dgvPhieu = new DataGridView { Location = new Point(15, 35), Size = new Size(870, 180), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvPhieu.SelectionChanged += DgvPhieu_SelectionChanged;
            this.Controls.Add(dgvPhieu);

            Label lbl2 = new Label { Text = "Chi tiết sản phẩm của phiếu đang chọn:", Location = new Point(15, 230), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            this.Controls.Add(lbl2);

            dgvChiTiet = new DataGridView { Location = new Point(15, 255), Size = new Size(870, 220), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            this.Controls.Add(dgvChiTiet);

            LoadData();
        }

        private void LoadData()
        {
            dgvPhieu.DataSource = null;
            dgvPhieu.DataSource = quanlynhaphang.DanhSachPhieuNhap;
        }

        private void DgvPhieu_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPhieu.SelectedRows.Count > 0)
            {
                PhieuNhapModel phieu = (PhieuNhapModel)dgvPhieu.SelectedRows[0].DataBoundItem;
                if (phieu != null)
                {
                    dgvChiTiet.DataSource = null;
                    dgvChiTiet.DataSource = phieu.ChiTiet;
                }
            }
        }
    }
}