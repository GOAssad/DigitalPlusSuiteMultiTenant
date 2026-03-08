# Guía Técnica del Instalador — DigitalOnePlus Fichador

**Fecha:** 2026-02-28
**Herramienta:** Inno Setup 6.x
**Script:** `Fichador\Installer\setup.iss`

---

## 1. Requisitos para Generar el Instalador

| Requisito | Versión / Detalle |
|---|---|
| Inno Setup | 6.x (descarga: https://jrsoftware.org/isinfo.php) |
| Visual Studio | 2019 o superior |
| .NET Framework 4.8 SDK | Para compilar el proyecto |
| Proyecto compilado en Release | `TEntradaSalida\bin\Release\` debe estar actualizado |
| SDK DigitalPersona | Presente en `Digital-Persona-SDK-master\` |

---

## 2. Estructura del Instalador

```
Fichador\
├── Installer\
│   ├── setup.iss              ← Script principal de Inno Setup
│   ├── app.config.template    ← Plantilla de configuración (sin credenciales)
│   └── Output\                ← Carpeta donde se genera el .exe (gitignore)
```

---

## 3. Cómo Generar el Instalador (paso a paso)

### Paso 1 — Compilar el proyecto en Release

Abrir `TEntradaSalida.sln` en Visual Studio y compilar en modo **Release | Any CPU**:

```
Build → Configuration Manager → Release | Any CPU
Build → Build Solution  (Ctrl+Shift+B)
```

Verificar que exista y esté actualizado:
```
TEntradaSalida\bin\Release\TEntradaSalida.exe
```

### Paso 2 — Abrir el script en Inno Setup

1. Abrir **Inno Setup Compiler**
2. `File → Open` → seleccionar `Fichador\Installer\setup.iss`

### Paso 3 — Compilar el instalador

```
Build → Compile  (F9)
```

El instalador generado quedará en:
```
Fichador\Installer\Output\DigitalOnePlus_Fichador_Setup_v1.0.exe
```

### Paso 4 — Distribuir

Entregar el archivo `.exe` generado al cliente.

---

## 4. Cómo Actualizar la Versión

Cuando hay una nueva versión del sistema:

1. **Compilar** el proyecto en Release (Paso 1 anterior).
2. Abrir `setup.iss` y actualizar el número de versión:
   ```pascal
   #define AppVersion "1.1"   ← cambiar aquí
   ```
3. Recompilar el instalador (Paso 3).
4. El nuevo instalador detectará si hay una versión anterior instalada y la reemplazará correctamente (`AppId` es el identificador único — no cambiarlo entre versiones).

---

## 5. Estructura del Script (setup.iss)

### Secciones principales

| Sección | Descripción |
|---|---|
| `[Setup]` | Metadatos, rutas, icono, compresión |
| `[Languages]` | Idioma español (`Spanish.isl`) |
| `[Tasks]` | Acceso directo escritorio, autoarranque |
| `[Files]` | Archivos a copiar en la instalación |
| `[Icons]` | Accesos directos a crear |
| `[Registry]` | Entrada de autoarranque (opcional) |
| `[Run]` | Instala RTE DigitalPersona + opción de abrir app |
| `[Code]` | Pascal Script: página DB, test conexión, escritura config, registro terminal |

### Variables `#define` al inicio del script

```pascal
#define AppVersion    "1.0"          ← versión que aparece en el instalador
#define SourceBin     "..\TEntradaSalida\bin\Release"  ← fuente de archivos
#define SourceRTE_x86 "..\..\Digital-Persona-SDK-master\RTE\Install"
#define SourceRTE_x64 "..\..\Digital-Persona-SDK-master\RTE\Install\x64"
#define SourceSQL     "..\..\Datos\2026-02-28 - Script.sql"  ← script de creación de BD
```

> Las rutas son **relativas al archivo setup.iss**. Al publicar una nueva versión del script SQL, actualizar `SourceSQL` con el nuevo nombre de archivo.

---

## 6. Template de Configuración (app.config.template)

**Archivo:** `Installer\app.config.template`

Contiene un único connection string con el placeholder `{{CONNECTION_STRING}}`:

```xml
<add name="ConTocayAnda" connectionString="{{CONNECTION_STRING}}" providerName="System.Data.SqlClient"/>
```

Durante la post-instalación, el Pascal Script reemplaza `{{CONNECTION_STRING}}` con la cadena construida a partir de los datos ingresados por el usuario:

```
Data Source=SERVIDOR;Initial Catalog=BD;User Id=USUARIO;Password=PASS;
```

**Importante:** Este template reemplaza al `TEntradaSalida.exe.config` del directorio `bin\Release`, que contiene las credenciales de desarrollo. El archivo de desarrollo **nunca se incluye** en el instalador.

---

## 7. Flujo de Instalación

```
1. InitializeSetup()
   └── Verifica .NET 4.8 (bloqueante si no está)

2. Wizard pages:
   ├── Bienvenida
   ├── Seleccionar carpeta destino
   ├── Configuración de Base de Datos  ← página custom
   │   ├── Campos: Servidor, Base de datos, Usuario, Contraseña
   │   ├── Botón "Verificar conexion y BD":
   │   │     - Conecta a [master] con las credenciales ingresadas
   │   │     - Consulta sys.databases para el nombre de BD ingresado
   │   │     - BD existe     → mensaje verde  (bBDExiste = True)
   │   │     - BD no existe  → mensaje amarillo (bBDExiste = False)
   │   │     - Error          → mensaje rojo, no puede continuar
   │   └── Bloquea "Siguiente" hasta verificación exitosa
   ├── Accesos directos / opciones
   └── Listo para instalar

3. Instalación de archivos
   ├── Todos los archivos de bin\Release (sin .pdb, sin config de dev)
   ├── app.config.template → TEntradaSalida.exe.config (con placeholder)
   └── Script SQL → {tmp}\crear_bd.sql (se borra al finalizar)

4. [Run] - Instalar RTE DigitalPersona
   └── DP_RTE_x86.exe /quiet  (en sistemas x86)
   └── DP_RTE_x64.exe /quiet  (en sistemas x64)

5. CurStepChanged(ssPostInstall)
   ├── Si bBDExiste = False → CrearBaseDeDatos()
   │     - Genera {tmp}\crear_bd.ps1 con los datos del wizard
   │     - PowerShell lee crear_bd.sql (UTF-16 LE)
   │     - Sustituye [DigitalPlus] por el nombre ingresado
   │     - Ejecuta cada batch (dividido por GO) contra master
   │     - Si falla: muestra error pero no aborta la instalación
   ├── EscribirConfig()    → reemplaza {{CONNECTION_STRING}} en .config
   └── RegistrarTerminal() → EXEC GRALTerminales_SP (no aborta si falla)

6. Finalización
   └── Opción de ejecutar la app
```

### ¿Por qué PowerShell para crear la BD?

El script SQL (`2026-02-28 - Script.sql`) tiene dos particularidades que impiden ejecutarlo directamente desde ADODB:

1. **Codificación UTF-16 LE** — ADODB no puede leer el archivo directamente. PowerShell usa `[System.Text.Encoding]::Unicode` para leerlo correctamente.
2. **Separadores `GO`** — `GO` es un comando del cliente SQL (SSMS/sqlcmd), no T-SQL. ADODB no lo entiende. PowerShell divide el script por `GO` y ejecuta cada batch por separado.

### Sustitución del nombre de BD en el script

El script tiene hardcodeado `DigitalPlus` como nombre de BD. PowerShell reemplaza:

| Patrón original | Reemplazado por |
|---|---|
| `[DigitalPlus]` | `[nombre_ingresado]` |
| `DigitalPlus.mdf` | `nombre_ingresado.mdf` |
| `DigitalPlus_log.ldf` | `nombre_ingresado_log.ldf` |
| `N'DigitalPlus'` | `N'nombre_ingresado'` |

---

## 8. Registro de Terminal en GRALTerminales

Al finalizar la instalación, el script ejecuta el SP `GRALTerminales_SP` con:

| Parámetro | Valor |
|---|---|
| `@sTerminalID` | `COMPUTERNAME` (nombre del equipo Windows) |
| `@sDescripcion` | `"Terminal " + COMPUTERNAME` |
| `@sSucursalID` | `""` (vacío — debe completarse manualmente desde el sistema de gestión) |
| `@sMensajeBienVenida` | `"Bienvenido, Nmu()!"` |
| `@sIPV4` | `""` (vacío) |

> El campo `@sSucursalID` queda vacío. El operador debe asignarlo desde el módulo de administración del sistema.
> Si el SP falla por cualquier motivo (terminal ya existe, permisos, etc.), la instalación **no se interrumpe**.

---

## 9. Detección de DigitalPersona RTE

El script **siempre ejecuta** el instalador del RTE durante la instalación. El RTE es un MSI, por lo que si ya está instalado, el sistema detecta la versión existente y no reinstala.

Comportamiento esperado:
- **No instalado:** instala el RTE correctamente.
- **Ya instalado misma versión:** sin efecto (MSI detecta y saltea).
- **Ya instalado versión anterior:** actualiza.

---

## 10. Archivos Generados / Gitignore

Agregar a `.gitignore`:
```
Installer/Output/
```

Los archivos `.exe` del instalador generado no deben subirse al repositorio. Generar bajo demanda antes de cada distribución.

---

## 11. Solución de Problemas de Compilación del Script

| Error | Causa | Solución |
|---|---|---|
| `Source file does not exist` | No se compiló el proyecto en Release | Compilar primero en VS |
| `File not found: Huellaksk.ico` | Ruta del icono incorrecta | Verificar que exista `TEntradaSalida\Resources\Huellaksk.ico` |
| `File not found: DP_RTE_x86.exe` | SDK no está en la ruta esperada | Verificar ruta `Digital-Persona-SDK-master\RTE\Install\` |
| `File not found: Spanish.isl` | Idioma español no instalado en Inno Setup | Instalar Inno Setup con idiomas adicionales |

---

*Fin de Guía Técnica*
