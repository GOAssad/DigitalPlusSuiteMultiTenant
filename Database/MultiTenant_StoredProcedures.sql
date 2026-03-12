-- ============================================================
-- Stored Procedures para DigitalPlusMultiTenant
-- Usados por Fichador y Administrador (apps desktop)
-- ============================================================

USE DigitalPlusMultiTenant;
GO

-- ============================================================
-- LEGAJO
-- ============================================================

-- Insertar/Actualizar legajo (MERGE por NumeroLegajo + EmpresaId)
IF OBJECT_ID('EscritorioLegajoActualizar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajoActualizar;
GO
CREATE PROCEDURE EscritorioLegajoActualizar
    @NumeroLegajo VARCHAR(50),
    @Apellido NVARCHAR(200),
    @Nombre NVARCHAR(200),
    @SectorId INT,
    @CategoriaId INT,
    @IsActive BIT,
    @HorarioID INT = NULL,
    @HasCalendarioPersonalizado BIT = 0,
    @nSucursalId INT = 0,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @LegajoId INT;

    IF EXISTS (SELECT 1 FROM Legajo WHERE NumeroLegajo = @NumeroLegajo AND EmpresaId = @EmpresaId)
    BEGIN
        UPDATE Legajo SET
            Apellido = @Apellido,
            Nombre = @Nombre,
            SectorId = @SectorId,
            CategoriaId = @CategoriaId,
            IsActive = @IsActive,
            HorarioId = @HorarioID,
            HasCalendarioPersonalizado = @HasCalendarioPersonalizado,
            UpdatedAt = GETUTCDATE()
        WHERE NumeroLegajo = @NumeroLegajo AND EmpresaId = @EmpresaId;

        SELECT @LegajoId = Id FROM Legajo WHERE NumeroLegajo = @NumeroLegajo AND EmpresaId = @EmpresaId;
    END
    ELSE
    BEGIN
        INSERT INTO Legajo (NumeroLegajo, Apellido, Nombre, SectorId, CategoriaId, IsActive,
                           HorarioId, HasCalendarioPersonalizado, EmpresaId, CreatedAt)
        VALUES (@NumeroLegajo, @Apellido, @Nombre, @SectorId, @CategoriaId, @IsActive,
                @HorarioID, @HasCalendarioPersonalizado, @EmpresaId, GETUTCDATE());

        SET @LegajoId = SCOPE_IDENTITY();
    END

    -- Manejar LegajoSucursal
    IF @nSucursalId > 0
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM LegajoSucursal WHERE LegajoId = @LegajoId AND SucursalId = @nSucursalId)
        BEGIN
            DELETE FROM LegajoSucursal WHERE LegajoId = @LegajoId;
            INSERT INTO LegajoSucursal (LegajoId, SucursalId) VALUES (@LegajoId, @nSucursalId);
        END
    END
END
GO

-- Eliminar legajo y datos relacionados
IF OBJECT_ID('RRHHLegajos_DeleteTodo', 'P') IS NOT NULL DROP PROCEDURE RRHHLegajos_DeleteTodo;
GO
CREATE PROCEDURE RRHHLegajos_DeleteTodo
    @NumeroLegajo VARCHAR(50),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @LegajoId INT;
    SELECT @LegajoId = Id FROM Legajo WHERE NumeroLegajo = @NumeroLegajo AND EmpresaId = @EmpresaId;

    IF @LegajoId IS NOT NULL
    BEGIN
        DELETE FROM LegajoHuella WHERE LegajoId = @LegajoId;
        DELETE FROM LegajoPin WHERE LegajoId = @LegajoId;
        DELETE FROM LegajoSucursal WHERE LegajoId = @LegajoId;
        DELETE FROM LegajoDomicilio WHERE LegajoId = @LegajoId;
        DELETE FROM Fichada WHERE LegajoId = @LegajoId;
        DELETE FROM IncidenciaLegajo WHERE LegajoId = @LegajoId;
        DELETE FROM Vacacion WHERE LegajoId = @LegajoId;
        DELETE FROM EventoCalendario WHERE LegajoId = @LegajoId;
        DELETE FROM Legajo WHERE Id = @LegajoId;
    END
END
GO

-- ============================================================
-- FICHADAS
-- ============================================================

