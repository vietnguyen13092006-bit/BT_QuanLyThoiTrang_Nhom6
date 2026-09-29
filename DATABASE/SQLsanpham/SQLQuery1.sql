-- 1. Tạo Database mới (Nếu chưa tạo)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLyCuaHangThoiTrang')
BEGIN
    CREATE DATABASE QuanLyCuaHangThoiTrang;
END
GO

-- 2. Chuyển sang sử dụng Database QuanLyCuaHangThoiTrang
USE QuanLyCuaHangThoiTrang;
GO

-- 3. Tạo Bảng SanPham (Nếu chưa có)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SanPham')
BEGIN
    CREATE TABLE SanPham (
        MaSanPham VARCHAR(20) PRIMARY KEY,
        TenSanPham NVARCHAR(100) NOT NULL,
        LoaiSanPham NVARCHAR(50),
        MauSac NVARCHAR(30),
        Size VARCHAR(10),
        GiaNhap DECIMAL(18, 2) DEFAULT 0,
        GiaBan DECIMAL(18, 2) DEFAULT 0,
        SoLuongTon INT DEFAULT 0,
        GhiChu NVARCHAR(255)
    );
END
GO

-- 4. Thêm sẵn 2 dữ liệu mẫu để kiểm tra hiển thị lên C#
INSERT INTO SanPham (MaSanPham, TenSanPham, LoaiSanPham, MauSac, Size, GiaNhap, GiaBan, SoLuongTon, GhiChu)
VALUES 
('SP001', N'Áo thun Nam Basic', N'Áo thun', N'Đen', 'M', 100000, 180000, 50, N'Hàng bán chạy'),
('SP002', N'Quần Jean Slimfit', N'Quần jean', N'Xanh', 'L', 200000, 350000, 30, N'Mẫu mới 2026');
GO

-- 5. Xem lại dữ liệu vừa tạo
SELECT * FROM SanPham;
GO
ALTER TABLE SanPham
ADD HinhAnh Varchar(200)