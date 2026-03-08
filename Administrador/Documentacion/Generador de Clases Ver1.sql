-- La variable que contendra las sentencias sql que deben ejecutarse dinamicamente
Declare @Ejecutor nvarchar(max)	-- Usada para SQL Dinamico (ejecuta creates y deletes de tablas)
Declare @Propiedades varchar(max)
set @Propiedades = ''

Declare @InicializarporID varchar(max)
set @InicializarporID = ''

Declare @InicializarporsID varchar(max)
set @InicializarporsID = ''

Declare @InicializarExiste varchar(max)
set @InicializarExiste = ''

Declare @Inicializar varchar(max)
set @Inicializar = ''

Declare @nameSpace varchar(100)
set @nameSpace = 'Acceso.Clases.Datos.RRHH'

Declare @clase varchar(100)
set @clase = 'RRHHFichadas' 

Declare @cCampo varchar(100)
set @cCampo = ''

Declare @cTipo varchar(20)
set @cTipo = ''

Declare @Clavestring varchar(20)
set @Clavestring = '****SACAR****'

Declare @Guardar varchar(max)
set @Guardar = ''
Declare @GuardarEstatico varchar(max)
set @GuardarEstatico = ''

Declare @Eliminar varchar(max)
set @Eliminar = ''

set @Ejecutor = 'using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acceso.Datos.Model;

namespace ' + @nameSpace  

set @Ejecutor = @Ejecutor + ' 
{ 
	public class ' + @clase + 'REP 

	{
		#region Propiedades 
		
			public bool Existe { get; set; }'


		DECLARE Datos CURSOR FOR 
			select b.name,
			CASE 
				WHEN c.name = 'Varchar' THEN 'string'
				WHEN c.name = 'nchar' THEN 'string'
				WHEN c.name = 'datetime' THEN 'DateTime'

				ELSE c.name
			END as tipo
			from sysobjects a 
			inner join syscolumns	b on a.id = b.id and a.name = +  @clase
			inner join systypes		c on b.xtype = c.xtype


		Open Datos
			FETCH NEXT FROM Datos INTO @cCampo, @cTipo
				WHILE @@FETCH_STATUS = 0
					BEGIN
				
					
					--	RECORRER AQUI LAS VARIABLES

						set @Propiedades = @Propiedades + '
						private ' + @cTipo  + ' _' + @cCampo + ';' +
						'
						public ' + @cTipo + ' ' + @cCampo + ' 
							{ 
								get { return _' + @cCampo + '; }' + '
								set { _' + @cCampo + ' = value; }' + '
							}'

					-- Lleno el Inicializar
					set @Inicializar = @Inicializar + 
						'_'+@cCampo + iif(@cTipo='Int', ' = 0 ;', ' = string.Empty ;') + CHAR(10) + 
						CHAR(9) + CHAR(9) + CHAR(9) + CHAR(9)+ CHAR(9) + CHAR(9)+ CHAR(9) 
					----------------------------------------------------
					-- Lleno el InicializarExiste
					set @InicializarExiste = @InicializarExiste + CHAR(9) + CHAR(9) + CHAR(9) +
						'_'+@cCampo + ' = obj.' + @cCampo + ';' + char(10)
						FETCH NEXT FROM Datos INTO @cCampo, @cTipo
					-------------------------------------------------------------------------
					-- Guardar()
					set @Guardar = @Guardar + 'obj.'+ @cCampo + ' = _' + @cCampo + ';' + char(10)+ CHAR(9) + CHAR(9) + CHAR(9)


					END
			
					set @Propiedades = @Propiedades + '
				#endregion ' + '

				public '+ @clase + 'REP () {}'

		CLOSE Datos
		DEALLOCATE Datos

------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------
		set @InicializarporID = 'public void InicializarpornID (int colid)' + '
					{
						using (AccesosEntidades db = new AccesosEntidades())
						{
							var obj = db.' + @clase + '.Where(c => c.id == colid).FirstOrDefault(); 
							if (obj == null)
							{
							Inicializar();
							}
							else
							{' + @InicializarExiste + '
					
								Existe = true;
							}
						}
					}'
------------------------------------------------------------------------------------------------------------
			set @InicializarporsID = 
			'public void InicializarpornsID (string colsid)' + '
			{
				using (AccesosEntidades db = new AccesosEntidades())
				{
					var obj = db.' + @clase + '.Where(c => c.' + @Clavestring + '  == colsid).FirstOrDefault(); 
					if (obj == null)
					{
					Inicializar();
					}
					else
					{' + @InicializarExiste + '
					
					Existe = true;
					}
				}
			}'

			
			set @GuardarEstatico = ' public bool Guardar()
			{

				using (AccesosEntidades db = new AccesosEntidades())
				{
					if (!Existe)
					{
						try
						{' +
						@clase + ' obj = new ' + @clase + '();' + CHAR(10) +
						@Guardar +

						'db.' + @clase + '.Add(obj);
						db.Entry(obj).State = System.Data.Entity.EntityState.Added;
						db.SaveChanges();
						}
						catch (Exception ex)
						{
							System.Windows.Forms.MessageBox.Show("Error Almacenando el usuario " + "\n" + ex.Message);
							return false;
						}
					} 
					else
					{
						try		
						{
						' + @clase + ' obj = new ' + @clase + '();' +
						@Guardar + '
						db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
						db.SaveChanges();
						}
						catch (Exception ex)
                    {

                        System.Windows.Forms.MessageBox.Show("Error Actualizando el registro " + "\n" + ex.Message);
                        return false;
                    }
				}
				return true;
			}
		}'		
------------------------------------------------------------------------------------------------------------

 Set @Eliminar = 'public bool eliminar()
		{
            using (AccesosEntidades db = new AccesosEntidades())
            {
                if (Existe)
                {

                    try
                    {
                        ' + @clase  + ' ousr = new ' + @clase + '();
 
                        ousr.id = _id;
                        db.Entry(ousr).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {

                        System.Windows.Forms.MessageBox.Show("Error Eliminando el usuario " + "\n" + ex.Message);
                        return false;
                    }
                }
            }
            return true;
        }'


------------------------------------------------------------------------------------------------------------
			
set @Inicializar = char(10) + CHAR(9) + CHAR(9) +  CHAR(9) + 'private void Inicializar() 
			{'+ char(10) + @Inicializar + '
				
				Existe = false;
			}' 
			

Print @Ejecutor + @Propiedades + @InicializarporID + @InicializarporsID
Print @GuardarEstatico + @Eliminar + @Inicializar + CHAR(10) + char(9) + '}' + char(10) + '}' 
