-- ============================================================
-- SP: Insert a new activation code (admin use)
-- The actual code is generated outside SQL; only the hash is stored.
-- ============================================================

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
