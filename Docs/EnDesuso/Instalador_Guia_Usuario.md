# Guía de Instalación — DigitalOnePlus Fichador

**Versión:** 1.0
**Fecha:** 2026-02-28

---

## Antes de Comenzar

Tenga a mano los siguientes datos antes de iniciar la instalación:

| Dato | Descripción | Ejemplo |
|---|---|---|
| Servidor SQL | IP o nombre del servidor de base de datos | `192.168.0.11` |
| Base de datos | Nombre de la base de datos del sistema | `Tocayanda` |
| Usuario SQL | Usuario con acceso a la base de datos | `sa` |
| Contraseña SQL | Contraseña del usuario SQL | _(consultar al administrador)_ |

> Si no tiene estos datos, **no continúe**. Solicítelos al administrador del sistema antes de instalar.

---

## Requisitos del Equipo

| Requisito | Detalle |
|---|---|
| Sistema operativo | Windows 7 SP1 o superior (recomendado Windows 10/11) |
| .NET Framework | Versión 4.8 o superior |
| Permisos | **Administrador local** del equipo |
| Lector de huellas | DigitalPersona uAreU 4500 (conectar **después** de instalar) |
| Conexión a red | El equipo debe poder acceder al servidor SQL durante la instalación |

---

## Pasos de Instalación

### Paso 1 — Ejecutar el instalador

Hacer doble clic en:
```
DigitalOnePlus_Fichador_Setup_v1.0.exe
```

Si Windows muestra una advertencia de seguridad (Control de cuentas de usuario), hacer clic en **Sí**.

---

### Paso 2 — Pantalla de bienvenida

Hacer clic en **Siguiente**.

---

### Paso 3 — Carpeta de instalación

La carpeta predeterminada es:
```
C:\Program Files\DigitalOnePlus\TEntradaSalida\
```

Se puede cambiar si es necesario. En la mayoría de los casos dejar el valor predeterminado y hacer clic en **Siguiente**.

---

### Paso 4 — Configuración de Base de Datos ⚠️ Paso importante

Esta pantalla es **la más importante** de la instalación.

1. Completar los campos:

   - **Servidor SQL:** dirección IP o nombre del servidor
     Ejemplo: `192.168.0.11` o `servidor\instancia` o `servidor,1433`
   - **Nombre de la base de datos:** nombre de la BD a usar (existente o nueva)
     Ejemplo: `DigitalPlus`
   - **Usuario SQL:** nombre de usuario
     Ejemplo: `sa`
   - **Contraseña SQL:** contraseña del usuario

2. Hacer clic en **Verificar conexion y BD**.

3. Esperar el resultado:
   - ✅ **"OK - Base de datos encontrada"** → la BD ya existe, se usará la existente. Puede continuar.
   - 🟡 **"OK - La base de datos no existe. Será creada"** → la BD se creará automáticamente durante la instalación. Puede continuar.
   - ❌ **"Error: ..."** → verificar los datos y volver a intentar.

> **No se puede continuar sin una verificación exitosa.** El botón "Siguiente" queda bloqueado hasta entonces.

> Si la base de datos **no existe**, el instalador la creará automáticamente con todas las tablas y configuración necesaria. Este proceso puede tardar **2 a 5 minutos** dependiendo del servidor.

**Errores comunes en este paso:**

| Mensaje | Causa probable | Solución |
|---|---|---|
| Timeout / no responde | El servidor no es accesible desde este equipo | Verificar la IP y que el equipo esté en la misma red |
| Login failed | Usuario o contraseña incorrectos | Verificar las credenciales con el administrador |
| No se pudo ejecutar PowerShell | Restricción de seguridad del equipo | Contactar al administrador de IT |

---

### Paso 5 — Accesos directos

Seleccionar las opciones deseadas:
- ☑ **Crear acceso directo en el Escritorio** (recomendado)
- ☐ Iniciar automáticamente con Windows (solo si la terminal debe arrancar sola)

Hacer clic en **Siguiente**.

---

### Paso 6 — Listo para instalar

Revisar el resumen y hacer clic en **Instalar**.

Durante la instalación verá:
- Copia de archivos del sistema
- Instalación del driver DigitalPersona (puede tardar 1-2 minutos)
- Configuración de la conexión a base de datos
- Registro de la terminal en el sistema

---

### Paso 7 — Finalización

Cuando aparezca la pantalla de finalización:

1. Dejar marcado **"Ejecutar DigitalOnePlus Fichador ahora"** si desea probar inmediatamente.
2. Hacer clic en **Finalizar**.

---

### Paso 8 — Conectar el lector de huellas

**Ahora** conectar el lector DigitalPersona uAreU 4500 al puerto USB.

Windows instalará automáticamente los drivers (el software necesario ya fue instalado en el paso 6).

Si el lector ya estaba conectado antes de instalar, desconectarlo y volver a conectarlo para que Windows lo reconozca correctamente.

---

## Verificación Post-Instalación

Al abrir la aplicación por primera vez:

1. Debe aparecer la pantalla principal del fichador.
2. La **sucursal** debe mostrarse en pantalla.
   - Si aparece en blanco, la terminal fue registrada pero aún no tiene sucursal asignada. Contactar al administrador para asignarla desde el sistema de gestión.
3. Al apoyar el dedo en el lector:
   - El semáforo debe ponerse **amarillo** (leyendo).
   - Si el empleado está registrado: **verde** (fichaje registrado).
   - Si no se reconoce: **rojo**.

---

## Desinstalar el Sistema

1. Ir a **Panel de Control → Programas → Desinstalar un programa**.
2. Seleccionar **DigitalOnePlus Fichador**.
3. Hacer clic en **Desinstalar** y seguir los pasos.

También puede desinstalar desde:
- **Menú Inicio → DigitalOnePlus Fichador → Desinstalar DigitalOnePlus Fichador**

> La desinstalación elimina los archivos del programa y los accesos directos. **No elimina la base de datos** ni los registros de fichadas.

---

## Contacto y Soporte

Ante cualquier inconveniente durante la instalación, contactar al administrador del sistema con la siguiente información:
- Mensaje de error exacto (captura de pantalla si es posible)
- Nombre del equipo (aparece en la barra de título del error)
- Sistema operativo instalado

---

*Fin de Guía de Usuario*
