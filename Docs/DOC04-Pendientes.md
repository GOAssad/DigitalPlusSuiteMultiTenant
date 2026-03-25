# DIGITALPLUS - Lista de Pendientes

**Fecha:** 2026-03-24

---

## COMPLETADO RECIENTEMENTE

### Sesion 2026-03-24 (noche)
- [x] **Plan Enterprise completo:** Vista dedicada Portal MT (ilimitado, renovacion, contacto), activacion manual desde Portal Licencias, reversion de plan
- [x] **Solicitud Enterprise desde Portal MT:** Boton "Solicitar Enterprise" con modal y motivo, registro en SolicitudSoporte, email a notify@integraia.tech, banner pendiente
- [x] **Enterprise LSQ parcial:** Campo LsqVariantIdCustom en Empresas, boton "Generar link de pago" en Portal Licencias, create-checkout con custom_data.plan="enterprise", webhook lee custom_data.plan
- [x] **Middleware jaula excluye Enterprise manual:** EsEnterpriseManual (plan=enterprise + PlanOrigen=manual) no se bloquea por expiracion
- [x] **Completar solicitudes al activar/revertir:** Al activar Enterprise o revertir plan, las solicitudes Enterprise pendientes se marcan como Completadas
- [x] **Badge tipo Enterprise en Portal Licencias:** Dashboard muestra badge celeste "Enterprise" en vez de rojo "Eliminacion"
- [x] **Revertir plan desde Portal Licencias:** Boton "Revertir plan" con selector Free/Basic/Pro, resetea PlanOrigen y limpia LSQ
- [x] **Fix combo plan LicenciaDetalle:** "Premium" cambiado a "Pro"
- [x] **Limpieza DOC04:** Sesiones antiguas colapsadas, items completados marcados
- [x] **Limpieza gitignore:** Patron publish-*/ excluye carpetas deploy, 290+ binarios removidos del repo
- [x] **DOC01 actualizado a v17.0** con Enterprise, jaula, confirmacion inline, EventoCalendario, permisos sucursal
- [x] Commits 820801a, 93e19c7, 4a141bf, 3eeee88, 444a92b pushed
- [x] Deploy Portal MT, Portal Licencias, Azure Functions a Azure

