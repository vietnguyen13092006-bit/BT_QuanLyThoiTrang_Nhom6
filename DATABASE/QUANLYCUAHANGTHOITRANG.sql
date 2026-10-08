USE master;
GO

-- 1. Xóa Database cũ nếu tồn tại để làm sạch
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyCuaHangThoiTrang')
BEGIN
    ALTER DATABASE QuanLyCuaHangThoiTrang SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyCuaHangThoiTrang;
END
GO

-- 2. Tạo Database dùng chung cho cả 3 bạn
CREATE DATABASE QuanLyCuaHangThoiTrang;
GO

USE QuanLyCuaHangThoiTrang;
GO

-- =========================================================
-- BẢNG 1: NHÂN VIÊN (Dùng chung cho Quản lý & Báo cáo)
-- =========================================================
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    ChucVu NVARCHAR(50)
);

-- =========================================================
-- BẢNG 2: SẢN PHẨM (Dùng chung cho Kho, Bán hàng & Báo cáo)
-- =========================================================
CREATE TABLE SanPham (
    MaSP VARCHAR(20) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    LoaiSP NVARCHAR(50),
    MauSac NVARCHAR(30),
    Size VARCHAR(10),
    GiaNhap DECIMAL(18, 2) DEFAULT 0,  -- Dành cho bên Phiếu nhập
    GiaBan DECIMAL(18, 2) DEFAULT 0,   -- Dành cho bên Hóa đơn bán
    SoLuongTon INT DEFAULT 0,          -- Tự tăng khi nhập, tự giảm khi bán
    HinhAnh VARCHAR(200),              -- Đường dẫn ảnh đại diện sản phẩm
    GhiChu NVARCHAR(255)
);

-- =========================================================
-- BẢNG 3 & 4: PHIẾU NHẬP HÀNG (Mô-đun Quản lý Kho / Thống kê)
-- =========================================================
CREATE TABLE PhieuNhap (
    MaPN VARCHAR(20) PRIMARY KEY,
    NgayNhap DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNV),
    TongTienNhap DECIMAL(18,2) DEFAULT 0
);

CREATE TABLE ChiTietPhieuNhap (
    MaPN VARCHAR(20) FOREIGN KEY REFERENCES PhieuNhap(MaPN) ON DELETE CASCADE,
    MaSP VARCHAR(20) FOREIGN KEY REFERENCES SanPham(MaSP),
    SoLuong INT NOT NULL,
    GiaNhap DECIMAL(18,2) NOT NULL,
    ThanhTien AS (SoLuong * GiaNhap),
    PRIMARY KEY (MaPN, MaSP)
);

-- =========================================================
-- BẢNG 5 & 6: HÓA ĐƠN BÁN HÀNG (Mô-đun Quản lý Hóa đơn)
-- =========================================================
CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    NgayBan DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNV),
    TenKhachHang NVARCHAR(100),
    SDT VARCHAR(15),
    TongTien DECIMAL(18, 2) DEFAULT 0,
    TrangThai NVARCHAR(50) DEFAULT N'Đã thanh toán'
);

CREATE TABLE ChiTietHoaDon (
    MaHD VARCHAR(20) FOREIGN KEY REFERENCES HoaDon(MaHD) ON DELETE CASCADE,
    MaSP VARCHAR(20) FOREIGN KEY REFERENCES SanPham(MaSP),
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL,
    ThanhTien AS (SoLuong * DonGia),
    PRIMARY KEY (MaHD, MaSP)
);
GO

-- =========================================================
-- THÊM DỮ LIỆU MẪU ĐỂ CHẠY THỬ
-- =========================================================

GO
use QuanLyCuaHangThoiTrang
go
ALTER TABLE HoaDon ADD TienThue DECIMAL(18, 2) DEFAULT 0;
go
select* from HoaDon