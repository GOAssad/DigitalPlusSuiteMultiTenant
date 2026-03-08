
--[II_NewFamily_AsientoResumen4] '2018-07-01', '2018-07-31'
CREATE PROCEDURE [dbo].[II_NewFamily_AsientoResumen4] (@FechaDesde DATE , @FechaHasta DATE)

AS


if exists(select distinct 1 from sysobjects where name =  '_Temporal_Contable_Reporte4')
	  begin
	      drop table _Temporal_Contable_Reporte4
	end

-- meto todos los que quiero sumar
-- Retiro de Fondos
SELECT
    '0' as JRNENTRY, 'K-RFACUM' as SOURCDOC, 'Retiro de fondos' REFRENCE, @FechaHasta TRXDATE, @FechaHasta LSTDTEDT, 
	sum(GL20000.CRDTAMNT) CRDTAMNT, sum(GL20000.DEBITAMT) DEBITAMT,
    'Ret.Fond. Acumulados' ACTDESCR, 'Ret.Fond. Acumulados' SDOCDSCR, GL00105.ACTNUMST
into _Temporal_Contable_Reporte4
FROM PRD04.dbo.GL30000 (nolock) as GL20000 
	INNER JOIN PRD04.dbo.GL00100 (nolock) on GL20000.ACTINDX = GL00100.ACTINDX
    INNER JOIN PRD04.dbo.GL00105 (nolock) on GL00100.ACTINDX = GL00105.ACTINDX
WHERE
	GL20000.TRXDATE >= @FechaDesde AND GL20000.TRXDATE <= @FechaHasta and
	REFRENCE LIKE '%etiro%'  and  REFRENCE like '%fondo%'
	GROUP BY GL00105.ACTNUMST
union all
-- Viaticos
SELECT
    '1' as JRNENTRY, 'K-VIACUM' as SOURCDOC, 'Viaticos' REFRENCE, @FechaHasta TRXDATE, @FechaHasta LSTDTEDT, 
	sum(GL20000.CRDTAMNT) CRDTAMNT, sum(GL20000.DEBITAMT) DEBITAMT,
    'Viaticos Acumulados' ACTDESCR, 'Viaticos Acumulados' SDOCDSCR, GL00105.ACTNUMST
FROM PRD04.dbo.GL30000 (nolock) as GL20000 
	INNER JOIN PRD04.dbo.GL00100 (nolock) on GL20000.ACTINDX = GL00100.ACTINDX
    INNER JOIN PRD04.dbo.GL00105 (nolock) on GL00100.ACTINDX = GL00105.ACTINDX
WHERE
	GL20000.TRXDATE >= @FechaDesde AND GL20000.TRXDATE <= @FechaHasta and
	REFRENCE LIKE '%vi%'  and  REFRENCE like '%tico%' -- por si le mete el asento a la a	
	GROUP BY GL00105.ACTNUMST
-- Gastos a Rendir
union all
SELECT
    '2' as JRNENTRY, 'K-GRACUM' as SOURCDOC, 'Gastos a Rendir' REFRENCE, @FechaHasta TRXDATE, @FechaHasta LSTDTEDT, 
	sum(GL20000.CRDTAMNT) CRDTAMNT, sum(GL20000.DEBITAMT) DEBITAMT,
    'Gs.a Rendir Acumulados' ACTDESCR, 'Gs.a Rendir Acumulados' SDOCDSCR, GL00105.ACTNUMST
FROM PRD04.dbo.GL30000 (nolock) as GL20000 
	INNER JOIN PRD04.dbo.GL00100 (nolock) on GL20000.ACTINDX = GL00100.ACTINDX
    INNER JOIN PRD04.dbo.GL00105 (nolock) on GL00100.ACTINDX = GL00105.ACTINDX
WHERE
	GL20000.TRXDATE >= @FechaDesde AND GL20000.TRXDATE <= @FechaHasta and
	REFRENCE LIKE '%Gasto%'  and  REFRENCE like '%rendir%'
	GROUP BY GL00105.ACTNUMST


SELECT
    GL20000.JRNENTRY, GL20000.SOURCDOC, GL20000.REFRENCE, GL20000.TRXDATE, GL20000.LSTDTEDT, GL20000.CRDTAMNT, GL20000.DEBITAMT,
    GL00100.ACTDESCR,
    SY00900.SDOCDSCR,
    GL00105.ACTNUMST
FROM
    { oj ((PRD04.dbo.GL30000 GL20000	
				INNER JOIN PRD04.dbo.GL00100 GL00100 ON GL20000.ACTINDX = GL00100.ACTINDX)
				LEFT OUTER JOIN PRD04.dbo.SY00900 SY00900 ON GL20000.SOURCDOC = SY00900.SOURCDOC)
     INNER JOIN PRD04.dbo.GL00105 GL00105 ON
        GL00100.ACTINDX = GL00105.ACTINDX}
WHERE
    GL20000.TRXDATE between @fechaDesde and @FechaHasta and
	-- tengo que filtrar lo anterior para que no duplique
	((REFRENCE not LIKE '%etiro%'  and  REFRENCE not like '%fondo%') or
	(REFRENCE not LIKE '%vi%'  and  not REFRENCE like '%tico%') or 
	(REFRENCE not LIKE '%Gasto%'  and  REFRENCE not like '%rendir%'))
union all
SELECT * from _Temporal_Contable_Reporte4

