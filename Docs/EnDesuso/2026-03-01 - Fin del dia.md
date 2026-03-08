# Resumen de sesion — 2026-03-01 (continuacion 2026-03-02)

## Lo que se hizo

### 1. Nuevo script SQL completo (`Datos\DigitalPlus_Script.sql`)

Se recreo el script SQL desde cero en UTF-8 (el anterior era UTF-16 LE). Incluye:

- **Todas las funciones:** ConvertiraFecha, DiaDeSemana, PrimeraEntrada, UltimaSalida
- **24 tablas** con la columna `Foto varbinary(max)` en Legajos (que faltaba)
- **Vista:** EscritorioLegajosHuellasView
- **Indices y FK** completos
- **SPs de escritorio:** EscritorioFichadasSPSALIDA, EscritorioLegajoActualizar (con @Foto), EscritorioLegajosHuellasActualizar, RRHHLegajos_DeleteTodo
- **SP nuevo:** `GRALTerminales_SP` — registra terminales (upsert) desde el instalador
- **SPs web:** WebConsolidado, WebControlAcceso, WebHorasExtras, WebLlegadaTarde, WebAusencias, WebDashBoard*, WebCalculo*
- **Datos por defecto:** Sucursal, Sector, Categoria, Horario, Dias, Dedos
- **Identity:** INSERT de roles (ADMINISTRADOR, Registrado) y usuarios (admin/Admin@1234, user/User@1234) con password hashes PBKDF2
- **Tabla Noticias corregida:** Columnas `Nombre`, `Detalle`, `FechaDesde`, `FechaHasta`, `Privado` (el script original tenia `Titulo`, `Contenido`, `FechaPublicacion`, `Activa`, `UsuarioId` que no coincidian con la entidad EF Core `Noticia.cs`)

### 2. Nombre de empresa en Fichadas (`FrmFichar`)

- **FrmFichar.Designer.cs:** Agregado label `lblEmpresa` (Segoe UI 11pt bold, RoyalBlue) entre el header y la fecha. Controles desplazados 25px hacia abajo. Form height 570 -> 595.
- **FrmFichar.cs:** Lee `ConfigurationManager.AppSettings["NombreEmpresa"]` al cargar. Muestra en `lblEmpresa.Text` y en el titulo de la ventana (`"DigitalPlus Fichadas - NombreEmpresa"`).
- **TEntradaSalida.csproj:** Agregada referencia a `System.Configuration`.
- Titulo cambiado de "Fichador Kosiuko" a "DigitalPlus Fichadas".
- Compilacion exitosa.

### 3. Bug critico: Fichador guardaba en BD equivocada

**Sintoma:** La app Fichador guardaba fichadas en la BD original DigitalPlus en vez de la BD nueva creada por el instalador, a pesar de que TEntradaSalida.exe.config tenia la connection string correcta.

**Causa raiz:** Mismatch en el nombre de la connection string.
- `SQLServer.ActualizarProp()` en `Global.Datos\SQLServer.cs:457` busca `ConnectionStrings["Local"]`
- `fichador.app.config.template` tenia `name="ConTocayAnda"` (el Administrador tenia `name="local"` y funcionaba)
- Al no encontrar "Local", caia al fallback `CadenaporConfiguracion()` que usaba hardcoded `GUS-IDEAPAD/DigitalPlus`

**Fix:** En `InstaladorUnificado\fichador.app.config.template` se cambio `name="ConTocayAnda"` a `name="Local"`.

### 4. Instalador actualizado a v1.1

Cambios en `InstaladorUnificado\setup.iss`:
- Referencia al nuevo script: `DigitalPlus_Script.sql` (era `2026-02-28 - Script.sql`)
- Encoding del lector PS cambiado de `[System.Text.Encoding]::Unicode` a `UTF8`
- Version bumpeada a 1.1
- Instalador compilado: `Output\DigitalPlus_Suite_Setup_v1.1.exe`

### 5. Analisis arquitectonico de DigitalPlus Web (Blazor)

Se realizo un analisis completo de la app web Blazor en `DigitalPlusWeb_Claude\DigitalPlus\`:
- Blazor Server .NET 7.0, EF Core + Dapper hibrido
- 23 DbSets, 14 repositorios, ~90 paginas Razor
- Connection string hardcodeada en Program.cs (bloqueante para multi-tenant)
- Reporte generado en: `DigitalPlusWeb_Claude\Documentacion\Reporte_Arquitectura_DigitalPlusWeb.md`

**Decision del usuario:** No modificar la app actual para multi-tenant. Prefiere:
- **Corto plazo:** Publicar una instancia por empresa (solo cambia la connection string en appsettings.json)
- **Largo plazo:** Desarrollar una nueva app multi-tenant desde cero

---

## Estado de archivos modificados

| Archivo | Cambio |
|---|---|
| `Datos\DigitalPlus_Script.sql` | NUEVO — Script SQL completo UTF-8 con Foto, GRALTerminales_SP, Identity, Noticias corregida |
| `Fichador\TEntradaSalida\uAreu\FrmFichar.cs` | Agregado: using System.Configuration, lectura de NombreEmpresa |
| `Fichador\TEntradaSalida\uAreu\FrmFichar.Designer.cs` | Agregado: lblEmpresa, controles reposicionados, form height +25px |
| `Fichador\TEntradaSalida\TEntradaSalida.csproj` | Agregada referencia System.Configuration |
| `InstaladorUnificado\setup.iss` | Script SQL nuevo, encoding UTF8, version 1.1 |
| `InstaladorUnificado\fichador.app.config.template` | name="ConTocayAnda" -> name="Local" |
| `DigitalPlusWeb_Claude\Documentacion\Reporte_Arquitectura_DigitalPlusWeb.md` | NUEVO — Reporte de arquitectura |

## Pendientes para proxima sesion

- Probar instalador v1.1 end-to-end (instalar, fichar, verificar BD correcta)
- Probar que la web funcione apuntando a la BD nueva (error Noticias deberia estar resuelto)
- Si se decide publicar para un segundo cliente: duplicar deployment web con nueva connection string
- Planificacion del nuevo sistema multi-tenant (si el usuario quiere avanzar con eso)
