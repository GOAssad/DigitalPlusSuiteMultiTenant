-- ============================================================
-- MIGRACION: DigitalPlus -> DigitalPlusAdmin
-- Crea la BD DigitalPlusAdmin en Ferozo y mueve las tablas
-- de licencias y activation codes fuera de la BD del cliente.
--
-- EJECUTAR EN: Ferozo (sd-1985882-l.ferozo.com,11434)
-- CONEXION INICIAL: master (o cualquier BD que no sea DigitalPlus)
--
-- PASOS:
--   1. Crear BD DigitalPlusAdmin
--   2. Crear tablas (ActivationCodes, Licencias, LicenciasLog)
--   3. Migrar datos existentes desde DigitalPlus
--   4. Crear SPs en DigitalPlusAdmin
--   5. Limpiar DigitalPlus (solo objetos de licencias/activation)
-- ============================================================

-- ============================================================
-- PASO 1: Crear la BD si no existe
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'DigitalPlusAdmin')
BEGIN
    CREATE DATABASE [DigitalPlusAdmin];
    PRINT 'BD DigitalPlusAdmin creada.';
END
ELSE
    PRINT 'BD DigitalPlusAdmin ya existe.';
GO

USE [DigitalPlusAdmin];
GO

-- ============================================================
-- PASO 2: Crear tablas
-- ============================================================

-- ActivationCodes
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ActivationCodes')
BEGIN
    CREATE TABLE [dbo].[ActivationCodes] (
        [Id]            INT             IDENTITY(1,1) NOT NULL,
        [CodeHash]      NVARCHAR(64)    NOT NULL,
        [ExpiresAt]     DATETIME2(7)    NOT NULL,
        [UsedAt]        DATETIME2(7)    NULL,
        [CompanyName]   NVARCHAR(200)   NULL,
        [DbName]        NVARCHAR(60)    NULL,
        [MachineId]     NVARCHAR(200)   NULL,
        [InstallType]   NVARCHAR(10)    NULL,
        [CreatedAt]     DATETIME2(7)    NOT NULL DEFAULT SYSUTCDATETIME(),
        [CreatedBy]     NVARCHAR(100)   NULL,
        [Notes]         NVARCHAR(500)   NULL,
        CONSTRAINT [PK_ActivationCodes] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_ActivationCodes_CodeHash] UNIQUE ([CodeHash])
    );

    CREATE NONCLUSTERED INDEX [IX_ActivationCodes_CodeHash_ExpiresAt]
        ON [dbo].[ActivationCodes] ([CodeHash], [ExpiresAt])
        INCLUDE ([UsedAt]);

    PRINT 'Tabla ActivationCodes creada.';
END
ELSE
    PRINT 'Tabla ActivationCodes ya existe.';
GO

-- Licencias
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Licencias]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[Licencias] (
        [Id]              INT             IDENTITY(1,1) NOT NULL,
        [CompanyId]       NVARCHAR(100)   NOT NULL,
        [MachineId]       NVARCHAR(200)   NOT NULL,
        [LicenseType]     NVARCHAR(20)    NOT NULL,
        [Plan]            NVARCHAR(50)    NOT NULL DEFAULT 'free',
        [MaxLegajos]      INT             NOT NULL DEFAULT 5,
        [ActivationCode]  NVARCHAR(64)    NULL,
        [TrialStartedAt]  DATETIME2(7)    NULL,
        [TrialEndsAt]     DATETIME2(7)    NULL,
        [ExpiresAt]       DATETIME2(7)    NULL,
        [SuspendedAt]     DATETIME2(7)    NULL,
        [GraceEndsAt]     DATETIME2(7)    NULL,
        [LastHeartbeat]   DATETIME2(7)    NULL,
        [CreatedAt]       DATETIME2(7)    NOT NULL DEFAULT SYSUTCDATETIME(),
        [UpdatedAt]       DATETIME2(7)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT [PK_Licencias] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UQ_Licencias_Company_Machine] UNIQUE ([CompanyId], [MachineId])
    );

    CREATE NONCLUSTERED INDEX [IX_Licencias_CompanyId]
        ON [dbo].[Licencias] ([CompanyId])
        INCLUDE ([LicenseType], [ExpiresAt], [SuspendedAt]);

    PRINT 'Tabla Licencias creada.';
