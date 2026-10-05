USE master;
GO

-- 1. Nếu đã có Database cũ thì tự động xóa để làm mới sạch sẻ
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLyCuaHangThoiTrang')
BEGIN
    ALTER DATABASE QuanLyCuaHangThoiTrang SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyCuaHangThoiTrang;
END
GO
--Ghi chú:- Nếu dự án chung đã có bảng KhachHang / NhanVien / SanPham, viet chỉ cần-- thay đổi tên bảng và khóa ngoại ở phần bên dưới. 
--viet chỉ cần thay đổi tên bảng và khóa ngoại ở phần bên dưới. 
-- 2. Tạo Database mới
CREATE DATABASE QuanLyCuaHangThoiTrang;
GO
USE QuanLyCuaHangThoiTrang;
GO

-- ----------------------------------------------------------------------------
-- PHẦN 1: BẢNG DANH MỤC CƠ BẢN
-- ----------------------------------------------------------------------------
CREATE TABLE KhachHang (
    MaKH VARCHAR(10) PRIMARY KEY,
    TenKH NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15)
);

CREATE TABLE NhanVien (
    MaNV VARCHAR(10) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL
);

CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(10) PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL
);

CREATE TABLE SanPham (
    MaSP VARCHAR(10) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    MauSac NVARCHAR(30),
    GiaBan DECIMAL(18,2)
);

CREATE TABLE SizeTonKho (
    MaSP VARCHAR(10),
    Size VARCHAR(10),
    SoLuongTon INT DEFAULT 0,
    PRIMARY KEY (MaSP, Size),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------------------------------------------------------
-- PHẦN 2: MODULE BÁN HÀNG (HÓA ĐƠN)
-- ----------------------------------------------------------------------------
CREATE TABLE HoaDon (
    MaHD VARCHAR(10) PRIMARY KEY,
    NgayLap DATETIME DEFAULT GETDATE(),
    MaKH VARCHAR(10),
    MaNV VARCHAR(10),
    TongTien DECIMAL(18,2),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

CREATE TABLE ChiTietHoaDon (
    MaHD VARCHAR(10),
    MaSP VARCHAR(10),
    Size VARCHAR(10),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    ThanhTien AS (SoLuong * DonGia),
    PRIMARY KEY (MaHD, MaSP, Size),
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- ----------------------------------------------------------------------------
-- PHẦN 3: MODULE NHẬP HÀNG (PHIẾU NHẬP)
-- ----------------------------------------------------------------------------
CREATE TABLE PhieuNhap (
    MaPN VARCHAR(10) PRIMARY KEY,
    NgayNhap DATETIME DEFAULT GETDATE(),
    MaNCC VARCHAR(10),
    MaNV VARCHAR(10),
    TongTienNhap DECIMAL(18,2),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

CREATE TABLE ChiTietPhieuNhap (
    MaPN VARCHAR(10),
    MaSP VARCHAR(10),
    Size VARCHAR(10),
    SoLuongNhap INT,
    GiaNhap DECIMAL(18,2),
    ThanhTien AS (SoLuongNhap * GiaNhap),
    PRIMARY KEY (MaPN, MaSP, Size),
    FOREIGN KEY (MaPN) REFERENCES PhieuNhap(MaPN) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- ----------------------------------------------------------------------------
-- PHẦN 4: DỮ LIỆU MẪU MỚI
-- ----------------------------------------------------------------------------
INSERT INTO KhachHang (MaKH, TenKH, DienThoai) VALUES 
('KH01', N'Nguyễn Văn A', '0901234567'), 
('KH02', N'Lê Thị C', '0987654321');

INSERT INTO NhanVien (MaNV, TenNV) VALUES 
('NV01', N'Trần Thị B'), 
('NV02', N'Nguyễn Văn A');

INSERT INTO NhaCungCap (MaNCC, TenNCC) VALUES 
('NCC01', N'Công ty May Việt Tiến'), 
('NCC02', N'Xưởng PT2000');

INSERT INTO SanPham (MaSP, TenSP, MauSac, GiaBan) VALUES 
('SP01', N'Áo sơ mi Nam', N'Trắng', 250000),
('SP02', N'Quần Jeans Nữ', N'Xanh', 450000),
('SP03', N'Váy Nữ Công Sở', N'Đỏ', 600000);

INSERT INTO SizeTonKho (MaSP, Size, SoLuongTon) VALUES 
('SP01', 'S', 5), ('SP01', 'M', 15), ('SP01', 'L', 8), ('SP01', 'XL', 2),
('SP02', 'S', 30), ('SP02', 'M', 20), ('SP02', 'L', 10);

INSERT INTO HoaDon (MaHD, NgayLap, MaKH, MaNV, TongTien) VALUES 
('HD001', '2026-09-25', 'KH01', 'NV01', 850000),
('HD002', '2026-09-26', 'KH02', 'NV01', 1200000);

INSERT INTO ChiTietHoaDon (MaHD, MaSP, Size, SoLuong, DonGia) VALUES 
('HD001', 'SP01', 'M', 2, 250000),
('HD001', 'SP02', 'L', 1, 350000),
('HD002', 'SP03', 'S', 2, 600000);

INSERT INTO PhieuNhap (MaPN, NgayNhap, MaNCC, MaNV, TongTienNhap) VALUES 
('PN001', '2026-09-20', 'NCC01', 'NV02', 15000000),
('PN002', '2026-09-22', 'NCC02', 'NV01', 8500000);

INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, Size, SoLuongNhap, GiaNhap) VALUES 
('PN001', 'SP01', 'L', 50, 150000),
('PN001', 'SP01', 'M', 50, 150000),
('PN002', 'SP02', 'S', 30, 283333);