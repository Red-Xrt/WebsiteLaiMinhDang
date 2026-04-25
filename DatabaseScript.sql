
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SachOnline')
BEGIN
    CREATE DATABASE SachOnline;
END
GO

USE SachOnline;
GO


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

IF NOT EXISTS (SELECT * FROM ADMIN)
BEGIN
    INSERT INTO ADMIN (HoTen, DienThoai, TenDN, MatKhau, Quyen) VALUES
    (N'Quản trị viên', '0123456789', 'admin', 'admin', 1);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CHUDE')
BEGIN
    CREATE TABLE [dbo].[CHUDE] (
        [MaCD]     INT           IDENTITY (1, 1) NOT NULL,
        [TenChuDe] NVARCHAR (50) NOT NULL,
        CONSTRAINT [PK_CHUDE] PRIMARY KEY CLUSTERED ([MaCD] ASC)
    );
END
GO

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


IF NOT EXISTS (SELECT * FROM CHUDE)
BEGIN
    INSERT INTO CHUDE (TenChuDe) VALUES
    (N'Ngoại ngữ'),
    (N'Công nghệ thông tin'),
    (N'Luật'),
    (N'Văn học'),
    (N'Khoa học kỹ thuật'),
    (N'Nông nghiệp'),
    (N'Triết học-Chính trị'),
    (N'Lịch sử, địa lý'),
    (N'Kinh tế'),
    (N'Sách giáo khoa'),
    (N'Nghệ thuật sống');
END
GO

IF NOT EXISTS (SELECT * FROM NHAXUATBAN)
BEGIN
    INSERT INTO NHAXUATBAN (TenNXB, DiaChi, DienThoai) VALUES
    (N'NXB Trẻ', N'TP. Hồ Chí Minh', '0123456789'),
    (N'NXB Giáo Dục', N'Hà Nội', '0987654321'),
    (N'NXB Tổng Hợp', N'Đà Nẵng', '0912345678'),
    (N'NXB Kim Đồng', N'Hà Nội', '0909090909'),
    (N'NXB Khoa Học', N'Hà Nội', '0888888888');
END
GO

