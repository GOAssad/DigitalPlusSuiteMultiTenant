-- ============================================================
-- Migracion: Agregar EmpresaId a Licencias
-- Vincula licencias con Empresas.Id (INT) en lugar de depender
-- del string CompanyId que puede diferir entre portal y desktop.
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- ============================================================

-- 1. Agregar columna si no existe
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Licencias' AND COLUMN_NAME = 'EmpresaId'
)
BEGIN
    ALTER TABLE Licencias ADD EmpresaId INT NULL;
END
GO

-- 2. Popular EmpresaId para filas existentes
UPDATE l SET l.EmpresaId = e.Id
FROM Licencias l
INNER JOIN Empresas e ON l.CompanyId = e.CompanyId
WHERE l.EmpresaId IS NULL;
GO
