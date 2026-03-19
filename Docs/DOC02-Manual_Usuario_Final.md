# DIGITALPLUS - Manual del Usuario

**Version:** 14.0
**Fecha:** 2026-03-18

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
- **Movil:** El empleado ficha desde su smartphone accediendo a la PWA del portal, con validacion de ubicacion GPS.
- **Modo demostracion:** Para pruebas sin necesidad de hardware.

### Componentes del sistema

DigitalPlus se compone de tres aplicaciones:

| Aplicacion | Funcion |
|---|---|
| **DigitalPlus Fichador** | Terminal de fichaje donde los empleados registran entrada y salida |
| **DigitalPlus Administrador** | Aplicacion de gestion: legajos, horarios, reportes, configuracion |
| **Portal Web DigitalPlus** | Acceso via navegador para consulta de fichadas, reportes y gestion |
| **Digital One Mobile** | PWA de smartphone para fichada con validacion de ubicacion GPS |

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

Este es el instalador recomendado para la mayoria de las empresas. La base de datos esta alojada en la nube (no se necesita instalar nada de SQL Server en su equipo). Es rapido y liviano (~25 MB). Soporta dos modalidades de instalacion:

- **Con codigo de activacion:** Para empresas que ya fueron dadas de alta por el administrador del sistema.
- **Plan Free (sin codigo):** Para nuevas empresas que desean registrarse directamente con el plan gratuito.

#### Que necesita antes de empezar

1. El archivo del instalador: `DigitalPlus_Cloud_Setup_v1.0.exe`
2. El **codigo de activacion** de su empresa (si tiene uno), **o** datos para registrarse como Free (nombre de empresa, email, pais)
3. Conexion a internet (obligatoria durante la instalacion)

> **Nota:** Durante la instalacion puede elegir la carpeta de destino. Si necesita reinstalar con el mismo email (plan Free), el sistema detecta que ya existe y retorna los datos existentes sin crear duplicados.

#### Paso 1 - Ejecutar el instalador

Haga doble clic en el archivo `DigitalPlus_Cloud_Setup_v1.0.exe`. Si Windows muestra una advertencia de seguridad (SmartScreen o UAC), haga clic en **Mas informacion** y luego en **Ejecutar de todas formas**, y luego **Si** en la pantalla de permisos de administrador.

> [CAPTURA: Pantalla de UAC de Windows]

#### Paso 2 - Pantalla de bienvenida

Lea la informacion y haga clic en **Siguiente**.

> [CAPTURA: Pantalla de bienvenida del instalador liviano]

#### Paso 3 - Accesos directos y opciones

Seleccione las opciones deseadas:
- **Crear acceso directo en el Escritorio para Fichador** (recomendado para terminales de fichaje)
- **Crear acceso directo en el Escritorio para Administrador** (recomendado para el puesto de administracion)
- **Iniciar Fichador automaticamente al encender** (recomendado para terminales que siempre deben estar activas)
- **Iniciar Administrador automaticamente al encender** (opcional)

Haga clic en **Siguiente**.

> [CAPTURA: Pantalla de seleccion de accesos directos]

#### Paso 4 - Tipo de Instalacion

El instalador le presenta dos opciones:

- **Tengo un codigo de activacion:** Para empresas que ya fueron dadas de alta por el administrador.
- **Quiero registrarme con plan Free:** Para nuevas empresas que desean empezar con el plan gratuito sin necesidad de un codigo.

Seleccione la opcion que corresponda y haga clic en **Siguiente**.

> [CAPTURA: Pantalla de seleccion de tipo de instalacion: Codigo o Free]

#### Paso 4a - Con Codigo de Activacion

Si selecciono la opcion de codigo de activacion:

1. Escriba o pegue el **codigo de activacion** en el campo de texto. El codigo tiene formato `XXXX-XXXX-XXXX-XXXX-XXXX-XXXX` (puede variar en longitud).
2. Haga clic en el boton **Validar Codigo**
3. Espere la respuesta del servidor (requiere internet):
   - **Verde:** "Codigo valido. Empresa: [nombre de su empresa]" — Puede continuar con la instalacion
   - **Rojo:** "Codigo invalido o expirado" — Verifique que el codigo este bien escrito. Si persiste, contacte a su proveedor.

> [CAPTURA: Pantalla de ingreso de codigo de activacion con campo de texto y boton Validar]

> **Importante:** Sin un codigo valido no podra continuar con la instalacion. Si no tiene el codigo, contacte al administrador del sistema. Cada codigo esta asociado a una empresa especifica.

#### Paso 4b - Registro Free (sin codigo)

