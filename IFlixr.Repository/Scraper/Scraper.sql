﻿USE [master]
GO
/****** Object:  Database [IFlixr_Scraper]    Script Date: 11/19/2012 01:37:45 ******/
CREATE DATABASE [IFlixr_Scraper] ON  PRIMARY 
( NAME = N'IFlixr_Scraper', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\IFlixr_Scraper.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'IFlixr_Scraper_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\IFlixr_Scraper_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [IFlixr_Scraper] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IFlixr_Scraper].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IFlixr_Scraper] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET ANSI_NULLS OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET ANSI_PADDING OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET ARITHABORT OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [IFlixr_Scraper] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [IFlixr_Scraper] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [IFlixr_Scraper] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET  DISABLE_BROKER
GO
ALTER DATABASE [IFlixr_Scraper] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [IFlixr_Scraper] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [IFlixr_Scraper] SET  READ_WRITE
GO
ALTER DATABASE [IFlixr_Scraper] SET RECOVERY FULL
GO
ALTER DATABASE [IFlixr_Scraper] SET  MULTI_USER
GO
ALTER DATABASE [IFlixr_Scraper] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [IFlixr_Scraper] SET DB_CHAINING OFF
GO
USE [IFlixr_Scraper]
GO
/****** Object:  Table [dbo].[Video]    Script Date: 11/19/2012 01:37:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Video](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MovieUrl] [varchar](255) NOT NULL,
	[Duration] [int] NOT NULL,
	[Title] [varchar](512) NOT NULL,
	[PublishedDate] [datetime] NOT NULL,
	[Uploader] [varchar](100) NOT NULL,
	[Source] [tinyint] NOT NULL,
	[VideoId] [varchar](50) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Url] [varchar](512) NOT NULL,
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TempData]    Script Date: 11/19/2012 01:37:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](512) NOT NULL,
	[Value] [varchar](255) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Skip] [bit] NOT NULL,
 CONSTRAINT [PK_TempData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Video_Duration]    Script Date: 11/19/2012 01:37:46 ******/
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_Duration]  DEFAULT ((0)) FOR [Duration]
GO
/****** Object:  Default [DF_Video_Source]    Script Date: 11/19/2012 01:37:46 ******/
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_Source]  DEFAULT ((0)) FOR [Source]
GO
/****** Object:  Default [DF_Video_Type]    Script Date: 11/19/2012 01:37:46 ******/
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_Type]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF_TempData_Type]    Script Date: 11/19/2012 01:37:46 ******/
ALTER TABLE [dbo].[TempData] ADD  CONSTRAINT [DF_TempData_Type]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF_TempData_Skip]    Script Date: 11/19/2012 01:37:46 ******/
ALTER TABLE [dbo].[TempData] ADD  CONSTRAINT [DF_TempData_Skip]  DEFAULT ((0)) FOR [Skip]
GO
