-- ============================================================
-- Script: 003_TerminalMovil_Geoconfig.sql
-- Descripcion: Tablas para Terminal Movil (v2.0)
-- Fecha: 2026-03-12
-- NOTA: Este script es de referencia. Las tablas se crean
--       via migracion EF Core (dotnet ef database update).
-- ============================================================

-- TerminalMovil: smartphone registrado y autorizado para un empleado
CREATE TABLE TerminalMovil (
    Id               INT IDENTITY PRIMARY KEY,
    EmpresaId        INT NOT NULL,
    LegajoId         INT NOT NULL,
    DeviceId         NVARCHAR(200) NOT NULL,
    PublicKey        NVARCHAR(MAX) NOT NULL,
    Nombre           NVARCHAR(100) NULL,
    Plataforma       NVARCHAR(20) NULL,
    FechaRegistro    DATETIME2 NOT NULL DEFAULT GETDATE(),
    UltimoUso        DATETIME2 NULL,
    Activo           BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_TerminalMovil_Legajo FOREIGN KEY (LegajoId) REFERENCES Legajo(Id)
);
CREATE INDEX IX_TerminalMovil_DeviceId ON TerminalMovil(DeviceId);
CREATE INDEX IX_TerminalMovil_EmpresaId ON TerminalMovil(EmpresaId, LegajoId);

-- SucursalGeoconfig: validacion de presencia fisica por sucursal
CREATE TABLE SucursalGeoconfig (
    Id                 INT IDENTITY PRIMARY KEY,
    SucursalId         INT NOT NULL,
    EmpresaId          INT NOT NULL,
    WifiBSSID          NVARCHAR(50) NULL,
    WifiSSID           NVARCHAR(100) NULL,
    Latitud            DECIMAL(10,7) NULL,
    Longitud           DECIMAL(10,7) NULL,
    RadioMetros        INT NOT NULL DEFAULT 100,
    MetodoValidacion   NVARCHAR(20) NOT NULL DEFAULT 'WifiOGPS',
    Activo             BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_SucursalGeoconfig_Sucursal FOREIGN KEY (SucursalId) REFERENCES Sucursal(Id)
);

-- CodigoActivacionMovil: codigos de uso unico para vincular dispositivo a empleado
CREATE TABLE CodigoActivacionMovil (
    Id             INT IDENTITY PRIMARY KEY,
    EmpresaId      INT NOT NULL,
    LegajoId       INT NOT NULL,
    Codigo         NVARCHAR(10) NOT NULL,
    FechaCreacion  DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaExpira    DATETIME2 NOT NULL,
    Usado          BIT NOT NULL DEFAULT 0,
    UsadoEn        DATETIME2 NULL,
    DeviceId       NVARCHAR(200) NULL
);
CREATE UNIQUE INDEX IX_CodigoActivacionMovil_Codigo ON CodigoActivacionMovil(Codigo) WHERE Usado = 0;
