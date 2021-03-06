USE [master]
GO
/****** Object:  Database [IFlixr]    Script Date: 12/06/2012 11:56:20 ******/
CREATE DATABASE [IFlixr] ON  PRIMARY 
( NAME = N'IFlixr', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\IFlixr.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'IFlixr_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\IFlixr_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [IFlixr] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IFlixr].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IFlixr] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [IFlixr] SET ANSI_NULLS OFF
GO
ALTER DATABASE [IFlixr] SET ANSI_PADDING OFF
GO
ALTER DATABASE [IFlixr] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [IFlixr] SET ARITHABORT OFF
GO
ALTER DATABASE [IFlixr] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [IFlixr] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [IFlixr] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [IFlixr] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [IFlixr] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [IFlixr] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [IFlixr] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [IFlixr] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [IFlixr] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [IFlixr] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [IFlixr] SET  DISABLE_BROKER
GO
ALTER DATABASE [IFlixr] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [IFlixr] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [IFlixr] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [IFlixr] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [IFlixr] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [IFlixr] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [IFlixr] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [IFlixr] SET  READ_WRITE
GO
ALTER DATABASE [IFlixr] SET RECOVERY FULL
GO
ALTER DATABASE [IFlixr] SET  MULTI_USER
GO
ALTER DATABASE [IFlixr] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [IFlixr] SET DB_CHAINING OFF
GO
USE [IFlixr]
GO
/****** Object:  Table [dbo].[Show]    Script Date: 12/06/2012 11:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Show](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Title] [varchar](250) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[UniqueToken] [varchar](250) NULL,
	[Language] [tinyint] NOT NULL,
	[Year] [smallint] NOT NULL,
	[Country] [tinyint] NOT NULL,
	[Description] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Carousel]    Script Date: 12/06/2012 11:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Carousel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[TitleLink] [varchar](255) NOT NULL,
	[Language] [tinyint] NOT NULL,
	[Category] [tinyint] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Size] [tinyint] NOT NULL,
	[Rows] [tinyint] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Carousel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Banner]    Script Date: 12/06/2012 11:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Banner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImageName] [varchar](255) NOT NULL,
	[Category] [tinyint] NOT NULL,
	[Language] [tinyint] NOT NULL,
	[Link] [varchar](512) NOT NULL,
	[Title] [varchar](512) NOT NULL,
	[Text] [varchar](4000) NOT NULL,
 CONSTRAINT [PK_Banner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VideoLink]    Script Date: 12/06/2012 11:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoLink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShowId] [int] NOT NULL,
	[UniqueToken] [varchar](50) NOT NULL,
	[Url] [varchar](512) NOT NULL,
	[IndexNumber] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[Duration] [int] NOT NULL,
	[PublishedDate] [datetime] NOT NULL,
	[Title] [varchar](512) NOT NULL,
	[Uploader] [varchar](100) NOT NULL,
	[Source] [tinyint] NOT NULL,
 CONSTRAINT [PK_VideoLink] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_FK_VideoLink_Show] ON [dbo].[VideoLink] 
(
	[ShowId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaroselItem]    Script Date: 12/06/2012 11:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaroselItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CaroselId] [int] NOT NULL,
	[ShowId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_CaroselItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 12/06/2012 11:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Movie](
	[Id] [int] NOT NULL,
	[ReleaseData] [date] NOT NULL,
	[DirectedBy] [varchar](4000) NULL,
	[ProducedBy] [varchar](4000) NULL,
	[WrittenBy] [varchar](4000) NULL,
	[Starring] [varchar](4000) NULL,
	[MusicBy] [varchar](4000) NULL,
	[SongsBy] [varchar](4000) NULL,
	[CinematographyBy] [varchar](4000) NULL,
	[EditingBy] [varchar](4000) NULL,
	[Studio] [varchar](4000) NULL,
	[DistributedBy] [varchar](4000) NULL,
	[ScreenplayBy] [varchar](4000) NULL,
	[RunningTime] [smallint] NOT NULL,
 CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Image]    Script Date: 12/06/2012 11:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Image](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShowId] [int] NOT NULL,
	[FullPath] [varchar](255) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[SupportedDimensions] [int] NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Show_Language]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Show] ADD  CONSTRAINT [DF_Show_Language]  DEFAULT ((0)) FOR [Language]
GO
/****** Object:  Default [DF_Show_Country]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Show] ADD  CONSTRAINT [DF_Show_Country]  DEFAULT ((0)) FOR [Country]
GO
/****** Object:  Default [DF_Carousel_Language]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Carousel] ADD  CONSTRAINT [DF_Carousel_Language]  DEFAULT ((0)) FOR [Language]
GO
/****** Object:  Default [DF_Carousel_Category]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Carousel] ADD  CONSTRAINT [DF_Carousel_Category]  DEFAULT ((0)) FOR [Category]
GO
/****** Object:  Default [DF_Carousel_Type]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Carousel] ADD  CONSTRAINT [DF_Carousel_Type]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF_Carousel_Size]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Carousel] ADD  CONSTRAINT [DF_Carousel_Size]  DEFAULT ((1)) FOR [Size]
GO
/****** Object:  Default [DF_Carousel_Rows]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Carousel] ADD  CONSTRAINT [DF_Carousel_Rows]  DEFAULT ((1)) FOR [Rows]
GO
/****** Object:  Default [DF_Carousel_Active]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Carousel] ADD  CONSTRAINT [DF_Carousel_Active]  DEFAULT ((1)) FOR [Active]
GO
/****** Object:  Default [DF_Banner_Category]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Banner] ADD  CONSTRAINT [DF_Banner_Category]  DEFAULT ((0)) FOR [Category]
GO
/****** Object:  Default [DF_Banner_Language]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Banner] ADD  CONSTRAINT [DF_Banner_Language]  DEFAULT ((0)) FOR [Language]
GO
/****** Object:  Default [DF_VideoLink_Source]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[VideoLink] ADD  CONSTRAINT [DF_VideoLink_Source]  DEFAULT ((0)) FOR [Source]
GO
/****** Object:  Default [DF_CaroselItem_Active]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[CaroselItem] ADD  CONSTRAINT [DF_CaroselItem_Active]  DEFAULT ((1)) FOR [Active]
GO
/****** Object:  Default [DF_Movie_RunningTime]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Movie] ADD  CONSTRAINT [DF_Movie_RunningTime]  DEFAULT ((0)) FOR [RunningTime]
GO
/****** Object:  Default [DF_Image_IsDefault]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Image] ADD  CONSTRAINT [DF_Image_IsDefault]  DEFAULT ((1)) FOR [IsDefault]
GO
/****** Object:  ForeignKey [FK_Parent_Child]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Show]  WITH CHECK ADD  CONSTRAINT [FK_Parent_Child] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Show] ([Id])
GO
ALTER TABLE [dbo].[Show] CHECK CONSTRAINT [FK_Parent_Child]
GO
/****** Object:  ForeignKey [FK_VideoLink_Show]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[VideoLink]  WITH CHECK ADD  CONSTRAINT [FK_VideoLink_Show] FOREIGN KEY([ShowId])
REFERENCES [dbo].[Show] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VideoLink] CHECK CONSTRAINT [FK_VideoLink_Show]
GO
/****** Object:  ForeignKey [FK_CaroselItem_Carousel]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[CaroselItem]  WITH NOCHECK ADD  CONSTRAINT [FK_CaroselItem_Carousel] FOREIGN KEY([CaroselId])
REFERENCES [dbo].[Carousel] ([Id])
GO
ALTER TABLE [dbo].[CaroselItem] CHECK CONSTRAINT [FK_CaroselItem_Carousel]
GO
/****** Object:  ForeignKey [FK_CaroselItem_Show]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[CaroselItem]  WITH NOCHECK ADD  CONSTRAINT [FK_CaroselItem_Show] FOREIGN KEY([ShowId])
REFERENCES [dbo].[Show] ([Id])
GO
ALTER TABLE [dbo].[CaroselItem] CHECK CONSTRAINT [FK_CaroselItem_Show]
GO
/****** Object:  ForeignKey [FK_Movie_Show]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Movie]  WITH CHECK ADD  CONSTRAINT [FK_Movie_Show] FOREIGN KEY([Id])
REFERENCES [dbo].[Show] ([Id])
GO
ALTER TABLE [dbo].[Movie] CHECK CONSTRAINT [FK_Movie_Show]
GO
/****** Object:  ForeignKey [FK_Image_Show]    Script Date: 12/06/2012 11:56:23 ******/
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_Show] FOREIGN KEY([ShowId])
REFERENCES [dbo].[Show] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_Show]
GO
