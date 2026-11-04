USE master;
GO

-- 1. Xóa Database cũ nếu tồn tại để làm sạch
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyCuaHangThoiTrang')
BEGIN
    ALTER DATABASE QuanLyCuaHangThoiTrang SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyCuaHangThoiTrang;
END
GO

-- 2. Tạo Database dùng chung
CREATE DATABASE QuanLyCuaHangThoiTrang;
GO

USE QuanLyCuaHangThoiTrang;
GO

-- =========================================================
-- BẢNG 1: NHÂN VIÊN (Lưu hồ sơ nhân sự)
-- =========================================================
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    ChucVu NVARCHAR(50) DEFAULT N'Nhân viên'
);

-- =========================================================
-- BẢNG 2: TÀI KHOẢN (Gộp chung để đồng bộ với Nhân viên)
-- =========================================================
CREATE TABLE Users (
    Userid INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PassW VARCHAR(250) NOT NULL,
    SDT VARCHAR(15) NULL,
    Email NVARCHAR(100) NULL,
    AvatarPath NVARCHAR(255) NULL,
    MaNV VARCHAR(20) NULL, -- Liên kết sang bảng Nhân viên
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE SET NULL
);

-- =========================================================
-- BẢNG 3: SẢN PHẨM 
-- =========================================================
CREATE TABLE SanPham (
    MaSP VARCHAR(20) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    LoaiSP NVARCHAR(50),
    MauSac NVARCHAR(30),
    Size VARCHAR(10),
    GiaNhap DECIMAL(18, 2) DEFAULT 0,  
    GiaBan DECIMAL(18, 2) DEFAULT 0,   
    SoLuongTon INT DEFAULT 0,          
    HinhAnh VARCHAR(200),              
    GhiChu NVARCHAR(255)
);

-- =========================================================
-- BẢNG 4 & 5: PHIẾU NHẬP HÀNG & CHI TIẾT PHIẾU NHẬP
-- =========================================================
CREATE TABLE PhieuNhap (
    MaPN VARCHAR(20) PRIMARY KEY,
    NgayNhap DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNV),
    TongTienNhap DECIMAL(18,2) DEFAULT 0,
    Thue DECIMAL(18, 2) DEFAULT 0
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
-- BẢNG 6 & 7: HÓA ĐƠN BÁN HÀNG & CHI TIẾT HÓA ĐƠN
-- =========================================================
CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    NgayBan DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNV),
    TenKhachHang NVARCHAR(100),
    SDT VARCHAR(15),
    TongTien DECIMAL(18, 2) DEFAULT 0,
    TienThue DECIMAL(18, 2) DEFAULT 0,
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
-- 1. Thêm nhân viên Quản lý mẫu
INSERT INTO NhanVien (MaNV, TenNV, SDT, ChucVu) 
VALUES ('NV01', N'Nguyễn Văn Việt', '0987654321', N'Quản lý');

-- 2. Thêm tài khoản đăng nhập tương ứng cho Quản lý này (gắn MaNV = 'NV01')
INSERT INTO Users (Username, PassW, SDT, Email, MaNV) 
VALUES ('admin', '123456', '0987654321', 'viet@gmail.com', 'NV01');
SELECT Username, MaNV FROM Users WHERE Username = 'admin';
select* from Users
