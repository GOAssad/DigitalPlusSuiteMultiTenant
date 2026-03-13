# DIGITALPLUS - Lista de Pendientes

**Fecha:** 2026-03-12

---

## COMPLETADO RECIENTEMENTE

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

*Actualizado: 2026-03-12*
