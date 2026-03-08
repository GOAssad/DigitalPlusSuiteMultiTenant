# Manual de Instalacion — DigitalPlus Suite

## Que instala este programa

Este instalador configura en su equipo las dos aplicaciones del sistema DigitalPlus:

| Aplicacion | Funcion |
|---|---|
| **DigitalPlus Fichadas** | Registro de entrada y salida del personal mediante huella digital |
| **DigitalPlus Administrador** | Gestion de legajos, horarios, reportes e incidencias |

Ambas aplicaciones comparten la misma base de datos y se configuran en un solo paso.

---

## Requisitos previos

Antes de ejecutar el instalador, asegurese de contar con:

1. **Windows 7 SP1 o superior** (se recomienda Windows 10/11)
2. **Microsoft .NET Framework 4.8** instalado
   - Si no lo tiene, el instalador le avisara y podra descargarlo desde:
     https://dotnet.microsoft.com/download/dotnet-framework/net48
3. **SQL Server** accesible desde este equipo (puede ser local o en red)
   - Necesita conocer: nombre o IP del servidor, usuario y contrasena SQL
4. **Lector de huellas DigitalPersona** conectado (para la app de Fichadas)
5. **Permisos de administrador** en el equipo

---

## Paso a paso de la instalacion

### Paso 1 — Ejecutar el instalador

Haga doble clic en el archivo:

```
DigitalPlus_Suite_Setup_v1.0.exe
```

Si Windows le pide confirmacion de permisos de administrador, presione **Si**.

---

### Paso 2 — Pantalla de bienvenida

Se muestra la pantalla de bienvenida del instalador. Presione **Siguiente** para continuar.

---

### Paso 3 — Nombre de la empresa

Ingrese el nombre de su empresa tal como desea que aparezca en ambas aplicaciones.

- Ejemplo: `Transportes Lopez S.A.`
- Por defecto aparece "Mi Empresa"; reemplacelo con el nombre real.

Presione **Siguiente**.

---

### Paso 4 — URL del portal web

Ingrese la direccion web del portal DigitalPlus.

- Si su empresa tiene el portal en la nube, ingrese la URL proporcionada (ejemplo: `https://miempresa.azurewebsites.net/`)
- Si el portal esta en la red local, ingrese la IP (ejemplo: `http://192.168.0.100/`)
- Si no utiliza el portal web, deje el valor por defecto

Esta URL se usa cuando presiona el boton "DigitalPlusWeb" dentro del Administrador.

Presione **Siguiente**.

---

### Paso 5 — Carpeta de instalacion

Elija donde desea instalar las aplicaciones.

- **Ruta por defecto:** `C:\DigitalPlus`
- Puede cambiarla presionando **Examinar...** y eligiendo otra carpeta

Dentro de la carpeta elegida se crearan dos subcarpetas:

```
C:\DigitalPlus\
├── Fichadas\          ← Aplicacion de fichadas
└── Administrador\     ← Aplicacion de administracion
```

Presione **Siguiente**.

---

### Paso 6 — Configuracion de base de datos

Complete los datos de conexion a su servidor SQL Server:

| Campo | Que ingresar | Ejemplo |
|---|---|---|
| **Servidor SQL** | Nombre o IP del servidor (opcionalmente con puerto) | `SERVIDOR01` o `192.168.0.10,1433` |
| **Base de datos** | Nombre de la base de datos | `DigitalPlus` (dejar por defecto) |
| **Usuario SQL** | Usuario con permisos de administrador en SQL | `sa` |
| **Contrasena SQL** | Contrasena del usuario SQL | *(su contrasena)* |

Una vez completados los datos, presione el boton **Verificar conexion y BD**.

El resultado sera uno de estos:

- **"Conexion exitosa. La base de datos ya existe."** — Se usara la base existente sin modificarla.
- **"Conexion exitosa. La base de datos no existe."** — Se creara automaticamente durante la instalacion.
- **"Error: ..."** — Revise los datos ingresados y vuelva a intentar.

Solo podra continuar cuando la verificacion sea exitosa. Presione **Siguiente**.

---

### Paso 7 — Opciones adicionales

Seleccione las opciones que desee:

**Fichadas:**
- Crear acceso directo en el Escritorio *(recomendado)*
- Iniciar Fichadas automaticamente con Windows *(util para terminales de fichado)*

**Administrador:**
- Crear acceso directo en el Escritorio *(recomendado)*
- Iniciar Administrador automaticamente con Windows

Presione **Siguiente**.

