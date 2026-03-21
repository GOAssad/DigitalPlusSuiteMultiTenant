# DIGITAL ONE - Terminal Móvil (Etapa 2)
## Documento de Arquitectura y Especificación para Implementación

**Version:** 7.0
**Fecha:** 2026-03-20
**Generado por:** Claude Sonnet 4.6 / Claude Opus 4.6
**Continuación de:** DOC01-Reporte_Arquitectura_ProjectLeader.md
**Estado:** COMPLETADO. Backend, PWA y administración funcionales. Probado end-to-end. Email de activación automático implementado.

---

## 1. OBJETIVO

Incorporar al ecosistema Digital One la capacidad de registrar fichadas desde un **smartphone**, como alternativa al lector DigitalPersona físico, sin reemplazarlo.

El celular actúa como una terminal de fichado personal: el empleado se autentica con su huella dactilar (sensor nativo del dispositivo), el sistema valida que está físicamente presente en la sucursal correcta (WiFi o GPS), y registra la fichada en la misma tabla `Fichada` de la BD multi-tenant.

---

## 2. PRINCIPIOS DE DISEÑO

1. **No invasivo:** No modifica el flujo existente de fichado por huella DigitalPersona ni por PIN.
2. **Mismo modelo de datos:** Las fichadas móviles se insertan en la tabla `Fichada` existente, con `Origen = Movil` (enum `OrigenFichada`). No se agrega campo `TipoFichada` nuevo — se reutiliza el campo `Origen` existente.
3. **Multi-tenant nativo:** El empleado se identifica con sus credenciales; el sistema resuelve `EmpresaId` automáticamente.
4. **Sucursal automática:** La sucursal se resuelve por la ubicación GPS del dispositivo, sin que el empleado la seleccione. Solo se buscan sucursales asignadas al legajo (tabla LegajoSucursal).
5. **Sin compatibilidad de templates biométricos:** No se comparan huellas entre dispositivos. El sensor del celular valida al empleado localmente; el servidor valida que el dispositivo está autorizado para ese empleado.
6. **Anti-fraude por presencia física:** GPS como método de validación. WiFi BSSID no es accesible desde navegadores web (PWA) por restricciones de privacidad.

---

## 3. ARQUITECTURA GENERAL

```
SMARTPHONE (empleado)
+-------------------------------+
|  App Digital One Mobile       |
|  React Native (Android/iOS)   |
|                               |
|  1. Login con credenciales    |
|  2. Registro de dispositivo   |
|     (QR o código de activac.) |
|  3. Fichada:                  |
|     a. BiometricPrompt (OS)   |
|     b. Captura WiFi BSSID     |
|        o coordenadas GPS      |
|     c. Firma JWT con clave    |
|        privada del device     |
|     d. POST /api/mobile/      |
|        fichada                |
+---------------+---------------+
                |
                | HTTPS
                v
PORTAL MULTI-TENANT (Azure)
+-------------------------------+
|  MobileController             |
|  (nuevo, en PortalMultiTenant)|
|                               |
|  POST /api/mobile/login       |
|  POST /api/mobile/registrar   |
|  POST /api/mobile/fichada     |
|  GET  /api/mobile/estado      |
+---------------+---------------+
                |
                v
        BD DigitalPlusMultiTenant
        (tablas nuevas + Fichada existente)
```

---

## 4. MODELO DE DATOS

### 4.1 Tablas nuevas en `DigitalPlusMultiTenant`

#### `TerminalMovil`
Representa un smartphone registrado y autorizado para un empleado.

```sql
CREATE TABLE TerminalMovil (
    Id               INT IDENTITY PRIMARY KEY,
    EmpresaId        INT NOT NULL,
    LegajoId         INT NOT NULL,           -- FK a Legajo.Id
    DeviceId         NVARCHAR(200) NOT NULL,  -- UUID único del dispositivo
    PublicKey        NVARCHAR(MAX) NOT NULL,  -- Clave pública RSA del device (PEM)
    Nombre           NVARCHAR(100) NULL,      -- "Samsung Galaxy S24 de Juan"
    Plataforma       NVARCHAR(20) NULL,       -- 'android' | 'ios'
    FechaRegistro    DATETIME NOT NULL DEFAULT GETDATE(),
    UltimoUso        DATETIME NULL,
    Activo           BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_TerminalMovil_Legajo FOREIGN KEY (LegajoId) REFERENCES Legajo(Id)
);
CREATE INDEX IX_TerminalMovil_DeviceId ON TerminalMovil(DeviceId);
CREATE INDEX IX_TerminalMovil_EmpresaId ON TerminalMovil(EmpresaId, LegajoId);
```

#### `SucursalGeoconfig`
Configuración de validación de presencia física por sucursal.

