# SITIO WEB INTEGRAIA — Documentacion Tecnica

**Version:** 1.0
**Fecha:** 2026-03-23

---

## 1. INFORMACION GENERAL

| Campo | Valor |
|---|---|
| Dominio | https://integraia.tech |
| Hosting | Hostinger |
| Acceso SSH | `ssh -p 65002 u595555499@82.25.67.239` |
| Document root | `/home/u595555499/domains/integraia.tech/public_html/` |
| Stack | HTML + CSS + JS + PHP (formulario de contacto) |
| Repositorio | `DigitalPlusSuiteMultiTenant/SitioWeb/` |

---

## 2. ESTRUCTURA DE ARCHIVOS

```
SitioWeb/
  index.html                    Landing page IntegraIA (empresa)
  digital-one.html              Pagina de producto Digital One
  digital-one-help.html         Centro de Ayuda online (manual usuario)
  enviar.php                    Backend formulario de contacto
  images/
    Logo-IntegraIA.png          Logo principal de IntegraIA
    digital-one/                Imagenes del producto Digital One
      hero-digital-one.png      Hero composicion (3 interfaces)
      fichador-huella.png       Fichador modo huella
      fichador-pin.png          Fichador modo PIN
      fichador-semaforo.png     Fichador con semaforo verde
      fichador-movil.png        Fichador movil PWA
      administrador-principal.png  Administrador pantalla principal
      administrador-huellas.png    Administrador registro huellas
      portal-dashboard.png      Portal Web dashboard
      portal-legajos.png        Portal Web lista legajos
      portal-fichadas.png       Portal Web fichadas
      digital-one-hero.svg      Hero SVG decorativo
      mobile-fichada-gps.svg    Ilustracion GPS movil
    logos/
      digital-one.svg           Logo Digital One (completo)
      digital-one-icon.svg      Icono Digital One (huella)
      face-one.svg              Logo Face One
      face-one-icon.svg         Icono Face One
      erpassistant.svg          Logo ERPAssistant
      erpassistant-icon.svg     Icono ERPAssistant
  Instalador/
    DigitalPlus_Cloud_Setup_v1.0.exe   Instalador cloud (~25 MB)
    Manual_Usuario_DigitalOne.pdf      Manual en PDF
    DOC02-Manual_Usuario_Final.md      Manual fuente markdown
  Docs/
    INSTRUCCIONES_PAGINA_DIGITAL_ONE.md   Instrucciones de contenido
    PENDIENTES_DIGITAL_ONE.md             Pendientes del sitio
    generar_manual.py                     Script generacion PDF
```

---

## 3. PAGINAS

### 3.1 index.html — Landing IntegraIA

Pagina principal de la empresa IntegraIA. Secciones:
- Hero con badge, stats y CTA
- Quienes Somos (3 cards)
- Servicios (6 cards: transformacion digital, ERP, IA, optimizacion, analytics, desarrollo)
- Productos (3 cards: Digital One, Face One, ERPAssistant)
- Proceso (5 pasos)
- Formulario de contacto (AJAX → enviar.php)
- Footer + WhatsApp flotante

### 3.2 digital-one.html — Producto Digital One

Pagina de producto orientada a decision de compra. Audiencia: duenos de empresa, RRHH. Secciones:
- Hero con CTA
- Modalidades de fichada (4 cards: Huella, PIN, Movil, QR)
- Semaforo visual (feedback inmediato)
- Fichada movil (PWA + GPS + PIN)
- Administrador desktop (rol simplificado: huellas + foto)
- Portal Web (8 features: dashboard, empleados, fichadas, calendario, incidencias, estructura, terminales, usuarios)
- Reportes (4 cards: asistencia, llegadas tarde, ausencias, horas)
- Modo Kiosko (circuito en 5 pasos)
- Multi-empresa
- Descargas (instalador cloud, completo, manual, PWA movil)
- Planes (Trial, Basico, Profesional, Enterprise)
- CTA final

### 3.3 digital-one-help.html — Centro de Ayuda

Manual de usuario completo online basado en DOC02-Manual_Usuario_Final.md. Caracteristicas:
- **Sidebar navegable** con indice por seccion
- **Buscador** que filtra secciones por texto
- **30+ anclas** para deep-linking desde el Portal MT
- **Highlight automatico** del link activo segun scroll
- **Responsive** con sidebar colapsable en mobile
- Mismo look & feel dark theme del sitio

#### Anclas disponibles para deep-linking

| Ancla | Seccion |
|---|---|
| `#introduccion` | Introduccion al sistema |
| `#componentes` | Componentes (Fichador, Admin, Portal, Mobile) |
| `#requisitos` | Requisitos del equipo |
| `#instalacion` | Instalacion general |
| `#instalacion-completa` | Instalador completo (local) |
| `#instalacion-cloud` | Instalador cloud (nube) |
| `#instalacion-free` | Registro Free |
| `#primeros-pasos` | Primeros pasos |
| `#fichador` | Pantalla principal del Fichador |
| `#fichador-huella` | Fichada por huella digital |
| `#fichador-pin` | Fichada por PIN |
| `#fichador-cambiar-pin` | Cambiar PIN |
| `#fichador-qr` | Fichada por QR (camara) |
| `#fichador-demo` | Modo demostracion |
| `#fichador-modos` | Cambio de modo y deteccion hardware |
| `#administrador` | Administrador desktop |
| `#administrador-huellas` | Registrar huellas digitales |
| `#administrador-foto` | Capturar foto |
| `#administrador-movil` | Dispositivo movil (desde Admin) |
| `#portal` | Portal Web acceso y primer login |
| `#portal-roles` | Roles de usuario |
| `#portal-dashboard` | Dashboard |
| `#portal-legajos` | Gestion de empleados |
| `#portal-importar-excel` | Importar desde Excel |
| `#portal-legajo-form` | Formulario de legajo (pestanas) |
| `#portal-fichadas` | Fichadas |
| `#portal-calendario` | Calendario personal |
| `#portal-incidencias` | Incidencias y vacaciones |
| `#portal-reportes` | Reportes |
| `#portal-estructura` | Estructura organizacional |
| `#portal-usuarios` | Usuarios |
| `#terminales-moviles` | Terminales moviles |
| `#activar-dispositivo` | Activar dispositivo movil |
| `#fichada-movil` | Fichada desde el celular |
| `#kiosko` | Modo Kiosko |
| `#credenciales-qr` | Credenciales QR |
| `#licencias` | Sistema de licencias |
| `#licencias-trial` | Periodo de prueba |
| `#licencias-activar` | Activar licencia |
| `#faq` | Preguntas frecuentes |
| `#soporte` | Soporte tecnico |