END
ELSE
    PRINT 'Tabla Licencias ya existe.';
GO

-- LicenciasLog
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LicenciasLog]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[LicenciasLog] (
        [Id]          BIGINT          IDENTITY(1,1) NOT NULL,
        [LicenciaId]  INT             NOT NULL,
        [Action]      NVARCHAR(50)    NOT NULL,
        [App]         NVARCHAR(30)    NULL,
        [Details]     NVARCHAR(500)   NULL,
        [IP]          NVARCHAR(45)    NULL,
        [Timestamp]   DATETIME2(7)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT [PK_LicenciasLog] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_LicenciasLog_Licencia] FOREIGN KEY ([LicenciaId])
            REFERENCES [dbo].[Licencias]([Id])
    );

    CREATE NONCLUSTERED INDEX [IX_LicenciasLog_LicenciaId]
        ON [dbo].[LicenciasLog] ([LicenciaId])
        INCLUDE ([Action], [Timestamp]);

    PRINT 'Tabla LicenciasLog creada.';
END
ELSE
    PRINT 'Tabla LicenciasLog ya existe.';
GO

-- ============================================================
-- PASO 3: Migrar datos existentes desde DigitalPlus
-- ============================================================

-- Migrar ActivationCodes
IF EXISTS (SELECT 1 FROM [DigitalPlus].sys.tables WHERE name = 'ActivationCodes')
BEGIN
    SET IDENTITY_INSERT [DigitalPlusAdmin].[dbo].[ActivationCodes] ON;

    INSERT INTO [DigitalPlusAdmin].[dbo].[ActivationCodes]
        ([Id], [CodeHash], [ExpiresAt], [UsedAt], [CompanyName], [DbName], [MachineId], [InstallType], [CreatedAt], [CreatedBy], [Notes])
    SELECT
        [Id], [CodeHash], [ExpiresAt], [UsedAt], [CompanyName], [DbName], [MachineId], [InstallType], [CreatedAt], [CreatedBy], [Notes]
    FROM [DigitalPlus].[dbo].[ActivationCodes] src
    WHERE NOT EXISTS (
        SELECT 1 FROM [DigitalPlusAdmin].[dbo].[ActivationCodes] dst WHERE dst.[Id] = src.[Id]
    );

    SET IDENTITY_INSERT [DigitalPlusAdmin].[dbo].[ActivationCodes] OFF;
    PRINT 'Datos ActivationCodes migrados.';
END
ELSE
    PRINT 'No existe ActivationCodes en DigitalPlus (nada que migrar).';
GO

-- Migrar Licencias
IF EXISTS (SELECT 1 FROM [DigitalPlus].sys.tables WHERE name = 'Licencias')
BEGIN
    SET IDENTITY_INSERT [DigitalPlusAdmin].[dbo].[Licencias] ON;

    INSERT INTO [DigitalPlusAdmin].[dbo].[Licencias]
        ([Id], [CompanyId], [MachineId], [LicenseType], [Plan], [MaxLegajos], [ActivationCode],
         [TrialStartedAt], [TrialEndsAt], [ExpiresAt], [SuspendedAt], [GraceEndsAt],
         [LastHeartbeat], [CreatedAt], [UpdatedAt])
    SELECT
        [Id], [CompanyId], [MachineId], [LicenseType], [Plan], [MaxLegajos], [ActivationCode],
        [TrialStartedAt], [TrialEndsAt], [ExpiresAt], [SuspendedAt], [GraceEndsAt],
        [LastHeartbeat], [CreatedAt], [UpdatedAt]
    FROM [DigitalPlus].[dbo].[Licencias] src
    WHERE NOT EXISTS (
        SELECT 1 FROM [DigitalPlusAdmin].[dbo].[Licencias] dst WHERE dst.[Id] = src.[Id]
    );

    SET IDENTITY_INSERT [DigitalPlusAdmin].[dbo].[Licencias] OFF;
    PRINT 'Datos Licencias migrados.';
END
ELSE
    PRINT 'No existe Licencias en DigitalPlus (nada que migrar).';