Si selecciono el plan Free:

1. Ingrese el **nombre de su empresa** (razon social o nombre comercial)
2. Ingrese su **email** (sera el email del administrador y el acceso al Portal Web)
3. Seleccione su **pais** del combo desplegable (la lista se carga dinamicamente desde el servidor via API `/api/paises`)
4. Haga clic en **Siguiente**

El sistema realiza una **validacion previa** del email (via API `/api/validar-free`). Si el email ya esta registrado en otra empresa, se muestra un mensaje de error y no permite continuar.

> [CAPTURA: Pantalla de registro Free con campos: nombre empresa, email, pais]

> **Importante:** El registro Free se ejecuta **despues de la instalacion** (post-install). Si el usuario cancela la instalacion antes de que finalice, no queda ningun dato parcial en el servidor. Esto evita que se cree "basura" en caso de cancelacion.

Al completarse el registro exitosamente:
- Se envia automaticamente un **email de bienvenida** con las credenciales de acceso al Portal Web (email + contraseña temporal)
- Se muestra un **mensaje de confirmacion** indicando que las credenciales fueron enviadas al email proporcionado

#### Paso 5 - Instalacion

Haga clic en **Instalar**. El instalador realizara automaticamente:
- Copia de archivos del Fichador y Administrador
- Instalacion del driver del lector de huellas DigitalPersona
- Configuracion de la conexion a la base de datos en la nube (usando el codigo validado)
- Cifrado de la configuracion de seguridad (proteccion DPAPI)

> [CAPTURA: Progreso de instalacion liviana]

#### Paso 6 - Finalizacion

Haga clic en **Finalizar**. Opcionalmente puede marcar ejecutar las aplicaciones inmediatamente.

#### Que sucede automaticamente despues de instalar

Al abrir el **Fichador** por primera vez, el sistema realiza un **auto-registro de la terminal**: detecta el nombre de la computadora y la registra automaticamente en la base de datos, asociandola a la sucursal principal de su empresa. **No es necesario dar de alta la computadora manualmente** desde el portal web ni desde el Administrador.

Si necesita cambiar la sucursal asignada a una terminal, puede hacerlo desde el **Portal Web** en la seccion **Terminales**.

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
3. Cargue los **Legajos** (empleados) — puede tomarles foto con la camara web
4. Registre las **huellas digitales** de cada empleado (o asigne PINs)
5. Abra **DigitalPlus Fichador** en la terminal de fichaje — la computadora se registra automaticamente como terminal asociada a la sucursal principal
6. Pruebe fichando con un empleado registrado

> **Nota:** No es necesario registrar las terminales (computadoras de fichaje) manualmente. El Fichador las registra automaticamente la primera vez que se ejecuta en cada computadora.

---

## 4. DIGITALPLUS FICHADOR

### Pantalla principal

Al abrir el Fichador, vera la pantalla de fichaje con diseño oscuro profesional:
- **Barra superior:** Reloj digital grande con logos de empresa e Integra IA
- **Nombre de empresa y fecha** debajo del reloj
- **Barra de modos:** Botones horizontales que muestran los modos disponibles (Huella, PIN, QR, Demo). El modo activo se resalta en dorado. Solo se muestran los modos disponibles segun el hardware conectado
- **Semaforo visual:** Tres circulos (rojo/amarillo/verde) indican el estado de la fichada
- **Panel de fichada:** Cambia segun el modo activo
- **Barra inferior:** Nombre de sucursal asignada

> [CAPTURA: Pantalla principal del Fichador con tema oscuro mostrando barra de modos]

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

Si el legajo no tiene un PIN asignado, el sistema le preguntara **"¿Desea crear uno ahora?"** con botones **Si** y **No**. Si acepta, se abre el formulario de creacion de PIN donde debe:

1. Ingresar un nuevo PIN (4 a 6 digitos)
2. Confirmar el nuevo PIN

Si elige **No**, puede fichar de otra manera (huella) o volver mas tarde.

> [CAPTURA: Dialogo preguntando si desea crear un PIN con botones Si/No]

> [CAPTURA: Formulario de creacion de PIN con campos: Nuevo PIN, Confirmar PIN]

#### Cambio forzado por el administrador

Si el administrador marco "Forzar cambio de PIN" para el empleado, al ingresar su numero de legajo el sistema muestra un mensaje **obligatorio**: **"El administrador requiere que cambie su PIN"** con un unico boton **OK**. No hay opcion de cancelar ni de omitir el cambio. Al presionar OK, se abre directamente el formulario de creacion de PIN **sin pedir el PIN anterior**.

