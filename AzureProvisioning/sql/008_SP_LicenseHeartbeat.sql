-- ============================================================
-- SP: License_Heartbeat
-- Actualiza LastHeartbeat y retorna estado actual de la licencia.
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- ============================================================

CREATE OR ALTER PROCEDURE [dbo].[License_Heartbeat]
    @CompanyId      NVARCHAR(100),
    @MachineId      NVARCHAR(200),
    @App            NVARCHAR(30)  = NULL,
    @ActiveLegajos  INT           = 0,
    @Result         INT           OUTPUT,  -- 0=OK, 1=no encontrada
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

    -- Buscar licencia
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
        SET @Result = 1;  -- No encontrada
        RETURN;
    END

    -- Actualizar LastHeartbeat
    UPDATE [dbo].[Licencias]
    SET [LastHeartbeat] = @Now,
        [UpdatedAt]     = @Now
    WHERE [Id] = @LicenciaId;

    -- Log del heartbeat
    INSERT INTO [dbo].[LicenciasLog] ([LicenciaId], [Action], [App], [Details])
    VALUES (@LicenciaId, 'heartbeat', @App,
            'Legajos: ' + CAST(@ActiveLegajos AS NVARCHAR(10)));

    SET @Result = 0;
END
GO
