-- Armar Store Procedures
DECLARE @TABLA VARCHAR(50)
SET @TABLA = 'GRALSUCURSALES'


declare @Cabecera varchar(100)
declare @Parametros varchar(max); Set @Parametros = ''
declare @Variables varchar(max); Set @Variables = ''
declare @Campos varchar(max); Set @Campos = ''
declare @Instrucc varchar(max); Set @Instrucc = ''

declare @InstruccionSelect varchar(max); Set @InstruccionSelect = 'Select * from '
declare @VarSelect varchar(max); Set @VarSelect = ''

declare @VarUpdate varchar(max); set @VarUpdate = ''
declare @Where varchar(max); set @Where = ''


Select @Cabecera = 'CREATE PROCEDURE ' + @TABLA + '_SP' + CHAR(13) 


select  @Parametros = @Parametros + 
'@'+rtrim(B.NAME) + ' ' + c.name +
CASE b.xtype 
		when 52 then ''
		when 56 then ''
		when 60 then ''
		when 104 then ''
	else '(' +
	 ltrim(str(b.length)) + ')' 
END + ', ' 
FROM SYSOBJECTS A 
	INNER JOIN SYSCOLUMNS B ON A.ID = B.ID
	inner join systypes C ON b.xtype = c.xtype
WHERE A.NAME = @TABLA
AND b.colstat = 0
order by b.colorder

select  @Campos = @Campos + 
rtrim(B.NAME) + ', ' + char(13)
FROM SYSOBJECTS A 
	INNER JOIN SYSCOLUMNS B ON A.ID = B.ID
	inner join systypes C ON b.xtype = c.xtype
WHERE A.NAME = @TABLA
AND b.colstat = 0
order by b.colorder

---- INSERT --------------------------------------------------
select  @Variables = @Variables + '@' + 
rtrim(B.NAME) + ', ' 
FROM SYSOBJECTS A 
	INNER JOIN SYSCOLUMNS B ON A.ID = B.ID
	inner join systypes C ON b.xtype = c.xtype
WHERE A.NAME = @TABLA
AND b.colstat = 0
order by b.colorder
-------------------------------------------------------------

---- UPDATE -------------------------------------------------
Select @VarUpdate = @VarUpdate + 
rtrim(b.name) + ' = @' + RTRIM(b.name) + ', ' + char(13) 
FROM SYSOBJECTS A 
	INNER JOIN SYSCOLUMNS B ON A.ID = B.ID
	inner join systypes C ON b.xtype = c.xtype
WHERE A.NAME = @TABLA
AND b.colstat = 0
order by b.colorder
--------------------------------------------------------------
---- Instruccion Select
Select @varSelect = @VarSelect + b.name  + ' = @' + b.name + ', '
From sys.objects o
Inner Join sys.indexes i On o.object_id = i.object_id 
Inner Join sys.index_columns ic On i.object_id = ic.object_id And i.index_id = ic.index_id 
inner join sys.columns b on ic.object_id = b.object_id and ic.column_id = b.column_id
where o.name = @TABLA --and i.name like 'IX%'
Set @InstruccionSelect = @InstruccionSelect + @TABLA + ' Where ' + @VarSelect

---------------------------------------------------------------------------------------------


		Set @Campos = char(13) + '('  + Substring(@Campos, 1 , len(@Campos) - 3) + ')'
		Set @Variables = char(13) + '('  + Substring(@Variables, 1 , len(@Variables) - 1) + ')'

		Set @Instrucc = ' Insert into ' + @Tabla +  @Campos + char(13) + ' Values ' + @Variables

		Set @Where = ' Where ' + 'AGREGAR ACA EL WHERE'



		Print @Cabecera + @Parametros + char(13) + '@TipoSQL smallint' + char(13) +  ' AS ' 
			+ Char(13) + char(13) 
			+ 'IF @TipoSQL = 0 ' + CHAR(13) + 
			'BEGIN' + CHAR(13) + 
				'		' 
			+ substring(@InstruccionSelect,1, len(@InstruccionSelect)-1) + CHAR(13) + ' END ' + CHAR(13) + CHAR(13)
			+ 'IF @TipoSQL = 1 ' + CHAR(13) + 
				'BEGIN' + CHAR(13) + 
				'		' 
			+ @Instrucc + CHAR(13) + CHAR(13) + 'END' + char(13) + char(13) 
			+ 'IF @TipoSQL = 2 ' + CHAR(13) + 
				'BEGIN' + CHAR(13) + 
				'		' 
			+ 'UPDATE ' + @Tabla + ' set ' + substring(@VarUpdate, 1, len(@VarUpdate)-3) + CHAR(13) + char(13) + 'END'



