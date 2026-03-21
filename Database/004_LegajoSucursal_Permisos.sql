-- ============================================================
-- 004: Agregar columnas de permisos a LegajoSucursal
-- y actualizar SP EscritorioFichadasSPSALIDA con validacion
-- ============================================================

USE DigitalPlusMultiTenant;
GO

-- 1. Agregar columnas de permisos (default true = todos habilitados)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LegajoSucursal') AND name = 'PermiteHuella')
    ALTER TABLE LegajoSucursal ADD PermiteHuella BIT NOT NULL DEFAULT 1;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LegajoSucursal') AND name = 'PermitePin')
    ALTER TABLE LegajoSucursal ADD PermitePin BIT NOT NULL DEFAULT 1;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LegajoSucursal') AND name = 'PermiteQr')
    ALTER TABLE LegajoSucursal ADD PermiteQr BIT NOT NULL DEFAULT 1;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LegajoSucursal') AND name = 'PermiteMovil')
    ALTER TABLE LegajoSucursal ADD PermiteMovil BIT NOT NULL DEFAULT 1;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LegajoSucursal') AND name = 'PermiteKiosko')
    ALTER TABLE LegajoSucursal ADD PermiteKiosko BIT NOT NULL DEFAULT 1;
GO

-- 2. Actualizar SP EscritorioFichadasSPSALIDA con validacion de permisos
IF OBJECT_ID('EscritorioFichadasSPSALIDA', 'P') IS NOT NULL DROP PROCEDURE EscritorioFichadasSPSALIDA;
GO
CREATE PROCEDURE EscritorioFichadasSPSALIDA
    @nSucursalID INT,
    @nLegajoID INT,
    @dRegistro DATETIME,
    @sAccion VARCHAR(50) OUTPUT,
    @EmpresaId INT,
    @Origen NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar permisos de fichada por sucursal
    IF @nSucursalID > 0
    BEGIN
        -- Verificar que el legajo esta asignado a esta sucursal
        IF NOT EXISTS (SELECT 1 FROM LegajoSucursal WHERE LegajoId = @nLegajoID AND SucursalId = @nSucursalID)
        BEGIN
            SET @sAccion = 'DENEGADO';
            RETURN;
        END

        -- Verificar permiso especifico por origen
        IF @Origen IS NOT NULL
        BEGIN
            DECLARE @permitido BIT = 1;

            SELECT @permitido = CASE
                WHEN @Origen = 'Huella' THEN PermiteHuella
                WHEN @Origen = 'PIN'    THEN PermitePin
                WHEN @Origen = 'QR'     THEN PermiteQr
                WHEN @Origen = 'Movil'  THEN PermiteMovil
                WHEN @Origen = 'Kiosko' THEN PermiteKiosko
                ELSE 1
            END
            FROM LegajoSucursal
            WHERE LegajoId = @nLegajoID AND SucursalId = @nSucursalID;

            IF @permitido = 0
            BEGIN
                SET @sAccion = 'DENEGADO';
                RETURN;
            END
        END
    END

    -- Determinar si es Entrada o Salida basado en la ultima fichada del dia
    DECLARE @UltimoTipo VARCHAR(1);

    SELECT TOP 1 @UltimoTipo = Tipo
    FROM Fichada WITH (UPDLOCK)
    WHERE LegajoId = @nLegajoID
      AND CAST(FechaHora AS DATE) = CAST(@dRegistro AS DATE)
      AND EmpresaId = @EmpresaId
    ORDER BY FechaHora DESC;

    IF @UltimoTipo IS NULL OR @UltimoTipo = 'S'
        SET @sAccion = 'E';
    ELSE
        SET @sAccion = 'S';

    -- Anti-duplicado: ignorar fichadas < 30 segundos
    IF EXISTS (
        SELECT 1 FROM Fichada WITH (UPDLOCK)
        WHERE LegajoId = @nLegajoID
          AND EmpresaId = @EmpresaId
          AND ABS(DATEDIFF(SECOND, FechaHora, @dRegistro)) < 30
    )
    BEGIN
        -- No insertar, devolver accion de la ultima fichada
        RETURN;
    END

    INSERT INTO Fichada (EmpresaId, LegajoId, SucursalId, FechaHora, Tipo, Origen, CreatedAt)
    VALUES (@EmpresaId, @nLegajoID, @nSucursalID, @dRegistro, @sAccion, @Origen, GETUTCDATE());
END
GO

PRINT 'LegajoSucursal permisos + SP actualizado exitosamente';
GO
