# DIGITALPLUS - Lista de Pendientes

**Fecha:** 2026-03-14

---

## COMPLETADO RECIENTEMENTE

### Sesion 2026-03-14
- [x] Roles en Portal MT: SuperAdmin, AdminEmpresa, Operador, Consulta aplicados
- [x] Form pages restringidos por rol (estructura=Admin, operaciones=Admin+Operador)
- [x] Botones de accion ocultos por AuthorizeView en todas las listas
- [x] SuperAdmin removido del combo de creacion de usuarios
- [x] Descripcion de roles en UsuarioForm (que puede hacer cada rol)
- [x] Portal Licencias: boton Limpiar Empresa (elimina transaccionales, mantiene entidades)
- [x] Portal Licencias: boton Eliminar Empresa (borra absolutamente todo de MT y Admin)
- [x] Doble confirmacion para Limpiar/Eliminar (escribir nombre de empresa)
- [x] Portal Licencias: mini dashboard "Uso del sistema" en detalle de empresa (legajos, fichadas, usuarios, sucursales, terminales, ultima fichada, fichadas por origen, dias activos 30d, fichadas 15d por dispositivo)
- [x] Contraste visual mejorado en ambos portales (labels uppercase bold, inputs con borde/sombra, card headers oscuros con linea dorada)
- [x] Servicio de email SMTP con MailKit (smtp.hostinger.com:465 SSL, notify@integraia.tech)
- [x] Pestaña "Movil" en LegajoForm: estado del dispositivo + generar codigo de activacion + envio automatico por email
- [x] Email de activacion con template HTML profesional (codigo + boton deep link)
- [x] Deep link en PWA: lee codigo de URL (?code=XXX) y lo pre-carga en el campo
- [x] Boton Reintentar/Reenviar email en LegajoForm
- [x] Fix re-activacion innecesaria: si legajo ya tiene terminal activa y cambia DeviceId (cache borrada), se actualiza automaticamente sin pedir codigo nuevo
- [x] Icono PWA actualizado: D1 dorado sobre fondo oscuro (192px y 512px)
- [x] .gitignore actualizado (excluye publish/, deploy zips)

### Sesion 2026-03-13 (noche)
- [x] Rediseno PWA Mobile: tema oscuro navy, mapa GPS con anillos animados, reloj en vivo, boton teal, GPS watch continuo
- [x] CRUD Sucursales mejorado: nuevos campos (Direccion, Localidad, Provincia, Telefono, Email)
- [x] Mapa Leaflet/OpenStreetMap integrado en formulario de sucursales (buscador Nominatim, geocoding, reverse geocoding, marker arrastrable, circulo de radio)
- [x] Campos WiFi removidos de UI (BSSID no accesible desde PWA), metodo forzado a SoloGPS
- [x] Validacion GPS por sucursal asignada: fichada movil valida LegajoSucursal antes de resolver GPS
- [x] Errores claros en fichada: "No tiene sucursales asignadas" / "Sin configuracion GPS" / "No se detecto sucursal"
- [x] Migracion EF Core AddSucursalFields aplicada en Ferozo
- [x] Deploy Portal MT a Azure

### Sesion 2026-03-13 (tarde)
- [x] PWA Terminal Movil: app completa en wwwroot/mobile/ (login, activacion, fichada GPS, historial)
- [x] PWA deployada y probada end-to-end en iPhone (Safari) y Android (Chrome) contra Azure
- [x] MobileHabilitado (empresa): flag en Empresa (MT) y Empresas (Admin), checkbox en EmpresaDetalle.razor (Licencias)
- [x] MobileHabilitado (legajo): flag en Legajo, checkbox en LegajoForm.razor (Portal MT, visible solo si empresa mobile habilitada)
- [x] MobileHabilitado claim: CustomClaimsPrincipalFactory agrega claim al login, ITenantService.MobileHabilitado
- [x] NavMenu condicional: links Terminales Moviles y Fichado Movil solo visibles si empresa MobileHabilitado
- [x] Gestion de PIN desde Portal MT: asignar, cambiar, resetear PIN en LegajoForm (seccion PIN Movil)
- [x] Iconos de origen en FichadasList: huella (bi-fingerprint), PIN (bi-dialpad), movil (bi-phone), manual (bi-pencil-square), web (bi-globe), demo (bi-play-circle)
- [x] Terminal en fichadas: muestra nombre terminal (desktop) o "Dispositivo movil" (origen Movil)
- [x] DatabaseName editable en EmpresaDetalle.razor (Licencias), constraint UNIQUE eliminado
- [x] Datos habilitados en Ferozo: Kosiuko y New Family MobileHabilitado=1, legajo 1968 MobileHabilitado=1
- [x] Deploy Portal MT y Portal Licencias a Azure
- [x] Migracion EF Core AddMobileHabilitado aplicada en Ferozo
- [x] DOC01-DOC05 actualizados