```sql
CREATE TABLE SucursalGeoconfig (
    Id                 INT IDENTITY PRIMARY KEY,
    SucursalId         INT NOT NULL,           -- FK a Sucursal.Id
    EmpresaId          INT NOT NULL,
    -- WiFi
    WifiBSSID          NVARCHAR(50) NULL,       -- MAC del router: "AA:BB:CC:DD:EE:FF"
    WifiSSID           NVARCHAR(100) NULL,      -- Nombre de la red (referencia, no se valida)
    -- GPS
    Latitud            DECIMAL(10,7) NULL,
    Longitud           DECIMAL(10,7) NULL,
    RadioMetros        INT NULL DEFAULT 100,
    -- Método
    MetodoValidacion   NVARCHAR(20) NOT NULL DEFAULT 'WifiOGPS',
    -- 'SoloWifi' | 'SoloGPS' | 'WifiOGPS' | 'WifiYGPS' | 'Ninguno'
    Activo             BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_SucursalGeoconfig_Sucursal FOREIGN KEY (SucursalId) REFERENCES Sucursal(Id)
);
```

#### `CodigoActivacionMovil`
Códigos de uso único para vincular un dispositivo a un empleado.

```sql
CREATE TABLE CodigoActivacionMovil (
    Id             INT IDENTITY PRIMARY KEY,
    EmpresaId      INT NOT NULL,
    LegajoId       INT NOT NULL,
    Codigo         NVARCHAR(10) NOT NULL,      -- 6-8 chars alfanumérico mayúscula
    FechaCreacion  DATETIME NOT NULL DEFAULT GETDATE(),
    FechaExpira    DATETIME NOT NULL,          -- DEFAULT +24hs
    Usado          BIT NOT NULL DEFAULT 0,
    UsadoEn        DATETIME NULL,
    DeviceId       NVARCHAR(200) NULL          -- se completa al usar
);
CREATE UNIQUE INDEX IX_CodigoActivacionMovil_Codigo ON CodigoActivacionMovil(Codigo) WHERE Usado = 0;
```

### 4.2 Modificaciones a tablas existentes

#### Tabla `Fichada` — nuevo valor en `TipoFichada`
No requiere cambio de schema si `TipoFichada` es NVARCHAR. Solo agregar el valor `'Movil'` al enum lógico de la aplicación.

Si el campo no existe aún, agregar:
```sql
ALTER TABLE Fichada ADD TipoFichada NVARCHAR(20) NULL DEFAULT 'Huella';
-- Valores posibles: 'Huella' | 'PIN' | 'Demo' | 'Movil'
```

#### Tabla `Sucursal` — sin cambios
La geoconfiguración se maneja en la tabla separada `SucursalGeoconfig` para no modificar la entidad existente.

---

## 5. API — ENDPOINTS NUEVOS

Todos los endpoints se crean en **`PortalMultiTenant/Controllers/MobileController.cs`**.

### 5.1 `POST /api/mobile/login`
Autentica al empleado y retorna un JWT de sesión móvil.

**Request:**
```json
{
  "legajo": "00123",
  "password": "su_contraseña"
}
```

**Response 200:**
```json
{
  "token": "eyJ...",           // JWT, expira en 8hs
  "legajoId": 45,
  "nombreEmpleado": "García, Juan",
  "empresaId": 2,
  "nombreEmpresa": "Kosiuko SA",
  "dispositivoRegistrado": true  // false si este device aún no está vinculado
}
```

**Notas de implementación:**
- Validar contra tabla `Legajo` (no contra Identity de portal web — los empleados no son usuarios web).
- El `EmpresaId` se resuelve del `Legajo` autenticado.
- Incluir `DeviceId` en el header `X-Device-Id` para detectar si ya está registrado.
- El login acepta un parámetro opcional `codigoActivacion` para resolver la empresa correcta cuando el número de legajo existe en múltiples empresas. La PWA captura el parámetro `?code=XXXX` desde el deep link del email de activación y lo envía junto con la solicitud de login.

---

