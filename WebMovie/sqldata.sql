USE [Movie]
GO
/****** Object:  Table [dbo].[COMMENTS]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COMMENTS](
	[comment_id] [int] IDENTITY(1,1) NOT NULL,
	[Makh] [int] NULL,
	[Maphim] [int] NULL,
	[comment] [text] NULL,
	[rating] [int] NULL,
	[thoigian] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[comment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KHACHHANG]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KHACHHANG](
	[MaKh] [int] IDENTITY(1,1) NOT NULL,
	[Taikhoan] [varchar](50) NULL,
	[Matkhau] [varchar](50) NOT NULL,
	[Email] [varchar](100) NULL,
	[DienthoaiKh] [varchar](50) NULL,
	[MaQuyen] [int] NULL,
	[Hoten] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_KHACHHANG] PRIMARY KEY CLUSTERED 
(
	[MaKh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LICHSU]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LICHSU](
	[Maphim] [int] NOT NULL,
	[Makh] [int] NOT NULL,
 CONSTRAINT [PK_LICHSU] PRIMARY KEY CLUSTERED 
(
	[Maphim] ASC,
	[Makh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NAMPHATHANH]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NAMPHATHANH](
	[MaNam] [int] IDENTITY(1,1) NOT NULL,
	[Nam] [char](10) NOT NULL,
 CONSTRAINT [PK_NAMPHATHANH] PRIMARY KEY CLUSTERED 
(
	[MaNam] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHIM]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHIM](
	[Maphim] [int] IDENTITY(1,1) NOT NULL,
	[TenPhim] [nvarchar](100) NULL,
	[Daodien] [nvarchar](50) NULL,
	[Dienvien] [nvarchar](200) NULL,
	[Noidung] [nvarchar](max) NULL,
	[Dotuoi] [char](10) NULL,
	[Thoiluong] [nvarchar](20) NULL,
	[Ngonngu] [nvarchar](50) NULL,
	[Linkphim] [nvarchar](200) NULL,
	[Trailer] [nvarchar](max) NULL,
	[MaNam] [int] NULL,
	[MaQG] [int] NULL,
	[Anhbia] [varchar](50) NULL,
	[Phimbo] [bit] NULL,
	[Phanphim] [int] NULL,
	[Tapphim] [int] NULL,
 CONSTRAINT [PK_PHIM] PRIMARY KEY CLUSTERED 
(
	[Maphim] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHIMTHELOAI]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHIMTHELOAI](
	[Maphim] [int] NOT NULL,
	[MaTL] [int] NOT NULL,
 CONSTRAINT [PK_PHIMTHELOAI] PRIMARY KEY CLUSTERED 
(
	[Maphim] ASC,
	[MaTL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QUOCGIA]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QUOCGIA](
	[MaQG] [int] IDENTITY(1,1) NOT NULL,
	[TenQG] [nvarchar](50) NULL,
 CONSTRAINT [PK_QUOCGIA] PRIMARY KEY CLUSTERED 
(
	[MaQG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QUYEN]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QUYEN](
	[MaQuyen] [int] NOT NULL,
	[TenQuyen] [char](10) NULL,
 CONSTRAINT [PK_Quyen] PRIMARY KEY CLUSTERED 
(
	[MaQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[THELOAI]    Script Date: 11/05/2023 10:06:04 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[THELOAI](
	[MaTL] [int] IDENTITY(1,1) NOT NULL,
	[TenTL] [nvarchar](50) NULL,
 CONSTRAINT [PK_THELOAI] PRIMARY KEY CLUSTERED 
(
	[MaTL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[COMMENTS] ADD  DEFAULT (getdate()) FOR [thoigian]
GO
ALTER TABLE [dbo].[COMMENTS]  WITH CHECK ADD FOREIGN KEY([Makh])
REFERENCES [dbo].[KHACHHANG] ([MaKh])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[COMMENTS]  WITH CHECK ADD FOREIGN KEY([Maphim])
REFERENCES [dbo].[PHIM] ([Maphim])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[KHACHHANG]  WITH CHECK ADD  CONSTRAINT [FK_KHACHHANG_Quyen] FOREIGN KEY([MaQuyen])
REFERENCES [dbo].[QUYEN] ([MaQuyen])
GO
ALTER TABLE [dbo].[KHACHHANG] CHECK CONSTRAINT [FK_KHACHHANG_Quyen]
GO
ALTER TABLE [dbo].[LICHSU]  WITH CHECK ADD  CONSTRAINT [FK_LICHSU_KHACHHANG] FOREIGN KEY([Makh])
REFERENCES [dbo].[KHACHHANG] ([MaKh])
GO
ALTER TABLE [dbo].[LICHSU] CHECK CONSTRAINT [FK_LICHSU_KHACHHANG]
GO
ALTER TABLE [dbo].[LICHSU]  WITH CHECK ADD  CONSTRAINT [FK_LICHSU_PHIM] FOREIGN KEY([Maphim])
REFERENCES [dbo].[PHIM] ([Maphim])
GO
ALTER TABLE [dbo].[LICHSU] CHECK CONSTRAINT [FK_LICHSU_PHIM]
GO
ALTER TABLE [dbo].[PHIM]  WITH CHECK ADD  CONSTRAINT [FK_PHIM_NAMPHATHANH] FOREIGN KEY([MaNam])
REFERENCES [dbo].[NAMPHATHANH] ([MaNam])
GO
ALTER TABLE [dbo].[PHIM] CHECK CONSTRAINT [FK_PHIM_NAMPHATHANH]
GO
ALTER TABLE [dbo].[PHIM]  WITH CHECK ADD  CONSTRAINT [FK_PHIM_QUOCGIA] FOREIGN KEY([MaQG])
REFERENCES [dbo].[QUOCGIA] ([MaQG])
GO
ALTER TABLE [dbo].[PHIM] CHECK CONSTRAINT [FK_PHIM_QUOCGIA]
GO
ALTER TABLE [dbo].[PHIMTHELOAI]  WITH CHECK ADD  CONSTRAINT [FK_PHIMTHELOAI_PHIM] FOREIGN KEY([Maphim])
REFERENCES [dbo].[PHIM] ([Maphim])
GO
ALTER TABLE [dbo].[PHIMTHELOAI] CHECK CONSTRAINT [FK_PHIMTHELOAI_PHIM]
GO
ALTER TABLE [dbo].[PHIMTHELOAI]  WITH CHECK ADD  CONSTRAINT [FK_PHIMTHELOAI_THELOAI] FOREIGN KEY([MaTL])
REFERENCES [dbo].[THELOAI] ([MaTL])
GO
ALTER TABLE [dbo].[PHIMTHELOAI] CHECK CONSTRAINT [FK_PHIMTHELOAI_THELOAI]
GO
