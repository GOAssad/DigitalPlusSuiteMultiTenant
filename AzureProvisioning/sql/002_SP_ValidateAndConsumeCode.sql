-- ============================================================
-- SP: Validate and consume an activation code (atomic)
-- Uses UPDLOCK+HOLDLOCK to prevent race conditions
-- Result codes: 0=OK, 1=NotFound, 2=Expired, 3=AlreadyUsed
-- ============================================================

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