### 5.2 `POST /api/mobile/registrar-dispositivo`
Registra o reemplaza el dispositivo de un empleado usando un código de activación.

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "codigo": "XK9M2P",
  "deviceId": "550e8400-e29b-41d4-a716-446655440000",
  "publicKey": "-----BEGIN PUBLIC KEY-----\nMIIBIjAN...\n-----END PUBLIC KEY-----",
  "nombreDispositivo": "Galaxy S24 de Juan",
  "plataforma": "android"
}
```

**Response 200:**
```json
{
  "ok": true,
  "mensaje": "Dispositivo registrado correctamente"
}
```

**Notas de implementación:**
- Verificar que el código no esté expirado ni usado.
- Verificar que el `LegajoId` del código coincida con el del token.
- Insertar en `TerminalMovil`. Si ya existe un registro activo para ese `LegajoId`, desactivarlo (un empleado = un dispositivo activo).
- Marcar el código como usado.
- Todo en una transacción.

---

### 5.3 `POST /api/mobile/fichada`
Registra una fichada desde el dispositivo móvil.

**Headers:**
```
Authorization: Bearer {token}
X-Device-Id: 550e8400-e29b-41d4-a716-446655440000
X-Signature: {firma_base64}   // firma RSA del body con la clave privada del device
```

**Request:**
```json
{
  "timestamp": "2026-03-12T09:15:00Z",
  "wifiBSSID": "AA:BB:CC:DD:EE:FF",
  "wifiSSID": "DigitalOne-Oficina",
  "latitud": -34.5270,
  "longitud": -58.4995,
  "tipoFichada": "Entrada"      // "Entrada" | "Salida" | "Auto"
}
```

**Response 200:**
```json
{
  "ok": true,
  "fichadaId": 9821,
  "tipo": "Entrada",
  "sucursalId": 3,
  "sucursalNombre": "Sede Central",
  "fechaHora": "2026-03-12T09:15:00Z",
  "mensaje": "Entrada registrada correctamente"
}
```

**Response 403 — fuera de ubicación:**
```json
{
  "ok": false,
  "codigo": "UBICACION_INVALIDA",
  "mensaje": "No se detectó ninguna sucursal habilitada para fichado móvil en tu ubicación actual."
}
```

**Lógica del servidor (orden de ejecución):**

```
1. Validar JWT y extraer EmpresaId + LegajoId
2. Buscar TerminalMovil activa para DeviceId + LegajoId + EmpresaId
   → Si no existe: 403 DISPOSITIVO_NO_REGISTRADO
3. Verificar firma RSA del body con PublicKey almacenada
   → Si inválida: 403 FIRMA_INVALIDA
4. Verificar timestamp (no aceptar fichadas con > 5 minutos de diferencia con servidor)
   → Si desfasado: 400 TIMESTAMP_INVALIDO
5. Resolver sucursal:
   a. Obtener todas las SucursalGeoconfig activas de la empresa
   b. Para cada una, según MetodoValidacion:
      - 'SoloWifi':  wifiBSSID del request == WifiBSSID de la config
      - 'SoloGPS':   distancia(lat/lon request, lat/lon config) <= RadioMetros
      - 'WifiOGPS':  cualquiera de los dos coincide  ← DEFAULT
      - 'WifiYGPS':  ambos deben coincidir
      - 'Ninguno':   siempre válido (para empleados remotos habilitados)
   c. Si ninguna sucursal coincide: 403 UBICACION_INVALIDA
   d. Si más de una coincide: usar la de mayor prioridad (menor Id)
6. Determinar tipo (Entrada/Salida) si tipoFichada == "Auto":
   → buscar última fichada del legajo en el día, alternar
7. INSERT INTO Fichada (EmpresaId, LegajoId, SucursalId, FechaHora, TipoFichada, ...)
   VALUES (@empresaId, @legajoId, @sucursalId, @timestamp, 'Movil', ...)
