using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FORM_DKY
{
    public static class ImageHelper
    {
        /// <summary>
        /// Lấy ảnh từ thư mục Images theo tên file
        /// </summary>
        /// <param name="fileName">Tên file ảnh (VD: "logo.png", "bg.jpg")</param>
        public static Image GetImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            // Đường dẫn đến thư mục Images
            string path = Path.Combine(Application.StartupPath, "Images", fileName);

            if (File.Exists(path))
            {
                // Dùng FileStream đọc để tránh bị khóa file (Lock File)
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return Image.FromStream(fs);
                }
            }

            return null; // Trả về null nếu không tìm thấy file
        }
    }
}