-- Tạo database
CREATE DATABASE QuanLyBanHang;
GO

USE QuanLyBanHang;
GO

-- 1. Bảng Hóa Đơn (Lưu thông tin chung của hóa đơn)
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(20) PRIMARY KEY,
    NgayLapHoaDon DATETIME NOT NULL,
    NhanVienLap NVARCHAR(100) NOT NULL,
    TenKhachHang NVARCHAR(100) NOT NULL,
    PhuongThucThanhToan NVARCHAR(50) NOT NULL,
    TongTienThanhToan DECIMAL(18, 2) DEFAULT 0
);
GO

-- 2. Bảng Chi Tiết Hóa Đơn (Lưu danh sách sản phẩm, màu sắc, kích thước, số lượng)
CREATE TABLE ChiTietHoaDon (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon VARCHAR(20) FOREIGN KEY REFERENCES HoaDon(MaHoaDon) ON DELETE CASCADE,
    TenSanPham NVARCHAR(150) NOT NULL,
    MauSac NVARCHAR(50) NOT NULL,
    KichThuoc NVARCHAR(20) NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL,
    ThanhTien AS (SoLuong * DonGia) -- Cột tính tự động trong SQL
);
GO

-- Thêm dữ liệu mẫu để test tính năng tra cứu và thống kê
INSERT INTO HoaDon (MaHoaDon, NgayLapHoaDon, NhanVienLap, TenKhachHang, PhuongThucThanhToan, TongTienThanhToan) 
VALUES ('HD001', '2026-09-29 08:30:00', N'Nguyễn Văn An', N'Trần Thị Bình', N'Tiền mặt', 550000);

INSERT INTO ChiTietHoaDon (MaHoaDon, TenSanPham, MauSac, KichThuoc, SoLuong, DonGia) 
VALUES ('HD001', N'Áo sơ mi nam', N'Trắng', 'L', 2, 200000),
       ('HD001', N'Quần tây nam', N'Đen', '32', 1, 150000);

INSERT INTO HoaDon (MaHoaDon, NgayLapHoaDon, NhanVienLap, TenKhachHang, PhuongThucThanhToan, TongTienThanhToan) 
VALUES ('HD002', '2026-09-29 10:15:00', N'Lê Thị Hoa', N'Phạm Văn Cường', N'Chuyển khoản', 300000);

INSERT INTO ChiTietHoaDon (MaHoaDon, TenSanPham, MauSac, KichThuoc, SoLuong, DonGia) 
VALUES ('HD002', N'Áo thun polo', N'Xanh', 'M', 2, 150000);
GO