8. UPDATE TerminalMovil SET UltimoUso = GETDATE() WHERE Id = @terminalMovilId
9. Retornar respuesta 200
```

**Notas adicionales de implementación:**
- `FechaHora` se asigna usando `Clock.Now` (hora local Argentina) en lugar de `request.Timestamp` (UTC) para mantener consistencia con las apps desktop que también escriben en hora local.
- El SP `EscritorioFichadasSPSALIDA` tiene protección anti-duplicado: usa `UPDLOCK` para serializar inserciones concurrentes e ignora fichadas que disten menos de 30 segundos de la última fichada registrada para el mismo legajo.

---

### 5.4 `GET /api/mobile/estado`
Retorna el estado actual del empleado (para mostrar en la pantalla principal de la app).

**Headers:** `Authorization: Bearer {token}`, `X-Device-Id: ...`

**Response 200:**
```json
{
  "legajoId": 45,
  "nombre": "García, Juan",
  "empresaNombre": "Kosiuko SA",
  "dispositivoActivo": true,
  "ultimaFichada": {
    "tipo": "Entrada",
    "fechaHora": "2026-03-12T09:15:00Z",
    "sucursal": "Sede Central"
  },
  "fichadasHoy": [
    { "tipo": "Entrada", "fechaHora": "2026-03-12T09:15:00Z" }
  ]
}
```

---

## 6. APLICACIÓN MÓVIL

### 6.1 Stack

| Decisión | Elección | Justificación |
|---|---|---|
| Framework | React Native (Expo) | Madurez de librerías biométricas, GPS, WiFi; ecosistema mobile más robusto para MVP |
| Lenguaje | TypeScript | Consistencia y seguridad de tipos |
| Biometría | `expo-local-authentication` | Android + iOS, sin acceso al template |
| WiFi | `react-native-wifi-reborn` | Acceso al BSSID del AP conectado |
| GPS | `expo-location` | Permisos simplificados con Expo |
| Criptografía | `react-native-quick-crypto` | Generación de par RSA y firma de requests |
| Storage seguro | `expo-secure-store` | Almacenamiento de clave privada en Keystore/Keychain |
| HTTP | `axios` | Interceptores para JWT y firma automática |
| Navegación | `expo-router` | File-based routing, alineado con convenciones modernas |
| Estado global | `zustand` | Liviano, suficiente para este scope |

### 6.2 Estructura de carpetas

```
DigitalOneMobile/
├── app/
│   ├── (auth)/
│   │   └── login.tsx              ← Pantalla de login
│   ├── (main)/
│   │   ├── index.tsx              ← Pantalla principal / fichar
│   │   ├── historial.tsx          ← Últimas fichadas
│   │   └── configuracion.tsx      ← Info del dispositivo
│   └── activar.tsx                ← Registro de dispositivo (código QR / manual)
├── components/
│   ├── FichadaButton.tsx          ← Botón principal con animación semáforo
│   ├── EstadoUbicacion.tsx        ← Indicador WiFi/GPS
│   └── UltimaFichada.tsx
├── services/
│   ├── api.ts                     ← Cliente axios con interceptores
│   ├── auth.ts                    ← Login, token management
│   ├── biometria.ts               ← Wrapper expo-local-authentication
│   ├── ubicacion.ts               ← WiFi BSSID + GPS
│   └── crypto.ts                  ← Generación RSA, firma de requests
├── store/
│   └── useAppStore.ts             ← Estado global (zustand)
├── constants/
│   └── config.ts                  ← API_BASE_URL, timeouts
└── app.json
```

### 6.3 Flujo de pantallas

```
Inicio de la app
    │
    ├─ Sin token guardado ──→ [LOGIN]
    │                           │ credenciales OK
    │                           ↓
    │                        ¿Dispositivo registrado?
    │                           │ NO
    │                           ↓
    │                        [ACTIVAR DISPOSITIVO]
    │                           │ ingresar código de activación
    │                           │ o escanear QR
    │                           ↓
    │                        Generación de par RSA en device
    │                        POST /api/mobile/registrar-dispositivo
    │
    └─ Con token válido ───→ [PANTALLA PRINCIPAL]
                                │
                                │ empleado presiona "FICHAR"
                                ↓
                             Solicitar biometría al SO (huella/face)
                                │ OK
                                ↓
                             Capturar WiFi BSSID + GPS coords
                                │
                                ↓
                             Firmar payload con clave privada
                                │
                                ↓
                             POST /api/mobile/fichada
                                │
                          ┌─────┴──────┐
                         OK          ERROR
                          │             │
                    Semáforo verde   Semáforo rojo
                    + animación      + mensaje de error
                    + tipo fichada
```

### 6.4 Seguridad del dispositivo

**Generación de claves (al registrar):**
```typescript
// crypto.ts
import { generateKeyPair } from 'react-native-quick-crypto';
import * as SecureStore from 'expo-secure-store';

