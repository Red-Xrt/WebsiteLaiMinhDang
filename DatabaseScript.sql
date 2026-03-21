-- =======================================================
-- Script TẠO DATABASE SachOnline
-- =======================================================

-- 1. TẠO DATABASE
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SachOnline')
BEGIN
    CREATE DATABASE SachOnline;
END
GO

USE SachOnline;
GO

-- =======================================================
-- 2. TẠO CÁC BẢNG (TABLES)
-- LƯU Ý: Chạy theo thứ tự để không bị lỗi Foreign Key (Khoá ngoại)
-- =======================================================

-- 2.1 Bảng ADMIN
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ADMIN')
BEGIN
    CREATE TABLE [dbo].[ADMIN] (
        [MaAd]      INT            IDENTITY (1, 1) NOT NULL,
        [HoTen]     NVARCHAR (50)  NULL,
        [DienThoai] VARCHAR (10)   NULL,
        [TenDN]     VARCHAR (15)   NULL,
        [MatKhau]   VARCHAR (15)   NULL,
        [Quyen]     INT            NULL,
        CONSTRAINT [PK_ADMIN] PRIMARY KEY CLUSTERED ([MaAd] ASC)
    );
END
GO

-- 2.2 Bảng CHUDE
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CHUDE')
BEGIN
    CREATE TABLE [dbo].[CHUDE] (
        [MaCD]     INT           IDENTITY (1, 1) NOT NULL,
        [TenChuDe] NVARCHAR (50) NOT NULL,
        CONSTRAINT [PK_CHUDE] PRIMARY KEY CLUSTERED ([MaCD] ASC)
    );
END
GO

-- 2.3 Bảng KHACHHANG
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'KHACHHANG')
BEGIN
    CREATE TABLE [dbo].[KHACHHANG] (
        [MaKH]      INT            IDENTITY (1, 1) NOT NULL,
        [HoTen]     NVARCHAR (50)  NOT NULL,
        [TaiKhoan]  VARCHAR (15)   NULL,
        [MatKhau]   VARCHAR (15)   NOT NULL,
        [Email]     VARCHAR (50)   NULL,
        [DiaChi]    NVARCHAR (50)  NULL,
        [DienThoai] VARCHAR (10)   NULL,
        [NgaySinh]  SMALLDATETIME  NULL,
        CONSTRAINT [PK_KHACHHANG] PRIMARY KEY CLUSTERED ([MaKH] ASC)
    );
END
GO

-- 2.4 Bảng NHAXUATBAN
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NHAXUATBAN')
BEGIN
    CREATE TABLE [dbo].[NHAXUATBAN] (
        [MaNXB]     INT            IDENTITY (1, 1) NOT NULL,
        [TenNXB]    NVARCHAR (100) NOT NULL,
        [DiaChi]    NVARCHAR (150) NULL,
        [DienThoai] NVARCHAR (15)  NULL,
        CONSTRAINT [PK_NHAXUATBAN] PRIMARY KEY CLUSTERED ([MaNXB] ASC)
    );
END
GO

-- 2.5 Bảng TACGIA
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TACGIA')
BEGIN
    CREATE TABLE [dbo].[TACGIA] (
        [MaTG]      INT            IDENTITY (1, 1) NOT NULL,
        [TenTG]     NVARCHAR (50)  NOT NULL,
        [DiaChi]    NVARCHAR (100) NULL,
        [TieuSu]    NTEXT          NULL,
        [DienThoai] VARCHAR (15)   NULL,
        CONSTRAINT [PK_TACGIA] PRIMARY KEY CLUSTERED ([MaTG] ASC)
    );
END
GO

-- 2.6 Bảng DONDATHANG
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DONDATHANG')
BEGIN
    CREATE TABLE [dbo].[DONDATHANG] (
        [MaDonHang]         INT           IDENTITY (1, 1) NOT NULL,
        [DaThanhToan]       BIT           NULL,
        [TinhTrangGiaoHang] INT           NULL,
        [NgayDat]           SMALLDATETIME NULL,
        [NgayGiao]          SMALLDATETIME NULL,
        [MaKH]              INT           NULL,
        CONSTRAINT [PK_DONDATHANG] PRIMARY KEY CLUSTERED ([MaDonHang] ASC),
        CONSTRAINT [FK_DDH_KH] FOREIGN KEY ([MaKH]) REFERENCES [dbo].[KHACHHANG] ([MaKH])
    );
END
GO

-- 2.7 Bảng SACH
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SACH')
BEGIN
    CREATE TABLE [dbo].[SACH] (
        [MaSach]      INT            IDENTITY (1, 1) NOT NULL,
        [TenSach]     NVARCHAR (100) NOT NULL,
        [MoTa]        NTEXT          NULL,
        [AnhBia]      VARCHAR (50)   NULL,
        [NgayCapNhat] SMALLDATETIME  NULL,
        [SoLuongBan]  INT            NULL,
        [GiaBan]      MONEY          NULL,
        [MaCD]        INT            NULL,
        [MaNXB]       INT            NULL,
        CONSTRAINT [PK_SACH] PRIMARY KEY CLUSTERED ([MaSach] ASC),
        CONSTRAINT [FK_S_CD] FOREIGN KEY ([MaCD]) REFERENCES [dbo].[CHUDE] ([MaCD]),
        CONSTRAINT [FK_Sach_NXB] FOREIGN KEY ([MaNXB]) REFERENCES [dbo].[NHAXUATBAN] ([MaNXB])
    );
END
GO

-- 2.8 Bảng CHITIETDATHANG
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CHITIETDATHANG')
BEGIN
    CREATE TABLE [dbo].[CHITIETDATHANG] (
        [MaDonHang] INT             NOT NULL,
        [MaSach]    INT             NOT NULL,
        [SoLuong]   INT             NULL,
        [DonGia]    DECIMAL (9, 2)  NULL,
        CONSTRAINT [PK_CHITIETDATHANG] PRIMARY KEY CLUSTERED ([MaDonHang] ASC, [MaSach] ASC),
        CONSTRAINT [FK_CTDH_DDH] FOREIGN KEY ([MaDonHang]) REFERENCES [dbo].[DONDATHANG] ([MaDonHang]),
        CONSTRAINT [FK_CTDH_S] FOREIGN KEY ([MaSach]) REFERENCES [dbo].[SACH] ([MaSach])
    );
END
GO

-- 2.9 Bảng VIETSACH
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VIETSACH')
BEGIN
    CREATE TABLE [dbo].[VIETSACH] (
        [MaTG]   INT           NOT NULL,
        [MaSach] INT           NOT NULL,
        [VaiTro] NVARCHAR (30) NULL,
        [ViTri]  NVARCHAR (30) NULL,
        CONSTRAINT [PK_VIETSACH] PRIMARY KEY CLUSTERED ([MaTG] ASC, [MaSach] ASC),
        CONSTRAINT [FK_VS_S] FOREIGN KEY ([MaSach]) REFERENCES [dbo].[SACH] ([MaSach]),
        CONSTRAINT [FK_VS_TG] FOREIGN KEY ([MaTG]) REFERENCES [dbo].[TACGIA] ([MaTG])
    );
END
GO

-- =======================================================
-- HOÀN TẤT
-- =======================================================
PRINT N'Đã tạo thành công database SachOnline và các bảng kèm khoá ngoại.';
GO
