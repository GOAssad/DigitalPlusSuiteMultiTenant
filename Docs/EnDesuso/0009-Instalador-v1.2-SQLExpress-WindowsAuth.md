# Instalador v1.2 — SQL Express incluido + Windows Auth

**Fecha:** 2026-03-04
**Archivo:** `InstaladorUnificado\setup.iss`

## Contexto

El instalador v1.1 requeria que el usuario ingresara manualmente servidor SQL, base de datos, usuario y password. Esto implicaba que el cliente ya tuviera SQL Server instalado y configurado, lo cual generaba friccion en la instalacion.

En v1.2 se simplifica al maximo: el instalador incluye SQL Server Express 2019 y lo instala automaticamente si no detecta una instancia existente. La conexion usa Windows Authentication, eliminando la necesidad de credenciales SQL.

## Cambios respecto a v1.1

| Aspecto | v1.1 | v1.2 |
|---|---|---|
| Paginas del wizard | 3 (Empresa, URL, BD) | 2 (Empresa, URL) |
| SQL Server | El usuario lo instala | Incluido (SQL Express 2019) |
| Autenticacion SQL | `User Id=sa;Password=...` | `Integrated Security=True` |
| Servidor SQL | Configurable por el usuario | Fijo: `.\SQLEXPRESS` |
| Base de datos | Configurable | Fija: `DigitalPlus` |
| Requiere internet | Si (para descargar SQL) | No (todo offline) |
| Tamano instalador | ~pocos MB | ~250MB+ |
| `ArchitecturesAllowed` | No especificado | `x64compatible` (solo 64-bit) |

## Flujo de instalacion v1.2

```
1. Verificar .NET 4.8 y arquitectura 64-bit
2. Wizard: Bienvenida
3. Wizard: Nombre de empresa
4. Wizard: URL del portal web
5. Wizard: Directorio de instalacion
6. Wizard: Opciones (accesos directos, auto-inicio)
7. Copiar archivos
8. [Post-install] Detectar SQL Server en registry
9. [Post-install] Si no hay SQL → instalar SQL Express 2019 silenciosamente
10. [Post-install] Crear BD DigitalPlus (si no existe)
11. [Post-install] Escribir configs (Fichador + Administrador)
12. [Post-install] Registrar terminal en GRALTerminales
13. [Post-install] Bootstrap Identity (usuarios admin/user)
14. [Post-install] Cifrar connectionStrings con DPAPI
15. [Post-install] Aplicar ACL NTFS restrictiva
16. Instalar driver DigitalPersona RTE
17. Pantalla final
```

## Deteccion de SQL Server

La funcion `DetectarSQLServer` busca en el registry:

```
HKLM\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL
```

Si hay al menos un valor (nombre de instancia), retorna `True`. Esto detecta cualquier edicion de SQL Server (Express, Standard, Developer, etc.).

La funcion `NecesitaInstalarSQL` se usa como `Check:` en la seccion `[Files]` para evitar extraer el instalador de SQL Express (249MB) si ya hay una instancia SQL instalada.

## Instalacion silenciosa de SQL Express 2019

Parametros utilizados:

```
SQLEXPR_x64_ENU.exe
  /Q                                    # Completamente silencioso
  /IACCEPTSQLSERVERLICENSETERMS         # Acepta licencia
  /ACTION="Install"                     # Accion: instalar
  /FEATURES=SQLENGINE                   # Solo motor de BD
  /INSTANCENAME="SQLEXPRESS"            # Nombre de instancia estandar
  /SQLSVCACCOUNT="NT AUTHORITY\SYSTEM"  # Cuenta de servicio
  /SQLSYSADMINACCOUNTS="BUILTIN\Administrators"  # Admins como sysadmin
  /TCPENABLED=1                         # Habilitar TCP/IP
  /NPENABLED=1                          # Habilitar Named Pipes
  /SECURITYMODE=SQL                     # Habilitar modo mixto
  /UPDATEENABLED=0                      # No buscar actualizaciones
```

Exit codes:
- `0` = exito
- `3010` = exito, requiere reinicio
- Otros = error (se muestra mensaje pero se intenta continuar)

**Tiempo estimado:** 3-10 minutos dependiendo del hardware.

## Connection string

Fija para todas las configs:

```
Data Source=.\SQLEXPRESS;Initial Catalog=DigitalPlus;Integrated Security=True;
```

Para ADODB (registro de terminal):

```
Provider=SQLOLEDB;Data Source=.\SQLEXPRESS;Initial Catalog=DigitalPlus;Integrated Security=SSPI;
```

## Creacion de BD

El script PowerShell (`BuildCrearBDScript`):

1. Conecta a `master` con Windows Auth
2. Verifica si `DigitalPlus` ya existe → si existe, sale con exit 0
3. Ejecuta `CREATE DATABASE [DigitalPlus]`
4. Lee `crear_bd.sql` (el script de schema)
5. Reemplaza `[DigitalPlus]` por el nombre real
6. Divide por `GO` y ejecuta cada batch
7. Salta: `CREATE DATABASE`, `ALTER DATABASE`, `USE`, `CREATE USER`, `ALTER ROLE`, `FULLTEXTSERVICEPROPERTY`

## Archivo SQL Express incluido

```
Ruta:    InstaladorUnificado\Prerequisites\SQLEXPR_x64_ENU.exe
Tamano:  ~249 MB
Version: SQL Server Express 2019
Origen:  https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SQLEXPR_x64_ENU.exe
```

## Requisitos del sistema

- Windows 10/11 64-bit (o Windows Server 2016+)
- .NET Framework 4.8+
- ~4.5 GB de espacio en disco (SQL Express + DigitalPlus)
- Privilegios de administrador

## Seguridad (sin cambios respecto a v1.1)

- DPAPI Machine-level para cifrar connectionStrings
- ACL NTFS: Admins/SYSTEM=Full, Users=RX
- ConfigProtector.exe incluido en `{app}\tools\`

## Notas

- Si el usuario ya tiene SQL Server (cualquier edicion), no se instala SQL Express
- La instancia debe llamarse `SQLEXPRESS` para que las connection strings funcionen. Si el usuario tiene una instancia con otro nombre, debera ajustar manualmente los .config
- El instalador ahora solo soporta 64-bit (`ArchitecturesAllowed=x64compatible`)
- El Azure provisioning (tabla ActivationCodes, Function App) queda disponible para uso futuro (modo cloud / v1.3)
