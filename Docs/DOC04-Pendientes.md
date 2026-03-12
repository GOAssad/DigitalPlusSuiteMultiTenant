# DIGITALPLUS - Lista de Pendientes

**Fecha:** 2026-03-09

---

## COMPLETADO RECIENTEMENTE

### Sesion 2026-03-09
- [x] Portal Multi-Tenant: logo y nombre de empresa en header izquierdo (endpoint `/api/empresa-logo` con cache 1 hora)
- [x] Portal Multi-Tenant y Licencias: fix double-submit en login (deshabilita boton mientras procesa)
- [x] Portal Licencias: seccion "Identidad de la Empresa" con logo, pagina web y 6 redes sociales (Facebook, Instagram, LinkedIn, X/Twitter, YouTube, TikTok)
- [x] Portal Licencias: boton "Editar" reemplaza "Ver" en lista de empresas
- [x] EmpresaInfoService: nuevos campos de redes sociales consultados desde DigitalPlusAdmin
- [x] Administrador: menu dinamico reemplaza botones fijos (Kosiuko/DigitalPlusWeb) por links de redes sociales cargados desde la ficha de empresa
- [x] BD Ferozo: columnas PaginaWeb, Facebook, Instagram, LinkedIn, Twitter, YouTube, TikTok agregadas a tabla Empresas
- [x] PortalLicencias sincronizado dentro de DigitalPlusSuiteMultiTenant (todo respaldado en GitHub)
- [x] Documentacion DOC01-DOC04 actualizada a v5.0/3.0/3.0

### Sesion 2026-03-08
- [x] Arquitectura multi-tenant implementada (TenantContext, EmpresaId, tablas singular)
- [x] BD DigitalPlusMultiTenant creada en Ferozo con schema EF Core (29 tablas)
- [x] Migracion datos Kosiuko a DigitalPlusMultiTenant (758 legajos, 784K fichadas)
- [x] Kosiuko registrada en DigitalPlusAdmin (Id=5, CodigoActivacion=EE509930E07E)
- [x] InstaladorLiviano adaptado para multi-tenant (EmpresaId en configs, paths corregidos)
- [x] Validacion de licencia deshabilitada en Fichador y Administrador (multi-tenant usa instalador)
- [x] Fichador: cambio voluntario de PIN (FrmCambiarPinVoluntario, link "Cambiar mi PIN")
- [x] Fichador: PinMustChange va directo a pedir nuevo PIN sin requerir el actual
- [x] Administrador: tab PINs muestra TODOS los legajos con filtro combo
- [x] Administrador: boton "Resetear PIN" para borrar PINs olvidados
- [x] SPs legacy (EscritorioLegajoPIN_ForzarCambio) reemplazados por SQL directo multi-tenant
- [x] Stored procedures de PIN creados en ambas BDs (local y Ferozo)
- [x] Logo empresa + IntegraIA en Fichador y Administrador (EmpresaInfoService)

---

## CRITICO (Bloquea flujo completo)

- [ ] **BuildClientConnectionString usa `sa`** - El portal de licencias arma el connection string para las empresas usando `sa`. DEBE usar un usuario SQL dedicado. Sin esto, el flujo de instalacion no es seguro para produccion.
- [ ] **Crear usuario SQL dedicado por empresa** - Revisar logica del InstaladorUnificado e integrar en `DatabaseProvisioningService` del portal.

## INMEDIATO (Validacion end-to-end)

- [ ] **Probar flujo completo cloud:** Crear empresa NUEVA en portal -> generar codigo -> instalar con InstaladorLiviano -> verificar que Fichador y Administrador conectan a Ferozo.
  - Nota: Kosiuko ya fue migrada manualmente. Falta probar el flujo automatico desde el portal.
- [ ] **Deploy del Portal de Licencias** a produccion (`licencias.digitaloneplus.com`).
  - Actualmente el InstaladorLiviano apunta a `https://localhost:7043/api/activar` (solo testing local).
- [ ] **Recompilar InstaladorLiviano** con URL de produccion una vez que el portal este deployado.
- [ ] **Verificar InstaladorLiviano con Ferozo** - Probar que al instalar con codigo EE509930E07E, las apps conecten correctamente a DigitalPlusMultiTenant en Ferozo.

