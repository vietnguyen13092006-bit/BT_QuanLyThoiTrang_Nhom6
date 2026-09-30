USE master;
GO

-- 1. Tạo Database mới tinh
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ShopThoiTrang2026DB')
BEGIN
    CREATE DATABASE ShopThoiTrang2026DB;
END
GO

USE ShopThoiTrang2026DB;
GO

-- 2. Tạo Bảng SanPham chuẩn có sẵn đầy đủ các cột (bao gồm HinhAnh)
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
        GhiChu NVARCHAR(255),
        HinhAnh NVARCHAR(255)
    );
END
GO

-- 3. Mồi sẵn 2 dữ liệu mẫu
IF NOT EXISTS (SELECT * FROM SanPham WHERE MaSanPham = 'SP001')
BEGIN
    INSERT INTO SanPham (MaSanPham, TenSanPham, LoaiSanPham, MauSac, Size, GiaNhap, GiaBan, SoLuongTon, GhiChu, HinhAnh)
    VALUES 
    ('SP001', N'Áo thun Nam Basic', N'Áo thun', N'Đen', 'M', 100000, 180000, 50, N'Hàng bán chạy', NULL),
    ('SP002', N'Quần Jean Slimfit', N'Quần jean', N'Xanh', 'L', 200000, 350000, 30, N'Mẫu mới', NULL);
END
GO

-- 4. Kiểm tra dữ liệu
SELECT * FROM SanPham;
GO