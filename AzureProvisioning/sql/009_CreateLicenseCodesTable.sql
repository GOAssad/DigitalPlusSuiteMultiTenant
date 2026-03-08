-- ============================================================
-- Tabla: LicenseCodes
-- Codigos de activacion de licencia (plan, legajos, duracion)
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'LicenseCodes')
BEGIN
    CREATE TABLE [dbo].[LicenseCodes] (
        [Id]            INT             IDENTITY(1,1) NOT NULL,
        [CodeHash]      NVARCHAR(64)    NOT NULL,
        [Plan]          NVARCHAR(50)    NOT NULL DEFAULT 'basic',
        [MaxLegajos]    INT             NOT NULL DEFAULT 25,
        [DurationDays]  INT             NOT NULL DEFAULT 365,       -- duracion de la licencia en dias
        [ExpiresAt]     DATETIME2(7)    NOT NULL,                   -- vencimiento del codigo (no de la licencia)
        [UsedAt]        DATETIME2(7)    NULL,
        [UsedByCompany] NVARCHAR(100)   NULL,
        [UsedByMachine] NVARCHAR(200)   NULL,
        [CreatedAt]     DATETIME2(7)    NOT NULL DEFAULT SYSUTCDATETIME(),
        [CreatedBy]     NVARCHAR(100)   NULL,
        [Notes]         NVARCHAR(500)   NULL,
        CONSTRAINT [PK_LicenseCodes] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UQ_LicenseCodes_CodeHash] UNIQUE ([CodeHash])
    );

    CREATE NONCLUSTERED INDEX [IX_LicenseCodes_CodeHash]
        ON [dbo].[LicenseCodes] ([CodeHash])
        INCLUDE ([UsedAt], [ExpiresAt]);

    PRINT 'Tabla LicenseCodes creada.';
END
ELSE
    PRINT 'Tabla LicenseCodes ya existe.';
GO
