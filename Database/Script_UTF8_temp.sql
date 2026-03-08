USE [master]
GO
/****** Object:  Database [DigitalPlus]    Script Date: 28/2/2026 10:40:25 ******/
CREATE DATABASE [DigitalPlus]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DigitalPlus', FILENAME = N'/var/opt/mssql/data/DigitalPlus.mdf' , SIZE = 199936KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DigitalPlus_log', FILENAME = N'/var/opt/mssql/data/DigitalPlus_log.ldf' , SIZE = 3466816KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DigitalPlus].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DigitalPlus] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DigitalPlus] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DigitalPlus] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DigitalPlus] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DigitalPlus] SET ARITHABORT OFF 
GO
ALTER DATABASE [DigitalPlus] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [DigitalPlus] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DigitalPlus] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DigitalPlus] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DigitalPlus] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DigitalPlus] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DigitalPlus] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DigitalPlus] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DigitalPlus] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DigitalPlus] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DigitalPlus] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DigitalPlus] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DigitalPlus] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DigitalPlus] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DigitalPlus] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DigitalPlus] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [DigitalPlus] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DigitalPlus] SET RECOVERY FULL 
GO
ALTER DATABASE [DigitalPlus] SET  MULTI_USER 
GO
ALTER DATABASE [DigitalPlus] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DigitalPlus] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DigitalPlus] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DigitalPlus] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DigitalPlus] SET DELAYED_DURABILITY = DISABLED 
GO
USE [DigitalPlus]
GO
/****** Object:  User [walter]    Script Date: 28/2/2026 10:40:26 ******/
CREATE USER [walter] FOR LOGIN [walter] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [walter]
GO
/****** Object:  UserDefinedFunction [dbo].[ConvertiraFecha]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConvertiraFecha]
(
	@Fecha datetime, @Hora int, @Min int
)

returns DateTime

AS

BEGIN
	
	DECLARE @FechaReturn DateTime
		
	set @FechaReturn = (SELECT Convert(datetime, Format(@Fecha, 'd') + ' ' +convert(varchar(2),@Hora) + ':'+ convert(varchar(2),@Min)))
	return  @FechaReturn
END




GO
/****** Object:  UserDefinedFunction [dbo].[DiaDeSemana]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[DiaDeSemana]
(
	@dia int
)

returns varchar(20)

as

begin
	declare @Nombre VarChar(20)

	if(@dia = 1) set @Nombre = 'Domingo'
	if(@dia = 2) set @Nombre = 'Lunes'
	if(@dia = 3) set @Nombre = 'Martes'
	if(@dia = 4) set @Nombre = 'Miercoles'
	if(@dia = 5) set @Nombre = 'Jueves'
	if(@dia = 6) set @Nombre = 'Viernes'
	if(@dia = 7) set @Nombre = 'Sabado'
	
	return @Nombre
end

GO
/****** Object:  UserDefinedFunction [dbo].[PrimeraEntrada]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[PrimeraEntrada]
(
	@Id int, @Fecha datetime
)

returns Datetime
as 
BEGIN

DECLARE @FechaReturn DateTime

	set @FechaReturn = (select Min(Registro) from Fichadas 
	where 
	Legajoid = @id and
	EntraSale = 'E' and
	Convert(Date,Registro) = @Fecha)

	return @FechaReturn

END

GO
/****** Object:  UserDefinedFunction [dbo].[UltimaSalida]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[UltimaSalida]
(
	@Id int, @Fecha datetime
)

returns Datetime
as 
BEGIN

DECLARE @FechaReturn DateTime

	set @FechaReturn = (select Max(Registro) from Fichadas 
	where 
	Legajoid = @id and
	EntraSale = 'S' and
	Convert(Date,Registro) = @Fecha)

	return @FechaReturn

END

GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 28/2/2026 10:40:26 ******/
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
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dedos]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dedos](
	[Id] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Dedos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dias]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dias](
	[Id] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Dias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feriados]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feriados](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Feriados] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fichadas]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fichadas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SucursalId] [int] NOT NULL,
	[Legajoid] [int] NOT NULL,
	[Registro] [datetime2](7) NOT NULL,
	[EntraSale] [nvarchar](1) NOT NULL,
 CONSTRAINT [PK_Fichadas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GRALUsuarios]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GRALUsuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
 CONSTRAINT [PK_GRALUsuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Horarios]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Horarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Horarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HorariosDias]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HorariosDias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HorarioId] [int] NOT NULL,
	[DiaId] [int] NOT NULL,
	[HoraDesde] [int] NOT NULL,
	[HoraHasta] [int] NOT NULL,
	[MinutoDesde] [int] NOT NULL,
	[MinutoHasta] [int] NOT NULL,
	[Cerrado] [int] NOT NULL,
 CONSTRAINT [PK_HorariosDias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HorariosDiasEventos]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HorariosDiasEventos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FechaDesde] [datetime2](7) NOT NULL,
	[FechaHasta] [datetime2](7) NOT NULL,
	[LegajoId] [int] NOT NULL,
	[Nota] [nvarchar](max) NULL,
 CONSTRAINT [PK_HorariosDiasEventos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Incidencias]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incidencias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Color] [nvarchar](15) NULL,
	[Abreviatura] [nvarchar](4) NULL,
 CONSTRAINT [PK_Incidencias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidenciasLegajos]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidenciasLegajos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IncidenciaId] [int] NOT NULL,
	[LegajoId] [int] NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Detalle] [nvarchar](max) NULL,
 CONSTRAINT [PK_IncidenciasLegajos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Legajos]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Legajos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[LegajoId] [nvarchar](25) NOT NULL,
	[SectorId] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[CategoriaId] [int] NOT NULL,
	[HorarioId] [int] NULL,
	[CalendarioPersonalizado] [bit] NOT NULL,
 CONSTRAINT [PK_Legajos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LegajosHuellas]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegajosHuellas](
	[LegajoId] [int] NOT NULL,
	[DedoId] [int] NOT NULL,
	[sLegajoId] [nvarchar](5) NOT NULL,
	[huella] [varbinary](max) NULL,
	[nFingerMask] [int] NOT NULL,
	[sLegajoNombre] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LegajosHuellas] PRIMARY KEY CLUSTERED 
