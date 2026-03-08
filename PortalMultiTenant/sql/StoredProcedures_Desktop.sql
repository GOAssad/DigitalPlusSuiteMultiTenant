-- =============================================================================
-- Stored Procedures para compatibilidad con apps desktop (Fichador/Administrador)
-- Base de datos: DigitalPlusMultiTenant
-- Todas las SPs requieren @EmpresaId para aislamiento multi-tenant
-- =============================================================================

USE [DigitalPlusMultiTenant]
GO

-- =============================================================================
-- SP: Registrar fichada de entrada/salida
-- Determina automaticamente si es E o S segun la ultima fichada del dia
-- =============================================================================
IF OBJECT_ID('EscritorioFichadasSPSALIDA', 'P') IS NOT NULL DROP PROCEDURE EscritorioFichadasSPSALIDA
GO
CREATE PROCEDURE EscritorioFichadasSPSALIDA
    @EmpresaId   INT,
    @nSucursalID INT,
    @nLegajoID   INT,
    @dRegistro   DATETIME,
    @nTerminalId INT = NULL,
    @sOrigen     NVARCHAR(20) = NULL,
    @sAccion     VARCHAR(1) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Determinar si es Entrada o Salida:
    -- Si la ultima fichada del dia es E -> ahora es S, sino -> E
    DECLARE @ultimoTipo NVARCHAR(1);
    SELECT TOP 1 @ultimoTipo = Tipo
    FROM Fichada
    WHERE EmpresaId = @EmpresaId
      AND LegajoId = @nLegajoID
      AND CAST(FechaHora AS DATE) = CAST(@dRegistro AS DATE)
    ORDER BY FechaHora DESC;

    IF @ultimoTipo = 'E'
        SET @sAccion = 'S'
    ELSE
        SET @sAccion = 'E'

    INSERT INTO Fichada (EmpresaId, LegajoId, SucursalId, TerminalId, FechaHora, Tipo, Origen, CreatedAt)
    VALUES (@EmpresaId, @nLegajoID, @nSucursalID, @nTerminalId, @dRegistro, @sAccion, @sOrigen, GETUTCDATE());
END
GO