export async function generarYGuardarClaves(): Promise<string> {
  const { privateKey, publicKey } = await generateKeyPair('RSA', {
    modulusLength: 2048,
  });
  // Guardar clave privada en Keystore (Android) / Keychain (iOS)
  await SecureStore.setItemAsync('dp_private_key', privateKey.export('pem'));
  return publicKey.export('pem'); // Se envía al servidor
}
```

**Firma de cada fichada:**
```typescript
export async function firmarPayload(payload: object): Promise<string> {
  const privateKeyPem = await SecureStore.getItemAsync('dp_private_key');
  const data = JSON.stringify(payload);
  const sign = createSign('SHA256');
  sign.update(data);
  return sign.sign(privateKeyPem, 'base64');
}
```

---

## 7. FLUJO DE ADMINISTRACIÓN (Administrador Desktop)

El administrador desktop (Acceso.exe) necesita una nueva sección para gestionar los dispositivos móviles de los empleados.

### 7.1 Nueva pestaña "Móvil" en el formulario de Legajo

Agregar una pestaña en `FrmLegajo` con:

- **Estado del dispositivo:** Activo / Sin dispositivo registrado
- **Información:** nombre del dispositivo, plataforma, fecha de registro, último uso
- **Botón "Generar código de activación":**
  - Genera un código alfanumérico de 8 caracteres en mayúsculas
  - Lo inserta en `CodigoActivacionMovil` con expiración de 24hs
  - Lo muestra en pantalla con un QR (o para copiar/enviar al empleado)
- **Botón "Desactivar dispositivo":**
  - Pone `Activo = 0` en el registro de `TerminalMovil`

### 7.2 Nueva sección en Portal Multi-Tenant

Agregar en el menú del Portal Multi-Tenant:

- **`/terminales-moviles`** — listado de dispositivos activos por empresa
  - Columnas: Empleado, Dispositivo, Plataforma, Último uso, Estado
  - Acciones: Desactivar, Generar nuevo código de activación

### 7.3 Botón "Activar Dispositivo" en Portal MT

El Portal Multi-Tenant incluye un botón **"Activar Dispositivo"** que permite generar códigos de activación para cualquier empleado, no solo para terminales ya existentes.

- El botón abre un modal con un **buscador de empleados** (con debounce de 400ms para evitar queries excesivas mientras el usuario escribe).
- Al seleccionar un empleado, se genera el código de activación y se **habilita automáticamente `MobileHabilitado`** en el legajo si no lo estaba.
- Esto simplifica el flujo: el administrador no necesita ir primero a editar el legajo para habilitar el fichado móvil.

---

## 8. CONFIGURACIÓN POR SUCURSAL

### 8.1 Administrador Desktop — nueva pestaña en Sucursales

Agregar en `FrmSucursal` una pestaña **"Fichado Móvil"** con:

- **Activar fichado móvil para esta sucursal** (checkbox)
- **Método de validación** (combo): WiFi o GPS | Solo WiFi | Solo GPS | WiFi y GPS | Sin restricción
- **Configuración WiFi:**
  - BSSID del router (campo de texto, formato `XX:XX:XX:XX:XX:XX`)
  - SSID (referencia informativa)
  - Botón de ayuda: "¿Cómo obtener el BSSID?"
- **Configuración GPS:**
  - Latitud y Longitud (campos numéricos)
  - Radio en metros (slider o campo, default 100m)
  - Botón "Usar ubicación actual" (si el equipo tiene GPS o IP geolocation)

### 8.2 Portal Multi-Tenant — misma funcionalidad

Agregar los mismos campos en la página de edición de Sucursal en el portal web.

---

## 9. CAMBIOS EN CAPA DE DATOS (PortalMultiTenant)

### 9.1 Nuevas entidades EF Core

```csharp
// Models/TerminalMovil.cs
public class TerminalMovil
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int LegajoId { get; set; }
    public string DeviceId { get; set; }
    public string PublicKey { get; set; }
    public string? Nombre { get; set; }
    public string? Plataforma { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? UltimoUso { get; set; }
    public bool Activo { get; set; }
    
    public Legajo Legajo { get; set; }
}

