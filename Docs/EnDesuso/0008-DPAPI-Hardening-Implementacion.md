# Implementacion MVP — DPAPI ProtectedConfiguration + Hardening

**Fecha:** 2026-03-03
**Objetivo:** Cifrar connectionStrings con DPAPI y eliminar credenciales hardcodeadas

---

## Archivos creados

| Archivo | Descripcion |
|---|---|
| `tools\ConfigProtector\ConfigProtector.csproj` | Proyecto .NET 4.8 (SDK-style) |
| `tools\ConfigProtector\Program.cs` | Helper que cifra/descifra connectionStrings con DPAPI DataProtectionConfigurationProvider |

## Archivos modificados

| Archivo | Cambio |
|---|---|
| **`Common\Global.Datos\SQLServer.cs`** | Eliminado `CadenaporConfiguracion()` con credenciales hardcodeadas (sa/Soporte1/GUS-IDEAPAD). `ActualizarProp()` ahora lanza `InvalidOperationException` si no encuentra ConnectionStrings["Local"]. Usa `SqlConnectionStringBuilder` para parsear InitialCatalog. Eliminado comentario con connection string hardcodeada (linea 15). |
| **`Common\Global.Datos\SQLAccess.cs`** | Misma limpieza: eliminado `CadenaporConfiguracion()`, `ActualizarProp()` con throw, eliminado comentario hardcodeado. Cambiado de leer "ConTocayAnda" a leer "Local". |
| **`Common\Global.DigitalPersona\EnrollmentForm.cs`** | Reemplazado `string SQLString = @"Data Source=NOTE\SQL2014;..."` (linea 97) con lectura de ConnectionStrings["Local"]. |
| **`Common\Global.Datos\ConexionBaseDatos.cs`** | Eliminado comentario con connection string hardcodeada (linea 35). |
| **`Common\Global.DigitalPersona\MainForm.cs`** | Eliminado comentario con connection string hardcodeada (linea 69). |
| **`Fichador\TEntradaSalida\app.config`** | Eliminadas connection strings ksk, ConTocayAnda. Solo queda "Local" con Integrated Security (dev). |
| **`Administrador\Acceso\app.config`** | Eliminadas ksk, Remoto, ConTocayAnda, PRD01, Azure. Solo queda "Local" con Integrated Security (dev). |
| **`InstaladorUnificado\setup.iss`** | Agregado `#define SourceProtector`, entrada en [Files] para ConfigProtector.exe, procedimientos `ProtegerConfigs` y `AplicarACL`, pasos 7-8 en CurStepChanged. |

## Templates (sin cambios necesarios)

- `InstaladorUnificado\fichador.app.config.template` — ya tenia solo "Local" ✅
- `InstaladorUnificado\administrador.app.config.template` — ya tenia solo "local" ✅

---

## Flujo del instalador (post-install actualizado)

```
1. Verificar/crear BD
2. Escribir Fichador config (texto plano con placeholders reemplazados)
3. Escribir Administrador config (texto plano con placeholders reemplazados)
4. Registrar terminal
5. Bootstrap Identity
6. **NUEVO** → ConfigProtector.exe cifra connectionStrings de ambos configs (DPAPI Machine)
7. **NUEVO** → icacls aplica ACL restrictiva (Admins=Full, SYSTEM=Full, Users=RX)
```

---

## Compilacion de ConfigProtector

Antes de compilar el instalador, compilar el helper:

```bash
cd DigitalOnePlus\tools\ConfigProtector
dotnet build -c Release
```

Output: `bin\Release\net48\ConfigProtector.exe`

Requisitos: .NET SDK instalado + .NET Framework 4.8 Targeting Pack.

**Alternativa sin dotnet CLI:** Abrir `ConfigProtector.csproj` en Visual Studio y compilar en Release.

---

## ConfigProtector — Uso

```
# Encriptar (usado automaticamente por el instalador)
ConfigProtector.exe "C:\DigitalPlus\Fichadas\TEntradaSalida.exe.config"

# Desencriptar (solo para soporte)
ConfigProtector.exe "C:\DigitalPlus\Fichadas\TEntradaSalida.exe.config" --decrypt
```

