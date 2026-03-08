-- ============================================================
-- ActivationCodes table for provisioning endpoint
-- Runs in: DigitalPlusAdmin (Ferozo - BD administrativa multi-tenant)
-- ============================================================

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
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ActivationCodes_CodeHash_ExpiresAt')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ActivationCodes_CodeHash_ExpiresAt]
        ON [dbo].[ActivationCodes] ([CodeHash], [ExpiresAt])
        INCLUDE ([UsedAt]);
END
GO
