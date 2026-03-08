-- ============================================================
-- Tabla: LicenciasLog
-- Registro de eventos de licencia (auditoria)
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LicenciasLog]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[LicenciasLog] (
        [Id]          BIGINT          IDENTITY(1,1) NOT NULL,
        [LicenciaId]  INT             NOT NULL,
        [Action]      NVARCHAR(50)    NOT NULL,   -- 'activate', 'heartbeat', 'suspend', 'unsuspend', 'expire', 'trial_start'
        [App]         NVARCHAR(30)    NULL,        -- 'Fichador', 'Administrador'
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
END
GO
