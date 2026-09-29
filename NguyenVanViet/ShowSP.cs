using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // Đã thêm thư viện làm việc với File/Path
using System.Text;
using System.Windows.Forms;

namespace FORM_DKY
{
    public partial class ShowSP : UserControl
    {
        public ShowSP()
        {
            InitializeComponent();
        }

        // Property lưu trữ và load hình ảnh vào picAnh
        private string _hinhAnh;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HinhAnh
        {
            get => _hinhAnh;
            set
            {
                _hinhAnh = value;
                if (!string.IsNullOrEmpty(value))
                {
                    // Lấy đường dẫn ảnh từ thư mục Images trong nơi chạy phần mềm
                    string imagePath = Path.Combine(Application.StartupPath, "Images", value);

                    if (File.Exists(imagePath))
                    {
                        // Dùng Stream đọc ảnh để không bị lock file khi chạy
                        using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                        {
                            picAnh.Image = Image.FromStream(fs);
                        }
                        picAnh.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        picAnh.Image = null;
                    }
                }
                else
                {
                    picAnh.Image = null;
                }
            }
        }

        // Hàm gán thông tin sản phẩm (đã thêm tham số hinhAnh ở cuối)
        public void ThongTinSP(string ma, string ten, decimal gia, string mau, string size, string ghichu, string hinhAnh = "")
        {
            lblTenSP.Text = ten;
            lblGiaBan.Text = string.Format("{0:N0} VNĐ", gia);

            // Gán tên file ảnh vào property HinhAnh
            this.HinhAnh = hinhAnh;

            // Xóa sự kiện Click cũ trước khi gán mới (tránh bị trùng lặp sự kiện khi load lại)
            btnChiTiet.Click -= BtnChiTiet_Click_Dynamic;
            btnChiTiet.Click += BtnChiTiet_Click_Dynamic;

            void BtnChiTiet_Click_Dynamic(object sender, EventArgs e)
            {
                MessageBox.Show($"Mã SP: {ma}\nTên: {ten}\nGiá: {string.Format("{0:N0} VNĐ", gia)}\nMàu: {mau}\nSize: {size}\nGhi chú: {ghichu}", "Chi tiết sản phẩm");
            }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            // Để trống hoặc dùng làm sự kiện mặc định trên designer
        }
    }
}