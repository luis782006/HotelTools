USE [master]
GO
/****** Object:  Database [Hotel_Tools]    Script Date: 27/11/2024 9:08:37 ******/
CREATE DATABASE [Hotel_Tools]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HotelTools', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\HotelTools.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HotelTools_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\HotelTools_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Hotel_Tools] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Hotel_Tools].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Hotel_Tools] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Hotel_Tools] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Hotel_Tools] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Hotel_Tools] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Hotel_Tools] SET ARITHABORT OFF 
GO
ALTER DATABASE [Hotel_Tools] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Hotel_Tools] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Hotel_Tools] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Hotel_Tools] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Hotel_Tools] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Hotel_Tools] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Hotel_Tools] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Hotel_Tools] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Hotel_Tools] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Hotel_Tools] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Hotel_Tools] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Hotel_Tools] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Hotel_Tools] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Hotel_Tools] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Hotel_Tools] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Hotel_Tools] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Hotel_Tools] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Hotel_Tools] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Hotel_Tools] SET  MULTI_USER 
GO
ALTER DATABASE [Hotel_Tools] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Hotel_Tools] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Hotel_Tools] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Hotel_Tools] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Hotel_Tools] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Hotel_Tools]
GO
/****** Object:  Schema [Empleados]    Script Date: 27/11/2024 9:08:38 ******/
CREATE SCHEMA [Empleados]
GO
/****** Object:  Schema [General]    Script Date: 27/11/2024 9:08:38 ******/
CREATE SCHEMA [General]
GO
/****** Object:  Schema [Inventarios]    Script Date: 27/11/2024 9:08:38 ******/
CREATE SCHEMA [Inventarios]
GO
/****** Object:  Schema [Quejas]    Script Date: 27/11/2024 9:08:38 ******/
CREATE SCHEMA [Quejas]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Empleados].[Cargo]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Empleados].[Cargo](
	[ID_Cargo] [decimal](18, 2) NOT NULL,
	[NombreCargo] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_cargo] PRIMARY KEY CLUSTERED 
(
	[ID_Cargo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Empleados].[Empleados]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Empleados].[Empleados](
	[ID_Empleado] [decimal](18, 2) NOT NULL,
	[Nombre] [nchar](20) NOT NULL,
	[Apellido] [nchar](40) NOT NULL,
	[ID_Cargo] [decimal](18, 2) NOT NULL,
	[ID_Departamento] [decimal](18, 2) NOT NULL,
	[NroContacto] [nvarchar](max) NOT NULL,
	[ID_Rol] [decimal](18, 2) NOT NULL,
	[FechaDesde] [datetime] NOT NULL,
	[FechaHasta] [datetime] NOT NULL,
	[Activo] [tinyint] NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_empleados] PRIMARY KEY CLUSTERED 
