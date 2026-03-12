# DIGITALPLUS - Manual del Usuario

**Version:** 4.0
**Fecha:** 2026-03-11

---

## INDICE

1. [Introduccion](#1-introduccion)
2. [Instalacion](#2-instalacion)
   - [Instalador Completo (Local)](#21-instalador-completo-local)
   - [Instalador Liviano (Nube)](#22-instalador-liviano-nube)
3. [Primeros Pasos](#3-primeros-pasos)
4. [DigitalPlus Fichador](#4-digitalplus-fichador)
5. [DigitalPlus Administrador](#5-digitalplus-administrador)
6. [Portal Web DigitalPlus](#6-portal-web-digitalplus)
7. [Sistema de Licencias](#7-sistema-de-licencias)
8. [Preguntas Frecuentes](#8-preguntas-frecuentes)
9. [Soporte Tecnico](#9-soporte-tecnico)

---

## 1. INTRODUCCION

### Que es DigitalPlus

DigitalPlus es un sistema de **control de asistencia y gestion de personal** que permite registrar los ingresos y egresos del personal de su empresa. El sistema funciona mediante:

- **Huella digital:** El empleado apoya su dedo en un lector USB y el sistema lo identifica automaticamente.
- **PIN:** El empleado ingresa su numero de legajo y un codigo PIN personal.
- **Modo demostracion:** Para pruebas sin necesidad de hardware.

### Componentes del sistema

DigitalPlus se compone de tres aplicaciones:

| Aplicacion | Funcion |
|---|---|
| **DigitalPlus Fichador** | Terminal de fichaje donde los empleados registran entrada y salida |
| **DigitalPlus Administrador** | Aplicacion de gestion: legajos, horarios, reportes, configuracion |
| **Portal Web DigitalPlus** | Acceso via navegador para consulta de fichadas, reportes y gestion |

### Requisitos del equipo

| Requisito | Detalle |
|---|---|
| Sistema operativo | Windows 10 o Windows 11 (64 bits) |
| .NET Framework | 4.8 o superior (normalmente ya incluido en Windows 10/11) |
| Permisos | **Administrador local** para instalar |
| Lector de huellas | DigitalPersona uAreU 4500 (opcional si usa PIN) |
| Conexion a internet | Requerida para instalacion en nube; opcional para local |

---

## 2. INSTALACION

DigitalPlus cuenta con **dos instaladores** segun el tipo de despliegue:

| Instalador | Tamano | Para quien |
|---|---|---|
| **Instalador Completo** | ~180 MB | Empresas que quieren su propia base de datos local |
| **Instalador Liviano** | ~25 MB | Empresas que usan la base de datos en la nube |

> **Nota:** Ambos instaladores instalan Fichador y Administrador juntos. La diferencia es donde se ubica la base de datos.

---

### 2.1 Instalador Completo (Local)

Este instalador crea todo lo necesario en su propio equipo, incluyendo SQL Server Express si no lo tiene instalado.

#### Paso 1 - Ejecutar el instalador

Haga doble clic en el archivo:
```
DigitalPlus_Suite_Setup_v1.3.exe
```

Si Windows muestra una advertencia de seguridad, haga clic en **Si**.

> [CAPTURA: Pantalla de UAC de Windows pidiendo permisos de administrador]

#### Paso 2 - Pantalla de bienvenida

Haga clic en **Siguiente**.

> [CAPTURA: Pantalla de bienvenida del instalador con logo DigitalPlus]

#### Paso 3 - Seleccionar modo de instalacion

El instalador le preguntara como desea configurar la base de datos:

- **Local:** Instala SQL Server Express en su equipo (recomendado para empresas pequenas)
- **Nube:** Conecta con una base de datos remota usando un codigo de activacion

Seleccione **Local** y haga clic en **Siguiente**.

> [CAPTURA: Pantalla de seleccion de modo LOCAL / NUBE]

#### Paso 4 - Instalacion de SQL Server Express (solo modo Local)

Si su equipo no tiene SQL Server Express instalado, el instalador lo descargara e instalara automaticamente. Este proceso puede tomar entre **5 y 15 minutos** dependiendo de su equipo.

> [CAPTURA: Barra de progreso instalando SQL Server Express]

Si ya tiene SQL Server Express, este paso se omite automaticamente.

#### Paso 5 - Configuracion de base de datos (modo Local)

El instalador creara automaticamente la base de datos con todas las tablas necesarias.

> [CAPTURA: Pantalla de progreso creando la base de datos]

#### Paso 6 - Accesos directos

Seleccione las opciones deseadas:
- Crear acceso directo en el Escritorio para **Fichador** (recomendado)
- Crear acceso directo en el Escritorio para **Administrador** (recomendado)
- Iniciar automaticamente con Windows (opcional)

> [CAPTURA: Pantalla de seleccion de accesos directos y opciones]

#### Paso 7 - Instalacion

Haga clic en **Instalar** y espere a que finalice. Durante la instalacion vera:
- Copia de archivos
- Instalacion del driver DigitalPersona (lector de huellas)
- Configuracion de la base de datos
- Cifrado de configuracion de seguridad

> [CAPTURA: Barra de progreso de la instalacion]

#### Paso 8 - Finalizacion

Haga clic en **Finalizar**. Opcionalmente puede marcar ejecutar las aplicaciones inmediatamente.

> [CAPTURA: Pantalla de finalizacion del instalador]

---

### 2.2 Instalador Liviano (Nube)

Este instalador es para empresas cuya base de datos esta alojada en la nube. Es mas rapido y liviano, pero requiere un **codigo de activacion** proporcionado por el administrador del sistema.

#### Paso 1 - Ejecutar el instalador

Haga doble clic en:
```
DigitalPlus_Cloud_Setup_v1.0.exe
```

> [CAPTURA: Pantalla de UAC de Windows]

#### Paso 2 - Pantalla de bienvenida

Haga clic en **Siguiente**.

> [CAPTURA: Pantalla de bienvenida del instalador liviano]

#### Paso 3 - Accesos directos

Seleccione los accesos directos deseados y haga clic en **Siguiente**.

> [CAPTURA: Pantalla de seleccion de accesos directos]

#### Paso 4 - Codigo de Activacion

Esta es la pantalla mas importante. Ingrese el codigo de activacion que le proporciono su proveedor.

1. Escriba el codigo en el campo de texto
2. Haga clic en **Validar Codigo**
3. Espere la respuesta:
   - **Verde:** "Codigo valido. Empresa: [nombre]" - Puede continuar
   - **Rojo:** "Codigo invalido o expirado" - Verifique el codigo

> [CAPTURA: Pantalla de ingreso de codigo de activacion con campo de texto y boton Validar]

> **Importante:** Sin un codigo valido no podra continuar con la instalacion. Si no tiene el codigo, contacte al administrador del sistema.

#### Paso 5 - URL del Portal Web (opcional)

Si su empresa tiene un portal web DigitalPlus, ingrese la URL. Si no la tiene, puede dejar el campo vacio.

> [CAPTURA: Pantalla de configuracion de URL del portal web]

#### Paso 6 - Instalacion y finalizacion

El proceso es igual al instalador completo: copiar archivos, instalar drivers y finalizar.

> [CAPTURA: Progreso de instalacion liviana]

---

### Conectar el lector de huellas (post-instalacion)

**Despues** de instalar, conecte el lector DigitalPersona uAreU 4500 al puerto USB. Windows instalara los drivers automaticamente (ya fueron incluidos durante la instalacion).

> [CAPTURA: Foto del lector DigitalPersona uAreU 4500 conectado a USB]

Si el lector ya estaba conectado antes de instalar, desconectelo y vuelva a conectarlo.

---

## 3. PRIMEROS PASOS

### Orden recomendado de configuracion

1. Abra **DigitalPlus Administrador**
2. Configure los datos basicos: Sucursales, Categorias, Horarios, Sectores
3. Cargue los **Legajos** (empleados)
4. Registre las **huellas digitales** de cada empleado (o asigne PINs)
5. Abra **DigitalPlus Fichador** en la terminal de fichaje
6. Pruebe fichando con un empleado registrado

---

## 4. DIGITALPLUS FICHADOR

### Pantalla principal

Al abrir el Fichador, vera la pantalla de fichaje con:
- Nombre de la sucursal asignada
- Reloj con fecha y hora actual
- Area de semaforo visual
- Panel de fichada (segun el modo activo)

> [CAPTURA: Pantalla principal del Fichador mostrando sucursal, reloj y semaforo]

### 4.1 Fichada por Huella Digital

Este es el modo principal si tiene un lector de huellas conectado.

1. El empleado apoya su dedo en el lector
2. El semaforo cambia a **amarillo** (procesando)
3. Resultado:
   - **Verde:** Fichada registrada exitosamente. Muestra nombre del empleado y tipo (Entrada/Salida)
   - **Rojo:** Huella no reconocida. El empleado debe volver a intentar.

> [CAPTURA: Fichador mostrando semaforo verde con nombre del empleado y "ENTRADA" o "SALIDA"]

> [CAPTURA: Fichador mostrando semaforo rojo indicando huella no reconocida]

### 4.2 Fichada por PIN

Si no hay lector de huellas o si el modo PIN esta habilitado:

1. El empleado ingresa su **numero de legajo**
2. Ingresa su **PIN** de 4 a 6 digitos
3. El sistema valida y registra la fichada

> [CAPTURA: Panel de fichada por PIN mostrando campo de legajo y campo de PIN]

#### Primera vez (sin PIN asignado)

Si el legajo no tiene un PIN asignado, el sistema le preguntara **"¿Desea crear uno ahora?"**. Si acepta, se abre el formulario de creacion de PIN donde debe:

1. Ingresar un nuevo PIN (4 a 6 digitos)
2. Confirmar el nuevo PIN

> [CAPTURA: Dialogo preguntando si desea crear un PIN con botones Si/No]

> [CAPTURA: Formulario de creacion de PIN con campos: Nuevo PIN, Confirmar PIN]

#### Cambio forzado por el administrador

Si el administrador marco "Forzar cambio de PIN" o "Resetear PIN" para el empleado, al ingresar su numero de legajo el sistema lo lleva **directamente** al formulario de creacion de PIN, **sin pedir el PIN anterior**. Se muestra el mensaje: **"El administrador requiere que cambie su PIN"**.

> [CAPTURA: Mensaje "El administrador requiere que cambie su PIN" con formulario de nuevo PIN]

#### PIN expirado

Si el PIN del empleado esta vencido, al ingresar el legajo y el PIN actual correctamente, el sistema le solicitara que cree un nuevo PIN antes de completar la fichada.

> [CAPTURA: Mensaje de PIN expirado con formulario de cambio de PIN]

#### Cambiar PIN

Existen **dos formas** de cambiar el PIN:

**1. Cambio voluntario - Link "Cambiar mi PIN"**

En la pantalla de fichada hay un link **"Cambiar mi PIN"** que abre un formulario donde el empleado debe ingresar:

1. Numero de legajo
2. PIN actual
3. Nuevo PIN (minimo 4 digitos)
4. Confirmar nuevo PIN

El sistema valida que el PIN actual sea correcto, que el nuevo PIN tenga al menos 4 digitos y que sea diferente al PIN actual.

> [CAPTURA: Link "Cambiar mi PIN" en la pantalla de fichada]

> [CAPTURA: Formulario de cambio voluntario de PIN con campos: Legajo, PIN actual, Nuevo PIN, Confirmar PIN]

**2. Cambio forzado por el administrador**

Cuando el administrador marca "Forzar cambio" o "Resetear PIN" desde la pestana de PINs en Configuracion, la proxima vez que el empleado ingrese su numero de legajo en el Fichador, el sistema lo lleva directamente al formulario de creacion de PIN sin pedir el PIN anterior (ver seccion "Cambio forzado por el administrador" mas arriba).

### 4.3 Modo Demostracion

Este modo permite fichadas sin hardware biometrico ni PIN. Solo para demostraciones.

1. Se muestra una lista de empleados
2. El operador selecciona el empleado
3. Se registra la fichada

> [CAPTURA: Panel de modo demostracion con lista de empleados]

### 4.4 Deteccion automatica de modo

El Fichador detecta automaticamente si hay un lector de huellas conectado:
- **Lector detectado:** Usa modo Huella
- **Sin lector + PIN habilitado:** Cambia automaticamente a modo PIN
- **Sin lector + Demo habilitado:** Cambia a modo Demo
- Si se desconecta el lector durante el uso, cambia de modo en tiempo real

### 4.5 Informacion de licencia

En la barra inferior del Fichador se muestra informacion segun el tipo de instalacion:

**Instalacion local:**
- Tipo de licencia (Trial / Activa)
- Dias restantes (si es trial)
- Plan contratado

**Instalacion multi-tenant (nube):**
- En lugar de datos de licencia, la barra muestra la informacion del **terminal/sucursal**. El terminal se identifica por el nombre de la maquina y se mapea automaticamente a una sucursal en la base de datos.
- Ejemplo: **"10 - Administracion Dardo Rocha"** muestra el ID de la sucursal y su nombre.

> [CAPTURA: Barra de estado inferior mostrando informacion de licencia (instalacion local)]

> [CAPTURA: Barra de estado inferior mostrando terminal y sucursal (instalacion nube)]

---

## 5. DIGITALPLUS ADMINISTRADOR

### Pantalla principal

Al abrir el Administrador, vera un menu lateral con todas las opciones disponibles. En la parte superior del menu se muestra el **logo de su empresa** (si fue cargado por el administrador del sistema). En la parte inferior del menu aparece el logo de Integra IA.

El menu incluye accesos directos a las **redes sociales y pagina web** de su empresa (si fueron configuradas). Estos links se cargan automaticamente desde el sistema y abren el navegador web al hacer clic.

> [CAPTURA: Pantalla principal del Administrador con menu lateral, logo de empresa y links de redes sociales]

### 5.1 Gestion de Legajos (Empleados)

Desde **RRHH > Legajos** puede:

- **Agregar** un nuevo empleado: nombre, legajo, categoria, horario, sector, sucursal
- **Modificar** datos de un empleado existente
- **Eliminar** un empleado (baja logica)
- **Tomar foto** del empleado usando la camara web
- **Registrar huellas** digitales (requiere lector conectado)
- **Asignar/Resetear PIN**

> [CAPTURA: Formulario de Legajos mostrando los campos de datos del empleado]

#### Registrar huellas

1. Seleccione el empleado en la lista
2. Haga clic en el boton de huella
3. Siga las instrucciones del asistente de enrolamiento
4. Apoye el mismo dedo 4 veces para capturar la huella
5. Repita con otro dedo si lo desea

> [CAPTURA: Dialogo de enrolamiento de huellas digitales]

#### Asignar PIN

1. Seleccione el empleado en la lista
2. Haga clic en el boton **PIN**
3. Ingrese un PIN de 4 a 6 digitos
4. Confirme el PIN

> [CAPTURA: Dialogo de asignacion de PIN con campo de ingreso]

### 5.2 Gestion de Fichadas

Desde **RRHH > Fichadas** puede:

- Ver todas las fichadas registradas
- Filtrar por empleado, fecha, tipo (Entrada/Salida)
- Agregar fichadas manuales (en caso de olvido del empleado)
- Ver llegadas tarde

> [CAPTURA: Pantalla de consulta de fichadas con filtros y grilla de datos]

### 5.3 Tablas del Sistema

| Opcion | Que gestiona |
|---|---|
| **Sucursales** | Ubicaciones fisicas de la empresa |
| **Categorias** | Clasificacion de empleados (ej: Operario, Administrativo) |
| **Horarios** | Definicion de horarios de trabajo |
| **Sectores** | Areas de la empresa (ej: Produccion, RRHH) |
| **Incidencias** | Tipos de ausencia (vacaciones, enfermedad, permiso) |
| **Feriados** | Dias no laborables |

> [CAPTURA: Pantalla de ABM de Sucursales como ejemplo de tabla del sistema]

### 5.4 Configuracion del Sistema

Desde el boton de **Configuracion** (icono de engranaje) accede a:

#### Pestana Fichada
- **Modo PIN:** Habilitar o deshabilitar fichada por PIN
- **Expiracion de PIN:** Cantidad de dias para que el PIN expire (0 = no expira)
- **Modo Demo:** Habilitar o deshabilitar modo demostracion

> [CAPTURA: Pantalla de Configuracion - Pestana Fichada con checkboxes y opciones]

#### Pestana PINs
Muestra **todos los legajos** con su estado de PIN. En la parte superior hay un combo de filtro con las siguientes opciones:

- **Todos:** Muestra todos los legajos
- **Con PIN activo:** Solo legajos con PIN vigente
- **Sin PIN:** Legajos que no tienen PIN asignado
- **Vencidos:** Legajos cuyo PIN ha expirado
- **Cambio pendiente:** Legajos a los que el administrador les forzo un cambio de PIN

> [CAPTURA: Pantalla de Configuracion - Pestana PINs con combo de filtro y grilla]

La grilla muestra las siguientes columnas:

| Columna | Descripcion |
|---|---|
| **Legajo** | Numero de legajo del empleado |
| **Nombre** | Nombre completo del empleado |
| **Estado PIN** | Activo / Sin PIN / Vencido / Cambio pendiente |
| **Ultimo cambio** | Fecha del ultimo cambio de PIN |

Debajo de la grilla hay dos botones de accion:

- **Forzar cambio de PIN:** Marca los legajos seleccionados con PinMustChange. La proxima vez que el empleado ingrese su numero de legajo en el Fichador, debera establecer un nuevo PIN. No se le pedira el PIN anterior.
- **Resetear PIN:** Elimina el PIN del empleado por completo. La proxima vez que ingrese su legajo en el Fichador, se le solicitara que cree un PIN nuevo. Use esta opcion cuando un empleado olvido su PIN.

> [CAPTURA: Botones "Forzar cambio de PIN" y "Resetear PIN" en la pestana PINs]

### 5.5 Reportes

El Administrador genera reportes con Microsoft ReportViewer:
- Asistencia por periodo
- Horas trabajadas
- Llegadas tarde
- Horas extras

Los reportes se pueden exportar a PDF y Excel.

> [CAPTURA: Ejemplo de reporte de asistencia generado]

### 5.6 Licencias

Desde el menu **Licencias** puede:
- Ver el estado actual de la licencia
- Ingresar un codigo de activacion para pasar de Trial a licencia completa
- Ver detalles: plan, legajos permitidos, vencimiento

> [CAPTURA: Pantalla de informacion de licencia en el Administrador]

---

## 6. PORTAL WEB DIGITALPLUS

### Acceso

Abra su navegador web y vaya a la direccion proporcionada por su administrador (ejemplo: `https://digitalplusportalmt.azurewebsites.net/`).

Ingrese sus credenciales de acceso (usuario y contrasena). El boton de inicio de sesion se deshabilita automaticamente mientras se procesa para evitar errores por doble clic.

> [CAPTURA: Pantalla de login del portal web]

### Primer inicio de sesion

Si es la primera vez que accede (con la contrasena temporal proporcionada por el administrador), el sistema le pedira que **cambie su contrasena** obligatoriamente antes de poder usar el portal. Ingrese una nueva contrasena y confirmela. Esta pantalla no se puede omitir ni navegar a otra seccion hasta completar el cambio.

> [CAPTURA: Pantalla de cambio obligatorio de contrasena]

### Empresa suspendida

Si al intentar iniciar sesion recibe el mensaje **"El acceso a su empresa ha sido suspendido"**, significa que el administrador del sistema ha desactivado temporalmente el acceso. Contacte a su proveedor para resolver la situacion. Lo mismo aplica a las aplicaciones de escritorio (Fichador y Administrador): si la empresa esta suspendida, las apps mostraran un aviso y se cerraran.

### Barra superior

Una vez dentro del portal, en la **barra superior** se muestra:
- **Lado izquierdo:** El logo y nombre de su empresa
- **Lado derecho:** El identificador de empresa y el nombre del usuario logueado, con menu desplegable para acceder a configuracion de cuenta y cerrar sesion

> [CAPTURA: Barra superior del portal con logo de empresa a la izquierda y datos de usuario a la derecha]

### Funcionalidades disponibles

| Seccion | Que puede hacer |
|---|---|
| **Legajos** | Ver y gestionar datos de empleados |
| **Fichadas** | Consultar fichadas por empleado y periodo |
| **Horarios** | Ver y modificar horarios de trabajo |
| **Categorias** | Gestionar categorias de empleados |
| **Sectores** | Gestionar sectores/areas |
| **Sucursales** | Gestionar ubicaciones |
| **Terminales** | Ver terminales de fichaje registradas |
| **Incidencias** | Cargar permisos, ausencias, vacaciones |
| **Feriados** | Gestionar dias feriados |
| **Variables** | Configuracion general del sistema |
| **Usuarios** | Gestionar accesos al portal |

> [CAPTURA: Dashboard principal del portal web mostrando el menu de navegacion]

> [CAPTURA: Ejemplo de consulta de fichadas desde el portal web]

---

## 7. SISTEMA DE LICENCIAS

> **Nota importante:** En instalaciones **multi-tenant (nube)**, el sistema de licencias funciona de manera diferente. La activacion se realiza durante la instalacion mediante el codigo de activacion. Las aplicaciones de escritorio **no validan licencias al iniciar** - se conectan directamente a la base de datos en la nube. El sistema de trial y licencias descrito a continuacion aplica **unicamente a instalaciones LOCALES**.

### Periodo de prueba (Trial)

Al instalar DigitalPlus por primera vez, el sistema se activa en **modo Trial** automaticamente:

| Limitacion | Valor |
|---|---|
| Duracion | 14 dias |
| Cantidad maxima de empleados | 5 legajos |
| Funcionalidades | Todas disponibles |

Durante el periodo de prueba, vera un indicador en la barra de estado mostrando los dias restantes.

> [CAPTURA: Barra de estado mostrando "Trial - 10 dias restantes"]

### Que pasa cuando vence el Trial

Si el periodo de prueba vence o supera los 5 empleados:
1. Se muestra una pantalla de bloqueo
2. El sistema **no permite fichar** hasta que se active una licencia
3. Los datos no se pierden

> [CAPTURA: Pantalla de bloqueo por trial vencido con campo para ingresar codigo]

### Activar una licencia

1. Solicite un **codigo de activacion** a su proveedor
2. Ingrese el codigo en la pantalla de bloqueo o desde el menu **Licencias**
3. El sistema se conecta al servidor de licencias y activa el plan contratado
4. Las limitaciones se levantan segun el plan

> [CAPTURA: Ingreso de codigo de activacion y mensaje de activacion exitosa]

### Planes disponibles

Los planes determinan la cantidad maxima de empleados (legajos) permitidos. Consulte con su proveedor los planes disponibles y precios.

### Requisitos de conectividad

El sistema de licencias requiere conexion a internet periodica:
- Al iniciar la aplicacion (para validar la licencia)
- Cada 4 horas (heartbeat automatico)
- Si pierde conectividad por mas de **72 horas**, el sistema se bloqueara temporalmente hasta recuperar la conexion

---

## 8. PREGUNTAS FRECUENTES

### El Fichador no reconoce el lector de huellas

1. Desconecte y vuelva a conectar el lector USB
2. Espere 10 segundos a que Windows lo reconozca
3. Si persiste, reinicie la aplicacion
4. Si el lector no funciona, puede usar modo PIN como alternativa

### Un empleado olvido fichar

El Administrador puede cargar fichadas manuales desde **RRHH > Fichadas > Fichada Manual**.

### Se perdio el codigo de activacion

Contacte a su proveedor (Integra IA) para que le regenere un nuevo codigo de activacion.

### El sistema dice "Licencia bloqueada por falta de conexion"

Verifique que el equipo tenga conexion a internet. Una vez restaurada la conexion, reinicie la aplicacion y se desbloqueara automaticamente.

### Puedo instalar Fichador en varias maquinas?

Si. Cada terminal de fichaje necesita su propia instalacion. Todas las terminales pueden apuntar a la misma base de datos.

### Como desinstalo DigitalPlus?

Vaya a **Panel de Control > Programas > Desinstalar un programa**, seleccione **DigitalPlus Suite** y siga las instrucciones. La desinstalacion no elimina la base de datos ni los registros de fichadas.

---

## 9. SOPORTE TECNICO

Ante cualquier inconveniente, contacte al administrador del sistema con la siguiente informacion:

- Mensaje de error exacto (captura de pantalla si es posible)
- Nombre del equipo
- Que estaba haciendo cuando ocurrio el error
- Sistema operativo instalado

**Soporte Integra IA:**
- Sitio web: www.digitaloneplus.com
- Email: soporte@digitaloneplus.com

---

*Fin del Manual del Usuario*