### Sesion 2026-03-13 (manana)
- [x] Tag v1.0-pre-mobile creado (commit 730589f) como punto de restauracion pre-v2
- [x] Entidades EF Core: TerminalMovil, SucursalGeoconfig, CodigoActivacionMovil
- [x] OrigenFichada enum: agregado valor Movil
- [x] Migracion EF Core AddTerminalMovilAndGeoconfig aplicada en Ferozo (3 tablas creadas)
- [x] MobileController con 4 endpoints JWT: login, registrar-dispositivo, fichada, estado
- [x] JWT Bearer auth configurado en Program.cs (convive con cookie auth)
- [x] UbicacionService: resolucion sucursal por WiFi BSSID o GPS (Haversine)
- [x] Tab "Movil" en FrmRRHHLegajos del Administrador desktop
- [x] DALs desktop: TerminalMovilDAL, SucursalGeoconfigDAL
- [x] Pagina /terminales-moviles en Portal MT
- [x] Pagina /fichado-movil (SucursalGeoconfigList) en Portal MT
- [x] NavMenu actualizado con links a Terminales Moviles y Fichado Movil
- [x] Script SQL de referencia: Database/003_TerminalMovil_Geoconfig.sql
- [x] Fix: appsettings.Development.json apunta a Ferozo (VS Debug conectaba a localhost)
- [x] DOC01 actualizado a v8.0 con seccion Terminal Movil

### Sesion 2026-03-12
- [x] Homologacion visual Phase 2 (Administrador): layout 80/20 en Legajos, panel camara ensanchado (650px), boton PIN reubicado en zona 20%
- [x] Fix foto legajo: parametro @Foto agregado en SP EscritorioLegajoActualizar y en llenarParametros() de RRHHLegajos.cs
- [x] Fix PIN forzado en Fichador: dialogo obligatorio con solo boton OK (sin escape), separado de creacion de PIN nuevo (Si/No)
- [x] SP EscritorioLegajoPIN_ForzarCambio deployado en Ferozo produccion
- [x] Homologacion visual Phase 3 (Portal MT): tema oscuro integraia.tech, sidebar gradient, login dark, loading gold, reconnect gold, branding "DIGITAL ONE"
- [x] Homologacion visual Phase 4 (Portal Licencias): misma paleta, branding "DIGITAL ONE Licencias", login dark, iconos SVG dorados
- [x] Fix floating labels en ambos portales (background transparent en form-floating)
- [x] Deploy Portal MT a Azure (digitalplusportalmt.azurewebsites.net)
- [x] Deploy Portal Licencias a Azure (digitalpluslicencias.azurewebsites.net)
- [x] Auto-registro de terminal: Fichador registra automaticamente la maquina en BD asociandola a sucursal por defecto
- [x] Compilacion Release de Fichador (TEntradaSalida.exe) y Administrador (Acceso.exe)
- [x] InstaladorLiviano compilado con InnoSetup (DigitalPlus_Cloud_Setup_v1.0.exe)
- [x] Documentacion DOC01-DOC04 actualizada (v7.0, v5.0, v5.0)

### Sesion 2026-03-11
- [x] Instalador liviano listo para produccion: API URL apunta a Azure, AdminConnection, AdminEmpresaId en templates
- [x] Fix bug critico: connection string name "local" -> "Local" en template Administrador
- [x] Usuario SQL dedicado dp_app_svc con permisos granulares (dp_role_app) - script create-dp-app-svc.ps1
- [x] Separacion CloudSql (sa, provisioning) vs ClientSql (dp_app_svc, apps) en Portal Licencias
- [x] API /api/activar retorna EmpresaId real de DigitalPlusMultiTenant (mapeo via Codigo/CompanyId)
- [x] Portal Licencias: CRUD usuarios admin, registro publico deshabilitado
- [x] Portal Licencias: endpoint /api/verificar-estado para apps desktop
- [x] Portal Licencias deployado en Azure (digitalpluslicencias.azurewebsites.net)
- [x] Portal MT: verificacion estado empresa en login (fail-open, consulta DigitalPlusAdmin)
- [x] Portal MT deployado en Azure (digitalplusportalmt.azurewebsites.net)
- [x] Desktop apps: verificacion estado empresa en Form_Load (Fichador + Administrador)
- [x] Desactivacion de empresas: cambiar Estado en Portal Licencias bloquea Portal MT y apps desktop