Si el administrador "Reseteo" el PIN (lo elimino), el comportamiento es igual al de "Primera vez" descrito arriba (ofrece crear uno nuevo con Si/No).

> [CAPTURA: Mensaje obligatorio "El administrador requiere que cambie su PIN" con boton OK]

> [CAPTURA: Formulario de nuevo PIN sin campo de PIN anterior]

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

### 4.4 Fichada por QR (Camara)

Si la computadora tiene una camara web (USB o integrada), el modo QR estara disponible:

1. Click en el boton **QR** en la barra de modos
2. Se activa la camara y aparece la vista previa en pantalla
3. El empleado muestra su **codigo QR personal** frente a la camara
4. El sistema lee el QR, valida que pertenezca a un empleado de la empresa y registra la fichada
5. **Semaforo verde** + nombre del empleado y ENTRADA/SALIDA

> [CAPTURA: Fichador en modo QR mostrando la camara activa]

**Importante:**
- El mismo QR no se puede usar dos veces en 5 segundos (cooldown anti-duplicados)
- Si el QR no corresponde a un empleado de esta empresa, aparece "QR leido pero no corresponde a esta empresa"
- El QR del empleado se genera desde el Portal Web (seccion Legajos) o desde la PWA mobile (tab "Mi QR")

### 4.5 Cambio de modo

La barra de modos en la parte superior del panel muestra los modos disponibles como botones:
- **Huella:** Solo si hay lector de huellas conectado
- **PIN:** Siempre disponible
- **QR:** Solo si hay camara web detectada
- **Demo:** Solo si esta habilitado por el administrador

El modo activo se muestra en dorado. Click en cualquier otro boton para cambiar de modo al instante.

### 4.6 Deteccion automatica de hardware

El Fichador detecta automaticamente el hardware disponible:
- **Lector de huellas detectado:** Arranca en modo Huella
- **Sin lector + camara:** Arranca en modo QR
- **Sin lector + sin camara:** Arranca en modo PIN
- Si se conecta/desconecta el lector durante el uso, cambia de modo automaticamente

### 4.5 Informacion de licencia

En la barra inferior del Fichador se muestra informacion segun el tipo de instalacion:

**Instalacion local:**
- Tipo de licencia (Trial / Activa)
- Dias restantes (si es trial)
- Plan contratado

**Instalacion multi-tenant (nube):**
- En lugar de datos de licencia, la barra muestra la informacion del **terminal/sucursal**. El terminal se identifica por el nombre de la maquina y se registra automaticamente en la primera ejecucion, asociandose a la sucursal principal de la empresa.
- Ejemplo: **"10 - Administracion Dardo Rocha"** muestra el ID de la sucursal y su nombre.
- Si la terminal no aparece asociada a ninguna sucursal, verifique la conexion a la base de datos.

> [CAPTURA: Barra de estado inferior mostrando informacion de licencia (instalacion local)]

> [CAPTURA: Barra de estado inferior mostrando terminal y sucursal (instalacion nube)]

---

## 5. DIGITALPLUS ADMINISTRADOR

### Pantalla principal

Al abrir el Administrador, vera un menu lateral con las opciones disponibles. En la parte superior del menu se muestra el **logo de su empresa** (si fue cargado por el administrador del sistema). En la parte inferior del menu aparece el logo de Integra IA.

El menu incluye accesos directos a las **redes sociales y pagina web** de su empresa (si fueron configuradas). Estos links se cargan automaticamente desde el sistema y abren el navegador web al hacer clic.

> **Importante:** El Administrador desktop tiene un rol simplificado: **solo se usa para registrar huellas digitales y tomar fotos** de los empleados. Todos los datos del legajo (nombre, apellido, sector, categoria, sucursal, horario, domicilio, etc.) se gestionan desde el **Portal Web**. En el Administrador, estos datos se muestran en modo solo lectura.

> [CAPTURA: Pantalla principal del Administrador con menu lateral, logo de empresa y links de redes sociales]

### 5.1 Legajos - Huellas y Foto

El formulario de legajos tiene **2 pestanas**:

**Pestana "Legajo":**
- Muestra los datos del empleado en **modo solo lectura** (nombre, apellido, sector, categoria, sucursal, horario, estado)
- **Registrar huellas digitales** (requiere lector DigitalPersona conectado)
- **Tomar foto** del empleado usando la camara web
- El boton **Guardar** solo persiste las huellas y la foto, no modifica los datos del legajo
- No hay boton de eliminar: los legajos no se pueden dar de baja desde la app desktop

**Pestana "Movil":**
- Ver estado del dispositivo movil del empleado
- Generar codigo de activacion para vincular un smartphone
- Desactivar dispositivo movil

