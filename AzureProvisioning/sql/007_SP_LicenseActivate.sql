-- ============================================================
-- SP: License_Activate
-- Crea o recupera licencia. Soporta trial (sin codigo) y activacion con codigo.
-- Acepta @EmpresaId (preferido) o @CompanyId (fallback para clientes viejos).
-- Si la empresa ya tiene una licencia activa, la nueva PC hereda el mismo plan.
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- ============================================================

CREATE OR ALTER PROCEDURE [dbo].[License_Activate]
    @CompanyId      NVARCHAR(100),
    @MachineId      NVARCHAR(200),
    @ActivationCode NVARCHAR(64)  = NULL,   -- CodeHash, NULL para trial
    @Plan           NVARCHAR(50)  = 'free',
    @MaxLegajos     INT           = 5,
    @ExpiresAt      DATETIME2(7)  = NULL,   -- NULL para trial (usa TrialEndsAt)
    @EmpresaId      INT           = NULL,   -- Preferido sobre CompanyId
    @Result         INT           OUTPUT,    -- 0=OK nueva, 1=OK existente, 2=error
    @LicenciaId     INT           OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
    DECLARE @ExistingId INT;
    DECLARE @ExistingType NVARCHAR(20);

    -- Variables para herencia de plan
    DECLARE @InheritPlan NVARCHAR(50);
    DECLARE @InheritMaxLegajos INT;
    DECLARE @InheritExpiresAt DATETIME2(7);
    DECLARE @InheritType NVARCHAR(20);

    BEGIN TRANSACTION;

    -- Buscar licencia existente: primero por EmpresaId+MachineId, luego por CompanyId+MachineId
    IF @EmpresaId IS NOT NULL
    BEGIN
        SELECT @ExistingId = [Id], @ExistingType = [LicenseType]
        FROM [dbo].[Licencias] WITH (UPDLOCK, HOLDLOCK)
        WHERE [EmpresaId] = @EmpresaId AND [MachineId] = @MachineId;
    END

    IF @ExistingId IS NULL
    BEGIN
        SELECT @ExistingId = [Id], @ExistingType = [LicenseType]
        FROM [dbo].[Licencias] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CompanyId] = @CompanyId AND [MachineId] = @MachineId;
    END

    IF @ExistingId IS NOT NULL
    BEGIN
        -- Ya existe: retornar la existente
        SET @LicenciaId = @ExistingId;

        -- Asegurar que EmpresaId este poblado
        IF @EmpresaId IS NOT NULL
        BEGIN
            UPDATE [dbo].[Licencias]
            SET [EmpresaId] = @EmpresaId, [UpdatedAt] = @Now
            WHERE [Id] = @ExistingId AND ([EmpresaId] IS NULL OR [EmpresaId] <> @EmpresaId);
        END

        -- Si viene con activation code y la licencia era trial, upgradeamos
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

            SET @Result = 0;  -- OK, upgraded
        END
        ELSE
        BEGIN
            -- Retornar la existente sin modificar
            SET @Result = 1;  -- OK, ya existia
        END

        COMMIT TRANSACTION;
        RETURN;
    END

    -- No existe: crear nueva
    -- Primero, buscar si la empresa ya tiene una licencia activa para heredar el plan
    IF @ActivationCode IS NULL AND @EmpresaId IS NOT NULL
    BEGIN
        SELECT TOP 1
            @InheritType = [LicenseType],
            @InheritPlan = [Plan],
            @InheritMaxLegajos = [MaxLegajos],
            @InheritExpiresAt = [ExpiresAt]
        FROM [dbo].[Licencias]
        WHERE [EmpresaId] = @EmpresaId
          AND [LicenseType] = 'active'
          AND [MachineId] <> 'pending'
        ORDER BY [CreatedAt] DESC;
    END

    -- Si no encontro por EmpresaId, buscar por CompanyId (empresas con slug distinto)
    IF @InheritPlan IS NULL AND @ActivationCode IS NULL
    BEGIN
        SELECT TOP 1
            @InheritType = [LicenseType],
            @InheritPlan = [Plan],
            @InheritMaxLegajos = [MaxLegajos],
            @InheritExpiresAt = [ExpiresAt]
        FROM [dbo].[Licencias]
        WHERE [EmpresaId] = @EmpresaId
          AND [LicenseType] = 'active'
          AND [MachineId] <> 'pending'
        ORDER BY [CreatedAt] DESC;
    END

    IF @ActivationCode IS NULL
    BEGIN
        IF @InheritPlan IS NOT NULL
        BEGIN
            -- Heredar plan de licencia existente de la empresa
            INSERT INTO [dbo].[Licencias]
                ([CompanyId], [MachineId], [LicenseType], [Plan], [MaxLegajos],
                 [ExpiresAt], [LastHeartbeat], [EmpresaId])
            VALUES
                (@CompanyId, @MachineId, 'active', @InheritPlan, @InheritMaxLegajos,
                 @InheritExpiresAt, @Now, @EmpresaId);

            SET @LicenciaId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[LicenciasLog] ([LicenciaId], [Action], [Details])
            VALUES (@LicenciaId, 'inherit_plan',
                'Heredado de empresa: Plan=' + @InheritPlan + ', MaxLegajos=' + CAST(@InheritMaxLegajos AS NVARCHAR(10)));
        END
        ELSE
        BEGIN
            -- Trial (no hay licencia existente de la empresa)
            INSERT INTO [dbo].[Licencias]
                ([CompanyId], [MachineId], [LicenseType], [Plan], [MaxLegajos],
                 [TrialStartedAt], [TrialEndsAt], [LastHeartbeat], [EmpresaId])
            VALUES
                (@CompanyId, @MachineId, 'trial', 'free', 5,
                 @Now, DATEADD(DAY, 14, @Now), @Now, @EmpresaId);

            SET @LicenciaId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[LicenciasLog] ([LicenciaId], [Action], [Details])
            VALUES (@LicenciaId, 'trial_start',
                'Plan: free, MaxLegajos: 5');
        END
    END
    ELSE
    BEGIN
        -- Licencia activa con codigo
        INSERT INTO [dbo].[Licencias]
            ([CompanyId], [MachineId], [LicenseType], [Plan], [MaxLegajos],
             [ActivationCode], [ExpiresAt], [LastHeartbeat], [EmpresaId])
        VALUES
            (@CompanyId, @MachineId, 'active', @Plan, @MaxLegajos,
             @ActivationCode, @ExpiresAt, @Now, @EmpresaId);

        SET @LicenciaId = SCOPE_IDENTITY();

        INSERT INTO [dbo].[LicenciasLog] ([LicenciaId], [Action], [Details])
        VALUES (@LicenciaId, 'activate',
            'Plan: ' + ISNULL(@Plan, 'free') + ', MaxLegajos: ' + CAST(ISNULL(@MaxLegajos, 5) AS NVARCHAR(10)));
    END

    SET @Result = 0;

    COMMIT TRANSACTION;
END
GO