(
	[ID_Empleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Empleados].[Rol]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Empleados].[Rol](
	[ID_Rol] [decimal](18, 2) NOT NULL,
	[NombreRol] [nchar](10) NOT NULL,
	[Seccion] [nchar](200) NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[ID_Rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [General].[Departamentos]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [General].[Departamentos](
	[ID_Departamento] [decimal](18, 2) NOT NULL,
	[NombreDepartamento] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_departamentos] PRIMARY KEY CLUSTERED 
(
	[ID_Departamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [General].[Habitaciones]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [General].[Habitaciones](
	[ID_NroHab] [decimal](18, 2) NOT NULL,
	[ID_CategoriasFK] [decimal](18, 2) NOT NULL,
	[MaxPersonas] [int] NOT NULL,
 CONSTRAINT [PK_Habitaciones] PRIMARY KEY CLUSTERED 
(
	[ID_NroHab] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [General].[Idioma]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [General].[Idioma](
	[ID_Idioma] [decimal](18, 2) NOT NULL,
	[NombreIdioma] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_idioma] PRIMARY KEY CLUSTERED 
(
	[ID_Idioma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [General].[Pais]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [General].[Pais](
	[ID_PaisOrigen] [decimal](18, 2) NOT NULL,
	[NombrePais] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_pais] PRIMARY KEY CLUSTERED 
(
	[ID_PaisOrigen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Inventarios].[Categorias]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Inventarios].[Categorias](
	[ID_Categoria] [decimal](18, 2) NOT NULL,
	[TipoHab] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED 
(
	[ID_Categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Inventarios].[CategoriasProductos]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Inventarios].[CategoriasProductos](
	[ID_CategoriasPro] [decimal](18, 2) NOT NULL,
	[NombreCatProductos] [varchar](60) NOT NULL,
 CONSTRAINT [PK_CategoriasProductos] PRIMARY KEY CLUSTERED 
(
	[ID_CategoriasPro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Inventarios].[DetallesFactura]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Inventarios].[DetallesFactura](
	[ID_Detalle] [decimal](18, 2) NOT NULL,
	[ID_FacturaFK] [decimal](18, 2) NOT NULL,
	[ID_ProductoFK] [decimal](18, 2) NOT NULL,
	[NroLinea] [nchar](10) NOT NULL,
	[Cantidad] [decimal](18, 2) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_DetallesFactura] PRIMARY KEY CLUSTERED 
(
	[ID_Detalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Inventarios].[Facturas]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Inventarios].[Facturas](
	[ID_NroFacturas] [decimal](18, 2) NOT NULL,
	[ID_ProveedorFK] [decimal](18, 2) NOT NULL,
	[ID_DetalleFK] [decimal](18, 2) NOT NULL,
	[ID_TipoCompra] [decimal](18, 2) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[TotalFactura] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Facturas] PRIMARY KEY CLUSTERED 
(
	[ID_NroFacturas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Inventarios].[HabitacionProductos]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Inventarios].[HabitacionProductos](
	[ID_HabProductos] [decimal](18, 2) NOT NULL,
	[ID_HabFK] [decimal](18, 2) NOT NULL,
	[ID_ProductosFK] [decimal](18, 2) NOT NULL,
	[Cantidad] [int] NOT NULL,
 CONSTRAINT [PK_HabitacionProductos] PRIMARY KEY CLUSTERED 
(
	[ID_HabProductos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Inventarios].[Modelos]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Inventarios].[Modelos](
	[ID_Modelos] [decimal](18, 2) NOT NULL,
	[NombreModelos] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Modelos] PRIMARY KEY CLUSTERED 
(
	[ID_Modelos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Inventarios].[Productos]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Inventarios].[Productos](
	[ID_Productos] [decimal](18, 2) NOT NULL,
	[Codigo] [varchar](80) NOT NULL,
	[Descripcion] [varchar](60) NOT NULL,
	[ID_CategoriaProFK] [decimal](18, 2) NOT NULL,
	[ID_ModelosFK] [decimal](18, 2) NOT NULL,
	[ID_NroFacturaFK] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED 
(
	[ID_Productos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Inventarios].[ProductosMovimientos]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Inventarios].[ProductosMovimientos](
	[ID_ProductosMovimientos] [decimal](18, 2) NOT NULL,
	[ID_Productos] [decimal](18, 2) NOT NULL,
	[ID_HabOrigen] [decimal](18, 2) NOT NULL,
	[ID_HabDestino] [decimal](18, 2) NOT NULL,
	[FechaMov] [datetime] NOT NULL,
	[ID_Empleado] [decimal](18, 2) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[ID_EmpleadoMov] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ProductosMovimientos] PRIMARY KEY CLUSTERED 
(
	[ID_ProductosMovimientos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Inventarios].[Proveedores]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Inventarios].[Proveedores](
	[ID_Proveedor] [decimal](18, 2) NOT NULL,
	[ID_RazonSocialFK] [decimal](18, 2) NOT NULL,
	[ID_RepresentanteFK] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Proveedores] PRIMARY KEY CLUSTERED 
(
	[ID_Proveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Inventarios].[RazonSocial]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Inventarios].[RazonSocial](
	[ID_RazonSocial] [decimal](18, 2) NOT NULL,
	[Nombre] [varchar](80) NOT NULL,
	[Cuil] [decimal](18, 2) NOT NULL,
	[DireccionFisica] [varchar](120) NOT NULL,
	[DireccionDigital] [varchar](160) NOT NULL,
	[TelefonoFijo] [varchar](20) NULL,
	[TelefonoCelular] [varchar](20) NULL,
	[Email] [nchar](60) NULL,
 CONSTRAINT [PK_RazonSocial] PRIMARY KEY CLUSTERED 
(
	[ID_RazonSocial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Inventarios].[Representante]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Inventarios].[Representante](
	[ID_Representante] [decimal](18, 2) NOT NULL,
	[Nombre] [varchar](60) NOT NULL,
	[TelefonoCelular] [varchar](20) NOT NULL,
	[Email] [varchar](60) NULL,
 CONSTRAINT [PK_Representante] PRIMARY KEY CLUSTERED 
(
	[ID_Representante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Inventarios].[TipoCompra]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Inventarios].[TipoCompra](
	[ID_TipoCompra] [decimal](18, 2) NOT NULL,
	[TipoDeCompra] [nchar](20) NOT NULL,
 CONSTRAINT [PK_TipoCompra] PRIMARY KEY CLUSTERED 
(
	[ID_TipoCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Quejas].[Estados]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Quejas].[Estados](
	[ID_Estado] [decimal](18, 2) NOT NULL,
	[NombreEstado] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_estados] PRIMARY KEY CLUSTERED 
(
	[ID_Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Quejas].[Historial]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Quejas].[Historial](
	[ID_Orden] [decimal](18, 2) NOT NULL,
	[ID_Quejas] [decimal](18, 2) NOT NULL,
	[ID_Empleado] [decimal](18, 2) NOT NULL,
	[observaciones] [nchar](10) NULL,
	[ID_Compra] [decimal](18, 2) NOT NULL,
	[aprobado] [nchar](10) NOT NULL,
	[ID_Imagen] [decimal](18, 2) NOT NULL,
	[ID_Estado] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Historial] PRIMARY KEY CLUSTERED 
(
	[ID_Orden] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Quejas].[Huespedes]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Quejas].[Huespedes](
	[ID_Huesped] [decimal](18, 2) NOT NULL,
	[Nombre] [nchar](40) NOT NULL,
	[Apellido] [nchar](50) NOT NULL,
	[FechaIn] [datetime] NOT NULL,
	[FechaOut] [datetime] NULL,
	[DiasAlojados] [int] NOT NULL,
	[ID_Idioma] [decimal](18, 2) NOT NULL,
	[ID_Pais] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Huespedes] PRIMARY KEY CLUSTERED 
(
	[ID_Huesped] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Quejas].[Imagen]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Quejas].[Imagen](
	[idImagen] [decimal](18, 2) NOT NULL,
	[imagen] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_imagen] PRIMARY KEY CLUSTERED 
(
	[idImagen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Quejas].[Prioridad]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Quejas].[Prioridad](
	[ID_Prioridad] [decimal](18, 2) NOT NULL,
	[NombrePrioridad] [int] NOT NULL,
 CONSTRAINT [PK_prioridad] PRIMARY KEY CLUSTERED 
(
	[ID_Prioridad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Quejas].[Quejas]    Script Date: 27/11/2024 9:08:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Quejas].[Quejas](
	[ID_Quejas] [decimal](18, 2) NOT NULL,
	[Quejas] [nvarchar](max) NOT NULL,
	[ID_Habitaciones] [decimal](18, 2) NOT NULL,
	[ID_Huesped] [decimal](18, 2) NOT NULL,
	[ID_Empleado] [decimal](18, 2) NOT NULL,
	[ID_DepartamentoRecibe] [decimal](18, 2) NOT NULL,
	[ID_DepartamentoEjecuta] [decimal](18, 2) NOT NULL,
	[ID_Prioridad] [decimal](18, 2) NOT NULL,
	[ID_EmpleadoAsignacion] [decimal](18, 2) NULL,
	[ID_Estado] [decimal](18, 2) NULL,
 CONSTRAINT [PK_quejas] PRIMARY KEY CLUSTERED 
(
	[ID_Quejas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [Empleados].[Empleados]  WITH CHECK ADD  CONSTRAINT [FK_Empleados_Cargo] FOREIGN KEY([ID_Cargo])
REFERENCES [Empleados].[Cargo] ([ID_Cargo])
GO
ALTER TABLE [Empleados].[Empleados] CHECK CONSTRAINT [FK_Empleados_Cargo]
GO
ALTER TABLE [Empleados].[Empleados]  WITH CHECK ADD  CONSTRAINT [FK_Empleados_Departamentos] FOREIGN KEY([ID_Departamento])
REFERENCES [General].[Departamentos] ([ID_Departamento])
GO
ALTER TABLE [Empleados].[Empleados] CHECK CONSTRAINT [FK_Empleados_Departamentos]
GO
ALTER TABLE [Empleados].[Empleados]  WITH CHECK ADD  CONSTRAINT [FK_Empleados_Rol] FOREIGN KEY([ID_Rol])
REFERENCES [Empleados].[Rol] ([ID_Rol])
GO
ALTER TABLE [Empleados].[Empleados] CHECK CONSTRAINT [FK_Empleados_Rol]
GO
ALTER TABLE [General].[Habitaciones]  WITH CHECK ADD  CONSTRAINT [FK_Habitaciones_Categorias] FOREIGN KEY([ID_CategoriasFK])
REFERENCES [Inventarios].[Categorias] ([ID_Categoria])
GO
ALTER TABLE [General].[Habitaciones] CHECK CONSTRAINT [FK_Habitaciones_Categorias]
GO
ALTER TABLE [Inventarios].[DetallesFactura]  WITH CHECK ADD  CONSTRAINT [FK_DetallesFactura_Facturas] FOREIGN KEY([ID_FacturaFK])
REFERENCES [Inventarios].[Facturas] ([ID_NroFacturas])
GO
ALTER TABLE [Inventarios].[DetallesFactura] CHECK CONSTRAINT [FK_DetallesFactura_Facturas]
GO
ALTER TABLE [Inventarios].[DetallesFactura]  WITH CHECK ADD  CONSTRAINT [FK_DetallesFactura_Productos] FOREIGN KEY([ID_ProductoFK])
REFERENCES [Inventarios].[Productos] ([ID_Productos])
GO
ALTER TABLE [Inventarios].[DetallesFactura] CHECK CONSTRAINT [FK_DetallesFactura_Productos]
GO
ALTER TABLE [Inventarios].[Facturas]  WITH CHECK ADD  CONSTRAINT [FK_Facturas_Proveedores1] FOREIGN KEY([ID_ProveedorFK])
REFERENCES [Inventarios].[Proveedores] ([ID_Proveedor])
GO
ALTER TABLE [Inventarios].[Facturas] CHECK CONSTRAINT [FK_Facturas_Proveedores1]
GO
ALTER TABLE [Inventarios].[Facturas]  WITH CHECK ADD  CONSTRAINT [FK_Facturas_TipoCompra] FOREIGN KEY([ID_TipoCompra])
REFERENCES [Inventarios].[TipoCompra] ([ID_TipoCompra])
GO
ALTER TABLE [Inventarios].[Facturas] CHECK CONSTRAINT [FK_Facturas_TipoCompra]
GO
ALTER TABLE [Inventarios].[HabitacionProductos]  WITH CHECK ADD  CONSTRAINT [FK_HabitacionProductos_Habitaciones] FOREIGN KEY([ID_HabFK])
REFERENCES [General].[Habitaciones] ([ID_NroHab])
GO
ALTER TABLE [Inventarios].[HabitacionProductos] CHECK CONSTRAINT [FK_HabitacionProductos_Habitaciones]
GO
ALTER TABLE [Inventarios].[HabitacionProductos]  WITH CHECK ADD  CONSTRAINT [FK_HabitacionProductos_Productos] FOREIGN KEY([ID_ProductosFK])
REFERENCES [Inventarios].[Productos] ([ID_Productos])
GO
ALTER TABLE [Inventarios].[HabitacionProductos] CHECK CONSTRAINT [FK_HabitacionProductos_Productos]
GO
ALTER TABLE [Inventarios].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_CategoriasProductos] FOREIGN KEY([ID_CategoriaProFK])
REFERENCES [Inventarios].[CategoriasProductos] ([ID_CategoriasPro])
GO
ALTER TABLE [Inventarios].[Productos] CHECK CONSTRAINT [FK_Productos_CategoriasProductos]
GO
ALTER TABLE [Inventarios].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_Facturas] FOREIGN KEY([ID_NroFacturaFK])
REFERENCES [Inventarios].[Facturas] ([ID_NroFacturas])
GO
ALTER TABLE [Inventarios].[Productos] CHECK CONSTRAINT [FK_Productos_Facturas]
GO
ALTER TABLE [Inventarios].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_Modelos] FOREIGN KEY([ID_ModelosFK])
REFERENCES [Inventarios].[Modelos] ([ID_Modelos])
GO
ALTER TABLE [Inventarios].[Productos] CHECK CONSTRAINT [FK_Productos_Modelos]
GO
ALTER TABLE [Inventarios].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_ProductosMovimientos] FOREIGN KEY([ID_Productos])
REFERENCES [Inventarios].[ProductosMovimientos] ([ID_ProductosMovimientos])
GO
ALTER TABLE [Inventarios].[Productos] CHECK CONSTRAINT [FK_Productos_ProductosMovimientos]
GO
ALTER TABLE [Inventarios].[ProductosMovimientos]  WITH CHECK ADD  CONSTRAINT [FK_ProductosMovimientos_Empleados] FOREIGN KEY([ID_Empleado])
REFERENCES [Empleados].[Empleados] ([ID_Empleado])
GO
ALTER TABLE [Inventarios].[ProductosMovimientos] CHECK CONSTRAINT [FK_ProductosMovimientos_Empleados]
GO
ALTER TABLE [Inventarios].[ProductosMovimientos]  WITH CHECK ADD  CONSTRAINT [FK_ProductosMovimientos_Empleados1] FOREIGN KEY([ID_EmpleadoMov])
REFERENCES [Empleados].[Empleados] ([ID_Empleado])
GO
ALTER TABLE [Inventarios].[ProductosMovimientos] CHECK CONSTRAINT [FK_ProductosMovimientos_Empleados1]
GO
ALTER TABLE [Inventarios].[ProductosMovimientos]  WITH CHECK ADD  CONSTRAINT [FK_ProductosMovimientos_Habitaciones] FOREIGN KEY([ID_HabOrigen])
REFERENCES [General].[Habitaciones] ([ID_NroHab])
GO
ALTER TABLE [Inventarios].[ProductosMovimientos] CHECK CONSTRAINT [FK_ProductosMovimientos_Habitaciones]
GO
ALTER TABLE [Inventarios].[ProductosMovimientos]  WITH CHECK ADD  CONSTRAINT [FK_ProductosMovimientos_Habitaciones1] FOREIGN KEY([ID_HabDestino])
REFERENCES [General].[Habitaciones] ([ID_NroHab])
GO
ALTER TABLE [Inventarios].[ProductosMovimientos] CHECK CONSTRAINT [FK_ProductosMovimientos_Habitaciones1]
GO
ALTER TABLE [Inventarios].[ProductosMovimientos]  WITH CHECK ADD  CONSTRAINT [FK_ProductosMovimientos_Productos] FOREIGN KEY([ID_Productos])
REFERENCES [Inventarios].[Productos] ([ID_Productos])
GO
ALTER TABLE [Inventarios].[ProductosMovimientos] CHECK CONSTRAINT [FK_ProductosMovimientos_Productos]
GO
ALTER TABLE [Inventarios].[Proveedores]  WITH CHECK ADD  CONSTRAINT [FK_Proveedores_RazonSocial] FOREIGN KEY([ID_RazonSocialFK])
REFERENCES [Inventarios].[RazonSocial] ([ID_RazonSocial])
GO
ALTER TABLE [Inventarios].[Proveedores] CHECK CONSTRAINT [FK_Proveedores_RazonSocial]
GO
ALTER TABLE [Inventarios].[Proveedores]  WITH CHECK ADD  CONSTRAINT [FK_Proveedores_Representante] FOREIGN KEY([ID_RepresentanteFK])
REFERENCES [Inventarios].[Representante] ([ID_Representante])
GO
ALTER TABLE [Inventarios].[Proveedores] CHECK CONSTRAINT [FK_Proveedores_Representante]
GO
ALTER TABLE [Quejas].[Historial]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Empleados] FOREIGN KEY([ID_Empleado])
REFERENCES [Empleados].[Empleados] ([ID_Empleado])
GO
ALTER TABLE [Quejas].[Historial] CHECK CONSTRAINT [FK_Historial_Empleados]
GO
ALTER TABLE [Quejas].[Historial]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Estados] FOREIGN KEY([ID_Estado])
REFERENCES [Quejas].[Estados] ([ID_Estado])
GO
ALTER TABLE [Quejas].[Historial] CHECK CONSTRAINT [FK_Historial_Estados]
GO
ALTER TABLE [Quejas].[Historial]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Facturas] FOREIGN KEY([ID_Compra])
REFERENCES [Inventarios].[Facturas] ([ID_NroFacturas])
GO
ALTER TABLE [Quejas].[Historial] CHECK CONSTRAINT [FK_Historial_Facturas]
GO
ALTER TABLE [Quejas].[Historial]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Imagen] FOREIGN KEY([ID_Imagen])
REFERENCES [Quejas].[Imagen] ([idImagen])
GO
ALTER TABLE [Quejas].[Historial] CHECK CONSTRAINT [FK_Historial_Imagen]
GO
ALTER TABLE [Quejas].[Historial]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Quejas] FOREIGN KEY([ID_Quejas])
REFERENCES [Quejas].[Quejas] ([ID_Quejas])
GO
ALTER TABLE [Quejas].[Historial] CHECK CONSTRAINT [FK_Historial_Quejas]
GO
ALTER TABLE [Quejas].[Huespedes]  WITH CHECK ADD  CONSTRAINT [FK_Huespedes_Idioma] FOREIGN KEY([ID_Idioma])
REFERENCES [General].[Idioma] ([ID_Idioma])
GO
ALTER TABLE [Quejas].[Huespedes] CHECK CONSTRAINT [FK_Huespedes_Idioma]
GO
ALTER TABLE [Quejas].[Huespedes]  WITH CHECK ADD  CONSTRAINT [FK_Huespedes_Pais] FOREIGN KEY([ID_Pais])
REFERENCES [General].[Pais] ([ID_PaisOrigen])
GO
ALTER TABLE [Quejas].[Huespedes] CHECK CONSTRAINT [FK_Huespedes_Pais]
GO
ALTER TABLE [Quejas].[Quejas]  WITH CHECK ADD  CONSTRAINT [FK_Quejas_Departamentos] FOREIGN KEY([ID_DepartamentoRecibe])
REFERENCES [General].[Departamentos] ([ID_Departamento])
GO
ALTER TABLE [Quejas].[Quejas] CHECK CONSTRAINT [FK_Quejas_Departamentos]
GO
ALTER TABLE [Quejas].[Quejas]  WITH CHECK ADD  CONSTRAINT [FK_Quejas_Departamentos1] FOREIGN KEY([ID_DepartamentoEjecuta])
REFERENCES [General].[Departamentos] ([ID_Departamento])
GO
ALTER TABLE [Quejas].[Quejas] CHECK CONSTRAINT [FK_Quejas_Departamentos1]
GO
ALTER TABLE [Quejas].[Quejas]  WITH CHECK ADD  CONSTRAINT [FK_Quejas_Empleados] FOREIGN KEY([ID_Empleado])
REFERENCES [Empleados].[Empleados] ([ID_Empleado])
GO
ALTER TABLE [Quejas].[Quejas] CHECK CONSTRAINT [FK_Quejas_Empleados]
GO
ALTER TABLE [Quejas].[Quejas]  WITH CHECK ADD  CONSTRAINT [FK_Quejas_Habitaciones] FOREIGN KEY([ID_Habitaciones])
REFERENCES [General].[Habitaciones] ([ID_NroHab])
GO
ALTER TABLE [Quejas].[Quejas] CHECK CONSTRAINT [FK_Quejas_Habitaciones]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quien registro el movimiento del producto' , @level0type=N'SCHEMA',@level0name=N'Inventarios', @level1type=N'TABLE',@level1name=N'ProductosMovimientos', @level2type=N'COLUMN',@level2name=N'ID_Empleado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quien movio el producto' , @level0type=N'SCHEMA',@level0name=N'Inventarios', @level1type=N'TABLE',@level1name=N'ProductosMovimientos', @level2type=N'COLUMN',@level2name=N'ID_EmpleadoMov'
GO
USE [master]
GO
ALTER DATABASE [Hotel_Tools] SET  READ_WRITE 
GO