// Models/SucursalGeoconfig.cs
public class SucursalGeoconfig
{
    public int Id { get; set; }
    public int SucursalId { get; set; }
    public int EmpresaId { get; set; }
    public string? WifiBSSID { get; set; }
    public string? WifiSSID { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public int RadioMetros { get; set; } = 100;
    public string MetodoValidacion { get; set; } = "WifiOGPS";
    public bool Activo { get; set; }
    
    public Sucursal Sucursal { get; set; }
}

// Models/CodigoActivacionMovil.cs
public class CodigoActivacionMovil
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int LegajoId { get; set; }
    public string Codigo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaExpira { get; set; }
    public bool Usado { get; set; }
    public DateTime? UsadoEn { get; set; }
    public string? DeviceId { get; set; }
}
```

### 9.2 Migración EF Core

Crear migración: `AddTerminalMovilAndGeoconfig`

Aplicar con: `dotnet ef database update`

---

## 10. MODIFICACIONES EN APPS DESKTOP (Acceso.Clases.Datos)

### 10.1 Nuevas clases de datos

Agregar en `Acceso.Clases.Datos`:

- `TerminalMovilDAL` — CRUD sobre `TerminalMovil` y `CodigoActivacionMovil`
  - `ObtenerPorLegajo(int legajoId, int empresaId)`
  - `GenerarCodigo(int legajoId, int empresaId)` → retorna el código generado
  - `Desactivar(int terminalMovilId)`

- `SucursalGeoconfigDAL`
  - `ObtenerPorSucursal(int sucursalId)`
  - `Guardar(SucursalGeoconfig config)`

Patrón: igual que los demás DAL del proyecto — ADO.NET directo, SQL parametrizado, sin ORM.

---

## 11. CONSIDERACIONES DE SEGURIDAD

| Riesgo | Mitigación |
|---|---|
| Empleado ficha desde casa (fuera de sucursal) | Validación WiFi BSSID + GPS en servidor, configurable por sucursal |
| Robo del celular | Código de activación nuevo invalidará el anterior; admin puede desactivar el device |
| Replay attack (reusar un POST capturado) | Timestamp en el payload firmado; servidor rechaza si > 5 min de diferencia |
| Falsificación de firma | Servidor verifica con `PublicKey` almacenada; clave privada nunca sale del dispositivo (SecureStore) |
| Fake GPS (app de spoofing) | Riesgo aceptado para PWA. Mitigacion futura: app nativa con deteccion de mock location. Auditable por coordenadas registradas |
| Token JWT robado | Corta expiración (8hs); `DeviceId` en header también verificado contra el token |
| Código de activación interceptado | Expira en 24hs; de uso único; debe coincidir con el `LegajoId` del token |

---

## 12. PLAN DE IMPLEMENTACIÓN

### Etapa 2a — Backend + Admin (sin app móvil)

| Tarea | Componente | Prioridad |
|---|---|---|
| Script SQL: crear `TerminalMovil`, `SucursalGeoconfig`, `CodigoActivacionMovil` | Database/ | Alta |
| Agregar campo `TipoFichada` a tabla `Fichada` si no existe | Database/ | Alta |
| Entidades EF Core + migración | PortalMultiTenant | Alta |
| `MobileController` con los 4 endpoints | PortalMultiTenant | Alta |
| Servicio de resolución de sucursal por ubicación (`UbicacionService`) | PortalMultiTenant | Alta |
| Pestaña "Móvil" en `FrmLegajo` (Administrador) | Administrador/Acceso | Media |
| Pestaña "Fichado Móvil" en `FrmSucursal` (Administrador) | Administrador/Acceso | Media |
| Página `/terminales-moviles` en Portal MT | PortalMultiTenant | Media |
| Campos geoconfig en página Sucursal del Portal MT | PortalMultiTenant | Media |
| Tests manuales del circuito completo vía Postman | — | Alta |

### Etapa 2b — App Móvil

| Tarea | Componente | Prioridad |
|---|---|---|
| Scaffold proyecto Expo + TypeScript | DigitalOneMobile/ | Alta |
| `crypto.ts`: generación RSA + firma | DigitalOneMobile/services | Alta |
| `ubicacion.ts`: WiFi BSSID + GPS | DigitalOneMobile/services | Alta |
| Pantalla Login | DigitalOneMobile/app | Alta |
| Pantalla Activar Dispositivo (código + QR) | DigitalOneMobile/app | Alta |
| Pantalla Principal / Fichar | DigitalOneMobile/app | Alta |
| Pantalla Historial | DigitalOneMobile/app | Media |
| Build Android (APK/AAB) para testing | — | Alta |
| Build iOS (requiere Mac o EAS Build) | — | Media |

---

## 13. ESTRUCTURA DE CARPETAS EN EL REPOSITORIO

```
DigitalPlusSuiteMultiTenant/
│
├── ... (estructura existente sin cambios)
│
├── DigitalOneMobile/                    ← NUEVO: App React Native
│   ├── app/
│   ├── components/
│   ├── services/
│   ├── store/
│   ├── constants/
│   ├── package.json
│   └── app.json
│
├── Database/
│   └── Migrations/
│       └── 003_TerminalMovil_Geoconfig.sql   ← NUEVO: script SQL
│
└── Docs/
    └── DOC02-Terminal_Movil_DigitalOne.md    ← Este documento
