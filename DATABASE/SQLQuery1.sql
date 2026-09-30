-- Tạo database hoàn toàn mới để tránh xung đột
CREATE DATABASE QuanLyBanHangDB;
GO
USE QuanLyBanHangDB;
GO

-- 1. Bảng Hóa Đơn
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(20) PRIMARY KEY,
    NgayLap DATETIME NOT NULL,
    NhanVien NVARCHAR(100) NOT NULL,
    KhachHang NVARCHAR(100) NOT NULL,
    PhuongThucTT NVARCHAR(50) NOT NULL,
    TongTien DECIMAL(18, 2) DEFAULT 0
);
GO

-- 2. Bảng Chi Tiết Hóa Đơn
CREATE TABLE ChiTietHoaDon (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon VARCHAR(20) FOREIGN KEY REFERENCES HoaDon(MaHoaDon) ON DELETE CASCADE,
    TenSP NVARCHAR(100) NOT NULL,
    MauSac NVARCHAR(50) NOT NULL,
    KichThuoc NVARCHAR(20) NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL
);
GO