GO

-- Migrar LicenciasLog
IF EXISTS (SELECT 1 FROM [DigitalPlus].sys.tables WHERE name = 'LicenciasLog')
BEGIN
    SET IDENTITY_INSERT [DigitalPlusAdmin].[dbo].[LicenciasLog] ON;

    INSERT INTO [DigitalPlusAdmin].[dbo].[LicenciasLog]
        ([Id], [LicenciaId], [Action], [App], [Details], [IP], [Timestamp])
    SELECT
        [Id], [LicenciaId], [Action], [App], [Details], [IP], [Timestamp]
    FROM [DigitalPlus].[dbo].[LicenciasLog] src
    WHERE NOT EXISTS (
        SELECT 1 FROM [DigitalPlusAdmin].[dbo].[LicenciasLog] dst WHERE dst.[Id] = src.[Id]
    );

    SET IDENTITY_INSERT [DigitalPlusAdmin].[dbo].[LicenciasLog] OFF;
    PRINT 'Datos LicenciasLog migrados.';
END
ELSE
    PRINT 'No existe LicenciasLog en DigitalPlus (nada que migrar).';
GO

-- ============================================================
-- PASO 4: Crear SPs en DigitalPlusAdmin
-- ============================================================

-- SP: Provisioning_ValidateAndConsumeCode
CREATE OR ALTER PROCEDURE [dbo].[Provisioning_ValidateAndConsumeCode]
    @CodeHash       NVARCHAR(64),
    @CompanyName    NVARCHAR(200),
    @DbName         NVARCHAR(60),
    @MachineId      NVARCHAR(200),
    @InstallType    NVARCHAR(10),
    @Result         INT OUTPUT,
    @ExpiresAt      DATETIME2(7) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    DECLARE @id INT, @usedAt DATETIME2(7), @expiry DATETIME2(7);

    SELECT @id = Id, @usedAt = UsedAt, @expiry = ExpiresAt
    FROM ActivationCodes WITH (UPDLOCK, HOLDLOCK)
    WHERE CodeHash = @CodeHash;

    IF @id IS NULL
    BEGIN
        SET @Result = 1;
        SET @ExpiresAt = NULL;
        COMMIT;
        RETURN;
    END

    SET @ExpiresAt = @expiry;

    IF @usedAt IS NOT NULL
    BEGIN
        SET @Result = 3;
        COMMIT;
        RETURN;
    END

    IF @expiry < SYSUTCDATETIME()
    BEGIN
        SET @Result = 2;
        COMMIT;
        RETURN;
    END

    UPDATE ActivationCodes
    SET UsedAt      = SYSUTCDATETIME(),
        CompanyName = @CompanyName,
        DbName      = @DbName,
        MachineId   = @MachineId,
        InstallType = @InstallType
    WHERE Id = @id;

    SET @Result = 0;

    COMMIT;
END
GO

-- SP: Provisioning_InsertActivationCode
CREATE OR ALTER PROCEDURE [dbo].[Provisioning_InsertActivationCode]
    @CodeHash       NVARCHAR(64),
    @ExpiresAt      DATETIME2(7),
    @CreatedBy      NVARCHAR(100) = NULL,
    @Notes          NVARCHAR(500) = NULL,
    @NewId          INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ActivationCodes (CodeHash, ExpiresAt, CreatedBy, Notes)
    VALUES (@CodeHash, @ExpiresAt, @CreatedBy, @Notes);

    SET @NewId = SCOPE_IDENTITY();
END
GO

-- SP: Provisioning_ListActivationCodes
CREATE OR ALTER PROCEDURE [dbo].[Provisioning_ListActivationCodes]
    @ShowAll BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF @ShowAll = 1
        SELECT Id, LEFT(CodeHash, 8) + '...' AS CodeHashPrefix,
               ExpiresAt, UsedAt, CompanyName, DbName, MachineId, InstallType,
               CreatedAt, CreatedBy, Notes
        FROM ActivationCodes
        ORDER BY CreatedAt DESC;
    ELSE
        SELECT Id, LEFT(CodeHash, 8) + '...' AS CodeHashPrefix,
               ExpiresAt, CreatedAt, CreatedBy, Notes
        FROM ActivationCodes
        WHERE UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME()
        ORDER BY CreatedAt DESC;