IF NOT EXISTS (SELECT * FROM SACH)
BEGIN
        INSERT INTO SACH (TenSach, MoTa, AnhBia, NgayCapNhat, SoLuongBan, GiaBan, MaCD, MaNXB) VALUES
    (N'Lập trình C# cơ bản', N'Cuốn sách cung cấp kiến thức toàn diện từ cơ bản đến nâng cao về ngôn ngữ lập trình C#. Rất phù hợp cho người mới bắt đầu muốn xây dựng nền tảng vững chắc trong phát triển phần mềm trên nền tảng .NET.', 'TH001.jpg', GETDATE(), 100, 150000, 2, 1),
    (N'Kỹ thuật Lập trình Web', N'Hướng dẫn chi tiết cách xây dựng website động chuyên nghiệp bằng công nghệ ASP.NET MVC. Bao gồm các bài tập thực hành từ việc thiết kế CSDL đến xây dựng giao diện người dùng.', 'TH002.jpg', GETDATE(), 50, 200000, 2, 2),
    (N'Giáo trình Tiếng Anh', N'Tài liệu chuẩn mực giúp cải thiện kỹ năng Tiếng Anh giao tiếp cơ bản và nâng cao. Cung cấp hàng loạt các tình huống giao tiếp thực tế và bài tập ứng dụng hữu ích trong đời sống hàng ngày.', 'TiengAnh01.jpg', GETDATE(), 200, 120000, 1, 3),
    (N'Đắc Nhân Tâm', N'Một trong những cuốn sách nghệ thuật sống nổi tiếng nhất thế giới. Đem lại những lời khuyên sâu sắc về cách đối nhân xử thế, làm phong phú thêm tâm hồn và tạo dựng các mối quan hệ bền vững.', 'Kt.jpg', GETDATE(), 500, 80000, 11, 1),
    (N'Lịch sử Việt Nam', N'Tóm tắt dòng chảy lịch sử Việt Nam qua các thời kỳ, từ thời đại Hùng Vương dựng nước cho đến thời kỳ hiện đại. Mang đến cái nhìn bao quát và niềm tự hào dân tộc cho người đọc.', 'TH003.jpg', GETDATE(), 150, 100000, 8, 4),
    (N'Tư duy Kinh Tế', N'Hướng dẫn cách tư duy và đầu tư kinh tế thông minh trong kỷ nguyên mới. Giúp bạn nắm bắt các quy luật tài chính cơ bản và áp dụng chúng vào việc quản lý tài sản cá nhân.', 'KT0001.jpg', GETDATE(), 80, 250000, 9, 2),
    (N'Học cách học lại', N'Cuốn sách đưa ra các phương pháp tiếp cận việc học một cách khoa học. Giúp người đọc khám phá lại cách não bộ tiếp thu thông tin, từ đó nâng cao hiệu suất làm việc và học tập.', 'Hoccachhoclai.jpg', GETDATE(), 90, 150000, 11, 1),
    (N'Kinh Tế Vi Mô', N'Giáo trình chuẩn mực cung cấp kiến thức nền tảng về Kinh Tế Vi Mô. Giải thích rõ ràng các nguyên lý cung cầu, hành vi người tiêu dùng và cấu trúc của các thị trường khác nhau.', 'KT0002.jpg', GETDATE(), 120, 180000, 9, 2),
    (N'Kinh Tế Vĩ Mô', N'Cung cấp cái nhìn tổng quan về nền kinh tế quốc gia, bao gồm các chủ đề về lạm phát, thất nghiệp, chính sách tiền tệ và tài khóa. Phù hợp cho sinh viên chuyên ngành và người đi làm.', 'KT0003.jpg', GETDATE(), 130, 185000, 9, 2),
    (N'Quản trị kinh doanh', N'Sách cung cấp các lý thuyết và thực tiễn về quản trị kinh doanh hiện đại. Khám phá cách các nhà lãnh đạo xuất sắc điều hành doanh nghiệp và đối phó với những biến động của thị trường.', 'KT0004.jpg', GETDATE(), 200, 210000, 9, 3),
    (N'Tin Học Cơ Bản', N'Giáo trình Tin Học Cơ Bản dành cho người mới tiếp xúc với máy tính. Hướng dẫn sử dụng hệ điều hành, các phần mềm văn phòng thông dụng như Word, Excel, PowerPoint một cách hiệu quả.', 'THCB.jpg', GETDATE(), 300, 100000, 2, 2),
    (N'Lập Trình C++ Cơ Bản', N'Giáo trình lập trình C++ đi từ những khái niệm nền tảng đến lập trình hướng đối tượng. Cuốn sách không thể thiếu đối với các sinh viên ngành công nghệ thông tin.', 'TH004.jpg', GETDATE(), 110, 160000, 2, 1),
    (N'Cấu Trúc Dữ Liệu và Giải Thuật', N'Đi sâu vào các cấu trúc dữ liệu kinh điển và thuật toán nền tảng. Giúp bạn tối ưu hóa mã nguồn, tăng cường tư duy logic và khả năng giải quyết các bài toán lập trình phức tạp.', 'TH005.jpg', GETDATE(), 90, 175000, 2, 2),
    (N'Lập Trình Web 2005', N'Tài liệu lịch sử giúp bạn hiểu về cách thức lập trình web trong những năm 2005. Phù hợp để nghiên cứu sự tiến hóa của công nghệ hoặc bảo trì các hệ thống mã nguồn cũ.', 'LTWeb2005.jpg', GETDATE(), 30, 80000, 2, 3),
    (N'Cơ Sở Dữ Liệu Oracle', N'Hướng dẫn toàn tập về việc cài đặt, quản trị và tối ưu hóa hệ quản trị CSDL lớn nhất thế giới Oracle. Cung cấp các câu lệnh SQL chuẩn xác và các kỹ thuật bảo mật tiên tiến.', 'Oracle.png', GETDATE(), 60, 250000, 2, 2),
    (N'Mô Hình MVC', N'Khám phá chuyên sâu về kiến trúc Model-View-Controller (MVC). Sách trình bày cách tách biệt các thành phần ứng dụng để mã nguồn trở nên dễ bảo trì, mở rộng và kiểm thử hơn.', 'mvc.jpg', GETDATE(), 150, 190000, 2, 1),
    (N'Lập trình Java', N'Cuốn sách hướng dẫn lập trình Java cho người mới bắt đầu. Bao gồm lập trình hướng đối tượng, xử lý ngoại lệ, và làm việc với các thư viện tiêu chuẩn mạnh mẽ của Java.', 'TH006.jpg', GETDATE(), 105, 155000, 2, 1),
    (N'Lập trình Python', N'Hướng dẫn cơ bản và trực quan về Python, ngôn ngữ lập trình phổ biến nhất hiện nay. Rất hữu ích cho các tác vụ phân tích dữ liệu, tự động hóa và học máy.', 'TH007.jpg', GETDATE(), 210, 165000, 2, 1),
    (N'Thiết kế Web HTML5 CSS3', N'Cẩm nang thiết kế giao diện website hiện đại với HTML5 và CSS3. Cung cấp những kỹ thuật thiết kế đáp ứng (Responsive Design) và tối ưu hóa trải nghiệm người dùng tuyệt vời.', 'TH008.jpg', GETDATE(), 120, 150000, 2, 1),
    (N'Kiến trúc máy tính', N'Tìm hiểu sâu sắc về cách các thiết bị phần cứng hoạt động cùng nhau để tạo nên một hệ thống máy tính. Khám phá các khái niệm như bộ vi xử lý, bộ nhớ và các hệ thống I/O.', 'KT0005.jpg', GETDATE(), 95, 170000, 2, 1),
    (N'Mạng máy tính', N'Giáo trình mạng máy tính từ cơ bản đến nâng cao. Khảo sát các mô hình tham chiếu OSI, TCP/IP, cấu hình định tuyến và các giao thức mạng phổ biến trong thực tế.', 'KT0006.jpg', GETDATE(), 85, 180000, 2, 1),
    (N'An toàn thông tin', N'Cuốn sách cung cấp nền tảng về bảo mật thông tin cơ bản. Phân tích các mối đe dọa không gian mạng và các phương pháp phòng chống mã độc, hacker cũng như mã hóa dữ liệu.', 'KT0007.jpg', GETDATE(), 160, 200000, 2, 1),
    (N'Phân tích thiết kế hệ thống', N'Sách cung cấp quy trình bài bản để Phân tích thiết kế Hệ thống thông tin. Bao gồm các biểu đồ UML chuẩn mực để mô hình hóa yêu cầu phần mềm của khách hàng.', 'KT0008.jpg', GETDATE(), 110, 190000, 2, 1),
    (N'Hệ điều hành', N'Giáo trình hệ điều hành giải thích các nguyên lý cốt lõi về quản lý tiến trình, quản lý bộ nhớ và hệ thống tập tin. Tương thích với các hệ điều hành phổ biến như Windows và Linux.', 'KT0009.jpg', GETDATE(), 75, 150000, 2, 1),
    (N'Trí tuệ nhân tạo', N'Giới thiệu tổng quan về các phương pháp Trí tuệ nhân tạo (AI). Khám phá các thuật toán tìm kiếm, hệ chuyên gia, suy luận logic và nền tảng của học máy (Machine Learning).', '130318.jpg', GETDATE(), 145, 220000, 2, 1),
    (N'Toán rời rạc', N'Toán học cho IT bao gồm lý thuyết đồ thị, logic toán học và các cấu trúc rời rạc. Cuốn sách là nền tảng không thể thiếu cho việc thiết kế thuật toán và lý thuyết tính toán.', '130499.jpg', GETDATE(), 65, 130000, 2, 1),
    (N'Đồ họa máy tính', N'Giáo trình đồ họa cung cấp các thuật toán vẽ đường thẳng, tô màu, và biến đổi không gian 2D/3D. Đặc biệt hữu ích cho các lập trình viên muốn lấn sân sang làm game hay thiết kế đồ họa.', 'Gt_Thcb.jpg', GETDATE(), 50, 175000, 2, 1),
    (N'Điện toán đám mây', N'Tìm hiểu xu hướng điện toán đám mây (Cloud Computing) hiện đại. Phân tích các dịch vụ IaaS, PaaS, SaaS cũng như ưu nhược điểm của các nền tảng AWS, Azure và Google Cloud.', 'thu.jpg', GETDATE(), 190, 250000, 2, 1);
