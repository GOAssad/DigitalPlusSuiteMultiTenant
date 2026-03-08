-- Correrlo para hacer clases de las tablas
-- se deben llenar las variables @Tabla con la tabla a la que se le va a construir la clase y
-- @NameSpace que es el espacio de nombres a la que va a pertenecer la clase
-- Una vez revisado, correrlo y hacer resultado a Archivo y poner el nombre de la tabla y la extension cs
-- cuando se incluya este archivo en el proyecto, seleccionar todo el texto y presionar ctrl + K + D para indentar bien.

Declare @Salto char(1);
Declare @Tab varchar(10);
Declare @NameSpace varchar(50)
Declare @Tabla varchar(50)

Declare @TextoArmadoClase varchar(500)
Declare @TextoVariables varchar(500)
Declare @TextoIntercambio varchar(500)
Declare @TextoPropiedades varchar(500)
Declare @TextoAgregar varchar(500)
--------------------------------------------------------------------------------
Set @Tabla = 'RRHHLegajosHuellas'
set @NameSpace = 'Acceso.Clases.Datos.RRHH'
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
declare @tamano int

-- Generar Clase Cliente
Print 'using System;' + @Salto + 'using System.Collections.Generic;' + @Salto + 
'using System.Linq;' + @Salto +  'using System.Data;' + @Salto + 'using System.Data.SqlClient;' + 
@Salto + 'using System.Configuration;'  + @Salto


print 'namespace ' + @nameSpace + @Salto +  '{' 

Print '	public class ' + @Tabla + ': EntidadSimple' + @Salto + '	{' 
Print '	string tabla = ' + CHAR(34) + @Tabla + CHAR(34) + ';' + @Salto


-- Armo la clase con los parametros segun los campos que estan en la tabla menos los que tienen identity

DECLARE Tabla_Cursor SCROLL CURSOR FOR 
select b.name as Campo, 
Tipo = 
CASE c.name 
	when 'int' THEN 'int'
	when 'varchar' THEN 'string'
END,
CASE c.name 
	when 'int' THEN 0
	when 'varchar' THEN c.length / 160
END
from sysobjects a
inner join syscolumns b on a.id = b.id
inner join systypes c on b.xtype = c.xtype
where a.name = @Tabla and not exists (select * from sys.identity_columns 
where b.name = sys.identity_columns.name)
order by b.colid;


open Tabla_Cursor;
FETCH NEXT FROM Tabla_Cursor INTO @Campo, @Tipo, @Tamano;
WHILE @@FETCH_STATUS = 0
	BEGIN 
	
		--protected (las variables que intercambian datos para las propiedades)
		set @TextoVariables = @TextoVariables + '	protected ' + @Tipo + ' _' + @Campo + ';' + @Salto

		-- public class	(Armado de la clase con los parametros
		set @TextoArmadoClase = @TextoArmadoClase + @Tipo + ' _' + @Campo + ', '
		
		-- Le paso a las variables los valores de los parametros
		set @TextoIntercambio = @TextoIntercambio + '		_' + @Campo + ' = _' + @Campo + ';' + @Salto 
		
		-- Armo las propiedades (aca me puedo llegar a quedar sin espacio en el varchar'
		set @TextoPropiedades = @TextoPropiedades + '		public ' + @Tipo + ' ' + @Campo + @Salto +
			'		{ ' + 
			@Salto + '		set { _' + @Campo + ' =  value; }' +
			@Salto + '		get { return _' + @Campo + '; }' + @Salto + 
			'		}' + @Salto + @Salto
		
		-- Metodo Agregar
		set @TextoAgregar = @TextoAgregar + '		fila[' + CHAR(34) + @Campo + CHAR(34) + '] = ' + @Campo + ';' + @Salto
		
		
	
		FETCH NEXT FROM Tabla_Cursor INTO @Campo, @Tipo, @Tamano;
	END;

CLOSE Tabla_Cursor;
DEALLOCATE Tabla_Cursor;


Set @TextoVariables =  Left(@TextoVariables,len(@TextoVariables) - 1) + @Salto
print @TextoVariables
print '		public ' +@Tabla + ' (' + Left(@TextoArmadoClase,len(@TextoArmadoClase) - 1)+ ' )' + @Salto + '		{' + @Salto
print left(@TextoIntercambio, len(@TextoIntercambio) - 1) + @Salto +  '		}'
print @TextoPropiedades
Print	'		public void Agregar()' + @Salto + 
		'		{' + @Salto + 
		'		Conectar(tabla);' + @Salto +
		'		DataRow fila;' + @Salto  +
		'		fila = Data.Tables[tabla].NewRow();' + @Salto + @TextoAgregar + @Salto +
		'		Data.Tables[tabla].Rows.Add(fila); '+ @Salto +
		'		AdaptadorDatos.Update(Data, tabla);' + @Salto +
		'		}' -- Metodo Agregar
		

Print @Salto + '	}' -- public Class
Print @Salto + '}' --Fin Namespace
