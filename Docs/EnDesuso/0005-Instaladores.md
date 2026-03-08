# TASK: Instalador unificado y dinámico para apps Desktop + Bootstrap de Base de Datos

## 🎯 Objetivo general
Retomar el trabajo de **instalación** de las 2 aplicaciones de escritorio ya adaptadas a la nueva base de datos, generando instaladores que:
1) configuren dinámicamente los **connection strings** y valores de app,
2) verifiquen/creen la base de datos si no existe,
3) inicialicen **usuarios y roles** en Identity,
4) y (solo en Administrador) configuren la URL del portal web (DigitalPlusWeb) sin hardcode.

---

## 🧩 Soluciones a estudiar
- **Fichador**: `TEntradaSalida.sln`
- **Administrador**: `DigitalOneAdministrador.sln`

Claude debe:
- Abrir ambas soluciones
- Identificar ejecutables, dependencias, DLLs, assets, SDKs, drivers, config files, etc.
- Determinar **qué componentes** deben instalarse y cuáles deben ir “incluidos” (local) vs “pre-requisitos” (externos).

---

## ✅ Requisitos del instalador (para ambos proyectos)

### 1) Preguntas obligatorias al usuario durante la instalación
El instalador debe solicitar:

**Base de datos**
- Servidor SQL (hostname o IP)
- Nombre de base de datos
- Usuario SQL
- Password SQL

**Empresa**
- Nombre de empresa (string)

### 2) Escritura de configuración en runtime (post-instalación)
- El **connection string** debe quedar persistido en el archivo de configuración correspondiente (App.config/Web.config/appsettings según aplique).
- Debe agregarse/actualizarse un valor **NombreEmpresa** (o equivalente) en configuración.
- Ese valor debe poder leerse en ejecución para mostrarlo en el pie de los formularios (no inventar UI nueva, solo asegurar que el valor queda disponible en config).

> Importante: el objetivo es que **NINGUNA app** tenga connection strings hardcodeados.

---

## 🗄️ Requisitos de Base de Datos (validación + creación)

### 3) Verificación de existencia de la base
El instalador debe:
- Conectarse al servidor con los datos ingresados.
- Corroborar si existe la base de datos indicada.
- Si **NO existe**, debe:
  - Crear la base
  - Crear el modelo completo compatible con el esquema de **DigitalPlus**
  - Ejecutar scripts necesarios para dejar el sistema inicializable

### 4) Script SQL Server compatible
El/los scripts incluidos en el instalador deben ser compatibles con **SQL Server 2014 en adelante**.

---

## 👤 Bootstrap de Identity (usuarios y roles)

### 5) Roles obligatorios
Insertar en `[AspNetRoles]` los registros con:
- `Name = 'ADMINISTRADOR'`
- `Name = 'Registrado'`

### 6) Usuarios obligatorios
Insertar 2 usuarios en `[AspNetUsers]`:
- Un usuario **Administrador**
- Un usuario **Normal**

> Definir de manera explícita:
- username / email (los valores deben ser constantes documentadas en el informe final)
- password (si se hashea, debe ser compatible con el esquema actual; si no se puede hashear desde SQL puro, implementar un mecanismo seguro desde app/instalador para crear users correctamente)

### 7) Asociaciones Usuario-Rol
- Asociar Administrador → Rol `ADMINISTRADOR`
- Asociar Normal → Rol `Registrado`
(usando la tabla puente correspondiente, típicamente `AspNetUserRoles`)

---

## 🌐 Requisito adicional SOLO para instalador de Administrador

### 8) URL del Portal Web (DigitalPlusWeb)
El instalador del **Administrador** debe preguntar:
- URL del portal web de DigitalPlus (ej: `https://...` o `http://ip:puerto/`)

Ese valor debe quedar guardado en configuración, y el botón/menú **DigitalPlusWeb** debe abrir la URL configurada.

Se debe reemplazar el hardcode actual:

```csharp
private void btnDigitalPlusWeb_Click(object sender, EventArgs e)
{
    System.Diagnostics.Process.Start("https://digitalplusapp.azurewebsites.net/");
}