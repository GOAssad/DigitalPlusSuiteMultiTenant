-- ============================================================
-- Tabla: Licencias
-- Almacena el estado de licencia por empresa + maquina
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Licencias]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[Licencias] (
        [Id]              INT             IDENTITY(1,1) NOT NULL,
        [CompanyId]       NVARCHAR(100)   NOT NULL,
        [MachineId]       NVARCHAR(200)   NOT NULL,
        [LicenseType]     NVARCHAR(20)    NOT NULL,          -- 'trial', 'active', 'suspended'
        [Plan]            NVARCHAR(50)    NOT NULL DEFAULT 'free',
        [MaxLegajos]      INT             NOT NULL DEFAULT 5, -- trial=5, configurable por plan
        [ActivationCode]  NVARCHAR(64)    NULL,               -- CodeHash si fue activado con codigo
        [TrialStartedAt]  DATETIME2(7)    NULL,
        [TrialEndsAt]     DATETIME2(7)    NULL,
        [ExpiresAt]       DATETIME2(7)    NULL,               -- NULL = sin vencimiento
        [SuspendedAt]     DATETIME2(7)    NULL,
        [GraceEndsAt]     DATETIME2(7)    NULL,               -- SuspendedAt + 7 dias
        [LastHeartbeat]   DATETIME2(7)    NULL,
        [CreatedAt]       DATETIME2(7)    NOT NULL DEFAULT SYSUTCDATETIME(),
        [UpdatedAt]       DATETIME2(7)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT [PK_Licencias] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UQ_Licencias_Company_Machine] UNIQUE ([CompanyId], [MachineId])
    );

    CREATE NONCLUSTERED INDEX [IX_Licencias_CompanyId]
        ON [dbo].[Licencias] ([CompanyId])
        INCLUDE ([LicenseType], [ExpiresAt], [SuspendedAt]);
END
GO