END
GO

-- SP: License_Activate
CREATE OR ALTER PROCEDURE [dbo].[License_Activate]
    @CompanyId      NVARCHAR(100),
    @MachineId      NVARCHAR(200),
    @ActivationCode NVARCHAR(64)  = NULL,
    @Plan           NVARCHAR(50)  = 'free',
    @MaxLegajos     INT           = 5,
    @ExpiresAt      DATETIME2(7)  = NULL,
    @Result         INT           OUTPUT,
    @LicenciaId     INT           OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
    DECLARE @ExistingId INT;
    DECLARE @ExistingType NVARCHAR(20);

    BEGIN TRANSACTION;

    SELECT @ExistingId = [Id], @ExistingType = [LicenseType]
    FROM [dbo].[Licencias] WITH (UPDLOCK, HOLDLOCK)
    WHERE [CompanyId] = @CompanyId AND [MachineId] = @MachineId;

    IF @ExistingId IS NOT NULL
    BEGIN
        SET @LicenciaId = @ExistingId;

        IF @ActivationCode IS NOT NULL AND @ExistingType = 'trial'
        BEGIN
            UPDATE [dbo].[Licencias]
            SET [LicenseType]    = 'active',
                [Plan]           = @Plan,
                [MaxLegajos]     = @MaxLegajos,
                [ActivationCode] = @ActivationCode,
                [ExpiresAt]      = @ExpiresAt,
                [SuspendedAt]    = NULL,
                [GraceEndsAt]    = NULL,
                [UpdatedAt]      = @Now
            WHERE [Id] = @ExistingId;

            INSERT INTO [dbo].[LicenciasLog] ([LicenciaId], [Action], [Details])
            VALUES (@ExistingId, 'upgrade_from_trial', 'Plan: ' + @Plan + ', MaxLegajos: ' + CAST(@MaxLegajos AS NVARCHAR(10)));

            SET @Result = 0;
        END
        ELSE
        BEGIN
            SET @Result = 1;
        END

        COMMIT TRANSACTION;
        RETURN;
    END

    IF @ActivationCode IS NULL
    BEGIN
        INSERT INTO [dbo].[Licencias]
            ([CompanyId], [MachineId], [LicenseType], [Plan], [MaxLegajos],
             [TrialStartedAt], [TrialEndsAt], [LastHeartbeat])
        VALUES
            (@CompanyId, @MachineId, 'trial', 'free', 5,
             @Now, DATEADD(DAY, 14, @Now), @Now);
    END
    ELSE
    BEGIN
        INSERT INTO [dbo].[Licencias]
            ([CompanyId], [MachineId], [LicenseType], [Plan], [MaxLegajos],
             [ActivationCode], [ExpiresAt], [LastHeartbeat])
        VALUES
            (@CompanyId, @MachineId, 'active', @Plan, @MaxLegajos,
             @ActivationCode, @ExpiresAt, @Now);
    END

    SET @LicenciaId = SCOPE_IDENTITY();

    INSERT INTO [dbo].[LicenciasLog] ([LicenciaId], [Action], [Details])
    VALUES (@LicenciaId,
            CASE WHEN @ActivationCode IS NULL THEN 'trial_start' ELSE 'activate' END,
            'Plan: ' + ISNULL(@Plan, 'free') + ', MaxLegajos: ' + CAST(ISNULL(@MaxLegajos, 5) AS NVARCHAR(10)));

    SET @Result = 0;

    COMMIT TRANSACTION;
END
GO

