USE [master]
GO
/****** Object:  Database [ProductDb]    Script Date: 10/6/2024 5:46:38 PM ******/
CREATE DATABASE [ProductDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ProductDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\ProductDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ProductDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\ProductDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ProductDb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProductDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ProductDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ProductDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ProductDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ProductDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ProductDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [ProductDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ProductDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ProductDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ProductDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ProductDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ProductDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ProductDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ProductDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ProductDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ProductDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ProductDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ProductDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ProductDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ProductDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ProductDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ProductDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ProductDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ProductDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ProductDb] SET  MULTI_USER 
GO
ALTER DATABASE [ProductDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ProductDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ProductDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ProductDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ProductDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ProductDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ProductDb] SET QUERY_STORE = OFF
GO
USE [ProductDb]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/6/2024 5:46:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [bigint] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Category'] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 10/6/2024 5:46:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [bigint] NOT NULL,
	[ProductTitle] [nvarchar](100) NULL,
	[Code] [nvarchar](100) NULL,
	[Brand] [nvarchar](100) NULL,
	[Price] [decimal](18, 0) NULL,
	[Description] [nvarchar](500) NULL,
	[CategoryId] [bigint] NULL,
	[SubCategoryId] [bigint] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCategory]    Script Date: 10/6/2024 5:46:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategory](
	[Id] [bigint] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[CategoryId] [bigint] NOT NULL,
 CONSTRAINT [PK_SubCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category'_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SubCategory] ADD  CONSTRAINT [DF_SubCategory_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_SubCategory] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[SubCategory] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_SubCategory]
GO
ALTER TABLE [dbo].[SubCategory]  WITH CHECK ADD  CONSTRAINT [FK_SubCategory_SubCategory] FOREIGN KEY([Id])
REFERENCES [dbo].[SubCategory] ([Id])
GO
ALTER TABLE [dbo].[SubCategory] CHECK CONSTRAINT [FK_SubCategory_SubCategory]
GO
/****** Object:  StoredProcedure [dbo].[GetProducts]    Script Date: 10/6/2024 5:46:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetProducts]
    @PageNumber INT,
    @PageSize INT,
    @Title NVARCHAR(255) = NULL,
    @Brand NVARCHAR(255) = NULL,
    @TotalCount INT OUTPUT -- Adding an output parameter
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate the offset for pagination
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    -- Declare a table variable to hold the filtered products
    DECLARE @Products TABLE (
        Id INT,
        ProductTitle NVARCHAR(255),
        Code NVARCHAR(50),
        Brand NVARCHAR(50),
        Price DECIMAL(18, 2),
        Description NVARCHAR(MAX),
        CreatedAt DATETIME,
        UpdatedAt DATETIME,
        IsDeleted BIT
    );

    -- Insert filtered products into the table variable
    INSERT INTO @Products (Id, ProductTitle, Code, Brand, Price, Description, CreatedAt, UpdatedAt, IsDeleted)
    SELECT Id, ProductTitle, Code, Brand, Price, Description, CreatedAt, UpdatedAt, IsDeleted
    FROM Product
    WHERE (@Title IS NULL OR ProductTitle LIKE '%' + @Title + '%')
      AND (@Brand IS NULL OR Brand LIKE '%' + @Brand + '%')
    ORDER BY CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Get total count of filtered products and set it to the output parameter
    SELECT @TotalCount = COUNT(*)
    FROM Product
    WHERE (@Title IS NULL OR ProductTitle LIKE '%' + @Title + '%')
      AND (@Brand IS NULL OR Brand LIKE '%' + @Brand + '%');

    -- Return the results
    SELECT 
        p.Id,
        p.ProductTitle,
        p.Code,
        p.Brand,
        p.Price,
        p.Description,
        p.CreatedAt,
        p.UpdatedAt,
        p.IsDeleted
    FROM @Products p;
END
GO
USE [master]
GO
ALTER DATABASE [ProductDb] SET  READ_WRITE 
GO