---

### Paso 8 — Listo para instalar

Se muestra un resumen. Presione **Instalar** para comenzar.

Durante la instalacion, el programa:

1. Copia los archivos de ambas aplicaciones
2. Instala el driver del lector de huellas DigitalPersona
3. Crea la base de datos (si no existia)
4. Configura ambas aplicaciones con los datos ingresados
5. Registra el equipo como terminal de fichado
6. Crea los usuarios iniciales del sistema

Este proceso puede demorar entre 1 y 3 minutos.

---

### Paso 9 — Instalacion finalizada

Al terminar, puede elegir ejecutar cualquiera de las dos aplicaciones inmediatamente:

- Ejecutar DigitalPlus Fichadas ahora
- Ejecutar DigitalPlus Administrador ahora

Presione **Finalizar**.

---

## Donde se guardan las aplicaciones

Con la ruta por defecto, los archivos quedan en:

```
C:\DigitalPlus\
│
├── Fichadas\
│   ├── TEntradaSalida.exe           ← Ejecutable de Fichadas
│   ├── TEntradaSalida.exe.config    ← Configuracion (connection string, empresa)
│   └── (DLLs del sistema)
│
└── Administrador\
    ├── Acceso.exe                   ← Ejecutable del Administrador
    ├── Acceso.exe.config            ← Configuracion (connection string, empresa, URL web)
    └── (DLLs del sistema)
```

---

## Como ejecutar las aplicaciones

### Desde el Escritorio

Si eligio crear accesos directos, encontrara en el escritorio:

- **DigitalPlus Fichadas** — abre la aplicacion de fichado
- **DigitalPlus Administrador** — abre la aplicacion de administracion

### Desde el Menu Inicio

Busque la carpeta **DigitalPlus** en el menu Inicio:

- DigitalPlus Fichadas
- DigitalPlus Administrador
- Desinstalar DigitalPlus Suite

### Directamente desde la carpeta

Navegue hasta la carpeta de instalacion y haga doble clic en:

- `C:\DigitalPlus\Fichadas\TEntradaSalida.exe`
- `C:\DigitalPlus\Administrador\Acceso.exe`

---

## Usuarios iniciales del sistema

El instalador crea automaticamente dos cuentas de usuario:

| Usuario | Contrasena | Rol | Uso |
|---|---|---|---|
| `admin` | `Admin@1234` | Administrador | Acceso completo al sistema y portal web |
| `user` | `User@1234` | Registrado | Acceso basico |

**Importante:** Cambie las contrasenas despues del primer inicio de sesion.

---

## Como desinstalar

1. Abra **Configuracion de Windows** > **Aplicaciones** > **Aplicaciones instaladas**
2. Busque **DigitalPlus Suite**
3. Presione **Desinstalar**

Esto elimina ambas aplicaciones, los accesos directos y las entradas del registro. La base de datos NO se elimina.

Tambien puede desinstalar desde el Menu Inicio: **DigitalPlus > Desinstalar DigitalPlus Suite**.

---

## Solucion de problemas frecuentes

### El instalador dice que falta .NET Framework 4.8

Descargue e instale .NET Framework 4.8 desde:
https://dotnet.microsoft.com/download/dotnet-framework/net48

Reinicie el equipo y vuelva a ejecutar el instalador.

### No se puede conectar al servidor SQL

- Verifique que el servicio SQL Server este en ejecucion en el servidor
- Verifique que el firewall permita conexiones en el puerto de SQL Server (por defecto 1433)
- Confirme que el usuario y contrasena SQL son correctos
- Si usa una instancia nombrada, indique el nombre completo (ejemplo: `SERVIDOR01\SQLEXPRESS`)

### El lector de huellas no funciona

- Asegurese de que el lector DigitalPersona esta conectado al puerto USB
- El driver se instala automaticamente durante la instalacion
- Si el driver no se instalo, busque en `C:\DigitalPlus\` o contacte a soporte

### Error al crear la base de datos

Si la creacion automatica fallo:

1. Revise el archivo de error en `%TEMP%\dp_crear_bd_error.txt`
2. Las aplicaciones se instalaron correctamente; solo falta la base de datos
3. Puede crear la base de datos manualmente con SQL Server Management Studio usando el script SQL proporcionado por su proveedor

### Las aplicaciones no inician

- Verifique que SQL Server este accesible desde este equipo
- Abra el archivo `.config` correspondiente y confirme que la connection string es correcta
- Verifique que la base de datos existe y tiene las tablas del sistema