-- Registrar fichada (entrada/salida automática)
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
    DECLARE @UltimoTipo VARCHAR(1);

    -- Determinar si es Entrada o Salida basado en la última fichada del día
    SELECT TOP 1 @UltimoTipo = Tipo
    FROM Fichada
    WHERE LegajoId = @nLegajoID
      AND CAST(FechaHora AS DATE) = CAST(@dRegistro AS DATE)
      AND EmpresaId = @EmpresaId
    ORDER BY FechaHora DESC;

    IF @UltimoTipo IS NULL OR @UltimoTipo = 'S'
        SET @sAccion = 'E';
    ELSE
        SET @sAccion = 'S';

    INSERT INTO Fichada (EmpresaId, LegajoId, SucursalId, FechaHora, Tipo, Origen, CreatedAt)
    VALUES (@EmpresaId, @nLegajoID, @nSucursalID, @dRegistro, @sAccion, @Origen, GETUTCDATE());
END
GO

-- Eliminar fichada
IF OBJECT_ID('RRHHFichadas_SP_ELIMINAR', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadas_SP_ELIMINAR;
GO
CREATE PROCEDURE RRHHFichadas_SP_ELIMINAR
    @Id INT,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Fichada WHERE Id = @Id AND EmpresaId = @EmpresaId;
END
GO

-- Fichada manual
IF OBJECT_ID('RRHHFichadas_SP_MANUAL', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadas_SP_MANUAL;
GO
CREATE PROCEDURE RRHHFichadas_SP_MANUAL
    @nSucursalID INT,
    @nLegajoID INT,
    @dRegistro DATETIME,
    @sEntraSale VARCHAR(1),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Fichada (EmpresaId, LegajoId, SucursalId, FechaHora, Tipo, Origen, CreatedAt)
    VALUES (@EmpresaId, @nLegajoID, @nSucursalID, @dRegistro, @sEntraSale, 1, GETUTCDATE());
END
GO

-- Traer fichadas por rango y legajos
IF OBJECT_ID('RRHHFichadas_SP_MANUAL_SELECT', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadas_SP_MANUAL_SELECT;
GO
CREATE PROCEDURE RRHHFichadas_SP_MANUAL_SELECT
    @ld VARCHAR(50),
    @lh VARCHAR(50),
    @fd DATETIME,
    @fh DATETIME,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.Id, l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sNombre,
           f.FechaHora AS dRegistro,
           CONVERT(VARCHAR(10), f.FechaHora, 103) AS Fecha,
           CONVERT(CHAR(5), f.FechaHora, 108) AS Hora,
           f.Tipo AS sEntraSale,
           s.Nombre AS sSucursal
    FROM Fichada f
    INNER JOIN Legajo l ON f.LegajoId = l.Id
    INNER JOIN Sucursal s ON f.SucursalId = s.Id
    WHERE l.NumeroLegajo BETWEEN @ld AND @lh
      AND f.FechaHora BETWEEN @fd AND @fh
      AND f.EmpresaId = @EmpresaId
    ORDER BY l.NumeroLegajo, f.FechaHora;
END
GO

-- Traer fichadas por grupo
IF OBJECT_ID('RRHHFichadas_SP_MANUAL_SELECT_GRUPO', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadas_SP_MANUAL_SELECT_GRUPO;
GO
CREATE PROCEDURE RRHHFichadas_SP_MANUAL_SELECT_GRUPO
    @ld VARCHAR(50),
    @fd DATETIME,
    @fh DATETIME,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.Id, l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sNombre,
           f.FechaHora AS dRegistro,
           CONVERT(VARCHAR(10), f.FechaHora, 103) AS Fecha,
           CONVERT(CHAR(5), f.FechaHora, 108) AS Hora,
           f.Tipo AS sEntraSale,
           s.Nombre AS sSucursal
    FROM Fichada f
    INNER JOIN Legajo l ON f.LegajoId = l.Id
    INNER JOIN Sucursal s ON f.SucursalId = s.Id
    WHERE l.NumeroLegajo >= @ld
      AND f.FechaHora BETWEEN @fd AND @fh
      AND f.EmpresaId = @EmpresaId
    ORDER BY l.NumeroLegajo, f.FechaHora;
END
GO

-- Listado de fichadas completo
IF OBJECT_ID('RRHHFichadas_SP_LISTADO', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadas_SP_LISTADO;
GO
CREATE PROCEDURE RRHHFichadas_SP_LISTADO
    @fromDate DATE,
    @toDate DATE,
    @legdesde VARCHAR(50),
    @leghasta VARCHAR(50),
    @grupo VARCHAR(50) = NULL,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.NumeroLegajo AS sLegajoID, l.Apellido AS sApellido, l.Nombre AS sNombre,
           h.Nombre AS HorarioSector, f.FechaHora AS dRegistro,
           CONVERT(VARCHAR(10), f.FechaHora, 103) AS Fecha,
           CONVERT(CHAR(5), f.FechaHora, 108) AS Hora,
           f.Tipo AS sEntraSale,
           s.Id AS sSucursalID, s.Id AS sSucursalGrupoID,
           s.Nombre AS sDescSucursal
    FROM Legajo l
    LEFT JOIN Horario h ON l.HorarioId = h.Id
    INNER JOIN Fichada f ON f.LegajoId = l.Id
    INNER JOIN Sucursal s ON f.SucursalId = s.Id
    WHERE f.FechaHora BETWEEN @fromDate AND @toDate
      AND l.NumeroLegajo BETWEEN @legdesde AND @leghasta
      AND l.EmpresaId = @EmpresaId
      AND (@grupo IS NULL OR s.Id = TRY_CAST(@grupo AS INT))
    ORDER BY l.NumeroLegajo, f.FechaHora;
END
GO

-- Llegadas tarde por legajo
IF OBJECT_ID('RRHHFichadasEntradaEstatusLegajo_SP_SELECT', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadasEntradaEstatusLegajo_SP_SELECT;
GO
CREATE PROCEDURE RRHHFichadasEntradaEstatusLegajo_SP_SELECT
    @SoloTarde BIT,
    @legajo VARCHAR(50),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Retorna fichadas de entrada del legajo con info de horario
    SELECT l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sNombre,
           f.FechaHora AS dRegistro,
           CONVERT(VARCHAR(10), f.FechaHora, 103) AS Fecha,
           CONVERT(CHAR(5), f.FechaHora, 108) AS Hora,
           f.Tipo AS sEntraSale
    FROM Fichada f
    INNER JOIN Legajo l ON f.LegajoId = l.Id
    WHERE l.NumeroLegajo = @legajo
      AND f.Tipo = 'E'
      AND f.EmpresaId = @EmpresaId
    ORDER BY f.FechaHora DESC;
END
GO

-- Entrada/estatus general
IF OBJECT_ID('RRHHFichadasEntradaEstatus_SP_SELECT', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadasEntradaEstatus_SP_SELECT;
GO
CREATE PROCEDURE RRHHFichadasEntradaEstatus_SP_SELECT
    @fechadesde DATETIME,
    @fechahasta DATETIME,
    @soloTarde BIT,
    @grupo VARCHAR(50) = NULL,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sNombre,
           f.FechaHora AS dRegistro,
           CONVERT(VARCHAR(10), f.FechaHora, 103) AS Fecha,
           CONVERT(CHAR(5), f.FechaHora, 108) AS Hora,
           f.Tipo AS sEntraSale,
           s.Nombre AS sDescSucursal
    FROM Fichada f
    INNER JOIN Legajo l ON f.LegajoId = l.Id
    INNER JOIN Sucursal s ON f.SucursalId = s.Id
    WHERE f.FechaHora BETWEEN @fechadesde AND @fechahasta
      AND f.Tipo = 'E'
      AND l.EmpresaId = @EmpresaId
      AND (@grupo IS NULL OR s.Id = TRY_CAST(@grupo AS INT))
    ORDER BY l.NumeroLegajo, f.FechaHora;
END
GO

-- Ausencias
IF OBJECT_ID('RRHHFichadasAusencias_SP_SELECT', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadasAusencias_SP_SELECT;
GO
CREATE PROCEDURE RRHHFichadasAusencias_SP_SELECT
    @Fecha DATETIME,
    @grupo VARCHAR(50) = NULL,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Legajos activos que NO tienen fichada en la fecha indicada
    SELECT l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sNombre,
           s.Nombre AS sDescSucursal
    FROM Legajo l
    LEFT JOIN LegajoSucursal ls ON l.Id = ls.LegajoId
    LEFT JOIN Sucursal s ON ls.SucursalId = s.Id
    WHERE l.IsActive = 1
      AND l.EmpresaId = @EmpresaId
      AND NOT EXISTS (
          SELECT 1 FROM Fichada f
          WHERE f.LegajoId = l.Id
            AND CAST(f.FechaHora AS DATE) = CAST(@Fecha AS DATE)
      )
      AND (@grupo IS NULL OR s.Id = TRY_CAST(@grupo AS INT))
    ORDER BY l.NumeroLegajo;
END
GO

-- Ausencias por legajo
IF OBJECT_ID('RRHHFichadasAusencias_x_Legajo_SP_SELECT', 'P') IS NOT NULL DROP PROCEDURE RRHHFichadasAusencias_x_Legajo_SP_SELECT;
GO
CREATE PROCEDURE RRHHFichadasAusencias_x_Legajo_SP_SELECT
    @legajo VARCHAR(50),
    @fdesde DATETIME,
    @fhasta DATETIME,
    @incidencia BIT,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sNombre,
           il.Fecha AS dRegistro,
           i.Nombre AS sIncidencia
    FROM IncidenciaLegajo il
    INNER JOIN Legajo l ON il.LegajoId = l.Id
    INNER JOIN Incidencia i ON il.IncidenciaId = i.Id
    WHERE l.NumeroLegajo = @legajo
      AND il.Fecha BETWEEN @fdesde AND @fhasta
      AND il.EmpresaId = @EmpresaId
    ORDER BY il.Fecha;
END
GO

-- ============================================================
-- HUELLAS
-- ============================================================

-- Insertar/actualizar huella
IF OBJECT_ID('EscritorioLegajosHuellasActualizar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajosHuellasActualizar;
GO
CREATE PROCEDURE EscritorioLegajosHuellasActualizar
    @LegajoId INT,
    @DedoId INT,
    @Huella VARBINARY(MAX),
    @FingerMask INT,
    @sLegajoNombre VARCHAR(200),
    @sLegajoID VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM LegajoHuella WHERE LegajoId = @LegajoId AND DedoId = @DedoId)
    BEGIN
        UPDATE LegajoHuella SET
            Huella = @Huella,
            FingerMask = @FingerMask
        WHERE LegajoId = @LegajoId AND DedoId = @DedoId;
    END
    ELSE
    BEGIN
        INSERT INTO LegajoHuella (LegajoId, DedoId, Huella, FingerMask)
        VALUES (@LegajoId, @DedoId, @Huella, @FingerMask);
    END
END
GO

-- ============================================================
-- PIN
-- ============================================================

-- Verificar PIN
IF OBJECT_ID('EscritorioLegajoPIN_Verificar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajoPIN_Verificar;
GO
CREATE PROCEDURE EscritorioLegajoPIN_Verificar
    @sLegajoID NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    -- Busca legajo activo con su PIN (si tiene)
    SELECT l.Id AS nLegajoID, l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sLegajoNombre,
           p.PinHash, p.PinSalt, p.PinMustChange, p.PinChangedAt
    FROM Legajo l
    LEFT JOIN LegajoPin p ON l.Id = p.LegajoId
    WHERE l.NumeroLegajo = @sLegajoID AND l.IsActive = 1;
END
GO

-- Cambiar PIN
IF OBJECT_ID('EscritorioLegajoPIN_Cambiar', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajoPIN_Cambiar;
GO
CREATE PROCEDURE EscritorioLegajoPIN_Cambiar
    @sLegajoID NVARCHAR(50),
    @PinHash NVARCHAR(100),
    @PinSalt NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @LegajoId INT;
    SELECT @LegajoId = Id FROM Legajo WHERE NumeroLegajo = @sLegajoID AND IsActive = 1;

    IF @LegajoId IS NOT NULL
    BEGIN
        IF EXISTS (SELECT 1 FROM LegajoPin WHERE LegajoId = @LegajoId)
        BEGIN
            UPDATE LegajoPin SET
                PinHash = @PinHash,
                PinSalt = @PinSalt,
                PinMustChange = 0,
                PinChangedAt = GETUTCDATE()
            WHERE LegajoId = @LegajoId;
        END
        ELSE
        BEGIN
            INSERT INTO LegajoPin (LegajoId, PinHash, PinSalt, PinMustChange, PinChangedAt, CreatedAt)
            VALUES (@LegajoId, @PinHash, @PinSalt, 0, GETUTCDATE(), GETUTCDATE());
        END
    END
END
GO

-- Lista de legajos activos (modo demo)
IF OBJECT_ID('EscritorioLegajosActivos_Lista', 'P') IS NOT NULL DROP PROCEDURE EscritorioLegajosActivos_Lista;
GO
CREATE PROCEDURE EscritorioLegajosActivos_Lista
AS
BEGIN
    SET NOCOUNT ON;
    -- Nota: el Fichador no pasa EmpresaId a este SP, se usa para modo demo
    -- En producción cada empresa tiene su BD, pero en multi-tenant dev necesitamos filtrar
    SELECT l.Id AS nLegajoID, l.NumeroLegajo AS sLegajoID,
           l.Apellido + ', ' + l.Nombre AS sLegajoNombre
    FROM Legajo l
    WHERE l.IsActive = 1
    ORDER BY l.NumeroLegajo;
END
GO

-- ============================================================
-- CATEGORIAS
-- ============================================================

IF OBJECT_ID('Categoria_SP', 'P') IS NOT NULL DROP PROCEDURE Categoria_SP;
GO
CREATE PROCEDURE Categoria_SP
    @Id INT,
    @Nombre NVARCHAR(200),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Categoria WHERE Id = @Id AND EmpresaId = @EmpresaId)
    BEGIN
        UPDATE Categoria SET Nombre = @Nombre, UpdatedAt = GETUTCDATE()
        WHERE Id = @Id AND EmpresaId = @EmpresaId;
    END
    ELSE
    BEGIN
        INSERT INTO Categoria (Nombre, EmpresaId, IsActive, CreatedAt)
        VALUES (@Nombre, @EmpresaId, 1, GETUTCDATE());
    END
END
GO

IF OBJECT_ID('Categoria_Delete', 'P') IS NOT NULL DROP PROCEDURE Categoria_Delete;
GO
CREATE PROCEDURE Categoria_Delete
    @Id INT,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Categoria WHERE Id = @Id AND EmpresaId = @EmpresaId;
END
GO

-- ============================================================
-- HORARIOS
-- ============================================================

IF OBJECT_ID('Horario_SP', 'P') IS NOT NULL DROP PROCEDURE Horario_SP;
GO
CREATE PROCEDURE Horario_SP
    @Id INT,
    @Nombre NVARCHAR(200),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Horario WHERE Id = @Id AND EmpresaId = @EmpresaId)
    BEGIN
        UPDATE Horario SET Nombre = @Nombre, UpdatedAt = GETUTCDATE()
        WHERE Id = @Id AND EmpresaId = @EmpresaId;
    END
    ELSE
    BEGIN
        INSERT INTO Horario (Nombre, EmpresaId, IsActive, CreatedAt)
        VALUES (@Nombre, @EmpresaId, 1, GETUTCDATE());
    END
END
GO

IF OBJECT_ID('HorarioDetalle_SP', 'P') IS NOT NULL DROP PROCEDURE HorarioDetalle_SP;
GO
CREATE PROCEDURE HorarioDetalle_SP
    @HorarioId INT,
    @DiaSemana INT,
    @HoraDesdeH INT,
    @HoraDesdeM INT,
    @HoraHastaH INT,
    @HoraHastaM INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @HoraDesde TIME = CAST(CAST(@HoraDesdeH AS VARCHAR) + ':' + CAST(@HoraDesdeM AS VARCHAR) AS TIME);
    DECLARE @HoraHasta TIME = CAST(CAST(@HoraHastaH AS VARCHAR) + ':' + CAST(@HoraHastaM AS VARCHAR) AS TIME);

    IF EXISTS (SELECT 1 FROM HorarioDetalle WHERE HorarioId = @HorarioId AND DiaSemana = @DiaSemana)
    BEGIN
        UPDATE HorarioDetalle SET HoraDesde = @HoraDesde, HoraHasta = @HoraHasta
        WHERE HorarioId = @HorarioId AND DiaSemana = @DiaSemana;
    END
    ELSE
    BEGIN
        INSERT INTO HorarioDetalle (HorarioId, DiaSemana, HoraDesde, HoraHasta, IsCerrado)
        VALUES (@HorarioId, @DiaSemana, @HoraDesde, @HoraHasta, 0);
    END
END
GO

-- ============================================================
-- FERIADOS
-- ============================================================

IF OBJECT_ID('Feriado_SP', 'P') IS NOT NULL DROP PROCEDURE Feriado_SP;
GO
CREATE PROCEDURE Feriado_SP
    @Id INT,
    @Nombre NVARCHAR(200),
    @Fecha DATE,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @Id > 0 AND EXISTS (SELECT 1 FROM Feriado WHERE Id = @Id AND EmpresaId = @EmpresaId)
    BEGIN
        UPDATE Feriado SET Nombre = @Nombre, Fecha = @Fecha
        WHERE Id = @Id AND EmpresaId = @EmpresaId;
    END
    ELSE
    BEGIN
        INSERT INTO Feriado (Nombre, Fecha, EmpresaId)
        VALUES (@Nombre, @Fecha, @EmpresaId);
    END
END
GO

IF OBJECT_ID('Feriado_Delete', 'P') IS NOT NULL DROP PROCEDURE Feriado_Delete;
GO
CREATE PROCEDURE Feriado_Delete
    @Id INT,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Feriado WHERE Id = @Id AND EmpresaId = @EmpresaId;
END
GO

-- ============================================================
-- INCIDENCIAS
-- ============================================================

IF OBJECT_ID('Incidencia_SP', 'P') IS NOT NULL DROP PROCEDURE Incidencia_SP;
GO
CREATE PROCEDURE Incidencia_SP
    @Id INT,
    @Nombre NVARCHAR(200),
    @Color NVARCHAR(10),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Incidencia WHERE Id = @Id AND EmpresaId = @EmpresaId)
    BEGIN
        UPDATE Incidencia SET Nombre = @Nombre, Color = @Color, UpdatedAt = GETUTCDATE()
        WHERE Id = @Id AND EmpresaId = @EmpresaId;
    END
    ELSE
    BEGIN
        INSERT INTO Incidencia (Nombre, Color, EmpresaId, IsActive, CreatedAt)
        VALUES (@Nombre, @Color, @EmpresaId, 1, GETUTCDATE());
    END
END
GO

IF OBJECT_ID('Incidencia_Delete', 'P') IS NOT NULL DROP PROCEDURE Incidencia_Delete;
GO
CREATE PROCEDURE Incidencia_Delete
    @Id INT,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Incidencia WHERE Id = @Id AND EmpresaId = @EmpresaId;
END
GO

IF OBJECT_ID('RRHHIncidenciasLegajos_Delete', 'P') IS NOT NULL DROP PROCEDURE RRHHIncidenciasLegajos_Delete;
GO
CREATE PROCEDURE RRHHIncidenciasLegajos_Delete
    @sIncidenciaID VARCHAR(50),
    @sLegajoID VARCHAR(50),
    @dRegistro DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    DELETE il FROM IncidenciaLegajo il
    INNER JOIN Legajo l ON il.LegajoId = l.Id
    INNER JOIN Incidencia i ON il.IncidenciaId = i.Id
    WHERE l.NumeroLegajo = @sLegajoID
      AND i.Id = TRY_CAST(@sIncidenciaID AS INT)
      AND il.Fecha = CAST(@dRegistro AS DATE);
END
GO

-- ============================================================
-- SUCURSALES
-- ============================================================

IF OBJECT_ID('Sucursal_SP', 'P') IS NOT NULL DROP PROCEDURE Sucursal_SP;
GO
CREATE PROCEDURE Sucursal_SP
    @Id INT,
    @Nombre NVARCHAR(200),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Sucursal WHERE Id = @Id AND EmpresaId = @EmpresaId)
    BEGIN
        UPDATE Sucursal SET Nombre = @Nombre, UpdatedAt = GETUTCDATE()
        WHERE Id = @Id AND EmpresaId = @EmpresaId;
    END
    ELSE
    BEGIN
        INSERT INTO Sucursal (Nombre, Codigo, EmpresaId, IsActive, CreatedAt)
        VALUES (@Nombre, '', @EmpresaId, 1, GETUTCDATE());
    END
END
GO

IF OBJECT_ID('Sucursal_Delete', 'P') IS NOT NULL DROP PROCEDURE Sucursal_Delete;
GO
CREATE PROCEDURE Sucursal_Delete
    @Id INT,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Sucursal WHERE Id = @Id AND EmpresaId = @EmpresaId;
END
GO

-- ============================================================
-- TERMINALES
-- ============================================================

IF OBJECT_ID('Terminal_SP', 'P') IS NOT NULL DROP PROCEDURE Terminal_SP;
GO
CREATE PROCEDURE Terminal_SP
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(500) = NULL,
    @SucursalId INT,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Terminal WHERE Nombre = @Nombre AND EmpresaId = @EmpresaId)
    BEGIN
        UPDATE Terminal SET Descripcion = @Descripcion, SucursalId = @SucursalId, UpdatedAt = GETUTCDATE()
        WHERE Nombre = @Nombre AND EmpresaId = @EmpresaId;
    END
    ELSE
    BEGIN
        INSERT INTO Terminal (Nombre, Descripcion, SucursalId, EmpresaId, IsActive, CreatedAt)
        VALUES (@Nombre, @Descripcion, @SucursalId, @EmpresaId, 1, GETUTCDATE());
    END
END
GO

IF OBJECT_ID('Terminal_Delete', 'P') IS NOT NULL DROP PROCEDURE Terminal_Delete;
GO
CREATE PROCEDURE Terminal_Delete
    @Nombre NVARCHAR(200),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Terminal WHERE Nombre = @Nombre AND EmpresaId = @EmpresaId;
END
GO

-- ============================================================
-- VACACIONES
-- ============================================================

IF OBJECT_ID('RRHHVacaciones_SP_SELECT_LEGAJO', 'P') IS NOT NULL DROP PROCEDURE RRHHVacaciones_SP_SELECT_LEGAJO;
GO
CREATE PROCEDURE RRHHVacaciones_SP_SELECT_LEGAJO
    @sLegajoID VARCHAR(50),
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.Id, v.FechaDesde AS dDesde, v.FechaHasta AS dHasta, v.Nota
    FROM Vacacion v
    INNER JOIN Legajo l ON v.LegajoId = l.Id
    WHERE l.NumeroLegajo = @sLegajoID AND v.EmpresaId = @EmpresaId
    ORDER BY v.FechaDesde DESC;
END
GO

IF OBJECT_ID('RRHHVacaciones_SP_INSERT_LEGAJO', 'P') IS NOT NULL DROP PROCEDURE RRHHVacaciones_SP_INSERT_LEGAJO;
GO
CREATE PROCEDURE RRHHVacaciones_SP_INSERT_LEGAJO
    @sLegajoID VARCHAR(50),
    @dDesde DATE,
    @dHasta DATE,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @LegajoId INT;
    SELECT @LegajoId = Id FROM Legajo WHERE NumeroLegajo = @sLegajoID AND EmpresaId = @EmpresaId;

    IF @LegajoId IS NOT NULL
    BEGIN
        INSERT INTO Vacacion (LegajoId, FechaDesde, FechaHasta, EmpresaId, CreatedAt)
        VALUES (@LegajoId, @dDesde, @dHasta, @EmpresaId, GETUTCDATE());
    END
END
GO

IF OBJECT_ID('RRHHVacaciones_SP_DELETE', 'P') IS NOT NULL DROP PROCEDURE RRHHVacaciones_SP_DELETE;
GO
CREATE PROCEDURE RRHHVacaciones_SP_DELETE
    @sLegajoID VARCHAR(50),
    @dDesde DATE,
    @dHasta DATE,
    @EmpresaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE v FROM Vacacion v
    INNER JOIN Legajo l ON v.LegajoId = l.Id
    WHERE l.NumeroLegajo = @sLegajoID
      AND v.FechaDesde = @dDesde
      AND v.FechaHasta = @dHasta
      AND v.EmpresaId = @EmpresaId;
END
GO

PRINT 'Stored Procedures creados exitosamente para DigitalPlusMultiTenant';
GO
