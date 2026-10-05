CREATE DATABASE BaoCaoThongKeDoanhThuDB;
GO

USE BaoCaoThongKeDoanhThuDB;
GO

-- Bảng Nhân Viên
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL
);

-- Bảng Phiếu Nhập
CREATE TABLE PhieuNhap (
    MaPN VARCHAR(20) PRIMARY KEY,
    NgayNhap DATETIME DEFAULT GETDATE()
);

-- Bảng Chi Tiết Phiếu Nhập
CREATE TABLE ChiTietPhieuNhap (
    MaPN VARCHAR(20),
    MaSP VARCHAR(20),
    Size VARCHAR(10),
    SoLuong INT,
    GiaNhap DECIMAL(18,2),
    PRIMARY KEY (MaPN, MaSP, Size)
);

-- Bảng Hóa Đơn
CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    NgayLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2),
    MaNV VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNV)
);

-- Bảng Chi Tiết Hóa Đơn
CREATE TABLE ChiTietHoaDon (
    MaHD VARCHAR(20) FOREIGN KEY REFERENCES HoaDon(MaHD),
    MaSP VARCHAR(20),
    Size VARCHAR(10),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    PRIMARY KEY (MaHD, MaSP, Size)
);

-- DỮ LIỆU MẪU ĐỂ CHẠY THỬ
INSERT INTO NhanVien VALUES ('NV01', N'Nguyễn Văn A'), ('NV02', N'Trần Thị B');

INSERT INTO PhieuNhap VALUES ('PN01', GETDATE());
INSERT INTO ChiTietPhieuNhap VALUES ('PN01', 'SP01', 'L', 100, 500000);

INSERT INTO HoaDon VALUES 
('HD001', GETDATE(), 850000, 'NV01'),
('HD002', GETDATE(), 1200000, 'NV02');

INSERT INTO ChiTietHoaDon VALUES 
('HD001', 'SP01', 'L', 1, 850000),
('HD002', 'SP01', 'L', 2, 600000);