-- ============================================================
-- 021 - Soporte para fichada por PIN
-- Agrega columnas PIN a Legajos y variables globales de config
-- ============================================================

-- 1. Columnas PIN en Legajos
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Legajos') AND name = 'PinHash')
BEGIN
    ALTER TABLE [dbo].[Legajos] ADD
        [PinHash] [nvarchar](128) NULL,
        [PinSalt] [nvarchar](64) NULL,
        [PinChangedAt] [datetime2](7) NULL,
        [PinMustChange] [bit] NOT NULL DEFAULT 0;
END
GO

-- 2. Variables globales para configuracion PIN
IF NOT EXISTS (SELECT 1 FROM VariablesGlobales WHERE sId = 'FichadaModoPIN')
    INSERT INTO VariablesGlobales (sId, Nombre, Detalle, TipoValor, Valor, Reiniciar)
    VALUES ('FichadaModoPIN', 'Habilitar fichada por PIN', 'Permite fichar ingresando legajo y PIN numerico', 'Fichada', 'false', 0);

IF NOT EXISTS (SELECT 1 FROM VariablesGlobales WHERE sId = 'PinExpiraDias')
    INSERT INTO VariablesGlobales (sId, Nombre, Detalle, TipoValor, Valor, Reiniciar)
    VALUES ('PinExpiraDias', 'Dias de expiracion de PIN', 'Cantidad de dias antes de obligar cambio de PIN (0 = no expira)', 'Fichada', '90', 0);

IF NOT EXISTS (SELECT 1 FROM VariablesGlobales WHERE sId = 'FichadaModoDemo')
    INSERT INTO VariablesGlobales (sId, Nombre, Detalle, TipoValor, Valor, Reiniciar)
    VALUES ('FichadaModoDemo', 'Habilitar modo demo', 'Permite fichar seleccionando legajo de una lista (solo para evaluacion)', 'Fichada', 'false', 0);
GO

-- 3. SP: Verificar PIN de un legajo
IF EXISTS (SELECT 1 FROM sys.objects WHERE name = 'EscritorioLegajoPIN_Verificar' AND type = 'P')
    DROP PROCEDURE EscritorioLegajoPIN_Verificar;
GO

CREATE PROCEDURE [dbo].[EscritorioLegajoPIN_Verificar]
    @sLegajoID NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.Id AS nLegajoID, l.LegajoId AS sLegajoID, l.Nombre AS sLegajoNombre,
           l.PinHash, l.PinSalt, l.PinChangedAt, l.PinMustChange
    FROM Legajos l
    WHERE l.LegajoId = @sLegajoID AND l.Activo = 1;
END
GO

-- 4. SP: Cambiar PIN de un legajo
IF EXISTS (SELECT 1 FROM sys.objects WHERE name = 'EscritorioLegajoPIN_Cambiar' AND type = 'P')
    DROP PROCEDURE EscritorioLegajoPIN_Cambiar;
GO

CREATE PROCEDURE [dbo].[EscritorioLegajoPIN_Cambiar]
    @sLegajoID NVARCHAR(25),
    @PinHash NVARCHAR(128),
    @PinSalt NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Legajos
    SET PinHash = @PinHash,
        PinSalt = @PinSalt,
        PinChangedAt = GETUTCDATE(),
        PinMustChange = 0
    WHERE LegajoId = @sLegajoID;
END
GO

-- 5. SP: Listar legajos activos (para modo demo)
IF EXISTS (SELECT 1 FROM sys.objects WHERE name = 'EscritorioLegajosActivos_Lista' AND type = 'P')
    DROP PROCEDURE EscritorioLegajosActivos_Lista;
GO

CREATE PROCEDURE [dbo].[EscritorioLegajosActivos_Lista]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.Id AS nLegajoID, l.LegajoId AS sLegajoID, l.Nombre AS sLegajoNombre
    FROM Legajos l
    WHERE l.Activo = 1
    ORDER BY l.Nombre;
END
GO

-- 6. SP: Forzar cambio de PIN (para admin)
IF EXISTS (SELECT 1 FROM sys.objects WHERE name = 'EscritorioLegajoPIN_ForzarCambio' AND type = 'P')
    DROP PROCEDURE EscritorioLegajoPIN_ForzarCambio;
GO

CREATE PROCEDURE [dbo].[EscritorioLegajoPIN_ForzarCambio]
    @sLegajoID NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Legajos SET PinMustChange = 1 WHERE LegajoId = @sLegajoID;
END
GO
