CREATE TABLE DoiTraHang (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDonLienQuan NVARCHAR(50),
    NgayDoiTra DATETIME,
    ThongTinKhachHang NVARCHAR(255),
    HinhThucDoiTra NVARCHAR(50),
    TenSanPham NVARCHAR(100),
    MauSac NVARCHAR(50),
    KichThuoc NVARCHAR(20),
    SoLuong INT,
    TienKhachThanhToanThem DECIMAL(18,2)
);