> Para dar de alta empleados, modificar sus datos o gestionar domicilios, sucursales y PINs, utilice el **Portal Web**.

> [CAPTURA: Formulario de Legajos mostrando datos en modo solo lectura y panel de camara/huellas]

#### Registrar huellas

1. Seleccione el empleado en la lista
2. Haga clic en el boton de huella
3. Siga las instrucciones del asistente de enrolamiento
4. Apoye el mismo dedo 4 veces para capturar la huella
5. Repita con otro dedo si lo desea

> [CAPTURA: Dialogo de enrolamiento de huellas digitales]

### 5.2 Reportes del Portal Web

El portal web ofrece reportes:
- **Asistencia Diaria:** Resumen de entradas y salidas por dia
- **Llegadas Tarde:** Listado de empleados que ingresaron despues de su horario
- **Ausencias:** Listado de empleados que no ficharon en dias laborables. Incluye checkbox "Incidencias" para mostrar u ocultar ausencias justificadas (incidencias registradas). Filtra por sector, motivo y legajo.
- **Horas Trabajadas:** Resumen de horas por empleado y periodo

Todos los reportes del portal se exportan a Excel y CSV.

> [CAPTURA: Ejemplo de reporte de asistencia generado]

### 5.3 Licencias

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
| **Terminales Moviles** | Gestionar dispositivos moviles registrados, generar codigos de activacion |
| **Fichado Movil** | Configurar validacion de ubicacion (WiFi/GPS) por sucursal |
| **PIN Movil** | Asignar, cambiar y resetear PIN de empleados (desde formulario de Legajos) |
| **Movil (tab en Legajo)** | Ver estado del dispositivo, generar codigo de activacion con envio automatico por email |
| **Incidencias** | Gestionar tipos de incidencia y registrar masivamente para todos los legajos |
| **Feriados** | Gestionar dias feriados |
| **Variables** | Configuracion general del sistema |
| **Usuarios** | Gestionar accesos al portal (AdminEmpresa puede crear Operador y Consulta) |

> [CAPTURA: Dashboard principal del portal web mostrando el menu de navegacion]

### Roles de usuario

El sistema maneja 4 roles con distintos niveles de acceso:

| Rol | Puede ver | Puede crear/editar |
|---|---|---|
| **AdminEmpresa** | Todo | Todo dentro de su empresa |
| **Operador** | Todo | Legajos, fichadas, vacaciones |
| **Consulta** | Todo (solo lectura) | Nada |
| **SuperAdmin** | Reservado para IntegraIA | Todo (cross-tenant) |

Al crear un usuario desde Administracion > Usuarios, cada rol muestra una descripcion de sus capacidades.

### Gestion de Legajos

El formulario de edicion de un legajo tiene las siguientes solapas:

**Datos:** Informacion personal (nombre, apellido, email, telefono), organizacion (sector, categoria, horario), estado, acceso movil y PIN. Incluye seccion **Domicilio** con 7 campos: Calle, Altura, Piso, Barrio, Localidad, Provincia, CodigoPostal. Si el legajo tiene una **foto** (capturada desde Administrador desktop), se muestra como imagen redondeada de 180x180 pixeles. En la lista de legajos, la foto aparece como avatar de 28px.

**Sucursales:** Asignar/desasignar sucursales al legajo. Determina en que sucursales puede fichar. **Al crear un nuevo legajo, es obligatorio asignar al menos una sucursal** (el tab Sucursales se muestra visible durante la creacion).

**Huellas:** Visualizacion de huellas registradas (la registracion se realiza desde la app de escritorio Administrador).

**Fichadas:** Historial completo de fichadas del legajo en un rango de fechas. Permite:
- **Nueva fichada manual:** Registra una fichada con fecha, hora, tipo (Entrada/Salida) y sucursal. Queda marcada con origen "Manual" y el usuario que la creo.
- **Editar fichada:** Modifica fecha, hora, tipo o sucursal de una fichada existente. Queda marcada con "Modificado por" y la fecha de modificacion (icono de lapiz amarillo).
- **Eliminar fichada:** Con confirmacion previa.
- Las fichadas manuales y las incidencias del periodo se muestran juntas en el listado.

