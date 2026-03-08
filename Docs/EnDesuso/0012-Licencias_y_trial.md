# v1.3 — Sistema de Licencias (MVP) con Azure como autoridad + Ticket firmado + Caché local DPAPI

## Objetivo
Implementar control de licencias para Fichador y Administrador:
- Trial (limitado por tiempo + legajos)
- Licencia activa
- Licencia vencida (fecha)
- Licencia suspendida (falta de pago)
- Modo offline con ventana limitada (grace/offline)

Debe ser difícil de adulterar: el estado de licencia NO debe depender de datos editables en SQL local.

## Decisión técnica
Modelo híbrido:
- Azure Functions es la autoridad
- Azure emite un "License Ticket" firmado digitalmente
- Cliente valida firma con clave pública embebida
- Ticket se cachea localmente protegido con DPAPI (Machine)
- Heartbeat periódico renueva/actualiza ticket
- Si pasan X días sin heartbeat → bloqueo

## Reglas MVP
- Trial:
  - Duración: 14 días desde instalación/primer uso
  - Límite: 50 legajos activos (o el criterio que propongas si encontrás algo mejor)
  - Si excede tiempo o límite → bloqueo y mensaje para activar
- Licencia paga:
  - Activa hasta ExpiresAt
- Falta de pago:
  - Estado SUSPENDED con gracia de 7 días (configurable)
- Sin internet:
  - Operar solo hasta NextCheckRequiredAt (ej 72hs)
  - Pasado ese deadline, bloquear hasta validar online

## Azure Functions (extensión)
Agregar endpoints:

1) POST /api/license/activate
Request:
{
  "activationCode": "...",
  "companyName": "...",
  "machineId": "...",
  "installType": "cloud|local"
}
Response:
{
  "ticket": "<json>",
  "signature": "<base64>"
}

2) POST /api/license/heartbeat
Request:
{
  "companyId": "...",
  "machineId": "...",
  "app": "Fichador|Administrador",
  "counters": {
     "legajosActivos": 0,
     "fichadasMes": 0
  }
}
Response: ticket+signature actualizado

Notas:
- Firmar ticket con clave privada (ideal: Key Vault key o secret).
- Validar activationCode (ya existe infraestructura en ActivationCodes).
- Guardar/actualizar tabla Licencias y LicenciasLog.

## Cliente (WinForms .NET Framework 4.8)
Crear librería común: DigitalPlus.Licensing

Funciones:
- MachineId: generar hash estable (sin PII cruda) y persistir.
- Load/Save ticket cacheado: DPAPI Machine.
- Validar firma del ticket (clave pública embebida).
- Evaluar reglas: trial/expira/suspendida/offline deadline.
- Mensajes claros al usuario (sin tecnicismos).

Integración:
- Fichador: validar al inicio + cada X horas
- Administrador: validar al inicio
- Si bloqueado: no permitir operar (pantalla modal).

## Datos
Tabla Licencias (cloud y local si aplica solo como auditoría):
- CompanyId, ActivationCode, Plan, ExpiresAt, Status, LastHeartbeatAt, NextCheckRequiredAt, LimitsJson, MachineId

LicenciasLog:
- Evento, Fecha, Detalle, MachineId

## Entregables
- Diseño de ticket (campos exactos)
- Implementación endpoints Azure + scripts SQL
- Implementación librería cliente + hooks en apps
- Pruebas: online, offline, trial, suspendida, vencida
- Documentación de operación (cómo activar/renovar/suspender)