END
GO

IF NOT EXISTS (SELECT * FROM KHACHHANG)
BEGIN
    INSERT INTO KHACHHANG (HoTen, TaiKhoan, MatKhau, Email, DiaChi, DienThoai, NgaySinh) VALUES
    (N'Nguyễn Văn A', 'nguyenvana', '123456', 'nguyenvana@gmail.com', N'123 Lê Lợi, Q1, TP.HCM', '0912345678', '1990-01-01'),
    (N'Trần Thị B', 'tranthib', 'password', 'tranthib@gmail.com', N'456 Nguyễn Trãi, Q5, TP.HCM', '0987654321', '1995-05-05');
END
GO

IF NOT EXISTS (SELECT * FROM TACGIA)
BEGIN
    INSERT INTO TACGIA (TenTG, DiaChi, TieuSu, DienThoai) VALUES
    (N'Nguyễn Nhật Ánh', N'TP. Hồ Chí Minh', N'Nhà văn nổi tiếng với các tác phẩm viết cho tuổi thơ.', '0901234567'),
    (N'Tony Buổi Sáng', N'Không rõ', N'Tác giả của những bài viết truyền cảm hứng cho giới trẻ.', '0988888888'),
    (N'Dale Carnegie', N'Mỹ', N'Tác giả cuốn Đắc Nhân Tâm huyền thoại.', '1234567890');
END
GO

IF NOT EXISTS (SELECT * FROM DONDATHANG)
BEGIN
    INSERT INTO DONDATHANG (DaThanhToan, TinhTrangGiaoHang, NgayDat, NgayGiao, MaKH) VALUES
    (1, 1, GETDATE() - 5, GETDATE() - 2, 1),
    (0, 0, GETDATE(), NULL, 2);
END
GO

IF NOT EXISTS (SELECT * FROM CHITIETDATHANG)
BEGIN
    INSERT INTO CHITIETDATHANG (MaDonHang, MaSach, SoLuong, DonGia) VALUES
    (1, 1, 2, 150000),
    (1, 4, 1, 80000),
    (2, 2, 1, 200000);
END
GO

IF NOT EXISTS (SELECT * FROM VIETSACH)
BEGIN
    INSERT INTO VIETSACH (MaTG, MaSach, VaiTro, ViTri) VALUES
    (3, 4, N'Tác giả', N'Chính'),
    (1, 1, N'Đồng tác giả', N'Chính');
END
GO

PRINT N'Đã tạo thành công database SachOnline và các bảng kèm khoá ngoại, cùng dữ liệu mẫu (Sample Data).';
GO