**Calendario:** Vista mensual tipo Google Calendar con grilla visual:
- **Grilla mensual:** Muestra los dias del mes en formato de calendario con navegacion entre meses y boton "Hoy". Los headers de dias son Lu Ma Mi Ju Vi Sa Do (sabado y domingo en rojo).
- **Horario habitual:** En cada celda del dia se muestra el horario asignado al legajo como badge gris (ej: "09:00-18:00"), tomado del Horario y HorarioDetalle configurados en la pestana Datos.
- **Eventos de calendario:** Alteraciones temporales del horario habitual (extensiones, convocatorias especiales, reemplazos). Se muestran como pills azules sobre la celda del dia con hora y descripcion. Los eventos **no anulan** el horario habitual, son una capa adicional. Cada evento tiene: fecha desde/hasta, hora desde/hasta y nota descriptiva.
- **Crear evento:** Click en cualquier dia abre un modal con campos de fecha, hora y nota. El evento se guarda directamente en la base de datos.
- **Editar evento:** Click sobre un evento existente abre el modal con los datos cargados para modificar.
- **Eliminar evento:** Desde el modal de edicion o desde la lista de eventos del mes.
- **Lista de eventos del mes:** Debajo de la grilla se muestra una tabla resumen con todos los eventos del mes actual, con botones para editar y eliminar.
- **Novedades:** Debajo del calendario se mantiene la seccion de novedades con:
  - **Registrar incidencia individual:** Seleccionar tipo de incidencia, rango de fechas y detalle. Se genera un registro por dia.
  - **Registrar vacaciones:** Rango de fechas y nota. Se muestra como bloque con el rango visible.
  - **Listado unificado:** Incidencias y vacaciones juntas, ordenadas por fecha, con filtro por rango y tipo.

**Movil:** Estado del dispositivo movil, generar codigo de activacion con envio automatico por email.

### Gestion de Incidencias

Las incidencias representan motivos de ausencia o novedades (vacaciones, enfermedad, feriados, etc.). Al crear una empresa se cargan 20 incidencias por defecto:

- **Legales (azul):** Vacaciones, feriado obligatorio, maternidad, paternidad, matrimonio, fallecimiento, mudanza, examen, donacion de sangre, dia del gremio.
- **Personales (naranja):** Enfermedad, accidente laboral, ausencia con/sin aviso, llegada tarde, suspension, capacitacion, tramite personal, licencia sin goce de sueldo, trabajo remoto.

**Registro masivo:** Desde la lista de incidencias, el boton verde permite registrar una incidencia para multiples legajos a la vez:
1. Seleccionar rango de fechas
2. Filtrar legajos por nombre, categoria, sector o sucursal
3. Seleccionar/quitar todos o individualmente
4. El sistema genera un registro por dia por legajo (evita duplicados)
5. Barra de progreso visible durante la operacion

**Eliminar masivo:** Mismo modal, boton "Eliminar del rango" borra los registros del rango seleccionado.

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

## 9. MODO KIOSKO Y FICHADA POR QR

### Que es el Modo Kiosko

El modo kiosko permite instalar una **tablet o dispositivo compartido** en una sucursal para que multiples empleados fichen mediante un codigo QR personal. A diferencia de la fichada movil (que usa GPS del celular), el kiosko esta fijo en un lugar y la presencia fisica del empleado frente a la camara es la validacion.

### Circuito completo

1. **Configurar el kiosko:** Desde el Portal Web, ir a **Estructura > Terminales Moviles > Registrar Kiosko**. Asignar nombre, sucursal y copiar el Device ID generado.
2. **Abrir el kiosko:** En el dispositivo (tablet/PC), abrir `https://[portal]/kiosko/` e ingresar el Device ID. La camara se activa automaticamente.
3. **Credencial QR del empleado:** Cada legajo tiene un QR unico. Se puede ver desde:
   - **Portal Web:** Lista de legajos, boton QR (icono ◻) por cada legajo
   - **PWA Mobile:** Tab "Mi QR" (tercera pestaña)
   - **Impreso:** Boton "Imprimir QR" genera credenciales en formato tarjeta
4. **Fichar:** El empleado muestra su QR frente a la camara del kiosko. El sistema valida que pertenezca a la sucursal del kiosko y registra Entrada o Salida automaticamente.
5. **Confirmacion:** Aparece un overlay verde con nombre, foto y tipo (Entrada/Salida) durante 3 segundos.

### Requisitos

- La empresa debe tener **Modo Kiosco habilitado** (activado desde Portal de Licencias)
- El legajo debe estar **asignado a la sucursal** del kiosko
- El dispositivo kiosko necesita **camara** y **conexion a internet**

### Imprimir credenciales QR

Desde el Portal Web > Legajos:
- **QR individual:** Click en el boton QR de un legajo > Imprimir
- **QR masivo:** Click en "Imprimir QR" (boton arriba de la lista) para imprimir credenciales de todos los legajos visibles en la pagina

---

## 10. SOPORTE TECNICO

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
