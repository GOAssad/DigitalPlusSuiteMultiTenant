USE [PRD01]
GO

/****** Object:  StoredProcedure [dbo].[DO_Diario]    Script Date: 28/3/2021 04:23:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[DO_Diario] 
@fd dateTime, @fh DateTime, @cd varchar(20), @ch varchar(20)

as

select c.actnumst Cuenta, b.actdescr Descripcion, sum(a.DebitAmt) Debito, sum(a.CrdtAmnt)  Credito
--, a.sourcdoc, d.sdocdscr
	from GL20000 A
	INNER JOIN GL00100 B on a.actindx = b.actindx
	inner join GL00105 c  ON a.actindx = c.actindx
	left join SY00900 d on a.sourcdoc = d.sourcdoc
	where a.trxdate between @fd and @fh and
	c.actnumst between @cd and @ch
group by c.actnumst , b.actdescr 
GO

/****** Object:  StoredProcedure [dbo].[DO_Diario_Agrupado]    Script Date: 28/3/2021 04:23:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[DO_Diario_Agrupado]
@fd dateTime, @fh DateTime, @cd varchar(20), @ch varchar(20)

as

select a.sourcdoc Grupo, d.sdocdscr Descripcion,  sum(a.DebitAmt) Debito, sum(a.CrdtAmnt)  Credito
	from GL20000 A
	INNER JOIN GL00100 B on a.actindx = b.actindx
	inner join GL00105 c  ON a.actindx = c.actindx
	left join SY00900 d on a.sourcdoc = d.sourcdoc
	where a.trxdate between @fd and @fh and
	c.actnumst between @cd and @ch
	group by a.sourcdoc, d.sdocdscr


--exec do_diario_Agrupado '20210319', '20210319','','ZZZZZZZ'

GO

