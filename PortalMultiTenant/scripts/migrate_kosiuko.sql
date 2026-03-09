-- =============================================================================
-- Migracion de datos Kosiuko: DigitalPlus -> DigitalPlusMultiTenant
-- EmpresaId = 2 (Kosiuko S.A.)
-- =============================================================================

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;

USE DigitalPlusMultiTenant;

DECLARE @EmpresaId INT = 2;
DECLARE @Now DATETIME2 = GETUTCDATE();

-- Habilitar IDENTITY_INSERT donde sea necesario para preservar IDs

BEGIN TRY
BEGIN TRANSACTION;

-- =============================================================================
-- 1. SUCURSALES
-- =============================================================================
PRINT '>> Migrando Sucursales...';

SET IDENTITY_INSERT Sucursal ON;

INSERT INTO Sucursal (Id, EmpresaId, Codigo, Nombre, IsActive, CreatedAt, CreatedBy)
SELECT
    s.Id,
    @EmpresaId,
    ISNULL(s.CodigoSucursal, CAST(s.Id AS NVARCHAR(10))),
    s.Nombre,
    1, -- IsActive
    @Now,
    'migration'
FROM DigitalPlus.dbo.Sucursales s
WHERE NOT EXISTS (SELECT 1 FROM Sucursal t WHERE t.Id = s.Id AND t.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Sucursal OFF;

PRINT '   Sucursales migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 2. SECTORES
-- =============================================================================
PRINT '>> Migrando Sectores...';

SET IDENTITY_INSERT Sector ON;

INSERT INTO Sector (Id, EmpresaId, Nombre, IsActive, CreatedAt, CreatedBy)
SELECT
    s.Id,
    @EmpresaId,
    s.Nombre,
    1,
    @Now,
    'migration'
FROM DigitalPlus.dbo.Sectores s
WHERE NOT EXISTS (SELECT 1 FROM Sector t WHERE t.Id = s.Id AND t.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Sector OFF;

PRINT '   Sectores migrados: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 3. CATEGORIAS
-- =============================================================================
PRINT '>> Migrando Categorias...';

SET IDENTITY_INSERT Categoria ON;

INSERT INTO Categoria (Id, EmpresaId, Nombre, IsActive, CreatedAt, CreatedBy)
SELECT
    c.Id,
    @EmpresaId,
    c.Nombre,
    1,
    @Now,
    'migration'
FROM DigitalPlus.dbo.Categorias c
WHERE NOT EXISTS (SELECT 1 FROM Categoria t WHERE t.Id = c.Id AND t.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Categoria OFF;

PRINT '   Categorias migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 4. HORARIOS
-- =============================================================================
PRINT '>> Migrando Horarios...';

SET IDENTITY_INSERT Horario ON;

INSERT INTO Horario (Id, EmpresaId, Nombre, IsActive, CreatedAt, CreatedBy)
SELECT
    h.Id,
    @EmpresaId,
    h.Nombre,
    1,
    @Now,
    'migration'
FROM DigitalPlus.dbo.Horarios h
WHERE NOT EXISTS (SELECT 1 FROM Horario t WHERE t.Id = h.Id AND t.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Horario OFF;

PRINT '   Horarios migrados: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 5. HORARIO DETALLES (HorariosDias -> HorarioDetalle)
-- =============================================================================
PRINT '>> Migrando HorarioDetalles...';

SET IDENTITY_INSERT HorarioDetalle ON;

INSERT INTO HorarioDetalle (Id, HorarioId, DiaSemana, HoraDesde, HoraHasta, IsCerrado)
SELECT
    hd.Id,
    hd.HorarioId,
    hd.DiaId, -- DiaSemana enum: 1=Domingo..7=Sabado
    CAST(DATEADD(MINUTE, hd.MinutoDesde, DATEADD(HOUR, hd.HoraDesde, CAST('00:00' AS TIME))) AS TIME),
    CAST(DATEADD(MINUTE, hd.MinutoHasta, DATEADD(HOUR, hd.HoraHasta, CAST('00:00' AS TIME))) AS TIME),
    CASE WHEN hd.Cerrado = 1 THEN 1 ELSE 0 END
FROM DigitalPlus.dbo.HorariosDias hd
WHERE EXISTS (SELECT 1 FROM Horario h WHERE h.Id = hd.HorarioId)
  AND NOT EXISTS (SELECT 1 FROM HorarioDetalle t WHERE t.Id = hd.Id);

SET IDENTITY_INSERT HorarioDetalle OFF;

PRINT '   HorarioDetalles migrados: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 6. TERMINALES
-- =============================================================================
PRINT '>> Migrando Terminales...';

SET IDENTITY_INSERT Terminal ON;

INSERT INTO Terminal (Id, EmpresaId, Nombre, Descripcion, SucursalId, IsActive, CreatedAt, CreatedBy)
SELECT
    t.Id,
    @EmpresaId,
    t.Nombre,
    t.Descripcion,
    t.SucursalId,
    1,
    @Now,
    'migration'
FROM DigitalPlus.dbo.Terminales t
WHERE EXISTS (SELECT 1 FROM Sucursal s WHERE s.Id = t.SucursalId)
  AND NOT EXISTS (SELECT 1 FROM Terminal nt WHERE nt.Id = t.Id AND nt.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Terminal OFF;

PRINT '   Terminales migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 7. LEGAJOS (split Nombre -> Apellido + Nombre)
-- =============================================================================
PRINT '>> Migrando Legajos...';

SET IDENTITY_INSERT Legajo ON;

INSERT INTO Legajo (Id, EmpresaId, NumeroLegajo, Apellido, Nombre, SectorId, CategoriaId, HorarioId,
                    IsActive, HasCalendarioPersonalizado, Foto, CreatedAt, CreatedBy)
SELECT
    l.Id,
    @EmpresaId,
    l.LegajoId, -- NumeroLegajo
    -- Split "Apellido, Nombre" por coma
    LTRIM(RTRIM(
        CASE
            WHEN CHARINDEX(',', l.Nombre) > 0 THEN LEFT(l.Nombre, CHARINDEX(',', l.Nombre) - 1)
            ELSE l.Nombre
        END
    )),
    LTRIM(RTRIM(
        CASE
            WHEN CHARINDEX(',', l.Nombre) > 0 THEN SUBSTRING(l.Nombre, CHARINDEX(',', l.Nombre) + 1, LEN(l.Nombre))
            ELSE ''
        END
    )),
    l.SectorId,
    l.CategoriaId,
    CASE WHEN l.HorarioId = 0 THEN NULL ELSE l.HorarioId END,
    ISNULL(l.Activo, 1),
    ISNULL(l.CalendarioPersonalizado, 0),
    l.Foto,
    @Now,
    'migration'
FROM DigitalPlus.dbo.Legajos l
WHERE EXISTS (SELECT 1 FROM Sector s WHERE s.Id = l.SectorId)
  AND EXISTS (SELECT 1 FROM Categoria c WHERE c.Id = l.CategoriaId)
  AND NOT EXISTS (SELECT 1 FROM Legajo nl WHERE nl.Id = l.Id AND nl.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Legajo OFF;

PRINT '   Legajos migrados: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 8. LEGAJO PIN (desde campos de Legajos)
-- =============================================================================
PRINT '>> Migrando LegajoPines...';

-- Solo migrar PINs si la BD origen tiene las columnas (agregadas post-deploy)
IF EXISTS (SELECT 1 FROM DigitalPlus.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Legajos' AND COLUMN_NAME = 'PinHash')
BEGIN
    EXEC sp_executesql N'
    INSERT INTO LegajoPin (LegajoId, PinHash, PinSalt, PinChangedAt, PinMustChange, CreatedAt)
    SELECT
        l.Id,
        l.PinHash,
        l.PinSalt,
        l.PinChangedAt,
        ISNULL(l.PinMustChange, 0),
        @pNow
    FROM DigitalPlus.dbo.Legajos l
    WHERE l.PinHash IS NOT NULL AND l.PinHash != ''''
      AND EXISTS (SELECT 1 FROM Legajo nl WHERE nl.Id = l.Id)
      AND NOT EXISTS (SELECT 1 FROM LegajoPin lp WHERE lp.LegajoId = l.Id);
    ', N'@pNow DATETIME2', @pNow = @Now;
    PRINT '   LegajoPines migrados: ' + CAST(@@ROWCOUNT AS VARCHAR);
END
ELSE
BEGIN
    PRINT '   LegajoPines: columnas PIN no existen en origen, omitido';
END

-- =============================================================================
-- 9. LEGAJO SUCURSALES
-- =============================================================================
PRINT '>> Migrando LegajoSucursales...';

INSERT INTO LegajoSucursal (LegajoId, SucursalId)
SELECT
    ls.LegajoId,
    ls.SucursalId
FROM DigitalPlus.dbo.LegajosSucursales ls
WHERE EXISTS (SELECT 1 FROM Legajo nl WHERE nl.Id = ls.LegajoId)
  AND EXISTS (SELECT 1 FROM Sucursal ns WHERE ns.Id = ls.SucursalId)
  AND NOT EXISTS (SELECT 1 FROM LegajoSucursal nls WHERE nls.LegajoId = ls.LegajoId AND nls.SucursalId = ls.SucursalId);

PRINT '   LegajoSucursales migrados: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 10. LEGAJO HUELLAS
-- =============================================================================
PRINT '>> Migrando LegajoHuellas...';

INSERT INTO LegajoHuella (LegajoId, DedoId, Huella, FingerMask)
SELECT
    lh.LegajoId,
    lh.DedoId,
    lh.huella,
    lh.nFingerMask
FROM DigitalPlus.dbo.LegajosHuellas lh
WHERE EXISTS (SELECT 1 FROM Legajo nl WHERE nl.Id = lh.LegajoId)
  AND NOT EXISTS (SELECT 1 FROM LegajoHuella nlh WHERE nlh.LegajoId = lh.LegajoId AND nlh.DedoId = lh.DedoId);

PRINT '   LegajoHuellas migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 11. INCIDENCIAS
-- =============================================================================
PRINT '>> Migrando Incidencias...';

SET IDENTITY_INSERT Incidencia ON;

INSERT INTO Incidencia (Id, EmpresaId, Nombre, Color, Abreviatura, IsActive, CreatedAt, CreatedBy)
SELECT
    i.Id,
    @EmpresaId,
    i.Nombre,
    i.Color,
    i.Abreviatura,
    1,
    @Now,
    'migration'
FROM DigitalPlus.dbo.Incidencias i
WHERE NOT EXISTS (SELECT 1 FROM Incidencia ni WHERE ni.Id = i.Id AND ni.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Incidencia OFF;

PRINT '   Incidencias migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 12. VACACIONES
-- =============================================================================
PRINT '>> Migrando Vacaciones...';

SET IDENTITY_INSERT Vacacion ON;

INSERT INTO Vacacion (Id, EmpresaId, LegajoId, FechaDesde, FechaHasta, Nota, CreatedAt, CreatedBy)
SELECT
    v.Id,
    @EmpresaId,
    v.LegajoId,
    CAST(v.FechaDesde AS DATE),
    CAST(v.FechaHasta AS DATE),
    v.Nota,
    @Now,
    'migration'
FROM DigitalPlus.dbo.Vacaciones v
WHERE EXISTS (SELECT 1 FROM Legajo nl WHERE nl.Id = v.LegajoId)
  AND NOT EXISTS (SELECT 1 FROM Vacacion nv WHERE nv.Id = v.Id AND nv.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Vacacion OFF;

PRINT '   Vacaciones migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 13. NOTICIAS
-- =============================================================================
PRINT '>> Migrando Noticias...';

SET IDENTITY_INSERT Noticia ON;

INSERT INTO Noticia (Id, EmpresaId, Titulo, Contenido, FechaDesde, FechaHasta, IsPrivada, CreatedAt, CreatedBy)
SELECT
    n.Id,
    @EmpresaId,
    n.Nombre,
    n.Detalle,
    CAST(n.FechaDesde AS DATE),
    CAST(n.FechaHasta AS DATE),
    ISNULL(n.Privado, 0),
    @Now,
    'migration'
FROM DigitalPlus.dbo.Noticias n
WHERE NOT EXISTS (SELECT 1 FROM Noticia nn WHERE nn.Id = n.Id AND nn.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Noticia OFF;

PRINT '   Noticias migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 14. VARIABLES SISTEMA (solo las que no existen)
-- =============================================================================
PRINT '>> Migrando VariablesSistema...';

INSERT INTO VariableSistema (EmpresaId, Clave, Descripcion, TipoValor, Valor, RequiereReinicio)
SELECT
    @EmpresaId,
    vg.sId,
    vg.Nombre,
    ISNULL(vg.TipoValor, 'string'),
    vg.Valor,
    ISNULL(vg.Reiniciar, 0)
FROM DigitalPlus.dbo.VariablesGlobales vg
WHERE vg.sId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM VariableSistema vs WHERE vs.EmpresaId = @EmpresaId AND vs.Clave = vg.sId);

PRINT '   VariablesSistema migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

-- =============================================================================
-- 15. FICHADAS (la tabla mas grande: ~780K registros)
-- =============================================================================
PRINT '>> Migrando Fichadas (esto puede tardar)...';

SET IDENTITY_INSERT Fichada ON;

INSERT INTO Fichada (Id, EmpresaId, LegajoId, SucursalId, TerminalId, FechaHora, Tipo, CreatedAt)
SELECT
    f.Id,
    @EmpresaId,
    f.LegajoId,
    f.SucursalId,
    NULL, -- TerminalId no existe en legacy
    f.Registro,
    f.EntraSale,
    f.Registro -- CreatedAt = fecha de fichada
FROM DigitalPlus.dbo.Fichadas f
WHERE EXISTS (SELECT 1 FROM Legajo nl WHERE nl.Id = f.LegajoId)
  AND EXISTS (SELECT 1 FROM Sucursal ns WHERE ns.Id = f.SucursalId)
  AND NOT EXISTS (SELECT 1 FROM Fichada nf WHERE nf.Id = f.Id AND nf.EmpresaId = @EmpresaId);

SET IDENTITY_INSERT Fichada OFF;

PRINT '   Fichadas migradas: ' + CAST(@@ROWCOUNT AS VARCHAR);

COMMIT TRANSACTION;
PRINT '';
PRINT '=== MIGRACION COMPLETADA EXITOSAMENTE ===';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'ERROR: ' + ERROR_MESSAGE();
    PRINT 'Linea: ' + CAST(ERROR_LINE() AS VARCHAR);
END CATCH
