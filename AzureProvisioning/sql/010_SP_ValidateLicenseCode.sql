-- ============================================================
-- SP: License_ValidateAndConsumeCode
-- Valida un codigo de licencia y lo marca como usado (atomico).
-- Result: 0=OK, 1=NotFound, 2=Expired, 3=AlreadyUsed
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- ============================================================

CREATE OR ALTER PROCEDURE [dbo].[License_ValidateAndConsumeCode]
    @CodeHash       NVARCHAR(64),
    @CompanyId      NVARCHAR(100),
    @MachineId      NVARCHAR(200),
    @Result         INT OUTPUT,
    @Plan           NVARCHAR(50) OUTPUT,
    @MaxLegajos     INT OUTPUT,
    @DurationDays   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    DECLARE @id INT, @usedAt DATETIME2(7), @expiry DATETIME2(7);

    SELECT @id = Id, @usedAt = UsedAt, @expiry = ExpiresAt,
           @Plan = [Plan], @MaxLegajos = MaxLegajos, @DurationDays = DurationDays
    FROM LicenseCodes WITH (UPDLOCK, HOLDLOCK)
    WHERE CodeHash = @CodeHash;

    IF @id IS NULL
    BEGIN
        SET @Result = 1;  -- No encontrado
        COMMIT;
        RETURN;
    END

    IF @usedAt IS NOT NULL
    BEGIN
        SET @Result = 3;  -- Ya usado
        COMMIT;
        RETURN;
    END

    IF @expiry < SYSUTCDATETIME()
    BEGIN
        SET @Result = 2;  -- Codigo expirado
        COMMIT;
        RETURN;
    END

    -- Marcar como usado
    UPDATE LicenseCodes
    SET UsedAt        = SYSUTCDATETIME(),
        UsedByCompany = @CompanyId,
        UsedByMachine = @MachineId
    WHERE Id = @id;

    SET @Result = 0;  -- OK

    COMMIT;
END
GO