### Sesion 2026-03-10
- [x] Auto-provisioning usuario admin al crear empresa (MultiTenantProvisioningService)
- [x] Portal MT: forzar cambio de contraseña en primer login (MustChangePassword, middleware, ForceChangePassword.razor)
- [x] Portal MT: DefaultConnection cambiado a Ferozo

### Sesion 2026-03-09
- [x] Portal Multi-Tenant: logo y nombre de empresa en header izquierdo (endpoint `/api/empresa-logo` con cache 1 hora)
- [x] Portal Multi-Tenant y Licencias: fix double-submit en login
- [x] Portal Licencias: seccion "Identidad de la Empresa" con logo, pagina web y 6 redes sociales
- [x] EmpresaInfoService: campos de redes sociales consultados desde DigitalPlusAdmin
- [x] Administrador: menu dinamico con links a redes sociales (reemplaza botones fijos hardcodeados)
- [x] BD Ferozo: columnas redes sociales agregadas a tabla Empresas
- [x] PortalLicencias sincronizado dentro de DigitalPlusSuiteMultiTenant

### Sesion 2026-03-08
- [x] Arquitectura multi-tenant implementada (TenantContext, EmpresaId, tablas singular)
- [x] BD DigitalPlusMultiTenant creada en Ferozo con schema EF Core (29 tablas)
- [x] Migracion datos Kosiuko a DigitalPlusMultiTenant (758 legajos, 784K fichadas)
- [x] Kosiuko registrada en DigitalPlusAdmin (Id=5, CodigoActivacion=EE509930E07E)
- [x] InstaladorLiviano adaptado para multi-tenant (EmpresaId en configs, paths corregidos)
- [x] Validacion de licencia deshabilitada en Fichador y Administrador
- [x] Fichador: cambio voluntario de PIN, PinMustChange, reseteo
- [x] Administrador: tab PINs, filtro combo, resetear PIN, forzar cambio
- [x] SPs legacy reemplazados por SQL directo multi-tenant
- [x] Logo empresa + IntegraIA en Fichador y Administrador

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

- [ ] **SuperAdmin cross-tenant** — admin@integraia.tech debe poder seleccionar empresa y ver datos de cualquier tenant. Requiere selector de empresa + bypass query filters.
- [ ] **Modo Kiosko para terminales moviles** — Flag ModoKiosko en TerminalesMoviles, permite que cualquier legajo de la empresa fiche desde ese dispositivo.
- [ ] **Registro de huellas desde el Portal Web** — Agente local liviano + WebSocket. PAUSADO (la funcionalidad la cubre el Administrador desktop).

## INMEDIATO (Validacion end-to-end)

- [ ] **Probar circuito completo en produccion (Ferozo):** Enviar instalador + codigo a otro usuario -> instalar -> verificar auto-registro terminal -> fichar -> ver en portal web
  - Nota: Kosiuko y New Family ya fueron probadas localmente. Falta prueba con usuario externo.

## SEGURIDAD SQL

- [ ] **Deploy appsettings.json de DigitalPlusWeb** con `dp_web_svc` (pausado hasta revisar sistema web completo)
- [ ] **Monitoreo de conexiones SQL** - Verificar que no hay conexiones residuales con `sa`
- [ ] **Deshabilitar `sa`** - Solo despues de estabilizacion

## DOCUMENTACION

- [x] DOC01 - Reporte Arquitectura Project Leader (v7.0)
- [x] DOC02 - Manual Usuario Final (v5.0)
- [x] DOC03 - Manual Portal Licencias Integra IA (v5.0)
- [x] DOC04 - Lista de Pendientes (actualizado)
- [ ] **Agregar capturas de pantalla** al DOC02 y DOC03 (requiere ejecutar las apps y tomar screenshots)

## FUNCIONALIDAD (Proximas sesiones)

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

*Actualizado: 2026-03-14*
