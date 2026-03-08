# DIGITALPLUS - Lista de Pendientes

**Fecha:** 2026-03-07

---

## CRITICO (Bloquea flujo completo)

- [ ] **BuildClientConnectionString usa `sa`** - El portal de licencias arma el connection string para las empresas usando `sa`. DEBE usar el usuario SQL dedicado de cada empresa. Sin esto, el flujo de instalacion liviana no es seguro.
- [ ] **Verificar creacion de usuario SQL por empresa** - Revisar como el InstaladorUnificado crea usuario SQL al provisionar en nube, e integrar esa logica en `DatabaseProvisioningService` del portal.

## INMEDIATO (Validacion end-to-end)

- [ ] **Probar flujo completo cloud:** Crear empresa en portal -> generar codigo -> instalar con InstaladorLiviano -> verificar que Fichador y Administrador conectan a la BD en Ferozo.
- [ ] **Deploy del Portal de Licencias** a produccion (`licencias.digitaloneplus.com`). Sin esto, el InstaladorLiviano apunta a una URL que no existe.

## SEGURIDAD SQL (Fases 2-5)

- [ ] **Deploy appsettings.json de DigitalPlusWeb** con `dp_web_svc` (pausado hasta revisar sistema web completo)
- [ ] **Verificacion funcional post-deploy** - Probar que DigitalPlusWeb funcione con el nuevo usuario SQL
- [ ] **Monitoreo de conexiones SQL** - Verificar que no hay conexiones residuales con `sa`
- [ ] **Deshabilitar `sa`** - Solo despues de estabilizacion de 7 dias

## DOCUMENTACION (esta sesion)

- [x] Crear DOC01 - Reporte Arquitectura Project Leader
- [x] Crear DOC02 - Manual Usuario Final (con placeholders para capturas)
- [x] Crear DOC03 - Manual Portal Licencias Integra IA
- [x] Crear DOC04 - Lista de Pendientes
- [x] Mover documentacion obsoleta a EnDesuso/
- [ ] **Agregar capturas de pantalla** al DOC02 y DOC03 (requiere ejecutar las apps y tomar screenshots)

## PRIORIDAD MEDIA

- [ ] **Revisar sistema web completo** (DigitalPlusWeb) antes de hacer deploy de seguridad SQL
- [ ] **Completar documentacion de los 3 manuales** con capturas reales
- [ ] **Tests automatizados** - No hay tests en ninguno de los componentes
- [ ] **Sistema de logging** - Las apps desktop no tienen logging estructurado (solo MessageBox)

## PRIORIDAD BAJA

- [ ] Auto-fetch datos fiscales desde APIs gubernamentales (AFIP, etc.)
- [ ] DigitalPlusWeb multi-tenant migration
- [ ] WhatsApp integration para notificaciones
- [ ] Contactos de empresa en el portal
- [ ] Valores de suscripcion / facturacion en el portal
- [ ] Ampliar DigitalPlusWeb con reportes avanzados
- [ ] Limpieza dead code en Administrador
- [ ] Evaluar migracion de .NET Framework 4.8 a .NET 8+ (apps desktop)
- [ ] Unificar los 3 repositorios Git en un monorepo (o documentar la relacion)
- [ ] Implementar backup automatizado para BDs de empresas en Ferozo
- [ ] Dashboard de monitoreo: heartbeats, licencias por vencer, empresas sin actividad

---

## NOTAS

- El deploy de appsettings.json de produccion (dp_web_svc) esta **PAUSADO** por decision del usuario hasta revisar todo el sistema web.
- No deshabilitar `sa` todavia.
- No tocar instalador ni portal admin hasta completar los pendientes criticos.
- El InstaladorLiviano compila OK (25MB) y esta en `Output\DigitalPlus_Cloud_Setup_v1.0.exe`.
- InnoSetup esta en `C:\Users\Gustavo\AppData\Local\Programs\Inno Setup 6\`.

---

*Actualizado: 2026-03-07*
