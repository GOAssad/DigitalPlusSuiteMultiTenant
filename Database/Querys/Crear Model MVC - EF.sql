-- Correrlo para hacer clases de las tablas
-- se deben llenar las variables @Tabla con la tabla a la que se le va a construir la clase y
-- @NameSpace que es el espacio de nombres a la que va a pertenecer la clase
-- Una vez revisado, correrlo y hacer resultado a Archivo y poner el nombre de la tabla y la extension cs
-- cuando se incluya este archivo en el proyecto, seleccionar todo el texto y presionar ctrl + K + D para indentar bien.

Declare @Salto char(1);
Declare @Tab varchar(10);
Declare @NameSpace varchar(50)
Declare @Tabla varchar(50)

Declare @TextoArmadoClase varchar(Max)
Declare @TextoVariables varchar(Max)
Declare @TextoIntercambio varchar(500)
Declare @TextoPropiedades varchar(500)
Declare @TextoAgregar varchar(500)
--------------------------------------------------------------------------------
Set @Tabla = 'Companias'
set @NameSpace = 'NextLogistic.Models'
---------------------------------------------------------------------------------
set @Salto = CHAR(10)
set @Tab = SPACE(10)
set @TextoArmadoClase = ''
set @TextoVariables = ''
set @TextoIntercambio = ''
set @TextoPropiedades = ''
set @TextoAgregar = ''

declare @Campo varchar(50)
declare @Tipo varchar(50)
declare @Tamano int
declare @esnull int

-- Generar Clase Cliente
Print 'using System;' + @Salto + 'using System.Collections.Generic;' + @Salto + 
'using System.Linq;' + @Salto +  'using System.Web;' + @Salto + 
'using System.ComponentModel.DataAnnotations;' + @Salto 


print 'namespace ' + @nameSpace + @Salto +  '{' 

-- Print '	public class ' + @Tabla + ': EntidadSimple' + @Salto + '	{' 

Print '	public class ' + @Tabla + @Salto + '	{' 
Print '	string tabla = ' + CHAR(34) + @Tabla + CHAR(34) + ';' + @Salto


-- Armo la clase con los parametros segun los campos que estan en la tabla menos los que tienen identity

DECLARE Tabla_Cursor SCROLL CURSOR FOR 
select b.name as Campo, 
Tipo = 
CASE c.name 
	when 'int' THEN 'int'
	when 'varchar' THEN 'string'
	when 'nvarchar' THEN 'string'
END,
Tamano =
CASE c.name 
	when 'int' THEN 0
	when 'varchar' THEN c.length / 160
	when 'nvarchar' THEN c.length / 80
END, isnullable as esnull

from sysobjects a
inner join syscolumns b on a.id = b.id
inner join systypes c on b.xtype = c.xtype and b.xusertype = c.xusertype
left join sys.identity_columns d on b.id = d.object_id
where a.name = @Tabla
order by b.colid;



open Tabla_Cursor;
FETCH NEXT FROM Tabla_Cursor INTO @Campo, @Tipo, @Tamano, @esnull;
WHILE @@FETCH_STATUS = 0
	BEGIN 
	
		--protected (las variables que intercambian datos para las propiedades)
		
		if (@Campo = 'Id')
			Begin Set @TextoVariables = @TextoVariables + '	[Key]' + @Salto end

		if (@esnull = 0)
			Begin Set @TextoVariables = @TextoVariables + '	[Required(ErrorMessage = "Debes ingresar {0}")]' + @Salto end

		set @TextoVariables = @TextoVariables + '	public ' + @Tipo + ' ' + @Campo + ' { get; set; }' + @Salto

		-- public class	(Armado de la clase con los parametros
		set @TextoArmadoClase = @TextoArmadoClase + @Tipo + ' _' + @Campo + ', '
		
	


	
		FETCH NEXT FROM Tabla_Cursor INTO @Campo, @Tipo, @Tamano, @esnull;
	END;

CLOSE Tabla_Cursor;
DEALLOCATE Tabla_Cursor;


Set @TextoVariables =  Left(@TextoVariables,len(@TextoVariables) - 1) + @Salto
print @TextoVariables





Print @Salto + '	}' -- public Class
Print @Salto + '}' --Fin Namespace
