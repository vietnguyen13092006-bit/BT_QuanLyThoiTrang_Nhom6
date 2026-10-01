USE master;
GO

-- Xóa database cũ nếu đã tồn tại
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyBanHangDB')
BEGIN
    ALTER DATABASE QuanLyBanHangDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyBanHangDB;
END
GO

-- Tạo lại database mới
CREATE DATABASE QuanLyBanHangDB;
GO

USE QuanLyBanHangDB;
GO

-- 1. Bảng Nhân viên
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL
);

-- 2. Bảng SanPham
CREATE TABLE SanPham (
    MaSP VARCHAR(20) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    Mau NVARCHAR(50),
    KichThuoc NVARCHAR(50),
    DonGia DECIMAL(18, 2) NOT NULL
);

-- 3. Bảng HoaDon
CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    NgayBan DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNV),
    TenKhachHang NVARCHAR(100),
    SDT VARCHAR(15),
    TongTien DECIMAL(18, 2)
);

-- 4. Bảng ChiTietHoaDon
CREATE TABLE ChiTietHoaDon (
    MaHD VARCHAR(20) FOREIGN KEY REFERENCES HoaDon(MaHD),
    MaSP VARCHAR(20) FOREIGN KEY REFERENCES SanPham(MaSP),
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL,
    ThanhTien AS (SoLuong * DonGia),
    PRIMARY KEY (MaHD, MaSP)
);

-- Thêm dữ liệu mẫu
INSERT INTO NhanVien (MaNV, TenNV) VALUES 
('NV01', N'Nguyen Van A'),
('NV02', N'Tran Thi B');

INSERT INTO SanPham (MaSP, TenSP, Mau, KichThuoc, DonGia) VALUES 
('SP01', N'Áo sơ mi nam', N'Trắng', 'L', 250000),
('SP02', N'Áo sơ mi nam', N'Xanh', 'M', 250000),
('SP03', N'Quần Jeans', N'Đen', '30', 400000),
('SP04', N'Váy nữ', N'Đỏ', 'S', 350000);