## SEGURIDAD SQL (Fases 2-5)

- [ ] **Deploy appsettings.json de DigitalPlusWeb** con `dp_web_svc` (pausado hasta revisar sistema web completo)
- [ ] **Verificacion funcional post-deploy** - Probar que DigitalPlusWeb funcione con el nuevo usuario SQL
- [ ] **Monitoreo de conexiones SQL** - Verificar que no hay conexiones residuales con `sa`
- [ ] **Deshabilitar `sa`** - Solo despues de estabilizacion de 7 dias

## DOCUMENTACION

- [x] Crear DOC01 - Reporte Arquitectura Project Leader (v5.0)
- [x] Crear DOC02 - Manual Usuario Final (v3.0)
- [x] Crear DOC03 - Manual Portal Licencias Integra IA (v3.0)
- [x] Crear DOC04 - Lista de Pendientes (actualizado)
- [x] Mover documentacion obsoleta a EnDesuso/
- [ ] **Agregar capturas de pantalla** al DOC02 y DOC03 (requiere ejecutar las apps y tomar screenshots)

## FUNCIONALIDAD (Proximas sesiones)

- [x] ~~1. Logo de empresa en reportes y formularios~~ COMPLETADO
- [ ] **2. Verificar durabilidad de licencias** - Analizar si la licencia es por terminal (MachineId) o permite instalacion ilimitada. Definir politica: una licencia por equipo vs licencia por empresa. Revisar LicenseManager, MachineIdProvider y flujo de activacion.
- [ ] **3. Adaptar InstaladorUnificado para trial web** - Que el usuario pueda registrarse desde la web, descargar el instalador y probar en modo LOCAL con SQL Express. El trial se corta en X dias/legajos. Analizar logica actual del instalador unificado (setup.iss) y adaptarla.
- [x] ~~4. Menu dinamico con web de la empresa~~ COMPLETADO - Links dinamicos a redes sociales en Administrador y portal
- [ ] **5. Link al portal web en Administrador** - Agregar en la app Administrador un acceso directo que abra el portal web multi-tenant en el navegador. Pendiente: cuando se haga el deploy del portal multi-tenant, agregar la URL como link en el menu.
- [ ] **8. Link a integraia.itech en Administrador** - Agregar en el menu del Administrador un link fijo a la pagina de Integra IA (integraia.itech), visible para todos los clientes.
- [ ] **6. PortalWeb paginacion en Asistencia Diaria** - En el Portal Web deberiamos implementar paginacion
- [ ] **7. Prueba de clonacion** - Clonar DigitalPlusSuiteMultiTenant desde GitHub en otra ubicacion y verificar que todo compila y es recuperable.

## PRIORIDAD MEDIA

- [ ] **Revisar PortalMultiTenant** en browser - Probar dashboard, CRUDs, reportes contra DigitalPlusMultiTenant
- [ ] **Completar documentacion** con capturas reales de las apps
- [ ] **Tests automatizados** - No hay tests en ninguno de los componentes
- [ ] **Sistema de logging** - Las apps desktop no tienen logging estructurado (solo MessageBox)

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

---

## NOTAS

- El deploy de appsettings.json de produccion (dp_web_svc) esta **PAUSADO** por decision del usuario.
- No deshabilitar `sa` todavia.
- El InstaladorLiviano apunta a `localhost:7043` para testing. Cambiar a URL de produccion antes de distribuir.
- InnoSetup esta en `C:\Users\Gustavo\AppData\Local\Programs\Inno Setup 6\`.
- MSBuild: `"C:/Program Files/Microsoft Visual Studio/18/Community/MSBuild/Current/Bin/MSBuild.exe"`
- Repos legacy (DigitalPlusDesk_Claude, DigitalPlusWeb_Claude) estan ARCHIVADOS. Todo se trabaja en DigitalPlusSuiteMultiTenant.
- BD Ferozo: `sd-1985882-l.ferozo.com,11434` - DigitalPlusMultiTenant, DigitalPlusAdmin, DigitalPlus (legacy)
- Todo el codigo (incluido Portal Licencias) esta en el repo GitHub: `GOAssad/DigitalPlusSuiteMultiTenant`

---

*Actualizado: 2026-03-09*