(
	[LegajoId] ASC,
	[DedoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LegajosSucursales]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegajosSucursales](
	[LegajoId] [int] NOT NULL,
	[SucursalId] [int] NOT NULL,
 CONSTRAINT [PK_LegajosSucursales] PRIMARY KEY CLUSTERED 
(
	[LegajoId] ASC,
	[SucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Noticias]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Noticias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NULL,
	[Detalle] [nvarchar](max) NULL,
	[FechaDesde] [datetime2](7) NOT NULL,
	[FechaHasta] [datetime2](7) NOT NULL,
	[Privado] [bit] NOT NULL,
 CONSTRAINT [PK_Noticias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sectores]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sectores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sectores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sucursales]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sucursales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[CodigoSucursal] [nvarchar](5) NOT NULL,
 CONSTRAINT [PK_Sucursales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Terminales]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Terminales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[SucursalId] [int] NOT NULL,
 CONSTRAINT [PK_Terminales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuariosSucursales]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuariosSucursales](
	[UsuarioId] [nvarchar](450) NOT NULL,
	[SucursalId] [int] NOT NULL,
 CONSTRAINT [PK_UsuariosSucursales] PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC,
	[SucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vacaciones]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vacaciones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LegajoId] [int] NOT NULL,
	[FechaDesde] [datetime2](7) NOT NULL,
	[FechaHasta] [datetime2](7) NOT NULL,
	[Nota] [nvarchar](max) NULL,
 CONSTRAINT [PK_Vacaciones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VariablesGlobales]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariablesGlobales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[sId] [nvarchar](450) NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Detalle] [nvarchar](max) NULL,
	[TipoValor] [nvarchar](max) NULL,
	[Valor] [nvarchar](max) NULL,
	[Reiniciar] [bit] NOT NULL,
 CONSTRAINT [PK_VariablesGlobales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[EscritorioLegajosHuellasView]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[EscritorioLegajosHuellasView]
as


select a.Id nLegajoId, a.LegajoId sLegajoID, a.Nombre as sApellido, a.Nombre as sNombre, a.Nombre as sLegajoNombre,
c.id as nSector, a.CategoriaId as nCategoria,a.Activo lActivo, e.Nombre as sHorarioID, b.DedoId as nDedo, b.huella as iHuella
from legajos a
inner join LegajosHuellas b on a.id = b.LegajoId
inner join Sectores c on a.SectorId = c.Id
inner join Categorias d on a.CategoriaId = d.id
inner join Horarios e on a.HorarioId = e.Id


GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Categorias_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Categorias_Nombre] ON [dbo].[Categorias]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Dedos_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Dedos_Nombre] ON [dbo].[Dedos]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Dias_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Dias_Nombre] ON [dbo].[Dias]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Fichadas_Legajoid_Registro]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_Fichadas_Legajoid_Registro] ON [dbo].[Fichadas]
(
	[Legajoid] ASC,
	[Registro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Fichadas_SucursalId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_Fichadas_SucursalId] ON [dbo].[Fichadas]
(
	[SucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Horarios_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Horarios_Nombre] ON [dbo].[Horarios]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HorariosDias_DiaId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_HorariosDias_DiaId] ON [dbo].[HorariosDias]
(
	[DiaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HorariosDias_HorarioId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_HorariosDias_HorarioId] ON [dbo].[HorariosDias]
(
	[HorarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HorariosDiasEventos_LegajoId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_HorariosDiasEventos_LegajoId] ON [dbo].[HorariosDiasEventos]
(
	[LegajoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Incidencias_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Incidencias_Nombre] ON [dbo].[Incidencias]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IncidenciasLegajos_IncidenciaId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_IncidenciasLegajos_IncidenciaId] ON [dbo].[IncidenciasLegajos]
(
	[IncidenciaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IncidenciasLegajos_LegajoId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_IncidenciasLegajos_LegajoId] ON [dbo].[IncidenciasLegajos]
(
	[LegajoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Legajos_CategoriaId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_Legajos_CategoriaId] ON [dbo].[Legajos]
(
	[CategoriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Legajos_HorarioId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_Legajos_HorarioId] ON [dbo].[Legajos]
(
	[HorarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Legajos_LegajoId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Legajos_LegajoId] ON [dbo].[Legajos]
(
	[LegajoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Legajos_SectorId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_Legajos_SectorId] ON [dbo].[Legajos]
(
	[SectorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LegajosHuellas_DedoId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_LegajosHuellas_DedoId] ON [dbo].[LegajosHuellas]
(
	[DedoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LegajosSucursales_SucursalId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_LegajosSucursales_SucursalId] ON [dbo].[LegajosSucursales]
(
	[SucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Sectores_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sectores_Nombre] ON [dbo].[Sectores]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Sucursales_CodigoSucursal]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sucursales_CodigoSucursal] ON [dbo].[Sucursales]
(
	[CodigoSucursal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Sucursales_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sucursales_Nombre] ON [dbo].[Sucursales]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Terminales_Nombre]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Terminales_Nombre] ON [dbo].[Terminales]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Terminales_SucursalId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_Terminales_SucursalId] ON [dbo].[Terminales]
(
	[SucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UsuariosSucursales_SucursalId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_UsuariosSucursales_SucursalId] ON [dbo].[UsuariosSucursales]
(
	[SucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vacaciones_LegajoId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE NONCLUSTERED INDEX [IX_Vacaciones_LegajoId] ON [dbo].[Vacaciones]
(
	[LegajoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_VariablesGlobales_sId]    Script Date: 28/2/2026 10:40:26 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_VariablesGlobales_sId] ON [dbo].[VariablesGlobales]
(
	[sId] ASC
)
WHERE ([sId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Fichadas]  WITH CHECK ADD  CONSTRAINT [FK_Fichadas_Legajos_Legajoid] FOREIGN KEY([Legajoid])
REFERENCES [dbo].[Legajos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Fichadas] CHECK CONSTRAINT [FK_Fichadas_Legajos_Legajoid]
GO
ALTER TABLE [dbo].[Fichadas]  WITH CHECK ADD  CONSTRAINT [FK_Fichadas_Sucursales_SucursalId] FOREIGN KEY([SucursalId])
REFERENCES [dbo].[Sucursales] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Fichadas] CHECK CONSTRAINT [FK_Fichadas_Sucursales_SucursalId]
GO
ALTER TABLE [dbo].[HorariosDias]  WITH CHECK ADD  CONSTRAINT [FK_HorariosDias_Dias_DiaId] FOREIGN KEY([DiaId])
REFERENCES [dbo].[Dias] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HorariosDias] CHECK CONSTRAINT [FK_HorariosDias_Dias_DiaId]
GO
ALTER TABLE [dbo].[HorariosDias]  WITH CHECK ADD  CONSTRAINT [FK_HorariosDias_Horarios_HorarioId] FOREIGN KEY([HorarioId])
REFERENCES [dbo].[Horarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HorariosDias] CHECK CONSTRAINT [FK_HorariosDias_Horarios_HorarioId]
GO
ALTER TABLE [dbo].[HorariosDiasEventos]  WITH CHECK ADD  CONSTRAINT [FK_HorariosDiasEventos_Legajos_LegajoId] FOREIGN KEY([LegajoId])
REFERENCES [dbo].[Legajos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HorariosDiasEventos] CHECK CONSTRAINT [FK_HorariosDiasEventos_Legajos_LegajoId]
GO
ALTER TABLE [dbo].[IncidenciasLegajos]  WITH CHECK ADD  CONSTRAINT [FK_IncidenciasLegajos_Incidencias_IncidenciaId] FOREIGN KEY([IncidenciaId])
REFERENCES [dbo].[Incidencias] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IncidenciasLegajos] CHECK CONSTRAINT [FK_IncidenciasLegajos_Incidencias_IncidenciaId]
GO
ALTER TABLE [dbo].[IncidenciasLegajos]  WITH CHECK ADD  CONSTRAINT [FK_IncidenciasLegajos_Legajos_LegajoId] FOREIGN KEY([LegajoId])
REFERENCES [dbo].[Legajos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IncidenciasLegajos] CHECK CONSTRAINT [FK_IncidenciasLegajos_Legajos_LegajoId]
GO
ALTER TABLE [dbo].[Legajos]  WITH CHECK ADD  CONSTRAINT [FK_Legajos_Categorias_CategoriaId] FOREIGN KEY([CategoriaId])
REFERENCES [dbo].[Categorias] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Legajos] CHECK CONSTRAINT [FK_Legajos_Categorias_CategoriaId]
GO
ALTER TABLE [dbo].[Legajos]  WITH CHECK ADD  CONSTRAINT [FK_Legajos_Horarios_HorarioId] FOREIGN KEY([HorarioId])
REFERENCES [dbo].[Horarios] ([Id])
GO
ALTER TABLE [dbo].[Legajos] CHECK CONSTRAINT [FK_Legajos_Horarios_HorarioId]
GO
ALTER TABLE [dbo].[Legajos]  WITH CHECK ADD  CONSTRAINT [FK_Legajos_Sectores_SectorId] FOREIGN KEY([SectorId])
REFERENCES [dbo].[Sectores] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Legajos] CHECK CONSTRAINT [FK_Legajos_Sectores_SectorId]
GO
ALTER TABLE [dbo].[LegajosHuellas]  WITH CHECK ADD  CONSTRAINT [FK_LegajosHuellas_Dedos_DedoId] FOREIGN KEY([DedoId])
REFERENCES [dbo].[Dedos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LegajosHuellas] CHECK CONSTRAINT [FK_LegajosHuellas_Dedos_DedoId]
GO
ALTER TABLE [dbo].[LegajosHuellas]  WITH CHECK ADD  CONSTRAINT [FK_LegajosHuellas_Legajos_LegajoId] FOREIGN KEY([LegajoId])
REFERENCES [dbo].[Legajos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LegajosHuellas] CHECK CONSTRAINT [FK_LegajosHuellas_Legajos_LegajoId]
GO
ALTER TABLE [dbo].[LegajosSucursales]  WITH CHECK ADD  CONSTRAINT [FK_LegajosSucursales_Legajos_LegajoId] FOREIGN KEY([LegajoId])
REFERENCES [dbo].[Legajos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LegajosSucursales] CHECK CONSTRAINT [FK_LegajosSucursales_Legajos_LegajoId]
GO
ALTER TABLE [dbo].[LegajosSucursales]  WITH CHECK ADD  CONSTRAINT [FK_LegajosSucursales_Sucursales_SucursalId] FOREIGN KEY([SucursalId])
REFERENCES [dbo].[Sucursales] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LegajosSucursales] CHECK CONSTRAINT [FK_LegajosSucursales_Sucursales_SucursalId]
GO
ALTER TABLE [dbo].[Terminales]  WITH CHECK ADD  CONSTRAINT [FK_Terminales_Sucursales_SucursalId] FOREIGN KEY([SucursalId])
REFERENCES [dbo].[Sucursales] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Terminales] CHECK CONSTRAINT [FK_Terminales_Sucursales_SucursalId]
GO
ALTER TABLE [dbo].[UsuariosSucursales]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosSucursales_AspNetUsers_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuariosSucursales] CHECK CONSTRAINT [FK_UsuariosSucursales_AspNetUsers_UsuarioId]
GO
ALTER TABLE [dbo].[UsuariosSucursales]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosSucursales_Sucursales_SucursalId] FOREIGN KEY([SucursalId])
REFERENCES [dbo].[Sucursales] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuariosSucursales] CHECK CONSTRAINT [FK_UsuariosSucursales_Sucursales_SucursalId]
GO
ALTER TABLE [dbo].[Vacaciones]  WITH CHECK ADD  CONSTRAINT [FK_Vacaciones_Legajos_LegajoId] FOREIGN KEY([LegajoId])
REFERENCES [dbo].[Legajos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Vacaciones] CHECK CONSTRAINT [FK_Vacaciones_Legajos_LegajoId]
GO
/****** Object:  StoredProcedure [dbo].[EscritorioFichadasSPSALIDA]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EscritorioFichadasSPSALIDA]
@nSucursalID int, @nLegajoID int, @dRegistro datetime,
@sAccion varchar(1) OUTPUT


 AS 

declare @sEntraSale varchar(1)



set @sEntraSale = ''
-- Busco los movimientos del dia de ese legajo
select @sEntraSale = isnull(EntraSale ,'E')
from Fichadas 
where LegajoID = @nLegajoID and 
convert(varchar(10), Registro, 103) = convert(varchar(10), getdate(),103)

if len(@sEntraSale) = 0 -- PRIMER MOVIMIENTO ES ENTRADA
BEGIN 
	SET @sAccion = 'E'
END
ELSE
BEGIN
	IF (@sEntraSale = 'E')
		BEGIN
			SET @sAccion = 'S'
		END
	ELSE
		BEGIN 
			SET @sAccion = 'E'
		END
END



Insert into Fichadas (SucursalID, LegajoID, Registro,EntraSale)
 Values (@nSucursalID, @nLegajoID, @dRegistro, @sAccion)
GO
/****** Object:  StoredProcedure [dbo].[EscritorioLegajoActualizar]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EscritorioLegajoActualizar]
@LegajoID varchar(15), @Nombre varchar(50), @SectorId int, @CategoriaId int, 
@Activo bit, @HorarioID int, @CalendarioPersonalizado bit, @nSucursalId int

AS 

declare @existe smallint
set @existe = 1  -- Insert

if Exists(Select * from Legajos where LegajoID = @LegajoID)
	set @existe = 2  -- Update

IF @existe = 1 
BEGIN
	Insert into Legajos
	(LegajoID, 
	Nombre, 
	SectorId, 
	CategoriaId,
	Activo,
	HorarioID,
	CalendarioPersonalizado)
	 Values 
	(@LegajoID, @Nombre, @SectorId, @CategoriaId, @Activo, 
	@HorarioID, @CalendarioPersonalizado)

	declare @legajo int
	set @legajo = SCOPE_IDENTITY();

	-- como estoy insertando, agrego tambien un registro en el legajossucursales para que aparezca en digital plus
	Insert Into LegajosSucursales (LegajoId, SucursalId) Values (@legajo, @nSucursalId)

END

IF @existe = 2 
BEGIN
			UPDATE Legajos set 
	Nombre = @Nombre, 
	SectorId = @SectorId,
	CategoriaId = @CategoriaId,
	Activo = @activo,
	HorarioID = @HorarioID,
	CalendarioPersonalizado = @CalendarioPersonalizado
	WHERE
	LegajoID = @LegajoID

	set @legajo = (select id from legajos where LegajoID = @LegajoID)

END





GO
/****** Object:  StoredProcedure [dbo].[EscritorioLegajosHuellasActualizar]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EscritorioLegajosHuellasActualizar]
@LegajoId int, @DedoId int, @Huella varbinary(max), @nFingerMask int, @sLegajoNombre varchar(100), @sLegajoID varchar(15)

 AS 


delete LegajosHuellas where sLegajoID = @sLegajoID and DedoId = @DedoId and nFingerMask = @nFingerMask

 Insert into LegajosHuellas
	(LegajoId, 
	DedoId, 
	Huella, 
	nFingerMask, 
	sLegajoNombre,
	sLegajoId)
	 Values 
	(@LegajoID, @DedoId, @Huella, @nFingerMask, @sLegajoNombre, @sLegajoID)


GO
/****** Object:  StoredProcedure [dbo].[RRHHLegajos_DeleteTodo]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[RRHHLegajos_DeleteTodo]
@legajo varchar(15)

as

delete LegajosHuellas where sLegajoID = @legajo
delete legajos where LegajoID = @legajo
GO
/****** Object:  StoredProcedure [dbo].[WebAusencias_Listado_General]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[WebAusencias_Listado_General]	
@fd datetime, @fh datetime, 
@leg int = 0,
@usuario varchar(max)

as


SET DATEFIRST 1

declare @script varchar(max) = ''
declare @whereLegajo varchar(100) = '';



if (@leg > 0)
	begin 
		set @whereLegajo = ' a.Id = ' + convert(nvarchar(10), @leg)  + ' and '
	end



if (@fh > getdate()) begin set @fh = DATEADD(d,1, getdate()) end



set @script = ' 
	set language spanish

	DECLARE @dim TABLE ([Fecha] Date, [nrodia] int)
    INSERT @dim([Fecha], [nrodia])

	SELECT d,  datepart(DW, d)
    FROM
    (
      SELECT
          d = DATEADD(DAY, rn - 1, ''' + Format(@fd,'dd/MM/yyyy') + ''')
      FROM
      (
          SELECT TOP (DATEDIFF(DAY, ''' + fORMAT(@fd,'dd/MM/yyy') + ''', ''' + Format(@fh,'dd/MM/yyy') + ''')) 
              rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id])
          FROM
              sys.all_objects AS s1
          CROSS JOIN
              sys.all_objects AS s2
          ORDER BY
              s1.[object_id]
      ) AS x
    ) AS y;

	select a.id, b.Fecha, a.LegajoId, a.Nombre, c.Nombre Sector
	from  @dim b
	cross join legajos a 
		inner join Sectores c on a.SectorId = c.Id
	where ' + @whereLegajo  +' not exists (
	   	 select dd.*, 
	                 cc.CodigoSucursal, dd.Registro
                    from Fichadas dd
	                inner join LegajosSucursales aa on aa.LegajoId = dd.Legajoid
	                inner join UsuariosSucursales bb on aa.SucursalId = bb.SucursalId
	                inner join sucursales cc on aa.SucursalId = cc.Id
	                inner join legajos ee on dd.Legajoid = ee.id 
	                where bb.UsuarioId  = ''' + @usuario + ''' and 
					dd.Registro between ''' + Format(@fd,'dd/MM/yyy') + ''' and ''' + Format(@fh,'dd/MM/yyy') + '''
					and ee.Activo = 1 
					and dd.LegajoId = 4 and dd.Legajoid = a.Id and FORMAT(b.Fecha,''dd/MM/yyy'')  = FORMAT(dd.registro,''dd/MM/yyy''))
		order by 2, 1'

		--print @script
		exec (@script)


		-- exec WebAusencias_Listado_General '20230325','20230331',4,'8b616a36-3ca7-4ec9-9684-e31828a65d0d'

		
GO
/****** Object:  StoredProcedure [dbo].[WebAusencias_Listado_General_ConSucursales]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- quien no ficho

CREATE procedure [dbo].[WebAusencias_Listado_General_ConSucursales]

@fd datetime = '20230601',
@fh datetime = '20230603' ,
@Su int = 0 , @le int = 0,
@usuario varchar(max) = null

as

-- exec WebAusencias_Listado_General_ConSucursales '20230601','20230603' ,18,1,'8b616a36-3ca7-4ec9-9684-e31828a65d0d'


set language spanish ; SET DATEFIRST 1

declare @script varchar(max) = '' ; 
declare @whereLegajo varchar(100) = '';
declare @whereSucursal varchar(100) = '';


if (@le > 0)
	begin 
		set @whereLegajo = ' and a.Id = ' + convert(nvarchar(5), @le)  
	end

if (@Su > 0)
	begin 
		set @whereSucursal = ' where SucursalId = ' + convert(nvarchar(5),@Su)
	end


	if (@fh > getdate()) begin set @fh = DATEADD(d,1, getdate()) end

set @script = ' 
	set language spanish

	DECLARE @dim TABLE ([Fecha] Date, [nrodia] int)
    INSERT @dim([Fecha], [nrodia])

	SELECT d,  datepart(DW, d)
    FROM
    (
      SELECT
          d = DATEADD(DAY, rn - 1, ''' + Format(@fd,'dd/MM/yyyy') + ''')
      FROM
      (
          SELECT TOP (DATEDIFF(DAY, ''' + fORMAT(@fd,'dd/MM/yyyy') + ''', ''' + Format(@fh,'dd/MM/yyyy') + ''')) 
              rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id])
          FROM
              sys.all_objects AS s1
          CROSS JOIN
              sys.all_objects AS s2
          ORDER BY
              s1.[object_id]
      ) AS x
    ) AS y; 

	--------------------------------------------------------------------------------
	 -- Todos los legajos con cada una de las fechas
	select  a.id,d.Fecha,a.LegajoId, a.Nombre, f.Nombre Sector,
	s.CodigoSucursal, s.Nombre NombreSucursal
--	b.SucursalId, b.EntraSale, b.Registro
	
	from Legajos a 
		cross join @dim as d

	--------------------------------------------------------------------------------- 


		-- Fichadas en rango de fechas seleccionado
		left join (Select * from Fichadas 
				where registro between ''' + Format(@fd,'dd/MM/yyy') + ''' and ''' + Format(@fh,'dd/MM/yyy') + ''' ) b 
				on a.Id = b.Legajoid 
				and  Format(d.Fecha,' + '''dd/MM/yyyy'''+ ') =  Format(b.Registro,' + '''dd/MM/yyyy''' + ')  
				
	-- necesito saber si los que no ficharon pertenecen a la sucursal elegida
		inner join (select b.id, a.LegajoId, b.CodigoSucursal, b.Nombre from LegajosSucursales a
			inner join Sucursales b on a.SucursalId = b.Id ' + @whereSucursal + ') s on a.id = s.LegajoId
	-------------------------------------------------------------------------------
	-- ya que estamos traigo el sector
	inner join Sectores f on a.SectorId = f.Id
	-------------------------------------------------------------------------------
	-- Sucursales del Usuario
	inner join (select * from UsuariosSucursales a where UsuarioId = ''' + @usuario + ''') as u 
			on s.id = u.SucursalId 
	--------------------------------------------------------------------------------
	where 
	-------------------------------------------------------------------------------
	-- si el registro esta en null es porque el legajo no ficho en esa fecha
	a.Activo = 1 and
	b.Registro is null '+ @whereLegajo + '
	----------------------------------------------------------------------------------
	order by 1
	-----------------------------------------------------------------------------------'
						  exec (@script)

						 -- print @script

						-- exec WebAusencias_Listado_General_ConSucursales '20230601','20230603' ,0,0,'8b616a36-3ca7-4ec9-9684-e31828a65d0d'
GO
/****** Object:  StoredProcedure [dbo].[WebCalculoMinutosMensualesCalendarioPorLegajo]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[WebCalculoMinutosMensualesCalendarioPorLegajo]
@LegajoId int
as

-- exec WebCalculoMinutosMensualesCalendarioPorLegajo 4

DECLARE @fechaInicial DATE = '2023-01-01'
DECLARE @fechaFinal DATE = DATEADD(month, 1, @fechaInicial)

--Calculo de cantidad de minutos que tiene que trabajar en una semana
-- esto es si tiene calendario fijo
select  b.DiaId, c.NombreDia, c.CantDiasenMes,
DATEDIFF(minute,
Convert(time,convert(varchar(2),b.horadesde)+':' + convert(varchar(2),b.minutoDesde)),
Convert(time,convert(varchar(2),b.HoraHasta)+':' + convert(varchar(2),b.MinutoHasta))) * c.CantDiasenMes MinutosPorCadaDia, 
b.HoraDesde, b.MinutoDesde, b.HoraHasta, b.MinutoHasta
from Legajos l
inner join  Horarios a on l.HorarioId = a.Id
inner join HorariosDias b on a.Id = b.HorarioId
inner join (SELECT .dbo.DiaDeSemana(dias.DiaSemana) NombreDia, dias.DiaSemana, Count(*) AS CantDiasenMes
FROM (
		SELECT DATEPART(weekday, fecha) AS DiaSemana
		FROM (
			SELECT DATEADD(day, number, @fechaInicial) AS fecha
			FROM master..spt_values
			WHERE type = 'P' AND DATEADD(day, number, @fechaInicial) < @fechaFinal
		) AS fechas
	) AS dias
	group by .dbo.DiaDeSemana(dias.DiaSemana), dias.DiaSemana
) as c on b.DiaId = c.DiaSemana
where l.id = @LegajoId and l.CalendarioPersonalizado = 0
UNION ALL
SELECT distinct DATEPART(weekday,b.FechaDesde) DiaId, c.NombreDia, c.CantDiasenMes,
DATEDIFF(minute, b.FechaDesde, b.FechaHasta) * c.CantDiasenMes MinutosPorCadaDia,
DATEPART(hour, b.FechaDesde) HoraDesde, DATEPART(MINUTE, b.FechaDesde) MinutoDesde,
DATEPART(hour, b.Fechahasta) HoraHasta, DATEPART(MINUTE, b.FechaHasta) MinutoHasta
from legajos l
inner join HorariosDiasEventos b on l.id = b.LegajoId
inner join (SELECT .dbo.DiaDeSemana(dias.DiaSemana) NombreDia, dias.DiaSemana, Count(*) AS CantDiasenMes
FROM (
		SELECT DATEPART(weekday, fecha) AS DiaSemana
		FROM (
			SELECT DATEADD(day, number, @fechaInicial) AS fecha
			FROM master..spt_values
			WHERE type = 'P' AND DATEADD(day, number, @fechaInicial) < @fechaFinal
		) AS fechas
	) AS dias
	group by .dbo.DiaDeSemana(dias.DiaSemana), dias.DiaSemana
) as c ON DATEPART(weekday,b.FechaDesde) = c.DiaSemana
where l.id = @LegajoId and l.CalendarioPersonalizado = 1
GO
/****** Object:  StoredProcedure [dbo].[WebConsolidado_Listado]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[WebConsolidado_Listado]
@id int, @fed DateTime, @feh DateTime 

as
-- WebConsolidado_Listado 6, '20221201', '20221231'
SET DATEFIRST 1

--set datefirst 7

if (@feh > getdate()) begin set @feh = DATEADD(d,1, getdate()) end
 
--Aquí creamos la variable tabla con los valores
DECLARE @dim TABLE ([Fecha] Date, [nrodia] int)
 
    INSERT @dim([Fecha], [nrodia])
    --SELECT d, .dbo.GetDiaSemana(dateadd(day,1,d))
	SELECT d,  datepart(DW, d)
    FROM
    (
      SELECT
          d = DATEADD(DAY, rn - 1, @fed)
      FROM
      (
          SELECT TOP (DATEDIFF(DAY, @fed, @feh)) 
              rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id])
          FROM
              sys.all_objects AS s1
          CROSS JOIN
              sys.all_objects AS s2
          ORDER BY
              s1.[object_id]
      ) AS x
    ) AS y;


select f.Fecha, a.id LegajoId,a.Nombre, s.Nombre Sucursal,
format(min(b.registro),'HH:mm') Entrada, Format(h.FechaDesde,'HH:mm') EntrCalen,
DATEDIFF(MINUTE,h.FechaDesde, min(b.Registro)) EntrDif, 
salida.Salida, salida.SalCalen, salida.SalDif

from @dim f
	left join Fichadas b on Format(f.Fecha,'d') = Format(b.Registro,'d')
	left join Legajos a on a.id = b.Legajoid
	left join Sucursales s on b.SucursalId = s.Id
	left join HorariosDiasEventos h on a.Id = h.LegajoId  --cambiar por left
		and Format(b.Registro,'d') = Format(h.FechaDesde,'d') 
	LEFT JOIN (select f.Fecha, a.id LegajoId,a.Nombre, s.Nombre Sucursal,
				format(max(b.registro),'HH:mm') Salida, Format(h.FechaHasta,'HH:mm') SalCalen,
				DATEDIFF(MINUTE,h.FechaHasta, max(b.Registro)) SalDif
				from @dim f
				left join Fichadas b on Format(f.Fecha,'d') = Format(b.Registro,'d')
				left join Legajos a on a.id = b.Legajoid
				left join Sucursales s on b.SucursalId = s.Id
				left join HorariosDiasEventos h on a.Id = h.LegajoId  --cambiar por left
					and Format(b.Registro,'d') = Format(h.FechaDesde,'d') 
				where a.id = @id and b.Registro between @fed and @feh
				and b.EntraSale = 'S'
				group by f.Fecha,  a.id, a.Nombre, s.Nombre, Format(h.FechaHasta,'HH:mm'),h.FechaHasta) salida 
				on f.Fecha = salida.Fecha and b.Legajoid = salida.LegajoId

where a.id = @id and b.Registro between @fed and @feh
and b.EntraSale = 'E'
group by f.Fecha,  a.id, a.Nombre, s.Nombre, Format(h.FechaDesde,'HH:mm'),
h.FechaDesde, salida.Salida, salida.SalCalen, salida.SalDif
-----------------------------------------------------------------------------------------
--- LOS DIAS QUE NO TIENE FICHADAS
union all
select f.Fecha, @id, a.Nombre, '' Sucursal, '' Entrada, '' EntrCalen, 0 EntrDif, '' Salida, '' SalCalen, 0
from @dim f
cross join Legajos a 
where id = @id and
 not exists (select '' from Fichadas b where Format(f.Fecha,'d') = Format(b.Registro,'d') and b.Legajoid = @id)
order by 1 desc
GO
/****** Object:  StoredProcedure [dbo].[WebControlAcceso_Listado]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[WebControlAcceso_Listado]
@id int, @fed DateTime, @feh DateTime 

-- WebControlAcceso_Listado 6, '20230201', '20230228'

as

SET DATEFIRST 1

--set datefirst 7


if (@feh > getdate()) begin set @feh = DATEADD(d,1, getdate()) end
 
--Aquí creamos la variable tabla con los valores
DECLARE @dim TABLE ([Fecha] Date, [nrodia] int)
 
    INSERT @dim([Fecha], [nrodia])
    --SELECT d, .dbo.GetDiaSemana(dateadd(day,1,d))
	SELECT d,  datepart(DW, d)
    FROM
    (
      SELECT
          d = DATEADD(DAY, rn - 1, @fed)
      FROM
      (
          SELECT TOP (DATEDIFF(DAY, @fed, @feh)) 
              rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id])
          FROM
              sys.all_objects AS s1
          CROSS JOIN
              sys.all_objects AS s2
          ORDER BY
              s1.[object_id]
      ) AS x
    ) AS y;


	-- Tipo 
	-- R = Registro
	-- I = Incidencia
	-- F = Feriados
	-- A = Ausencias
	-- N = Franco

	set language spanish

select f.Fecha, b.id, a.id LegajoId,  a.Nombre, s.Nombre Sucursal, b.EntraSale, b.Registro, 
CASE b.EntraSale
	WHEN 'E' THEN h.FechaDesde
	WHEN 'S' THEN h.FechaHasta	
end as HorarioCalendario,
CASE
	when DATEDIFF(MINUTE,h.FechaDesde, b.Registro) <= 0 and b.EntraSale = 'E' THEN 'Entro ' + LTRIM(CONVERT(VARCHAR(5), ABS(DATEDIFF(MINUTE,h.FechaDesde, b.Registro))))  + ' MIN Temprano'
	when DATEDIFF(MINUTE,h.FechaDesde, b.Registro) >  0 and b.EntraSale = 'E' THEN 'Entro ' + LTRIM(CONVERT(VARCHAR(5), ABS(DATEDIFF(MINUTE,h.FechaDesde, b.Registro))))  + ' MIN  Tarde'

	when DATEDIFF(MINUTE,h.FechaHasta, b.Registro) <= 0 and b.EntraSale = 'S' THEN 'Salio ' + LTRIM(CONVERT(VARCHAR(5), ABS(DATEDIFF(MINUTE,h.FechaHasta, b.Registro))))  + ' MIN Temprano'
	when DATEDIFF(MINUTE,h.FechaHasta, b.Registro) >  0 and b.EntraSale = 'S' THEN 'Salio ' + LTRIM(CONVERT(VARCHAR(5), ABS(DATEDIFF(MINUTE,h.FechaHasta, b.Registro))))  + ' MIN Tarde'
	when DATEPART(WEEK, b.Registro) <> isnull(DATEPART(week, h.FechaDesde),0) then 'Franco Trabajado'
--else 'Franco' 
END as Auditoria,
0 as IncidenciaId, 'R' as Tipo

from @dim f
	left join Fichadas b on Format(f.Fecha,'d') = Format(b.Registro,'d')
	left join Legajos a on a.id = b.Legajoid
	left join Sucursales s on b.SucursalId = s.Id
	left join HorariosDiasEventos h on a.Id = h.LegajoId  --cambiar por left
		and Format(b.Registro,'d') = Format(h.FechaDesde,'d') 
where a.id = @id and b.Registro between @fed and @feh and a.CalendarioPersonalizado = 1 

union all
--- Incidencias
select f.Fecha,0, a.id, a.Nombre, '' Sucursal, '' EntraSale, f.Fecha, '', c.Nombre, b.Id, 'I' as Tipo
from @dim f
inner join IncidenciasLegajos b on FORMAT(f.fecha,'d') = format(b.Fecha,'d')
inner join Incidencias c on c.id = b.IncidenciaId
inner join Legajos a on a.id = b.LegajoId and a.id = @id 

union all
-----------------------------------------------------------
-- Feriados  sirve para los de calendario fijo y personalizado ---------------
-----------------------------------------------------------
Select f.Fecha, 0, @id, '', '' Sucursal, '' EntraSale, f.Fecha, '', 'Feriado: ' + f.Nombre, 0, 'F' as Tipo
from Feriados f where Format(f.Fecha,'d') between Format(@fed,'d') and Format(@feh,'d') and
not exists (select '' from Fichadas b where Format(f.Fecha,'d') = Format(b.Registro,'d') and b.Legajoid = @id)

union all
-----------------------------------------------------------
-- Vacaciones sirve para los de calendario fijo y personalizado ---------------
-----------------------------------------------------------
Select f.Fecha, 0, @id Legajo, '', '' Sucursal, '' EntraSale, f.Fecha, '', 'Vacaciones', 0, 'V' as Tipo
from @dim f
inner join Vacaciones v on f.Fecha between v.FechaDesde and v.FechaHasta and v.LegajoId = @id
where
not exists (select '' from Fichadas b where Format(f.Fecha,'d') = Format(b.Registro,'d') and b.Legajoid = @id)

union all
----- todos los dias cruzados con los dias que no tiene fichadas
----- y ademas que no tenga un evento en horariosdiaseventos (sino es un franco)
----- y que no tenga incidencias porque estan en el union anterior
----- y que no sea feriado
select f.Fecha,0, a.id, a.Nombre, '' Sucursal, '' EntraSale, f.Fecha, '', 'Franco', 0 as IncidenciaId, 'N' as Tipo
from @dim f
cross join Legajos a 
where id = @id and a.CalendarioPersonalizado = 1 and
 not exists (select '' from Fichadas b where Format(f.Fecha,'d') = Format(b.Registro,'d') and b.Legajoid = @id) and
 not exists (select '' from incidenciasLegajos c where Format(f.Fecha,'d') = Format(c.Fecha,'d') and c.Legajoid = @id) and
 not exists (select '' from Feriados d where Format(f.Fecha,'d') = Format(d.Fecha,'d')) and 
 not exists (select '' from HorariosDiasEventos e where Format(f.Fecha, 'd') between Format(e.FechaDesde,'d') and Format(e.FechaHasta,'d') and e.legajoId = @Id) and
 not exists (select '' from Vacaciones v where Format(f.Fecha, 'd') between Format(v.FechaDesde,'d') and Format(v.FechaHasta,'d') and v.legajoId = @Id)


union all
----- Todo igual que el anterior pero esta en Horarios DiasEventos
select f.Fecha,0, a.id, a.Nombre, '' Sucursal, '' EntraSale, f.Fecha, '', 'No Vino', 0 as IncidenciaId, 'A' as Tipo
from @dim f
cross join Legajos a 
where id = @id and a.CalendarioPersonalizado = 1 and
 not exists (select '' from Fichadas b where Format(f.Fecha,'d') = Format(b.Registro,'d') and b.Legajoid = @id) and
 not exists (select '' from incidenciasLegajos c where Format(f.Fecha,'d') = Format(c.Fecha,'d') and c.Legajoid = @id) and
 not exists (select '' from Feriados d where Format(f.Fecha,'d') = Format(d.Fecha,'d')) and 
 not exists (select '' from Vacaciones v where Format(f.Fecha, 'd') between Format(v.FechaDesde,'d') and Format(v.FechaHasta,'d') and v.legajoId = @Id) and
 exists (select '' from HorariosDiasEventos e where Format(f.Fecha, 'd') between Format(e.FechaDesde,'d') and Format(e.FechaHasta,'d') and e.legajoId = @Id)

 --------- Ahora los que tienen el calendario fijo

union all
select f.Fecha, b.id, a.id LegajoId,  a.Nombre, s.Nombre Sucursal, b.EntraSale, b.Registro,
CASE b.EntraSale
	WHEN 'E' THEN .dbo.ConvertirAFecha(f.Fecha,HoraDesde,MinutoDesde) 
	WHEN 'S' THEN .dbo.ConvertirAFecha(f.Fecha,HoraHasta,MinutoHasta)
end as HorarioCalendario,
CASE
	when DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraDesde,MinutoDesde) , b.Registro) <= 0 and b.EntraSale = 'E' THEN 'Entro ' + LTRIM(CONVERT(VARCHAR(5), ABS(DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraDesde,MinutoDesde) , b.Registro))))  + ' MIN Temprano'
	when DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraDesde,MinutoDesde), b.Registro) >  0 and b.EntraSale = 'E' THEN 'Entro ' + LTRIM(CONVERT(VARCHAR(5),  ABS(DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraDesde,MinutoDesde), b.Registro))))  + ' MIN  Tarde'

	when DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraHasta,MinutoHasta) , b.Registro) <= 0 and b.EntraSale = 'S' THEN 'Salio ' + LTRIM(CONVERT(VARCHAR(5), ABS(DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraHasta,MinutoHasta) , b.Registro))))  + ' MIN Temprano'
	when DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraHasta,MinutoHasta) , b.Registro) >  0 and b.EntraSale = 'S' THEN 'Salio ' + LTRIM(CONVERT(VARCHAR(5), ABS(DATEDIFF(MINUTE,.dbo.ConvertirAFecha(f.Fecha,HoraHasta,MinutoHasta) , b.Registro))))  + ' MIN Tarde'
	when DATEPART(WEEK, b.Registro) <> isnull(DATEPART(week, Convert(datetime, convert(varchar(10),getdate(),103) + ' ' + convert(varchar(2),HoraDesde) + ':'+ convert(varchar(2),MinutoDesde)) ),0) then 'Franco Trabajado'
--else 'Franco' 
END as Auditoria, 0 as IncidenciaId, 'R' as Tipo
from @dim f
	left join Fichadas b on Format(f.Fecha,'d') = Format(b.Registro,'d')
	left join Dias d on d.id = datepart(WEEKDAY,b.registro)
	left join Legajos a on a.id = b.Legajoid
	left join Sucursales s on b.SucursalId = s.Id
	left join Horarios c on a.HorarioId = c.Id
	left join HorariosDias h on c.Id = h.HorarioId and d.Id = h.DiaId
	
where a.id = @id and b.Registro between @fed and @feh and a.CalendarioPersonalizado = 0 

union all 
----- Ausencias con calendarios fijos
select f.Fecha,0, a.id, a.Nombre, '' Sucursal, '' EntraSale, f.Fecha, '', 'No Vino', 0 as IncidenciaId, 'A' as Tipo
from @dim f
cross join Legajos a 
where id = @id and a.CalendarioPersonalizado = 0 and
 not exists (select '' from Fichadas b where Format(f.Fecha,'d') = Format(b.Registro,'d') and b.Legajoid = @id) and
 not exists (select '' from incidenciasLegajos c where Format(f.Fecha,'d') = Format(c.Fecha,'d') and c.Legajoid = @id) and
 not exists (select '' from Feriados d where Format(f.Fecha,'d') = Format(d.Fecha,'d')) and 
 not exists (select '' from Vacaciones v where Format(f.Fecha, 'd') between Format(v.FechaDesde,'d') and Format(v.FechaHasta,'d') and v.legajoId = @Id) and
 exists (select '' from Horarios u inner join HorariosDias e on u.id = e.HorarioId 
		where datepart(weekday,f.Fecha) = e.DiaId and a.HorarioId = u.Id and e.Cerrado = 0)

---- Franco con calendarios fijos
Union All
select f.Fecha,0, a.id, a.Nombre, '' Sucursal, '' EntraSale, f.Fecha, '', 'Franco', 0 as IncidenciaId, 'N' as Tipo
from @dim f
cross join Legajos a 
where id = @id and a.CalendarioPersonalizado = 0 and
 not exists (select '' from Fichadas b where Format(f.Fecha,'d') = Format(b.Registro,'d') and b.Legajoid = @id) and
 not exists (select '' from incidenciasLegajos c where Format(f.Fecha,'d') = Format(c.Fecha,'d') and c.Legajoid = @id) and
 not exists (select '' from Feriados d where Format(f.Fecha,'d') = Format(d.Fecha,'d')) and 
exists (select '' from Horarios u inner join HorariosDias e on u.id = e.HorarioId 
		where datepart(weekday,f.Fecha) = e.DiaId and a.HorarioId = u.Id and e.Cerrado = 1)

order by 1
GO
/****** Object:  StoredProcedure [dbo].[WebDashBoardMinutosTrabajadosMensualesPorLegajo]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[WebDashBoardMinutosTrabajadosMensualesPorLegajo]
@LegajoId int
-- modo 
-- TodoElMes
-- PorDia

-- WebDashBoardMinutosTrabajadosMensualesPorLegajo 418
-- WebDashBoardMinutosTrabajadosMensualesPorLegajo 418, 'TodoElMes'
as

set language spanish

Select a.LegajoId, a.Nombre, b.FechaDesde, c.FechaHasta, b.Nombre NombreSucursal,
					DATEDIFF(MINUTE, b.FechaDesde, c.FechaHasta) AS Minutos
					from Legajos a
					inner join 
					(select f1.LegajoId, Format(f1.Registro,'d') Registro , min(f1.Registro) fechaDesde, s1.Nombre 
						from fichadas f1
							inner join sucursales s1 on f1.SucursalId = s1.Id
						where month(f1.Registro) = month(getdate()) and year(f1.registro) = year(getdate()) 
						and f1.LegajoId = @LegajoId
						group by f1.LegajoId, Format(Registro,'d'), s1.Nombre )  b on a.id = b.LegajoId
					inner join 
					(select f.LegajoId, Format(f.Registro,'d') Registro , max(f.Registro) fechaHasta, s1.Nombre 
						from fichadas f
							inner join sucursales s1 on f.SucursalId = s1.Id
						where month(f.Registro) = month(getdate()) and year(f.registro) = year(getdate()) 
						and f.LegajoId = @LegajoId 
						group by f.LegajoId, Format(f.Registro,'d'), s1.Nombre ) c on a.id = c.LegajoId and Format(b.fechaDesde,'d') = Format(c.fechaHasta,'d')
					where a.Id = @LegajoId





/*
declare @query varchar(max)

if @modo = 'PorDia'

begin
	set @query = 'Select a.LegajoId, a.Nombre, b.FechaDesde, c.FechaHasta, b.Nombre NombreSucursal,
					DATEDIFF(MINUTE, b.FechaDesde, c.FechaHasta) AS Minutos
					from Legajos a
					inner join 
					(select f1.LegajoId, Format(f1.Registro,''d'') Registro , min(f1.Registro) fechaDesde, s1.Nombre 
						from fichadas f1
							inner join sucursales s1 on f1.SucursalId = s1.Id
						where month(f1.Registro) = month(getdate()) and year(f1.registro) = year(getdate()) 
						and f1.LegajoId = ' + convert(varchar(4),@LegajoId) + 
						'group by f1.LegajoId, Format(Registro,''d''), s1.Nombre )  b on a.id = b.LegajoId
					inner join 
					(select f.LegajoId, Format(f.Registro,''d'') Registro , max(f.Registro) fechaHasta, s1.Nombre 
						from fichadas f
							inner join sucursales s1 on f.SucursalId = s1.Id
						where month(f.Registro) = month(getdate()) and year(f.registro) = year(getdate()) 
						and f.LegajoId = ' + convert(varchar(4),@LegajoId) + ' 
						group by f.LegajoId, Format(f.Registro,''d''), s1.Nombre ) c on a.id = c.LegajoId and Format(b.fechaDesde,''d'') = Format(c.fechaHasta,''d'')
					where a.Id = ' + convert(varchar(4),@LegajoId)

	
end

if @modo = 'TodoElMes'
begin
	set @query = 'Select sum(DATEDIFF(MINUTE, b.FechaDesde, c.FechaHasta)) AS Minutos
					from Legajos a
					inner join 
					(select LegajoId, Format(Registro,''d'') Registro , min(Registro) fechaDesde 
						from fichadas 
						where month(Registro) = month(getdate()) and year(registro) = year(getdate()) 
						and LegajoId = ' + convert(varchar(4),@LegajoId) + 
						'group by LegajoId, Format(Registro,''d''))  b on a.id = b.LegajoId
					inner join 
					(select LegajoId, Format(Registro,''d'') Registro , max(Registro) fechaHasta 
						from fichadas 
						where month(Registro) = month(getdate()) and year(registro) = year(getdate()) 
						and LegajoId = ' + convert(varchar(4),@LegajoId) + ' 
						group by LegajoId, Format(Registro,''d'')) c on a.id = c.LegajoId and Format(b.fechaDesde,''d'') = Format(c.fechaHasta,''d'')
					where a.Id = ' + convert(varchar(4),@LegajoId)
end


execute (@query)

*/
GO
/****** Object:  StoredProcedure [dbo].[WebDashBoardMinutosTrabajadosMensualesPorSucursal]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[WebDashBoardMinutosTrabajadosMensualesPorSucursal]
@SucursalId int, @modo varchar(20)

-- modo 
-- TodoElMes
-- PorDia

-- WebDashBoardMinutosTrabajadosMensualesPorSucursal 43, 'PorDia'
-- WebDashBoardMinutosTrabajadosMensualesPorSucursal 43, 'TodoElMes'
as

set language spanish

declare @query varchar(max)

if @modo = 'PorDia'

begin
	set @query = 'Select a.CodigoSucursal, a.Nombre, b.FechaDesde, c.FechaHasta,
					DATEDIFF(MINUTE, b.FechaDesde, c.FechaHasta) AS Minutos
					from Sucursales a
					inner join 
					(select SucursalId, Format(Registro,''d'') Registro , min(Registro) fechaDesde 
						from fichadas 
						where month(Registro) = month(getdate()) and year(registro) = year(getdate()) 
						and SucursalId = ' + convert(varchar(4),@SucursalId) + 
						'group by SucursalId, Format(Registro,''d''))  b on a.id = b.SucursalId
					inner join 
					(select SucursalId, Format(Registro,''d'') Registro , max(Registro) fechaHasta 
						from fichadas 
						where month(Registro) = month(getdate()) and year(registro) = year(getdate()) 
						and SucursalId = ' + convert(varchar(4),@SucursalId) + ' 
						group by SucursalId, Format(Registro,''d'')) c on a.id = c.Sucursalid and Format(b.fechaDesde,''d'') = Format(c.fechaHasta,''d'')
					where a.Id = ' + convert(varchar(4),@SucursalId)

	
end

if @modo = 'TodoElMes'
begin
	set @query = 'Select sum(DATEDIFF(MINUTE, b.FechaDesde, c.FechaHasta)) AS Minutos
					from Sucursales a
					inner join 
					(select SucursalId, Format(Registro,''d'') Registro , min(Registro) fechaDesde 
						from fichadas 
						where month(Registro) = month(getdate()) and year(registro) = year(getdate()) 
						and SucursalId = ' + convert(varchar(4),@SucursalId) + 
						'group by SucursalId, Format(Registro,''d''))  b on a.id = b.SucursalId
					inner join 
					(select SucursalId, Format(Registro,''d'') Registro , max(Registro) fechaHasta 
						from fichadas 
						where month(Registro) = month(getdate()) and year(registro) = year(getdate()) 
						and SucursalId = ' + convert(varchar(4),@SucursalId) + ' 
						group by SucursalId, Format(Registro,''d'')) c on a.id = c.Sucursalid and Format(b.fechaDesde,''d'') = Format(c.fechaHasta,''d'')
					where a.Id = ' + convert(varchar(4),@SucursalId)
end


execute (@query)
GO
/****** Object:  StoredProcedure [dbo].[WebHorasExtras_Listado]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[WebHorasExtras_Listado]
@id int, @fed DateTime, @feh DateTime 

-- WebHorasExtras_Listado 6, '20230101', '20231231'

as

SET DATEFIRST 1

--set datefirst 7


if (@feh > getdate()) begin set @feh = DATEADD(d,1, getdate()) end
 

select b.Registro, b.Id, a.id LegajoId,  a.Nombre, s.Nombre Sucursal, b.EntraSale, b.Registro, 
CASE b.EntraSale
	WHEN 'E' THEN h.FechaDesde
	WHEN 'S' THEN h.FechaHasta	
end as HorarioCalendario,
CASE
	when DATEDIFF(MINUTE,h.FechaDesde, b.Registro) <  0 and b.EntraSale = 'E' THEN  ABS(DATEDIFF(MINUTE,h.FechaDesde, b.Registro))
else 0 
END as MinEntTar,
CASE
	when DATEDIFF(MINUTE,h.FechaHasta, b.Registro) >= 0 and b.EntraSale = 'S' THEN  ABS(DATEDIFF(MINUTE,h.FechaHasta, b.Registro))
else 0 
END as MinSalTem

from Fichadas b 
	left join Legajos a on a.id = b.Legajoid
	left join Sucursales s on b.SucursalId = s.Id
	left join HorariosDiasEventos h on a.Id = h.LegajoId  --cambiar por left
		and Format(b.Registro,'d') = Format(h.FechaDesde,'d') 
where a.id = @id and b.Registro between @fed and @feh and
 ((DATEDIFF(MINUTE,h.FechaDesde, b.Registro) >  0 and b.EntraSale = 'E')  or (DATEDIFF(MINUTE,h.FechaHasta, b.Registro) <= 0 and b.EntraSale = 'S'))

order by 1
GO
/****** Object:  StoredProcedure [dbo].[WebLlegadaTarde_Listado]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[WebLlegadaTarde_Listado]
@id int, @fed DateTime, @feh DateTime 

-- WebLlegadaTarde_Listado 6, '20230101', '20231231'

as

SET DATEFIRST 1

--set datefirst 7


if (@feh > getdate()) begin set @feh = DATEADD(d,1, getdate()) end
 

select b.Registro, b.Id, a.id LegajoId,  a.Nombre, s.Nombre Sucursal, b.EntraSale, b.Registro, 
CASE b.EntraSale
	WHEN 'E' THEN h.FechaDesde
	WHEN 'S' THEN h.FechaHasta	
end as HorarioCalendario,
CASE
	when DATEDIFF(MINUTE,h.FechaDesde, b.Registro) >  0 and b.EntraSale = 'E' THEN  ABS(DATEDIFF(MINUTE,h.FechaDesde, b.Registro))
else 0 
END as MinEntTar,
CASE
	when DATEDIFF(MINUTE,h.FechaHasta, b.Registro) <= 0 and b.EntraSale = 'S' THEN  ABS(DATEDIFF(MINUTE,h.FechaHasta, b.Registro))
else 0 
END as MinSalTem

from Fichadas b 
	left join Legajos a on a.id = b.Legajoid
	left join Sucursales s on b.SucursalId = s.Id
	left join HorariosDiasEventos h on a.Id = h.LegajoId  --cambiar por left
		and Format(b.Registro,'d') = Format(h.FechaDesde,'d') 
where a.id = @id and b.Registro between @fed and @feh and
 ((DATEDIFF(MINUTE,h.FechaDesde, b.Registro) >  0 and b.EntraSale = 'E')  or (DATEDIFF(MINUTE,h.FechaHasta, b.Registro) <= 0 and b.EntraSale = 'S'))

order by 1
GO
/****** Object:  StoredProcedure [dbo].[WebLlegadaTarde_Listado_General]    Script Date: 28/2/2026 10:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[WebLlegadaTarde_Listado_General]
@fed DateTime,  @feh DateTime , 
@suc int = 0, @leg int = 0, @usuarioId nvarchar(max) 
--@suc int = 0, @leg int = 0, @usuarioId nvarchar(max) = '54a6fdac-b884-46a5-b49f-69dbd67bc674'



-- WebLlegadaTarde_Listado_General '01/03/2023','14/03/2023',0,0,'54a6fdac-b884-46a5-b49f-69dbd67bc674'
-- WebLlegadaTarde_Listado_General '20230301','20230313', 0, 0, '54a6fdac-b884-46a5-b49f-69dbd67bc674'

as
SET DATEFORMAT dmy

declare @whereSucursal varchar(100) = '';
declare @whereLegajo varchar(100) = '';


if (@suc > 0) 
	begin
		set @whereSucursal = ' and b.SucursalId = ' + convert(nvarchar(10),@suc) 
	end

if (@leg > 0)
	begin 
		set @whereLegajo = ' and b.LegajoId = ' + convert(nvarchar(10), @leg) 
	end

declare @scriptCalendarioConvencional nvarchar(max) = ''
declare @scriptCalendarioPersonalizado nvarchar(max) = ''
declare @script varchar(max) = ''




-- Select 1 -  select Calendario Fijo 'Entradas'
-- Select 2 -  select Calendario Fijo 'Salidas'
-- Select 3 -  select calendario personalizado entradas y salidas
set @scriptCalendarioConvencional  = '
set language spanish
Select  b.*, s.Nombre NombreSucursal, a.Nombre NombreLegajo, s.CodigoSucursal, 
.dbo.ConvertiraFecha(b.Registro, i.HoraDesde, i.MinutoDesde) HorarioCalendario,
CASE
	when DATEDIFF(MINUTE,.dbo.ConvertiraFecha(b.Registro, i.HoraDesde, i.MinutoDesde), b.Registro) >  0 and 
		b.EntraSale = ''E'' THEN  ABS(DATEDIFF(MINUTE,.dbo.ConvertiraFecha(b.Registro, i.HoraDesde, i.MinutoDesde), b.Registro))
else 0 
END as MinEntTar, 0 MinSaleTem
from fichadas b
inner join legajos a on b.Legajoid = a.Id
inner join Sucursales s on b.SucursalId = s.Id
inner join horarios h on a.HorarioId = h.id
inner join HorariosDias i on h.id = i.HorarioId and DATEPART(weekday, b.Registro)  = i.DiaId
	inner join (-- Primera Entrada de un legajo
		select b.legajoId,convert(Date,Registro) Fecha, Min(b.Registro) FechaEntrada
		from fichadas b
		where 
		b.Registro between ''' + format(@fed,'dd/MM/yyyy') + ''' and ''' + format(@feh,'dd/MM/yyyy') + ''' and b.EntraSale = ''E''
		group by b.Legajoid, convert(Date,Registro)) as ed on 
				b.LegajoId = ed.Legajoid and
				convert(Date,b.registro) = ed.Fecha and
				DATEPART(Hour, b.registro) = datepart(hour, ed.FechaEntrada) and
				DATEPART(minute, b.registro) = datepart(minute, ed.FechaEntrada) and
				datepart(second, b.Registro) = datepart(second, ed.FechaEntrada)  
				inner join UsuariosSucursales u on b.SucursalId = u.SucursalId and u.UsuarioId =  ''' + @usuarioId + '''
	where
				b.EntraSale = ''E'' and a.CalendarioPersonalizado = 0 and 
				DATEDIFF(MINUTE,.dbo.ConvertiraFecha(b.Registro, i.HoraDesde, i.MinutoDesde), b.Registro) >  0 '
				+  @whereSucursal +  @whereLegajo +
' union all
Select   b.*, s.Nombre NombreSucursal, a.Nombre NombreLegajo, s.CodigoSucursal, 
.dbo.ConvertiraFecha(b.Registro, i.HoraHasta, i.MinutoHasta) HorarioCalendario,
0,
CASE
	when DATEDIFF(MINUTE,.dbo.ConvertiraFecha(b.Registro, i.HoraHasta, i.MinutoHasta), b.Registro) <= 0 and
		b.EntraSale = ''S'' THEN  ABS(DATEDIFF(MINUTE,.dbo.ConvertiraFecha(b.Registro, i.HoraHasta, i.MinutoHasta), b.Registro))
else 0 
END as MinSalTem
from fichadas b
inner join legajos a on b.Legajoid = a.Id
inner join Sucursales s on b.SucursalId = s.Id
inner join horarios h on a.HorarioId = h.id
inner join HorariosDias i on h.id = i.HorarioId and DATEPART(weekday, b.Registro)  = i.DiaId
	inner join (-- Primera Entrada de un legajo
		select b.legajoId,convert(Date,Registro) Fecha, Max(b.Registro) FechaSalida
		from fichadas b
		where 
		b.Registro between ''' + format(@fed,'dd/MM/yyyy') + ''' and ''' + format(@feh,'dd/MM/yyyy') + ''' and b.EntraSale = ''S''
		group by b.Legajoid, convert(Date,Registro)) as ed on 
				b.LegajoId = ed.Legajoid and
				convert(Date,b.registro) = ed.Fecha and
				DATEPART(Hour, b.registro) = datepart(hour, ed.FechaSalida) and
				DATEPART(minute, b.registro) = datepart(minute, ed.FechaSalida) and
				datepart(second, b.Registro) = datepart(second, ed.FechaSalida) 
		inner join UsuariosSucursales u on b.SucursalId = u.SucursalId and u.UsuarioId =  ''' + @usuarioId + '''
	where 
	b.EntraSale = ''S'' AND  CalendarioPersonalizado = 0 and
	DATEDIFF(MINUTE,.dbo.ConvertiraFecha(b.Registro, i.HoraHasta, i.MinutoHasta), b.Registro) <= 0 '
	+  @whereSucursal +  @whereLegajo 


set @scriptCalendarioPersonalizado = ' Union all 
	select b.Id, b.SucursalId, a.id LegajoId, b.Registro, b.EntraSale, s.Nombre as NombreSucursal,
	a.Nombre NombreLegajo, s.CodigoSucursal,
	CASE b.EntraSale
	WHEN ''E'' THEN h.FechaDesde
	WHEN ''S'' THEN h.FechaHasta	
	end as HorarioCalendario,
	CASE
	when DATEDIFF(MINUTE,h.FechaDesde, b.Registro) >  0 and b.EntraSale = ''E'' THEN  ABS(DATEDIFF(MINUTE,h.FechaDesde, b.Registro))
	else 0 
	END as MinEntTar,
	CASE
	when DATEDIFF(MINUTE,h.FechaHasta, b.Registro) <= 0 and b.EntraSale = ''S'' THEN  ABS(DATEDIFF(MINUTE,h.FechaHasta, b.Registro))
	else 0 
	END as MinSalTem
	from Fichadas b 
	left join Legajos a on a.id = b.Legajoid
	left join Sucursales s on b.SucursalId = s.Id
	left join HorariosDiasEventos h on a.Id = h.LegajoId  --cambiar por left
		and Format(b.Registro,''dd/MM/yyyy'') = Format(h.FechaDesde,''dd/MM/yyyy'') 
	inner join UsuariosSucursales u on b.SucursalId = u.SucursalId and u.UsuarioId =  ''' + @usuarioId + '''
	where 
	b.Registro between ''' + format(@fed,'dd/MM/yyyy') + ''' and ''' + format(@feh,'dd/MM/yyyy') + ''' and 
	 ((DATEDIFF(MINUTE,h.FechaDesde, b.Registro) >  0 and b.EntraSale = ''E'')  or
	 (DATEDIFF(MINUTE,h.FechaHasta, b.Registro) <= 0 and b.EntraSale = ''S'')) '
	 +  @whereSucursal +  @whereLegajo +
	' order by 3,4 '

set @script = @scriptCalendarioConvencional + @scriptCalendarioPersonalizado
--print @script


exec (@script)
GO
USE [master]
GO
ALTER DATABASE [DigitalPlus] SET  READ_WRITE 
GO