### Sesion 2026-03-24
- [x] **Suscripcion expirada: banner global** — Warning >7d, danger <=7d, expirado en todas las paginas del Portal MT
- [x] **Middleware "jaula" por suscripcion expirada** — Redirige a /configuracion/planes, usuario puede recontratar
- [x] **Boton "Volver a contratar"** en card plan actual + "Volver a suscribir" en gestion suscripcion
- [x] **Webhook subscription_cancelled separado de subscription_expired** — No degrada a free prematuramente
- [x] **CancelSubscription parsea ends_at** de respuesta LSQ para guardar PlanVencimiento
- [x] **Fichador: refresh sucursal terminal** antes de cada fichada (oTerminal.Inicializar)
- [x] **Fichador: actualiza sucursal** si config difiere de BD al arrancar (soporte reinstalacion)
- [x] **Instalador: selector de sucursal** despues de activar codigo (/api/activar devuelve sucursales)
- [x] **Instalador: SucursalId en config template** del Fichador
- [x] **Hostinger: unificada carpeta instalador/** (minuscula), eliminada duplicada Instalador/
- [x] **Confirmacion inline (Confirmar/Cancelar)** antes de eliminar/desactivar en 9 listados del Portal MT
- [x] **Lemon Squeezy integrado como pasarela de pago real** — Checkout, webhooks, cancelacion, alertas
- [x] **EventoCalendario integrado en 4 reportes** — Ausencias, LlegadasTarde, AsistenciaDiaria, HorasTrabajadas
- [x] **PermiteMovil condicionado** por MobileHabilitado del legajo
- [x] Commit 3c6919b pushed

### Sesion 2026-03-23 (completa)
- [x] **SitioWeb integrado al repo:** Carpeta SitioWeb/ con index.html, digital-one.html, enviar.php, images, instalador
- [x] **digital-one.html actualizado con DOC02:** Fichada QR (4ta modalidad), Administrador corregido (rol simplificado), Portal expandido (importacion Excel, calendario, incidencias, reportes), seccion Reportes (4 cards), seccion Modo Kiosko, link Ayuda en nav
- [x] **digital-one-help.html creado:** Centro de Ayuda online completo basado en DOC02 con sidebar navegable, buscador, 30+ anclas para deep-linking desde Portal MT
- [x] **Ayuda contextual en Portal MT:** Boton ? en top-bar + link Ayuda en sidebar. JS contextual (contextual-help.js) mapea 22 rutas del portal a anclas del help via polling 500ms
- [x] **Deploy sitio web a Hostinger** y **Deploy Portal MT a Azure**
- [x] **DOC06-SitioWeb_IntegraIA.md creado**
- [x] **Reescritura completa panel camara Administrador:** Eliminados todos los controles AForge, panel construido 100% programaticamente, PictureBox estandar SizeMode.Zoom, patron Fichador (lock+BeginInvoke), 3 estados (SinFoto/Preview/ConFoto), botones dark/gold
- [x] **Deteccion camara ocupada (Fichador + Administrador):** Timer 1.5s sin frames = ocupada, muestra nombre de la app que usa la camara
- [x] **Instalador mejorado:** PrepareToInstall detecta apps abiertas, pregunta antes de cerrar con taskkill
- [x] **Ciclo deploy web establecido:** Instalador se copia como nombre fijo a Hostinger, HTMLs se sincronizan
- [x] **Permisos fichada por sucursal VERIFICADOS:** PermiteHuella/Pin/Qr/Movil/Kiosko validados end-to-end (SP desktop + MobileController + Kiosko)
- [x] **PlanConfig.Valor decimal:** BD alterada a decimal(18,2), soporta precios con centavos en ambos portales
- [x] **Precio anual con descuento dinámico:** Cabecera planes muestra mensual + anual + % OFF calculado
- [x] **Precios en USD:** Leyenda "dolares estadounidenses" en seccion planes Portal MT
- [x] **Fix MaxTerminalesMoviles ∞:** Agregado a EsParametroCantidad en Portal Licencias

### Sesion 2026-03-23 (anterior)
- [x] **Suspender/reactivar empresa** — Portal Licencias zona peligrosa, sincroniza Estado (admin) + IsActive (MT)
- [x] **Validacion GPS SucursalForm** — Toggle deshabilitado sin coordenadas, auto-activa al poner ubicacion
- [x] **Revalidacion auth 1 min** — IdentityRevalidatingAuthenticationStateProvider verifica IsActive cada 1 min, SQL directo, fail-open
- [x] **Fix camara AForge** — SignalToStop + WaitForStop en DetenerCamara y FormClosed
- [x] **Upgrade plan genera CodigoActivacion** — Fix: al confirmar upgrade se regenera codigo si no existia
- [x] Deploy Portal MT y Portal Licencias a Azure
- [x] Commit d974e77 pushed

### Sesion 2026-03-20 (noche)
- [x] **Sistema de versionado automatico:** Script `Tools/update-build-number.ps1` genera BuildInfo.cs con timestamp YYYYMMDDHHMI en todos los proyectos
  - Fichador: version en barra titulo + label esquina inferior derecha (7pt, discreto)
  - Administrador: version en panel de bienvenida sidebar
  - Portal MT: version en login (debajo del copyright) + pie del sidebar
  - Portal Licencias: version en login (debajo del copyright) + pie del sidebar
  - Instalador: nombre del .exe incluye version (DigitalPlus_Cloud_Setup_v1.0.0-YYYYMMDDHHMI.exe)
  - BuildInfo.cs compartido via Global.Datos (desktop) y por proyecto (portales Blazor)
- [x] **Fix credenciales en activacion por codigo:** Endpoint `/api/activar` ahora devuelve email+password y envia email de bienvenida (igual que /api/activar-free)
  - Nuevo metodo BuscarAdminEmailPorEmpresaIdAsync en MultiTenantProvisioningService
  - Instalador parsea email/password y muestra MsgBox con credenciales
- [x] **Boton "Activar Dispositivo" en Terminales Moviles:** Modal con buscador de legajos para generar codigo de activacion sin terminal previo
  - Debounce 400ms para evitar concurrencia DbContext
  - Auto-activa MobileHabilitado al seleccionar legajo
- [x] **Fix cross-tenant login movil con codigo activacion:** Login acepta `codigoActivacion` para resolver empresa cuando legajo existe en multiples empresas
  - PWA captura `?code=XXXX` de la URL del email y lo envia junto con el login
- [x] **Fix timezone fichada movil:** FechaHora ahora usa Clock.Now (hora Argentina) en vez de request.Timestamp (UTC)
  - Queries de "hoy" usan Clock.Today en vez de DateTime.UtcNow.Date
- [x] **Anti-duplicado SP EscritorioFichadasSPSALIDA:** UPDLOCK + HOLDLOCK, ignora fichadas < 30 segundos
- [x] Deploy Portal MT, Portal Licencias a Azure
- [x] Instalador recompilado con version 1.0.0-202603202038

### Sesion 2026-03-20
- [x] **Importador Excel de legajos en Portal MT:** Carga masiva desde archivo .xlsx
  - Plantilla descargable con headers + hoja "Valores válidos" (Sectores, Categorías, Horarios, Sucursales del tenant)
  - Preview con validación fila por fila: campos requeridos, existencia de sector/categoría/horario/sucursal, duplicados
  - Filas válidas en blanco, filas con error en rojo con detalle
  - Legajos existentes se omiten (skip, no update)
  - Validación de límite de licencia antes de importar
  - QrToken auto-generado, MobileHabilitado=false, IsActive=true
  - Múltiples sucursales separadas por `;`
  - Archivos: ExcelImporter.cs (nuevo), LegajosList.razor (modal importación)
  - DOC02 actualizado a v15.0

### Sesion 2026-03-18 (noche)
- [x] **Fichador desktop QR (Fase 6):** Modo QR con camara USB en Fichador WinForms (AForge.Video.DirectShow + ZXing.Net)
- [x] **Rediseno Fichador dark theme:** Full dark theme, botones de modo pill, form 620x660, cards azul profundo
- [x] BuscarPorQrToken() en RRHHLegajosPin con validacion GUID y comparacion sin guiones (REPLACE)
- [x] Fix cierre form con camara: flag _cerrando + BeginInvoke elimina deadlock AForge/UI thread
- [x] Fichada.Origen cambiado de OrigenFichada? (enum) a string? para compatibilidad con BD nvarchar
- [x] Fix icono PIN: bi-dialpad no existe en Bootstrap Icons → bi-keyboard
- [x] Fix Asistencia Diaria: columnas Horas/Origen invertidas, Origen ahora muestra todos los origenes del dia
- [x] Fix concurrencia FichadasList: guard _cargando evita doble click en Buscar
- [x] Instalador Liviano: incluye DLLs AForge y ZXing
- [x] Deploy Portal MT a Azure
- [x] DOC01-DOC05 actualizados
- [x] Commit 3c234b3 pushed

### Sesion 2026-03-18
- [x] **Modo Kiosko + Fichada QR completo (Fases 1-5):**
  - BD: Legajo.QrToken (GUID, unique index), TerminalMovil.ModoKiosko + SucursalId, OrigenFichada.QR
  - Backend: POST /api/mobile/fichar-qr, GET /api/mobile/mi-qr, GET /api/mobile/kiosko-info
  - Portal Admin: Registrar Kiosko (modal), boton QR por legajo, impresion masiva de credenciales
  - PWA Mobile: tab "Mi QR" con QR grande del empleado
  - Kiosko Web: /kiosko/ con html5-qrcode (scanner camara), setup Device ID, overlay resultado
- [x] KioskoHabilitado: flag por empresa (Licencias + MT), claim, menu condicional
- [x] Fix cambio de plan: ActualizarLicenciaAsync aplica PlanConfig al cambiar plan (MaxLegajos, MaxSucursales, MaxFichadasMes)
- [x] Fix cross-tenant PIN SPs: EscritorioLegajoPIN_Verificar y _Cambiar ahora filtran por @EmpresaId
- [x] Fix Forzar cambio PIN: reemplazado SP inexistente por UPDATE inline con EmpresaId
- [x] UX dialogo PIN Administrador: mensaje explica [Si]=Resetear, [No]=Eliminar, [Cancelar]=Nada
- [x] Hora fichada QR: Clock.Now (Argentina) en vez de DateTime.UtcNow
- [x] Cooldown QR: compara CreatedAt (UTC) en vez de FechaHora para evitar mezcla timezones
- [x] Icono QR en fichadas: bi-qr-code en LegajoForm, FichadasList, AsistenciaDiaria
- [x] JS libraries: qrcode.min.js (generador), qr-helper.js (render+print), html5-qrcode.min.js (scanner)
- [x] Deploy Portal MT y Portal Licencias a Azure
- [x] Instalador recompilado con fix PIN SPs
- [x] DOC01-DOC05 actualizados

### Sesion 2026-03-17
- [x] Calendario visual tipo Google Calendar en tab Legajo (grilla mensual, eventos con HoraDesde/HoraHasta, guardado directo a BD)
- [x] EventoCalendario: agregados HoraDesde y HoraHasta (TimeOnly), migracion EF + ALTER TABLE Ferozo
- [x] Eliminado switch HasCalendarioPersonalizado de UI (todos los legajos tienen calendario, eventos son capa adicional)
- [x] Fix concurrencia buscador LegajosList (guard _cargando + cancelar debounce en Buscar)
- [x] Deploy Portal MT a Azure
- [x] DOC02 actualizado a v12.0

### Sesion 2026-03-16
- [x] Portal Licencias: tab Legajos en detalle empresa (query cross-DB, tabla con buscador, carga async)
- [x] Fix CRITICO: login movil cross-tenant — MobileController.Login filtra por empresa del dispositivo, rechaza si ambiguo
- [x] Fix CRITICO: RRHHLegajosPin desktop — agregado @EmpresaId a VerificarPin, CambiarPin, CargarLegajo, ListaLegajosActivos
- [x] FK compuesto en Fichada: `(LegajoId, EmpresaId) → Legajo(Id, EmpresaId)` impide mezcla cross-tenant a nivel BD
- [x] Limpieza de 2 fichadas huerfanas (EmpresaId no coincidia con LegajoEmpresaId)
- [x] Instalador recompilado con fix de PIN desktop
- [x] Deploy Portal Licencias y Portal MT a Azure
- [x] DOC01, DOC03, DOC04 actualizados

### Sesiones anteriores (2026-03-08 a 2026-03-15) — Resumen

<details>
<summary>2026-03-15 noche — Simplificar Administrador, foto+domicilio Portal MT</summary>

- Administrador simplificado: solo 2 pestanas (Legajo: huellas+foto, Movil), datos solo lectura, guardar solo persiste huellas+foto
- Portal MT: foto legajo, seccion Domicilio 7 campos, sucursal obligatoria, CRUD completo legajos
- Instalador: carpeta seleccionable, TLS 1.2, registro Free idempotente
- Connection strings: todos apuntan a Ferozo (localhost removido)
</details>

<details>
<summary>2026-03-14 — Roles Portal MT, Limpiar/Eliminar empresa, email SMTP, activacion movil</summary>

- Roles: SuperAdmin/AdminEmpresa/Operador/Consulta con restricciones por pagina y AuthorizeView
- Portal Licencias: Limpiar y Eliminar empresa con doble confirmacion, dashboard "Uso del sistema"
- Email SMTP con MailKit (notify@integraia.tech), email activacion movil con deep link
- Contraste visual mejorado, icono PWA D1 dorado
</details>

<details>
<summary>2026-03-13 — Terminal Movil v2 completa (backend + PWA + control acceso)</summary>

- Backend: MobileController 4 endpoints JWT, entidades EF Core, UbicacionService GPS
- PWA: login+activacion+fichada+historial, probada iPhone/Android contra Azure
- Control acceso: MobileHabilitado empresa+legajo, PIN desde Portal MT, menu condicional
- Sucursales: CRUD mejorado, mapa Leaflet, validacion GPS por sucursal asignada
- Rediseno PWA dark theme con mapa GPS y reloj en vivo
- Tag v1.0-pre-mobile (commit 730589f)
</details>

<details>
<summary>2026-03-12 — Homologacion visual, auto-registro terminal, instalador</summary>

- Homologacion visual: Administrador layout 80/20, Portal MT tema oscuro, Portal Licencias dark
- Fix foto legajo (SP), fix PIN forzado Fichador, fix floating labels
- Auto-registro terminal en BD, compilacion Release, InstaladorLiviano InnoSetup
- Deploy Portal MT y Licencias a Azure
</details>

<details>
<summary>2026-03-11 — Instalador produccion, usuario SQL dp_app_svc, verificacion estado</summary>

- Instalador apunta a Azure, fix connection string "Local"
- Usuario SQL dp_app_svc con permisos granulares, separacion CloudSql/ClientSql
- API /api/activar con EmpresaId real, /api/verificar-estado
- Desactivacion de empresas bloquea Portal MT y apps desktop
</details>

<details>
<summary>2026-03-08 a 2026-03-10 — Arquitectura MT, BD Ferozo, provisioning</summary>

- Arquitectura multi-tenant (TenantContext, EmpresaId, tablas singular)
- BD DigitalPlusMultiTenant en Ferozo (29 tablas, migracion Kosiuko 758 legajos + 784K fichadas)
- Auto-provisioning usuario admin, forzar cambio password primer login
- Logo empresa en header, identidad empresa (redes sociales), PortalLicencias sincronizado
- Fichador: PIN voluntario/forzado/reseteo. Administrador: tab PINs
</details>

---

## TERMINAL MOVIL v2: COMPLETADA

### Etapa 2a - Backend + Admin: COMPLETADA
- [x] Backend (MobileController, entidades, JWT, UbicacionService)
- [x] Admin desktop (tab Movil en Legajos)
- [x] Portal MT (paginas terminales-moviles y fichado-movil)
- [x] BD Ferozo (3 tablas creadas via migracion EF Core)

### Etapa 2b - App Movil (PWA): COMPLETADA
- [x] PWA en wwwroot/mobile/ (HTML+CSS+JS estatico)
- [x] Login con legajo + PIN
- [x] Activacion de dispositivo con codigo
- [x] Fichada con GPS
- [x] Historial de fichadas del dia
- [x] Service worker + manifest.json (instalable como PWA)
- [x] Probada end-to-end en iPhone y Android contra Azure

### Control de acceso: COMPLETADO
- [x] MobileHabilitado a nivel empresa (Portal Licencias + Portal MT)
- [x] MobileHabilitado a nivel legajo (Portal MT)
- [x] Menu condicional en NavMenu (claim MobileHabilitado)
- [x] Gestion de PIN desde Portal MT (asignar, cambiar, resetear)
- [x] Jwt config en appsettings.json de produccion
- [x] Deploy Portal MT y Portal Licencias a Azure

## PRIORIDAD ALTA

- [x] **Importador Excel de legajos en Portal MT** — COMPLETADO (sesion 2026-03-20)
- [x] ~~**Portal Licencias: tab Legajos en detalle empresa**~~ COMPLETADO 2026-03-16
- [ ] **SuperAdmin cross-tenant** — admin@integraia.tech debe poder seleccionar empresa y ver datos de cualquier tenant. Requiere selector de empresa + bypass query filters.
- [x] ~~**Modo Kiosko para terminales moviles**~~ COMPLETADO 2026-03-18
- [ ] **Registro de huellas desde el Portal Web** — Agente local liviano + WebSocket. PAUSADO (la funcionalidad la cubre el Administrador desktop).

## INMEDIATO (Validacion end-to-end)

- [ ] **Probar circuito completo en produccion (Ferozo):** Enviar instalador + codigo a otro usuario -> instalar -> verificar auto-registro terminal -> fichar -> ver en portal web
  - PARCIAL: New Family probada con instalador + fichada + terminal + sucursal + permisos (sesion 2026-03-24). Falta prueba con usuario externo.

## SEGURIDAD SQL

- [ ] **Deploy appsettings.json de DigitalPlusWeb** con `dp_web_svc` (pausado hasta revisar sistema web completo)
- [ ] **Monitoreo de conexiones SQL** - Verificar que no hay conexiones residuales con `sa`
- [ ] **Deshabilitar `sa`** - Solo despues de estabilizacion

## DOCUMENTACION

- [x] DOC01 - Reporte Arquitectura Project Leader (v7.0)
- [x] DOC02 - Manual Usuario Final (v16.0)
- [x] DOC03 - Manual Portal Licencias Integra IA (v5.0)
- [x] DOC04 - Lista de Pendientes (actualizado 2026-03-24)
- [x] DOC05 - Terminal Movil DigitalOne
- [x] DOC06 - Sitio Web IntegraIA (v1.0) — estructura, deploy Hostinger, anclas, integracion Portal MT
- [ ] **Agregar capturas de pantalla** al DOC02 y DOC03 (requiere ejecutar las apps y tomar screenshots)

## FUNCIONALIDAD (Proximas sesiones)

- [x] ~~**Validar permisos de fichada por sucursal**~~ COMPLETADO y VERIFICADO 2026-03-23 — PermiteHuella/Pin/Qr/Movil/Kiosko validados en SP desktop, MobileController y Kiosko
- [x] ~~**Fichador desktop QR (Fase 6)**~~ COMPLETADO 2026-03-18
- [x] ~~**Upgrade plan Fase 3: Lemon Squeezy**~~ COMPLETADO 2026-03-24 — Checkout, webhooks, cancelacion, alertas. Commits dae6f6f..8ee8edf
- [x] ~~**Probar baja de plan y verificar advertencia del sistema**~~ COMPLETADO 2026-03-24 — Cancelacion probada end-to-end con Lemon Squeezy (webhook + banner + jaula + recontratar)
- [ ] **Instalador web: cache de navegador puede descargar version vieja** — Instruir Ctrl+Shift+Supr antes de descargar
- [x] ~~**Plan Enterprise: tratamiento especial en pantalla de planes**~~ COMPLETADO 2026-03-24 — Vista dedicada, solicitud con email, activacion manual/LSQ, reversion
- [ ] **Enterprise LSQ end-to-end** — Probar flujo completo: crear producto custom en LSQ dashboard, cargar Variant ID, generar link, cliente paga, webhook activa plan Enterprise con cobro automatico recurrente
- [ ] **Probar Portal MT con distintos roles** — Verificar permisos por pagina con usuarios SuperAdmin, AdminEmpresa, y roles menores
- [ ] **TimeZone por Sucursal** — Campo TimeZone en tabla Sucursal para soporte multi-pais (reemplazar Clock.Now hardcodeado)
- [x] ~~**Generar PIN desde Portal MT**~~ COMPLETADO 2026-03-13 — LegajoForm tiene seccion PIN Movil (asignar, cambiar, resetear)
- [x] ~~**Upgrade de plan: validaciones**~~ COMPLETADO 2026-03-22 — Verificacion email obligatoria pre-upgrade, codigo activacion se regenera en confirmar
- [ ] **Verificar durabilidad de licencias** - Analizar si la licencia es por terminal (MachineId) o permite instalacion ilimitada. Definir politica.
- [ ] **Adaptar InstaladorUnificado para trial web** - Que el usuario pueda registrarse desde la web, descargar el instalador y probar en modo LOCAL con SQL Express.
- [ ] **Link al portal web en Administrador** - Agregar en la app Administrador un acceso directo que abra el portal web multi-tenant en el navegador.
- [ ] **Link a integraia.itech en Administrador** - Link fijo a la pagina de Integra IA en el menu.
- [ ] **Paginacion en Asistencia Diaria del portal MT** - Implementar paginacion en la consulta de fichadas.
- [ ] **Prueba de clonacion** - Clonar DigitalPlusSuiteMultiTenant desde GitHub en otra ubicacion y verificar que todo compila y es recuperable.
- [ ] **Remover diagnostico temporal del Fichador** (DiagnosticoBD, HuellaLog)
- [ ] **Revertir appsettings.json ClientSql** a dp_app_svc (actualmente con sa para debug)

## PRIORIDAD MEDIA

- [ ] **Revisar PortalMultiTenant** en browser - Probar dashboard, CRUDs, reportes contra DigitalPlusMultiTenant en Ferozo
- [ ] **Completar documentacion** con capturas reales de las apps
- [ ] **Tests automatizados** - No hay tests en ninguno de los componentes
- [ ] **Sistema de logging** - Las apps desktop no tienen logging estructurado

## PRIORIDAD BAJA

- [ ] Auto-fetch datos fiscales desde APIs gubernamentales (AFIP, etc.)
- [ ] WhatsApp integration para notificaciones
- [ ] Contactos de empresa en el portal
- [ ] Valores de suscripcion / facturacion en el portal
- [ ] Ampliar portal web con reportes avanzados
- [ ] Limpieza dead code en Administrador
- [ ] Evaluar migracion de .NET Framework 4.8 a .NET 8+ (apps desktop)
- [ ] Implementar backup automatizado para BDs de empresas en Ferozo
- [ ] Dashboard de monitoreo: heartbeats, licencias por vencer, empresas sin actividad
- [ ] InstaladorUnificado: fix compilacion InnoSetup (FichIcon path)
- [ ] Dominio personalizado para portales

---

## NOTAS

- El deploy de appsettings.json de produccion (dp_web_svc) esta **PAUSADO** por decision del usuario.
- No deshabilitar `sa` todavia.
- Portal Licencias en Azure: `digitalpluslicencias.azurewebsites.net`
- Portal MT en Azure: `digitalplusportalmt.azurewebsites.net`
- DigitalPlusApp (Azure): version anterior del portal, en uso actualmente por clientes - **NUNCA deployar aqui**
- InnoSetup: `C:\Users\Gustavo\AppData\Local\Programs\Inno Setup 6\`
- MSBuild: `"C:/Program Files/Microsoft Visual Studio/18/Community/MSBuild/Current/Bin/MSBuild.exe"`
- Repos legacy (DigitalPlusDesk_Claude, DigitalPlusWeb_Claude) estan ARCHIVADOS
- BD Ferozo: `sd-1985882-l.ferozo.com,11434` - DigitalPlusMultiTenant, DigitalPlusAdmin, DigitalPlus (legacy)
- Todo el codigo esta en GitHub: `GOAssad/DigitalPlusSuiteMultiTenant`

---

*Actualizado: 2026-03-24*