Exit codes:
- `0` = OK (cifrado, ya cifrado, descifrado)
- `1` = Uso incorrecto
- `2` = Archivo no encontrado
- `3` = Seccion connectionStrings no encontrada
- `4` = Error de DPAPI/configuracion

---

## ACL NTFS aplicada

```
icacls "C:\DigitalPlus" /inheritance:r
    /grant *S-1-5-32-544:(OI)(CI)F    ← Administrators: Full
    /grant *S-1-5-18:(OI)(CI)F        ← SYSTEM: Full
    /grant *S-1-5-32-545:(OI)(CI)RX   ← Users: Read & Execute
```

SIDs usados para independencia de idioma del SO (funciona en Windows en espanol, ingles, etc.).

---

## Notas de prueba

### Pre-requisitos
- [ ] Compilar ConfigProtector.exe (`dotnet build -c Release`)
- [ ] Compilar Fichador y Administrador en Release
- [ ] Compilar instalador con Inno Setup

### Test 1: Instalacion limpia
- [ ] Ejecutar `DigitalPlus_Suite_Setup_v1.2.exe` en PC limpia
- [ ] Ingresar datos de empresa, URL, y credenciales SQL
- [ ] Instalacion completa sin errores

### Test 2: Configs cifrados
- [ ] Abrir `C:\DigitalPlus\Fichadas\TEntradaSalida.exe.config` con Notepad
- [ ] Verificar que `<connectionStrings>` contiene `<EncryptedData>` (no texto plano)
- [ ] Abrir `C:\DigitalPlus\Administrador\Acceso.exe.config` con Notepad
- [ ] Verificar que `<connectionStrings>` contiene `<EncryptedData>` (no texto plano)

### Test 3: Apps conectan OK
- [ ] Ejecutar DigitalPlus Fichadas → debe conectar a la BD sin errores
- [ ] Ejecutar DigitalPlus Administrador → debe conectar a la BD sin errores
- [ ] Verificar que las fichadas se guardan en la BD correcta

### Test 4: ACL
- [ ] Ejecutar `icacls "C:\DigitalPlus"` desde CMD
- [ ] Verificar que muestra: Administrators=F, SYSTEM=F, Users=RX
- [ ] Como usuario estandar, intentar editar un .config → debe fallar (acceso denegado)

### Test 5: Sin fallback
- [ ] Renombrar temporalmente el .exe.config del Fichador
- [ ] Ejecutar el Fichador → debe mostrar error claro "No se encontro la cadena de conexion 'Local'..."
- [ ] NO debe conectar silenciosamente a GUS-IDEAPAD

### Test 6: Desencriptar (soporte)
- [ ] Ejecutar: `ConfigProtector.exe "C:\DigitalPlus\Fichadas\TEntradaSalida.exe.config" --decrypt`
- [ ] Abrir el .config → connectionStrings visible en texto plano
- [ ] Volver a cifrar: `ConfigProtector.exe "C:\DigitalPlus\Fichadas\TEntradaSalida.exe.config"`

---

## Hallazgos de seguridad corregidos

| Hallazgo | Severidad | Estado |
|---|---|---|
| Fallback hardcodeado sa/Soporte1 en SQLServer.cs | CRITICA | ✅ Eliminado — throw si falta config |
| Fallback hardcodeado en SQLAccess.cs | CRITICA | ✅ Eliminado — throw si falta config |
| Connection string hardcodeada activa en EnrollmentForm.cs | CRITICA | ✅ Reemplazada con lectura de config |
| Connection strings comentadas en 4 archivos .cs | ALTA | ✅ Eliminadas |
| Multiples connection strings en app.config de desarrollo | ALTA | ✅ Limpiadas (solo Local con Integrated Security) |
| Configs en texto plano en produccion | ALTA | ✅ Cifradas con DPAPI post-install |
| Permisos abiertos en directorio de instalacion | MEDIA | ✅ ACL restrictiva aplicada |

## Nota sobre MessageBox.Show(ex.Message)

Los `catch` en SQLServer.cs muestran `ex.Message` via MessageBox. Los mensajes de `SqlException` **no incluyen la connection string ni el password** — solo incluyen mensajes como "Login failed for user 'sa'" o "Cannot open database". Esto es aceptable y no requiere cambio.