#### Integracion con Portal MT

El Portal Web Multi-Tenant tiene ayuda contextual integrada:

1. **Boton ? en top-bar** — Icono en la barra superior que abre la seccion de ayuda correspondiente a la pagina actual del portal.
2. **Link "Ayuda" en sidebar** — Al final del menu lateral, link generico al centro de ayuda.
3. **JS contextual** — `wwwroot/js/contextual-help.js` mapea rutas del portal a anclas del help (polling cada 500ms para detectar navegacion Blazor via SignalR).

Mapeo de rutas Portal → Anclas Help:

| Ruta Portal MT | Ancla Help |
|---|---|
| `/` | `#portal-dashboard` |
| `/legajos` | `#portal-legajos` |
| `/legajos/{id}` | `#portal-legajo-form` |
| `/fichadas` | `#portal-fichadas` |
| `/horarios`, `/categorias`, `/sectores`, `/sucursales`, `/feriados` | `#portal-estructura` |
| `/terminales-moviles` | `#terminales-moviles` |
| `/fichado-movil` | `#fichada-movil` |
| `/incidencias`, `/vacaciones` | `#portal-incidencias` |
| `/reportes/*` | `#portal-reportes` |
| `/usuarios` | `#portal-usuarios` |
| `/configuracion/planes` | `#licencias` |

---

## 4. FORMULARIO DE CONTACTO

**Archivo:** `enviar.php`
- **Metodo:** POST
- **Campos:** nombre (req), empresa (opt), email (req), servicio (opt), mensaje (opt)
- **Destino:** `administracion@integraai.com.ar`
- **From:** `no-reply@integraia.tech`
- **Reply-To:** email del visitante
- **Respuesta:** JSON `{"ok": true}` o `{"ok": false, "error": "..."}`

---

## 5. DISENO

### Paleta de colores
| Variable | Valor | Uso |
|---|---|---|
| `--bg` | `#050810` | Fondo principal |
| `--surface` | `#0b1120` | Superficie elevada |
| `--gold` | `#c9a84c` | Acento principal |
| `--gold-light` | `#e8c97a` | Acento hover |
| `--text` | `#e8eaf0` | Texto principal |
| `--muted` | `#7a8394` | Texto secundario |
| `--accent-blue` | `#3a7bfd` | Acento secundario |

### Tipografia
- **Headings:** Cormorant Garamond (serif), weights 300-700
- **Body:** Figtree (sans-serif), weights 300-600
- **Fuente:** Google Fonts CDN

### Responsive
- Breakpoint principal: 900px (hamburger menu, grids a 1 columna)
- Breakpoint secundario: 500px (planes a 1 columna)
- Nav fija con efecto scroll

---

## 6. DEPLOY A HOSTINGER

### Via SFTP (paramiko)

```python
import paramiko
ssh = paramiko.SSHClient()
ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
ssh.connect('82.25.67.239', port=65002, username='u595555499', password='[password]')
sftp = ssh.open_sftp()
sftp.put('SitioWeb/digital-one-help.html', '/home/u595555499/domains/integraia.tech/public_html/digital-one-help.html')
sftp.close()
ssh.close()
```

### Via SSH manual

```bash
sftp -P 65002 u595555499@82.25.67.239
cd domains/integraia.tech/public_html
put digital-one.html
put digital-one-help.html
exit
```

---

## 7. PRINCIPIOS DE EVOLUCION

- El sitio NO es estatico. Evoluciona en paralelo con el sistema.
- Cuando se agrega o mejora funcionalidad en Digital One, evaluar si corresponde reflejarla en el sitio.
- La fuente de verdad para el contenido del sitio es **DOC02-Manual_Usuario_Final.md**. Cuando DOC02 se actualiza, identificar que secciones del sitio impacta.
- El sitio se construye incrementalmente. Gustavo decide cuando publicar.
- La pagina de ayuda (`digital-one-help.html`) debe mantenerse sincronizada con DOC02.

---

## 8. IMAGENES PENDIENTES

| Archivo | Estado |
|---|---|
| `hero-digital-one.png` | Existe (composicion 3 interfaces) |
| `fichador-huella.png` | Existe |
| `fichador-pin.png` | Existe |
| `fichador-semaforo.png` | Existe |
| `fichador-movil.png` | Existe |
| `administrador-principal.png` | Existe |
| `administrador-huellas.png` | Existe |
| `portal-dashboard.png` | Existe |
| `portal-legajos.png` | Existe |
| `portal-fichadas.png` | Existe |
| `fichador-qr.png` | **Pendiente** (captura modo QR con camara) |
| `kiosko.png` | **Pendiente** (tablet con camara QR activa) |
| `multi-empresa.png` | **Pendiente** (diagrama conceptual multi-empresa) |

---

*Actualizado: 2026-03-23*