```

---

## 14. NOTAS PARA CLAUDE CODE

Al implementar este documento, tener en cuenta:

1. **Convenciones del proyecto:** Seguir exactamente el mismo estilo de código que en `PortalMultiTenant` — nombres de clases, inyección de dependencias, estructura de controllers, manejo de `EmpresaId` via `TenantContext` o claim del JWT.

2. **Tabla `Fichada`:** El campo `TipoFichada` puede ya existir o no — verificar antes de aplicar el ALTER. Si existe, solo agregar el valor `'Movil'` a la lógica de la app.

3. **Sin cambios en Fichador desktop:** El `TEntradaSalida.exe` no se toca en esta etapa.

4. **Autenticación de empleados en mobile:** Los empleados NO tienen cuenta en ASP.NET Identity del portal. El login mobile usa credenciales propias de la tabla `Legajo` (campo `PIN` o una nueva columna `PasswordHash`). Evaluar con el equipo si se agrega `PasswordHash` a `Legajo` o si se usa el PIN existente como contraseña mobile.

5. **UbicacionService:** Implementar como servicio inyectable en el controller. La lógica de resolución de sucursal por WiFi/GPS debe ser fácilmente testeable de forma aislada.

6. **Logs:** Registrar en una tabla `FichadaMovilLog` (o en el sistema de logging existente) los intentos fallidos de fichada (ubicación inválida, firma inválida) para auditoría.

---

---

## 15. ESTADO DE IMPLEMENTACION (actualizado 2026-03-13)

### Etapa 2a — Backend + Admin: COMPLETADA

| Tarea | Estado | Notas |
|---|---|---|
| Entidades EF Core (TerminalMovil, SucursalGeoconfig, CodigoActivacionMovil) | HECHO | En Domain/Entities/, con ITenantEntity |
| Migracion EF Core `AddTerminalMovilAndGeoconfig` | HECHO | Aplicada en Ferozo y localhost |
| OrigenFichada enum + valor Movil | HECHO | No se usa TipoFichada nuevo |
| MobileController (4 endpoints JWT) | HECHO | Controllers/MobileController.cs |
| JWT Bearer auth en Program.cs | HECHO | Convive con cookie auth |
| UbicacionService | HECHO | Infrastructure/Services/UbicacionService.cs |
| Tab "Movil" en FrmRRHHLegajos | HECHO | Generar codigo + desactivar dispositivo |
| TerminalMovilDAL + SucursalGeoconfigDAL | HECHO | En Acceso.Clases.Datos/Generales/ |
| Pagina /terminales-moviles | HECHO | Blazor Server, filtros, ordenamiento |
| Pagina /fichado-movil (geoconfig) | HECHO | Blazor Server, modal de edicion |
| NavMenu actualizado | HECHO | 2 links en grupo Estructura |
| Script SQL referencia | HECHO | Database/003_TerminalMovil_Geoconfig.sql |

### Etapa 2b — App Movil (PWA): COMPLETADA

Se descartó React Native/Expo por incompatibilidades de SDK con Expo Go en iOS. Se implementó como PWA (HTML+CSS+JS estático) servida desde `wwwroot/mobile/` del portal Blazor.

| Tarea | Estado | Notas |
|---|---|---|
| PWA Login (legajo + PIN) | HECHO | wwwroot/mobile/app.js |
| PWA Activación de dispositivo | HECHO | Código de activación de 8 caracteres |
| PWA Fichada con GPS | HECHO | Geolocation API del browser |
| PWA Historial del día | HECHO | Desde GET /api/mobile/estado |
| Service Worker + manifest.json | HECHO | Instalable como PWA |
| Probado en iPhone (Safari) | HECHO | Fix crypto.randomUUID fallback |
| Probado en Android (Chrome) | HECHO | |
| MobileHabilitado (empresa) | HECHO | Checkbox en Portal Licencias |
| MobileHabilitado (legajo) | HECHO | Checkbox en LegajoForm Portal MT |
| Menu condicional | HECHO | Claim + ITenantService |
| Gestion de PIN desde Portal MT | HECHO | Asignar, cambiar, resetear |
| Deploy a Azure | HECHO | digitalplusportalmt.azurewebsites.net/mobile/ |

### Decisiones de implementacion vs especificacion

| Especificacion | Implementacion real | Motivo |
|---|---|---|
| Campo `TipoFichada` nuevo en tabla Fichada | Se reutiliza `Origen` (enum OrigenFichada) con valor `Movil` | El campo Origen ya cumplia el mismo proposito |
| Login con password del legajo | Login con PIN (SHA256 hash) | Los legajos ya tienen PIN; no agregar otro campo de credencial |
| Pestaña geoconfig en FrmSucursal desktop | Geoconfig solo en Portal MT web | No existe FrmSucursal dedicado en desktop (usa CtrEntidadPanel generico) |
| EF Core table names plural | TerminalesMoviles, SucursalGeoconfigs, CodigosActivacionMovil | Consistente con convencion EF Core del proyecto |
| App React Native (Expo) | PWA (HTML+CSS+JS estatico) | Expo Go en iOS incompatible con SDK recientes; PWA funciona en todos los browsers |
| Firma RSA en cada fichada | Firma RSA opcional (se valida si se envia) | PWA no tiene acceso facil a crypto RSA nativo |
| WiFi BSSID como metodo primario | GPS como metodo primario | Browsers no exponen WiFi BSSID; Geolocation API si esta disponible |

---

### Sesion 2026-03-13 (noche): Rediseno PWA + CRUD Sucursales + Validacion GPS

| Tarea | Estado | Notas |
|---|---|---|
| Rediseno PWA Mobile (tema oscuro, GPS visual) | HECHO | Dark theme navy, anillos GPS animados, reloj en vivo, boton teal |
| GPS watch continuo | HECHO | watchPosition con tracking de accuracy, estados acquiring/verified/error |
| CRUD Sucursales mejorado | HECHO | Nuevos campos: Direccion, Localidad, Provincia, Telefono, Email |
| Mapa Leaflet integrado en formulario | HECHO | OpenStreetMap + Nominatim (geocoding/reverse), marker arrastrable, circulo radio |
| Campos WiFi removidos de UI | HECHO | BSSID no accesible desde PWA; metodo forzado a SoloGPS |
| Validacion GPS por sucursal asignada | HECHO | Fichada consulta LegajoSucursal antes de resolver GPS |
| Mensajes de error claros | HECHO | "No tiene sucursales asignadas" / "Sin config GPS" / "No se detecto sucursal" |
| Migracion EF Core AddSucursalFields | HECHO | Aplicada en Ferozo |
| Deploy Portal MT | HECHO | digitalplusportalmt.azurewebsites.net |
| Probado end-to-end con GPS real | HECHO | Sucursal creada con coordenadas reales, fichada exitosa |

### Nota sobre WiFi BSSID

Los navegadores web (Chrome, Safari, Firefox) **no permiten leer el BSSID** (MAC del router) desde JavaScript por razones de privacidad. Solo apps nativas (Android/iOS) pueden acceder a esa informacion. Por lo tanto, para la PWA se usa exclusivamente GPS como metodo de validacion. Los campos WiFi se mantienen en el modelo de datos para futura app nativa pero no se muestran en la UI.

---

### Sesion 2026-03-18: Modo Kiosko + Fichada QR

| Tarea | Estado | Notas |
|---|---|---|
| Legajo.QrToken (GUID, unique index) | HECHO | Backfill NEWID() para legajos existentes, auto-genera al crear |
| TerminalMovil: ModoKiosko, SucursalId | HECHO | LegajoId nullable, PublicKey nullable para kioskos |
| OrigenFichada.QR | HECHO | Nuevo valor en enum |
| POST /api/mobile/fichar-qr | HECHO | Valida device kiosko + QrToken + empresa + sucursal + cooldown |
| GET /api/mobile/mi-qr | HECHO | Devuelve QrToken del empleado logueado |
| GET /api/mobile/kiosko-info | HECHO | Info del kiosko para UI |
| Portal: Registrar Kiosko | HECHO | Modal con nombre + sucursal + DeviceID auto-generado |
| Portal: QR por legajo | HECHO | Modal con QR visual + foto + datos |
| Portal: Imprimir QR masivo | HECHO | Genera tarjetas de credenciales para impresion |
| PWA: tab "Mi QR" | HECHO | Tercera tab con QR grande usando qrcode-generator |
| Kiosko web /kiosko/ | HECHO | Setup DeviceID, scanner html5-qrcode, overlay resultado 3seg |
| KioskoHabilitado (empresa) | HECHO | Flag en Empresa, switch en Portal Licencias, claim en login |
| Icono QR en fichadas | HECHO | bi-qr-code en LegajoForm, FichadasList, AsistenciaDiaria |
| Hora fichada: Clock.Now | HECHO | Argentina time en vez de UTC (pendiente: timezone por sucursal) |
| Cooldown 30seg | HECHO | Compara CreatedAt (UTC) para evitar mezcla timezones |
| Fix cross-tenant PIN SPs | HECHO | Verificar y Cambiar ahora filtran por @EmpresaId |
| Fix cambio de plan | HECHO | Aplica PlanConfig al cambiar plan |
| Kiosko manifest.json fullscreen | HECHO | Instalable como PWA en tablets |
| JS libs: qrcode.min.js, html5-qrcode.min.js | HECHO | Generador QR (20KB) + Scanner camara (375KB) |

### COMPLETADO: Fase 6 — Fichador Desktop QR (commit 3c234b3)

Lectura de QR por camara USB en Fichador WinForms (.NET Framework 4.8):
- **AForge.Video.DirectShow 2.2.5** para captura de camara USB/integrada
- **ZXing.Net 0.16.9** para decodificar QR desde frames (timer 250ms)
- Nuevo modo `ModoFichada.QR` junto a Huella/PIN/Demo
- DAL: `RRHHLegajosPin.BuscarPorQrToken()` con validacion GUID y comparacion sin guiones (REPLACE)
- Deteccion automatica: si hay camara, modo QR disponible; si no, se oculta
- Cooldown 5 segundos: mismo QR no repite fichada
- sOrigen = "QR" enviado al SP
- **Rediseno completo dark theme:** Fondo navy #0D111C, cards #161C30, botones de modo pill horizontales, form 620x660
- **Fix deadlock cierre:** Flag `volatile bool _cerrando` + `BeginInvoke` (no `Invoke`) en VideoDevice_NewFrame
- **Instalador:** DLLs AForge y ZXing incluidas en setup-liviano.iss
- **Fichada.Origen ahora es string?** (no enum) para compatibilidad directa con BD nvarchar
- **Fix icono PIN:** bi-dialpad no existe en Bootstrap Icons → bi-keyboard
- **Fix Asistencia Diaria:** Columnas invertidas + muestra todos los origenes del dia

### Pendiente: TimeZone por Sucursal

Actualmente FechaHora usa `Clock.Now` (hardcodeado Argentina UTC-3). Para soporte multi-pais:
- Agregar campo `TimeZone` (string) a tabla Sucursal
- Default: "America/Argentina/Buenos_Aires"
- Selector de timezone en form de sucursal del Portal MT
- Usar en todos los endpoints de fichada (QR, movil, etc)

---

*Fin del documento — Terminal Móvil Digital One v5.0*
