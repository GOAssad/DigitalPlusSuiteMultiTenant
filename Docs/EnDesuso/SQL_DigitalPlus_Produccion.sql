-- ============================================================
-- Script de cambios en base de datos DigitalPlus
-- Para ejecutar en produccion (Ferozo: sd-1985882-l.ferozo.com,11434)
-- Fecha: 2026-03-01
-- ============================================================

-- 1. Agregar columna Foto a la tabla Legajos
-- (ignorar si ya existe)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('Legajos') AND name = 'Foto'
)
    ALTER TABLE Legajos ADD Foto varbinary(max) NULL
GO

-- 2. Actualizar SP EscritorioLegajoActualizar para guardar y borrar la foto
ALTER PROCEDURE [dbo].[EscritorioLegajoActualizar]
@LegajoID varchar(15), @Nombre varchar(50), @SectorId int, @CategoriaId int,
@Activo bit, @HorarioID int, @CalendarioPersonalizado bit, @nSucursalId int,
@Foto varbinary(max) = NULL

AS

declare @existe smallint
set @existe = 1  -- Insert

if Exists(Select * from Legajos where LegajoID = @LegajoID)
    set @existe = 2  -- Update

IF @existe = 1
BEGIN
    Insert into Legajos
    (LegajoID, Nombre, SectorId, CategoriaId, Activo, HorarioID, CalendarioPersonalizado, Foto)
    Values
    (@LegajoID, @Nombre, @SectorId, @CategoriaId, @Activo, @HorarioID, @CalendarioPersonalizado, @Foto)

    declare @legajo int
    set @legajo = SCOPE_IDENTITY();

    -- Insertar en LegajosSucursales para que aparezca en DigitalPlus
    Insert Into LegajosSucursales (LegajoId, SucursalId) Values (@legajo, @nSucursalId)
END

IF @existe = 2
BEGIN
    UPDATE Legajos SET
        Nombre                 = @Nombre,
        SectorId               = @SectorId,
        CategoriaId            = @CategoriaId,
        Activo                 = @Activo,
        HorarioID              = @HorarioID,
        CalendarioPersonalizado = @CalendarioPersonalizado,
        Foto                   = @Foto
    WHERE LegajoID = @LegajoID

    set @legajo = (select id from legajos where LegajoID = @LegajoID)
END
GO
