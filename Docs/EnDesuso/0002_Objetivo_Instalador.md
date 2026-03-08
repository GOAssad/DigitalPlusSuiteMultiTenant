# Objetivo: Construcción de Instalador Profesional
## Sistema C# .NET Framework 4.8 (WinForms)

## Contexto

El objetivo ahora NO es continuar con auditoría ni refactor estructural.

El objetivo es construir un INSTALADOR profesional que permita instalar el sistema en una PC cliente de manera simple, guiada y segura.

La instalación debe permitir configurar dinámicamente la conexión a base de datos y dejar la aplicación lista para usar sin editar manualmente archivos .config.

---

# Objetivos del Instalador

El instalador debe:

1. Permitir configurar dinámicamente la Connection String.
2. Permitir elegir carpeta de instalación.
3. Crear accesos directos (Escritorio y Menú Inicio).
4. Verificar prerequisitos (.NET Framework 4.8).
5. Resolver dependencias del SDK/Driver DigitalPersona.
6. Dejar el sistema funcional al finalizar la instalación.

---

# Tecnología del Instalador

Evaluar y elegir UNA de las siguientes opciones, justificando técnicamente la decisión:

## Opción A: Advanced Installer (recomendado si hay licencia)

Ventajas:
- Soporte nativo para diálogos SQL.
- Escritura automática en archivos config.
- Manejo sencillo de prerequisitos.
- Instalador profesional tipo MSI.

Desventajas:
- Puede requerir licencia según edición.

---

## Opción B: Inno Setup (recomendado si se busca solución gratuita)

Ventajas:
- Sin costo.
- Totalmente configurable.
- Permite crear wizard personalizado.

Desventajas:
- Requiere scripting.
- Se debe programar la validación de conexión SQL.

---

## Opción C: Visual Studio Installer Projects

Ventajas:
- Integrado a Visual Studio.
- Fácil creación de MSI básico.

Desventajas:
- Personalización limitada para wizard dinámico SQL.

---

# Requerimientos Funcionales del Instalador

## 1. Configuración de Base de Datos

El instalador debe solicitar:

- Servidor SQL (hostname o IP)
- Instancia o puerto (opcional)
- Nombre de base de datos
- Tipo de autenticación:
  - Windows Authentication
  - SQL Authentication (usuario + contraseña)
- Botón "Probar conexión"

Si la prueba falla:
- No permitir continuar.
- Mostrar mensaje claro.

Al confirmar, debe generar dinámicamente la Connection String y escribirla en:

TEntradaSalida.exe.config  
ConnectionStrings["ConTocayAnda"]

Opcionalmente guardar datos adicionales en appSettings para diagnóstico.

---

## 2. Carpeta de Instalación

- Carpeta por defecto:
  C:\Program Files\DigitalOnePlus\TEntradaSalida\
- Permitir modificar ubicación.

---

## 3. Accesos Directos

Debe crear:

- Acceso directo en Escritorio.
- Carpeta en Menú Inicio con:
  - Acceso principal.
  - Desinstalador.

Opcional:
- Checkbox "Ejecutar al iniciar Windows".

---

## 4. Prerequisitos

El instalador debe:

- Verificar que .NET Framework 4.8 esté instalado.
- Validar presencia de DigitalPersona Runtime/Driver.
- En caso de no estar presente:
  - Instalarlo automáticamente si se dispone del redistribuible.
  - O mostrar advertencia clara.

---

## 5. Configuración Final

Al finalizar instalación:

- Escribir archivo de configuración definitivo.
- Permitir opción "Ejecutar aplicación al finalizar".
- Opcional: Registrar la PC como terminal en base de datos
  - Utilizar Environment.MachineName
  - Manejar error sin abortar instalación.

---

# Implementación Técnica en el Repositorio

Se debe:

1. Crear carpeta Installer/ o Setup/.
2. Generar proyecto o script del instalador.
3. Asegurar que el build Release incluya:
   - Ejecutable principal.
   - Archivo config.
   - DLLs necesarias.
4. Crear archivo config template con placeholders.
5. El instalador debe reemplazar placeholders por valores ingresados.

---

# Documentación Obligatoria

Crear dentro de DocumentacionClaude:

- Instalador_Guia_Tecnica.md
- Instalador_Guia_Usuario.md

La guía técnica debe explicar:
- Cómo generar el instalador.
- Cómo actualizarlo ante nueva versión.

La guía de usuario debe explicar:
- Cómo instalar el sistema.
- Qué datos se deben tener antes de iniciar.

---

# Preguntas que deben resolverse antes de finalizar

1. ¿Se debe soportar SQL Authentication y Windows Authentication?
2. ¿Nombre definitivo del connection string?
3. ¿Los usuarios tendrán permisos de administrador?
4. ¿Se dispone del redistribuible oficial de DigitalPersona?
5. ¿Se debe registrar automáticamente la terminal en la base?
6. Nombre comercial final del producto e ícono oficial.

---

# Definition of Done

Se considera terminado cuando:

- Se genera instalador (.exe o .msi).
- En una PC limpia:
  - Se instala correctamente.
  - Se configuran datos SQL.
  - Se crean accesos directos.
  - La aplicación inicia sin errores.
- Documentación completa entregada.

---

Fin del documento.