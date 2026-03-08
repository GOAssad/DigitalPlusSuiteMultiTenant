-- ============================================================
-- SP: List activation codes (admin visibility)
-- @ShowAll = 0: only unused + unexpired
-- @ShowAll = 1: all codes
-- ============================================================

CREATE OR ALTER PROCEDURE [dbo].[Provisioning_ListActivationCodes]
    @ShowAll BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF @ShowAll = 1
        SELECT Id, LEFT(CodeHash, 8) + '...' AS CodeHashPrefix,
               ExpiresAt, UsedAt, CompanyName, DbName, MachineId, InstallType,
               CreatedAt, CreatedBy, Notes
        FROM ActivationCodes
        ORDER BY CreatedAt DESC;
    ELSE
        SELECT Id, LEFT(CodeHash, 8) + '...' AS CodeHashPrefix,
               ExpiresAt, CreatedAt, CreatedBy, Notes
        FROM ActivationCodes
        WHERE UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME()
        ORDER BY CreatedAt DESC;
END
GO