-- =============================================================================
-- SP: Verificar PIN de un legajo
-- Retorna datos del legajo + hash/salt del PIN
-- =============================================================================
IF OBJECT_ID('EscritorioLegajoPIN_Verificar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajoPIN_Verificar
GO
CREATE PROCEDURE EscritorioLegajoPIN_Verificar
    @EmpresaId  INT,
    @sLegajoID  NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id AS nLegajoID,
        l.NumeroLegajo AS sLegajoID,
        l.Apellido + ', ' + l.Nombre AS sLegajoNombre,
        p.PinHash,
        p.PinSalt,
        p.PinMustChange,
        p.PinChangedAt
    FROM Legajo l
    LEFT JOIN LegajoPin p ON p.LegajoId = l.Id
    WHERE l.EmpresaId = @EmpresaId
      AND l.NumeroLegajo = @sLegajoID
      AND l.IsActive = 1;
END
GO

-- =============================================================================
-- SP: Cambiar PIN de un legajo
-- =============================================================================
IF OBJECT_ID('EscritorioLegajoPIN_Cambiar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajoPIN_Cambiar
GO
CREATE PROCEDURE EscritorioLegajoPIN_Cambiar
    @EmpresaId  INT,
    @sLegajoID  NVARCHAR(25),
    @PinHash    NVARCHAR(100),
    @PinSalt    NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LegajoId INT;
    SELECT @LegajoId = Id FROM Legajo WHERE EmpresaId = @EmpresaId AND NumeroLegajo = @sLegajoID AND IsActive = 1;

    IF @LegajoId IS NULL RETURN;

    IF EXISTS (SELECT 1 FROM LegajoPin WHERE LegajoId = @LegajoId)
        UPDATE LegajoPin
        SET PinHash = @PinHash, PinSalt = @PinSalt, PinMustChange = 0, PinChangedAt = GETUTCDATE()
        WHERE LegajoId = @LegajoId;
    ELSE
        INSERT INTO LegajoPin (LegajoId, PinHash, PinSalt, PinMustChange, PinChangedAt, CreatedAt)
        VALUES (@LegajoId, @PinHash, @PinSalt, 0, GETUTCDATE(), GETUTCDATE());
END
GO

-- =============================================================================
-- SP: Lista de legajos activos (para modo demo y seleccion)
-- =============================================================================
IF OBJECT_ID('EscritorioLegajosActivos_Lista', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajosActivos_Lista
GO
CREATE PROCEDURE EscritorioLegajosActivos_Lista
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id AS nLegajoID,
        l.NumeroLegajo AS sLegajoID,
        l.Apellido + ', ' + l.Nombre AS sLegajoNombre
    FROM Legajo l
    WHERE l.EmpresaId = @EmpresaId AND l.IsActive = 1
    ORDER BY l.Apellido, l.Nombre;
END
GO

-- =============================================================================
-- SP: Actualizar huella de un legajo
-- =============================================================================
IF OBJECT_ID('EscritorioLegajosHuellasActualizar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajosHuellasActualizar
GO
CREATE PROCEDURE EscritorioLegajosHuellasActualizar
    @EmpresaId      INT,
    @LegajoId       INT,
    @DedoId         INT,
    @Huella         VARBINARY(MAX),
    @nFingerMask    INT,
    @sLegajoNombre  VARCHAR(200) = NULL,
    @sLegajoID      VARCHAR(25) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar que el legajo pertenece a la empresa
    IF NOT EXISTS (SELECT 1 FROM Legajo WHERE Id = @LegajoId AND EmpresaId = @EmpresaId)
        RETURN;

    IF EXISTS (SELECT 1 FROM LegajoHuella WHERE LegajoId = @LegajoId AND DedoId = @DedoId)
        UPDATE LegajoHuella
        SET Huella = @Huella, FingerMask = @nFingerMask
        WHERE LegajoId = @LegajoId AND DedoId = @DedoId;
    ELSE
        INSERT INTO LegajoHuella (LegajoId, DedoId, Huella, FingerMask)
        VALUES (@LegajoId, @DedoId, @Huella, @nFingerMask);
END
GO

-- =============================================================================
-- Vista: Todas las huellas con datos del legajo (filtrada por empresa via SP)
-- =============================================================================
IF OBJECT_ID('EscritorioLegajosHuellas_Lista', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajosHuellas_Lista
GO
CREATE PROCEDURE EscritorioLegajosHuellas_Lista
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.LegajoId AS nLegajoId,
        l.NumeroLegajo AS sLegajoID,
        h.DedoId AS nDedo,
        h.Huella AS iHuella,
        h.FingerMask AS nFingerMask,
        l.Apellido + ', ' + l.Nombre AS sLegajoNombre
    FROM LegajoHuella h
    INNER JOIN Legajo l ON l.Id = h.LegajoId
    WHERE l.EmpresaId = @EmpresaId AND l.IsActive = 1;
END
GO

-- =============================================================================
-- SP: Contar legajos activos (para verificacion de licencia)
-- =============================================================================
IF OBJECT_ID('EscritorioLegajosActivos_Count', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajosActivos_Count
GO
CREATE PROCEDURE EscritorioLegajosActivos_Count
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS Total FROM Legajo WHERE EmpresaId = @EmpresaId AND IsActive = 1;
END
GO

-- =============================================================================
-- SP: Crear o actualizar un legajo (ABM desde Administrador desktop)
-- =============================================================================
IF OBJECT_ID('EscritorioLegajoActualizar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajoActualizar
GO
CREATE PROCEDURE EscritorioLegajoActualizar
    @EmpresaId                INT,
    @LegajoID                 NVARCHAR(25),
    @Apellido                 NVARCHAR(100),
    @Nombre                   NVARCHAR(100),
    @SectorId                 INT,
    @CategoriaId              INT,
    @Activo                   BIT,
    @HorarioID                INT = NULL,
    @nSucursalId              INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LegId INT;
    SELECT @LegId = Id FROM Legajo WHERE NumeroLegajo = @LegajoID AND EmpresaId = @EmpresaId;

    IF @LegId IS NOT NULL
    BEGIN
        -- UPDATE existente
        UPDATE Legajo
        SET Apellido = @Apellido,
            Nombre = @Nombre,
            SectorId = @SectorId,
            CategoriaId = @CategoriaId,
            IsActive = @Activo,
            HorarioId = @HorarioID,
            UpdatedAt = GETUTCDATE()
        WHERE Id = @LegId AND EmpresaId = @EmpresaId;

        -- Actualizar sucursal (LegajoSucursal junction)
        IF @nSucursalId > 0
        BEGIN
            IF EXISTS (SELECT 1 FROM LegajoSucursal WHERE LegajoId = @LegId)
                UPDATE LegajoSucursal SET SucursalId = @nSucursalId WHERE LegajoId = @LegId;
            ELSE
                INSERT INTO LegajoSucursal (LegajoId, SucursalId) VALUES (@LegId, @nSucursalId);
        END
        ELSE
        BEGIN
            DELETE FROM LegajoSucursal WHERE LegajoId = @LegId;
        END
    END
    ELSE
    BEGIN
        -- INSERT nuevo
        INSERT INTO Legajo (EmpresaId, NumeroLegajo, Apellido, Nombre, SectorId, CategoriaId, HorarioId, IsActive, HasCalendarioPersonalizado, CreatedAt)
        VALUES (@EmpresaId, @LegajoID, @Apellido, @Nombre, @SectorId, @CategoriaId, @HorarioID, @Activo, 0, GETUTCDATE());

        SET @LegId = SCOPE_IDENTITY();

        IF @nSucursalId > 0
            INSERT INTO LegajoSucursal (LegajoId, SucursalId) VALUES (@LegId, @nSucursalId);
    END
END
GO

PRINT 'Stored Procedures para apps desktop creados exitosamente.'
GO
