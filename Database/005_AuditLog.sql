-- =============================================
-- AuditLog: Registro de auditoria por empresa
-- BD: DigitalPlusMultiTenant
-- =============================================

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditLog')
BEGIN
    CREATE TABLE AuditLog (
        Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
        EmpresaId       INT NOT NULL,
        Timestamp       DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
        UserId          NVARCHAR(450) NULL,       -- AspNetUsers.Id (null para acciones de sistema)
        UserName        NVARCHAR(256) NULL,        -- email o nombre (desnormalizado)
        Action          NVARCHAR(50) NOT NULL,     -- Login, LoginFailed, Logout, Create, Update, Delete, Import, Export, FichadaManual
        EntityType      NVARCHAR(100) NULL,        -- Legajo, Sucursal, Horario, etc.
        EntityId        NVARCHAR(50) NULL,          -- PK del registro afectado
        Description     NVARCHAR(500) NULL,         -- Resumen legible
        OldValues       NVARCHAR(MAX) NULL,         -- JSON valores anteriores (Update/Delete)
        NewValues       NVARCHAR(MAX) NULL,         -- JSON valores nuevos (Create/Update)
        IpAddress       NVARCHAR(45) NULL,          -- IP del request
        Source          NVARCHAR(30) NULL,           -- Portal, Mobile, Desktop, API, System

        CONSTRAINT FK_AuditLog_Empresa FOREIGN KEY (EmpresaId)
            REFERENCES Empresa(Id)
    );

    -- Indice principal: busqueda por empresa + fecha (descendente)
    CREATE NONCLUSTERED INDEX IX_AuditLog_Empresa_Timestamp
        ON AuditLog (EmpresaId, [Timestamp] DESC)
        INCLUDE (Action, EntityType, UserName);

    -- Indice secundario: busqueda por entidad especifica
    CREATE NONCLUSTERED INDEX IX_AuditLog_Entity
        ON AuditLog (EmpresaId, EntityType, EntityId)
        INCLUDE ([Timestamp], Action, UserName);

    -- Indice para depuracion por fecha
    CREATE NONCLUSTERED INDEX IX_AuditLog_Timestamp
        ON AuditLog ([Timestamp])
        INCLUDE (EmpresaId);

    PRINT 'Tabla AuditLog creada con indices.';
END
ELSE
    PRINT 'Tabla AuditLog ya existe.';
GO

-- Permisos para dp_app_svc (usuario de la app MT)
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_app_svc')
BEGIN
    GRANT SELECT, INSERT, DELETE ON AuditLog TO dp_app_svc;
    PRINT 'Permisos otorgados a dp_app_svc.';
END
GO