-- SP: License_Heartbeat
CREATE OR ALTER PROCEDURE [dbo].[License_Heartbeat]
    @CompanyId      NVARCHAR(100),
    @MachineId      NVARCHAR(200),
    @App            NVARCHAR(30)  = NULL,
    @ActiveLegajos  INT           = 0,
    @Result         INT           OUTPUT,
    @LicenciaId     INT           OUTPUT,
    @LicenseType    NVARCHAR(20)  OUTPUT,
    @LicensePlan    NVARCHAR(50)  OUTPUT,
    @MaxLegajos     INT           OUTPUT,
    @TrialEndsAt    DATETIME2(7)  OUTPUT,
    @ExpiresAt      DATETIME2(7)  OUTPUT,
    @SuspendedAt    DATETIME2(7)  OUTPUT,
    @GraceEndsAt    DATETIME2(7)  OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();

    SELECT
        @LicenciaId  = [Id],
        @LicenseType = [LicenseType],
        @LicensePlan = [Plan],
        @MaxLegajos  = [MaxLegajos],
        @TrialEndsAt = [TrialEndsAt],
        @ExpiresAt   = [ExpiresAt],
        @SuspendedAt = [SuspendedAt],
        @GraceEndsAt = [GraceEndsAt]
    FROM [dbo].[Licencias]
    WHERE [CompanyId] = @CompanyId AND [MachineId] = @MachineId;

    IF @LicenciaId IS NULL
    BEGIN
        SET @Result = 1;
        RETURN;
    END

    UPDATE [dbo].[Licencias]
    SET [LastHeartbeat] = @Now,
        [UpdatedAt]     = @Now
    WHERE [Id] = @LicenciaId;

    INSERT INTO [dbo].[LicenciasLog] ([LicenciaId], [Action], [App], [Details])
    VALUES (@LicenciaId, 'heartbeat', @App,
            'Legajos: ' + CAST(@ActiveLegajos AS NVARCHAR(10)));

    SET @Result = 0;
END
GO

-- ============================================================
-- PASO 5: Limpiar DigitalPlus (SOLO objetos de licencias/activation)
-- IMPORTANTE: Verificar que la migracion fue exitosa antes de ejecutar esto
-- ============================================================

USE [DigitalPlus];
GO

-- Verificar que los datos existen en DigitalPlusAdmin antes de borrar
DECLARE @countAdmin INT, @countOrigen INT;

-- Verificar ActivationCodes
SELECT @countOrigen = COUNT(*) FROM [DigitalPlus].[dbo].[ActivationCodes];
SELECT @countAdmin  = COUNT(*) FROM [DigitalPlusAdmin].[dbo].[ActivationCodes];

IF @countAdmin < @countOrigen
BEGIN
    RAISERROR('ERROR: ActivationCodes en DigitalPlusAdmin (%d) < DigitalPlus (%d). Abortando limpieza.', 16, 1, @countAdmin, @countOrigen);
    RETURN;
END

-- Verificar Licencias
SELECT @countOrigen = COUNT(*) FROM [DigitalPlus].[dbo].[Licencias];
SELECT @countAdmin  = COUNT(*) FROM [DigitalPlusAdmin].[dbo].[Licencias];

IF @countAdmin < @countOrigen
BEGIN
    RAISERROR('ERROR: Licencias en DigitalPlusAdmin (%d) < DigitalPlus (%d). Abortando limpieza.', 16, 1, @countAdmin, @countOrigen);
    RETURN;
END

PRINT 'Verificacion OK. Procediendo con limpieza de DigitalPlus...';

-- Eliminar SPs
IF OBJECT_ID('dbo.License_Heartbeat', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[License_Heartbeat];
IF OBJECT_ID('dbo.License_Activate', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[License_Activate];
IF OBJECT_ID('dbo.Provisioning_ListActivationCodes', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Provisioning_ListActivationCodes];
IF OBJECT_ID('dbo.Provisioning_InsertActivationCode', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Provisioning_InsertActivationCode];
IF OBJECT_ID('dbo.Provisioning_ValidateAndConsumeCode', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Provisioning_ValidateAndConsumeCode];

PRINT 'SPs eliminados de DigitalPlus.';

-- Eliminar tablas (orden por FK)
IF OBJECT_ID('dbo.LicenciasLog', 'U') IS NOT NULL
    DROP TABLE [dbo].[LicenciasLog];
IF OBJECT_ID('dbo.Licencias', 'U') IS NOT NULL
    DROP TABLE [dbo].[Licencias];
IF OBJECT_ID('dbo.ActivationCodes', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActivationCodes];

PRINT 'Tablas de licencias/activation eliminadas de DigitalPlus.';
PRINT 'Migracion completada exitosamente.';
GO
