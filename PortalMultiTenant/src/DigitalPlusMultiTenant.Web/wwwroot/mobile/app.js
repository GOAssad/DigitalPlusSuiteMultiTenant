"use strict";

const API_BASE = "/api/mobile";
const SESSION_PREFIX = "session_";

// --- Storage ---
const Storage = {
    get(key) { try { return localStorage.getItem(key); } catch { return null; } },
    set(key, val) { try { localStorage.setItem(key, val); } catch {} },
    remove(key) { try { localStorage.removeItem(key); } catch {} },
    getJSON(key) { const v = this.get(key); return v ? JSON.parse(v) : null; },
    setJSON(key, val) { this.set(key, JSON.stringify(val)); },
};

// --- State ---
let state = {
    deviceId: Storage.get("deviceId") || generateDeviceId(),
    token: null,
    user: null,
    deviceRegistered: false,
    empresaId: null,
};

let clockInterval = null;
let gpsWatchId = null;
let lastGpsCoords = null;
let pendingActivationCode = null;
let qrTokenLoaded = false;
let isOnline = navigator.onLine;
let pendingFichada = null; // Retry queue for offline fichadas
let sugeridoEntrada = true; // true = Entrada sugerida, false = Salida sugerida
let tipoInvertido = false;  // true = usuario invirtió manualmente

function generateDeviceId() {
    let id;
    try {
        id = crypto.randomUUID();
    } catch {
        id = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, c => {
            const r = Math.random() * 16 | 0;
            return (c === "x" ? r : (r & 0x3 | 0x8)).toString(16);
        });
    }
    Storage.set("deviceId", id);
    return id;
}

// ======================================================================
// Session management per company
// ======================================================================
function sessionKey(empresaId) {
    return SESSION_PREFIX + empresaId;
}

function saveSession(token, user, deviceRegistered) {
    const empresaId = user.empresaId;
    state.token = token;
    state.user = user;
    state.deviceRegistered = deviceRegistered;
    state.empresaId = empresaId;

    // Save per-company session
    Storage.setJSON(sessionKey(empresaId), {
        token,
        user,
        deviceRegistered,
        savedAt: Date.now(),
    });

    // Remember active company
    Storage.set("activeEmpresaId", String(empresaId));
}

function loadSession(empresaId) {
    const data = Storage.getJSON(sessionKey(empresaId));
    if (!data || !data.token || !data.user) return false;

    // Check if token looks expired (JWT exp claim)
    if (isTokenExpired(data.token)) {
        clearSession(empresaId);
        return false;
    }

    state.token = data.token;
    state.user = data.user;
    state.deviceRegistered = data.deviceRegistered;
    state.empresaId = empresaId;
    Storage.set("activeEmpresaId", String(empresaId));
    return true;
}

function clearSession(empresaId) {
    Storage.remove(sessionKey(empresaId));
    if (state.empresaId === empresaId) {
        state.token = null;
        state.user = null;
        state.deviceRegistered = false;
        state.empresaId = null;
    }
}

function clearAllSessions() {
    // Remove all session_* keys
    try {
        const keys = [];
        for (let i = 0; i < localStorage.length; i++) {
            const k = localStorage.key(i);
            if (k && k.startsWith(SESSION_PREFIX)) keys.push(k);
        }
        keys.forEach(k => localStorage.removeItem(k));
    } catch {}
    state.token = null;
    state.user = null;
    state.deviceRegistered = false;
    state.empresaId = null;
}

function getAllSavedSessions() {
    const sessions = [];
    try {
        for (let i = 0; i < localStorage.length; i++) {
            const k = localStorage.key(i);
            if (k && k.startsWith(SESSION_PREFIX)) {
                const data = Storage.getJSON(k);
                if (data && data.user && data.token && !isTokenExpired(data.token)) {
                    sessions.push(data);
                }
            }
        }
    } catch {}
    return sessions;
}

function isTokenExpired(token) {
    try {
        const payload = JSON.parse(atob(token.split(".")[1]));
        if (!payload.exp) return false;
        // Expired if less than 60 seconds remaining
        return payload.exp * 1000 < Date.now() - 60000;
    } catch {
        return true; // Malformed token = expired
    }
}

// ======================================================================
// API
// ======================================================================
async function api(endpoint, options = {}) {
    const headers = { "Content-Type": "application/json" };
    if (state.deviceId) headers["X-Device-Id"] = state.deviceId;
    if (state.token && options.auth !== false) {
        // Check token before sending
        if (isTokenExpired(state.token)) {
            handleExpiredToken();
            throw new Error("Sesión expirada. Ingrese nuevamente.");
        }
        headers["Authorization"] = "Bearer " + state.token;
    }

    const res = await fetch(API_BASE + endpoint, {
        method: options.method || "GET",
        headers,
        body: options.body ? JSON.stringify(options.body) : undefined,
    });

    const data = await res.json();

    if (res.status === 401 && options.auth !== false) {
        handleExpiredToken();
        throw new Error(data.mensaje || "Sesión expirada");
    }
    if (!res.ok) {
        const err = new Error(data.mensaje || "Error del servidor");
        err.codigo = data.codigo;
        throw err;
    }
    return data;
}

function handleExpiredToken() {
    if (state.empresaId) {
        clearSession(state.empresaId);
    }
    qrTokenLoaded = false;
    stopClock();
    stopGpsWatch();
    checkEmpresas();
}

// ======================================================================
// Screens
// ======================================================================
function showScreen(name) {
    document.querySelectorAll(".screen").forEach(s => s.classList.remove("active"));
    const el = document.getElementById("screen-" + name);
    if (el) el.classList.add("active");
}

function showLoading(show) {
    document.getElementById("loading").classList.toggle("hidden", !show);
}

function showError(id, msg) {
    const el = document.getElementById(id);
    el.textContent = msg;
    el.classList.remove("hidden");
}

function hideError(id) {
    document.getElementById(id).classList.add("hidden");
}

function showToast(msg, type) {
    const toast = document.getElementById("toast");
    if (!toast) return;
    toast.textContent = msg;
    toast.className = "toast " + (type || "info");
    toast.classList.remove("hidden");
    setTimeout(() => toast.classList.add("hidden"), 4000);
}

// ======================================================================
// Online / Offline
// ======================================================================
function updateOnlineStatus() {
    isOnline = navigator.onLine;
    const indicator = document.getElementById("offline-indicator");
    if (indicator) {
        indicator.classList.toggle("hidden", isOnline);
    }
    // Retry pending fichada when back online
    if (isOnline && pendingFichada) {
        retryPendingFichada();
    }
}

// ======================================================================
// Live Clock
// ======================================================================
function startClock() {
    updateClock();
    clockInterval = setInterval(updateClock, 1000);
}

function stopClock() {
    if (clockInterval) {
        clearInterval(clockInterval);
        clockInterval = null;
    }
}

function updateClock() {
    const el = document.getElementById("home-clock");
    if (!el) return;
    const now = new Date();
    el.textContent = now.toLocaleTimeString("es-AR", { hour: "2-digit", minute: "2-digit" });
}

// ======================================================================
// GPS Watch
// ======================================================================
function startGpsWatch() {
    if (!navigator.geolocation) {
        updateGpsStatus("gps-error", "GPS no disponible", "Este dispositivo no soporta geolocalización");
        document.querySelector(".gps-map")?.classList.add("no-gps");
        return;
    }

    updateGpsStatus("acquiring", "Obteniendo ubicación...", "");

    gpsWatchId = navigator.geolocation.watchPosition(
        pos => {
            lastGpsCoords = {
                latitud: pos.coords.latitude,
                longitud: pos.coords.longitude,
                accuracy: pos.coords.accuracy,
            };

            const accEl = document.getElementById("gps-accuracy");
            const distEl = document.getElementById("gps-distance");

            accEl.classList.remove("hidden");
            accEl.querySelector(".badge-text").textContent = "Precisión: " + Math.round(pos.coords.accuracy) + "m";

            distEl.classList.remove("hidden");
            if (pos.coords.accuracy <= 50) {
                distEl.querySelector(".badge-text").textContent = "Ubicación obtenida";
                updateGpsStatus("verified", "Ubicación verificada", "GPS validado · Precisión " + Math.round(pos.coords.accuracy) + "m");
                document.querySelector(".gps-map")?.classList.remove("no-gps");
            } else {
                distEl.querySelector(".badge-text").textContent = "Mejorando señal...";
                updateGpsStatus("acquiring", "Mejorando precisión...", "Precisión actual: " + Math.round(pos.coords.accuracy) + "m");
            }
        },
        err => {
            if (err.code === 1) {
                updateGpsStatus("gps-error", "Permiso GPS denegado", "Habilite la ubicación en configuración");
            } else {
                updateGpsStatus("gps-error", "Error obteniendo GPS", err.message);
            }
            document.querySelector(".gps-map")?.classList.add("no-gps");
        },
        { enableHighAccuracy: true, timeout: 15000, maximumAge: 5000 }
    );
}

function stopGpsWatch() {
    if (gpsWatchId !== null) {
        navigator.geolocation.clearWatch(gpsWatchId);
        gpsWatchId = null;
    }
    lastGpsCoords = null;
}

function updateGpsStatus(status, title, detail) {
    const el = document.getElementById("gps-status");
    if (!el) return;
    el.className = "gps-validation " + status;
    document.getElementById("gps-status-title").textContent = title;
    document.getElementById("gps-status-detail").textContent = detail;
}

// ======================================================================
// Location (for fichada)
// ======================================================================
function getLocation() {
    if (lastGpsCoords) {
        return Promise.resolve({ latitud: lastGpsCoords.latitud, longitud: lastGpsCoords.longitud });
    }
    return new Promise((resolve) => {
        if (!navigator.geolocation) {
            resolve(null);
            return;
        }
        navigator.geolocation.getCurrentPosition(
            pos => resolve({ latitud: pos.coords.latitude, longitud: pos.coords.longitude }),
            () => resolve(null),
            { enableHighAccuracy: true, timeout: 10000 }
        );
    });
}

// ======================================================================
// Time formatting
// ======================================================================
function formatTime(isoString) {
    try {
        const d = new Date(isoString);
        return d.toLocaleTimeString("es-AR", { hour: "2-digit", minute: "2-digit" });
    } catch { return ""; }
}

function formatTimeFull(isoString) {
    try {
        const d = new Date(isoString);
        return d.toLocaleTimeString("es-AR", { hour: "2-digit", minute: "2-digit", second: "2-digit" });
    } catch { return ""; }
}

// ======================================================================
// Login
// ======================================================================
async function handleLogin(e) {
    e.preventDefault();
    hideError("login-error");

    const legajo = document.getElementById("input-legajo").value.trim();
    const pin = document.getElementById("input-pin").value.trim();

    if (!legajo || !pin) {
        showError("login-error", "Ingrese legajo y PIN");
        return;
    }

    const btn = document.getElementById("btn-login");
    btn.disabled = true;
    showLoading(true);

    try {
        const codigoInput = document.getElementById("input-codigo");
        const codigoActivacion = pendingActivationCode || (codigoInput ? codigoInput.value.trim() : null) || null;

        const loginBody = { legajo, password: pin, codigoActivacion };
        if (state.empresaId) loginBody.empresaId = state.empresaId;

        const data = await api("/login", {
            method: "POST",
            body: loginBody,
            auth: false,
        });

        if (data.ok && data.token) {
            saveSession(data.token, {
                legajoId: data.legajoId,
                numeroLegajo: data.numeroLegajo || data.legajoId,
                nombreEmpleado: data.nombreEmpleado,
                empresaId: data.empresaId,
                nombreEmpresa: data.nombreEmpresa,
            }, data.dispositivoRegistrado || false);

            document.getElementById("input-legajo").value = "";
            document.getElementById("input-pin").value = "";

            if (data.dispositivoRegistrado) {
                enterHome();
            } else {
                showScreen("activar");
            }
        }
    } catch (err) {
        showError("login-error", err.message);
    } finally {
        btn.disabled = false;
        showLoading(false);
    }
}

// ======================================================================
// Activar dispositivo
// ======================================================================
async function handleActivar(e) {
    e.preventDefault();
    hideError("activar-error");

    const codigo = document.getElementById("input-codigo").value.trim();
    if (!codigo) {
        showError("activar-error", "Ingrese el código de activación");
        return;
    }

    const btn = document.getElementById("btn-activar");
    btn.disabled = true;
    showLoading(true);

    try {
        const data = await api("/registrar-dispositivo", {
            method: "POST",
            body: {
                codigo,
                deviceId: state.deviceId,
                publicKey: "",
                nombreDispositivo: navigator.userAgent.substring(0, 50),
                plataforma: /iPhone|iPad/.test(navigator.userAgent) ? "iOS" : "Android",
            },
        });

        if (data.ok) {
            state.deviceRegistered = true;
            // Update persisted session
            if (state.empresaId) {
                const sess = Storage.getJSON(sessionKey(state.empresaId));
                if (sess) {
                    sess.deviceRegistered = true;
                    Storage.setJSON(sessionKey(state.empresaId), sess);
                }
            }
            document.getElementById("input-codigo").value = "";
            enterHome();
        }
    } catch (err) {
        showError("activar-error", err.message);
    } finally {
        btn.disabled = false;
        showLoading(false);
    }
}

// ======================================================================
// Home
// ======================================================================
function enterHome() {
    updateHomeHeader();
    showScreen("home");
    startClock();
    startGpsWatch();
    loadEstado();
    updateSwitcherBadge();
}

function updateHomeHeader() {
    if (!state.user) return;
    document.getElementById("home-nombre").textContent = state.user.nombreEmpleado;
    document.getElementById("home-empresa").textContent = state.user.nombreEmpresa;
    document.getElementById("home-legajo").textContent = "Legajo: " + (state.user.numeroLegajo || state.user.legajoId);
}

function updateSwitcherBadge() {
    // Show company switcher button if device has multiple companies
    const switchBtn = document.getElementById("btn-switch-empresa");
    if (!switchBtn) return;

    const saved = getAllSavedSessions();
    if (saved.length > 1) {
        switchBtn.classList.remove("hidden");
    } else {
        // Also check server-side
        api("/mis-empresas", { auth: false })
            .then(data => {
                if (data.ok && data.empresas && data.empresas.length > 1) {
                    switchBtn.classList.remove("hidden");
                } else {
                    switchBtn.classList.add("hidden");
                }
            })
            .catch(() => {
                switchBtn.classList.add("hidden");
            });
    }
}

async function loadEstado() {
    try {
        const data = await api("/estado");
        if (data.ok) {
            const statusEl = document.getElementById("home-status");

            if (data.ultimaFichada) {
                const isEntrada = data.ultimaFichada.tipo === "Entrada";
                const tipo = isEntrada ? "Entrada" : "Salida";
                const hora = formatTime(data.ultimaFichada.fechaHora);
                document.getElementById("status-info").textContent = tipo + " — " + hora;
                sugeridoEntrada = !isEntrada; // Si última fue Entrada, sugerir Salida
                statusEl.classList.remove("hidden");
            } else {
                statusEl.classList.add("hidden");
                sugeridoEntrada = true; // Sin fichadas, sugerir Entrada
            }

            tipoInvertido = false;
            actualizarBotonFichar();
            renderHistorial(data.fichadasHoy || []);
        }
    } catch {
        // Silent fail — will retry on next interaction
    }
}

function actualizarBotonFichar() {
    const ficharText = document.getElementById("fichar-text");
    const btnSwap = document.getElementById("btn-swap-tipo");
    const esEntrada = tipoInvertido ? !sugeridoEntrada : sugeridoEntrada;
    ficharText.textContent = esEntrada ? "FICHAR ENTRADA" : "FICHAR SALIDA";
    if (btnSwap) {
        btnSwap.title = esEntrada ? "Cambiar a Salida" : "Cambiar a Entrada";
    }
}

function toggleTipoFichada() {
    tipoInvertido = !tipoInvertido;
    actualizarBotonFichar();
    // Breve feedback visual
    const btnSwap = document.getElementById("btn-swap-tipo");
    if (btnSwap) {
        btnSwap.classList.add("swap-active");
        setTimeout(() => btnSwap.classList.remove("swap-active"), 300);
    }
}

// ======================================================================
// Fichar (with retry on network error)
// ======================================================================
async function handleFichar() {
    const btn = document.getElementById("btn-fichar");
    const result = document.getElementById("fichar-result");
    btn.disabled = true;
    result.classList.add("hidden");

    try {
        const location = await getLocation();

        const esEntrada = tipoInvertido ? !sugeridoEntrada : sugeridoEntrada;
        const body = {
            timestamp: new Date().toISOString(),
            latitud: location?.latitud,
            longitud: location?.longitud,
            tipoFichada: tipoInvertido ? (esEntrada ? "Entrada" : "Salida") : "Auto",
        };

        const data = await api("/fichada", {
            method: "POST",
            body,
        });

        result.textContent = data.mensaje || (data.tipo + " registrada");
        result.className = "result-banner success";
        pendingFichada = null;
        await loadEstado();
    } catch (err) {
        // If network error, save for retry
        if (!navigator.onLine || err.message === "Failed to fetch") {
            pendingFichada = {
                timestamp: new Date().toISOString(),
                latitud: lastGpsCoords?.latitud,
                longitud: lastGpsCoords?.longitud,
                tipoFichada: body.tipoFichada,
            };
            Storage.setJSON("pendingFichada", {
                ...pendingFichada,
                empresaId: state.empresaId,
                token: state.token,
                deviceId: state.deviceId,
            });
            result.textContent = "Sin conexión. La fichada se enviará automáticamente al reconectar.";
            result.className = "result-banner warning";
        } else {
            result.textContent = err.message;
            result.className = "result-banner error";
        }
    } finally {
        btn.disabled = false;
        setTimeout(() => result.classList.add("hidden"), 6000);
    }
}

async function retryPendingFichada() {
    const saved = Storage.getJSON("pendingFichada");
    if (!saved) {
        pendingFichada = null;
        return;
    }

    // Only retry if we're in the same company session
    if (saved.empresaId !== state.empresaId || !state.token) {
        return;
    }

    try {
        const data = await api("/fichada", {
            method: "POST",
            body: {
                timestamp: saved.timestamp,
                latitud: saved.latitud,
                longitud: saved.longitud,
                tipoFichada: saved.tipoFichada || "Auto",
            },
        });

        Storage.remove("pendingFichada");
        pendingFichada = null;

        if (data.ok) {
            showToast("Fichada pendiente registrada: " + (data.tipo || "OK"), "success");
            loadEstado();
        }
    } catch {
        // Will retry on next online event
    }
}

// ======================================================================
// Historial
// ======================================================================
function renderHistorial(fichadas) {
    const list = document.getElementById("historial-list");
    const empty = document.getElementById("historial-empty");

    if (!fichadas.length) {
        list.innerHTML = "";
        empty.classList.remove("hidden");
        return;
    }

    empty.classList.add("hidden");
    list.innerHTML = fichadas.map(f => {
        const isEntrada = f.tipo === "Entrada";
        return `<div class="fichada-item">
            <div class="fichada-badge ${isEntrada ? 'entrada' : 'salida'}">${isEntrada ? 'E' : 'S'}</div>
            <div class="fichada-info">
                <div class="fichada-tipo">${isEntrada ? 'Entrada' : 'Salida'}</div>
                <div class="fichada-hora">${formatTimeFull(f.fechaHora)}</div>
            </div>
        </div>`;
    }).join("");
}

async function handleRefresh() {
    const btn = document.getElementById("btn-refresh");
    btn.disabled = true;
    hideError("historial-error");
    try {
        await loadEstado();
    } catch (err) {
        showError("historial-error", err.message);
    } finally {
        btn.disabled = false;
    }
}

// ======================================================================
// Mi QR (scoped per empresa)
// ======================================================================
async function loadMiQR() {
    const nombreEl = document.getElementById("miqr-nombre");
    const legajoEl = document.getElementById("miqr-legajo");
    const codeEl = document.getElementById("miqr-code");

    if (state.user) {
        nombreEl.textContent = state.user.nombreEmpleado;
        legajoEl.textContent = "Legajo: " + (state.user.numeroLegajo || state.user.legajoId);
    }

    if (qrTokenLoaded) return;

    try {
        const data = await api("/mi-qr");
        if (data.ok && data.qrToken) {
            codeEl.innerHTML = "";
            var qr = qrcode(0, "M");
            qr.addData(data.qrToken);
            qr.make();
            codeEl.innerHTML = qr.createSvgTag(6, 0);
            var svg = codeEl.querySelector("svg");
            if (svg) {
                svg.style.background = "#fff";
                svg.style.borderRadius = "12px";
                svg.style.padding = "12px";
            }
            qrTokenLoaded = true;
        }
    } catch (err) {
        showError("miqr-error", err.message);
    }
}

// ======================================================================
// Tabs
// ======================================================================
function handleTab(tabName) {
    // Update active tab styling
    document.querySelectorAll(".tab").forEach(t => t.classList.remove("active"));
    document.querySelectorAll(`.tab[data-tab="${tabName}"]`).forEach(t => t.classList.add("active"));

    if (tabName === "home") {
        showScreen("home");
        startClock();
        startGpsWatch();
        loadEstado();
    } else if (tabName === "historial") {
        stopClock();
        stopGpsWatch();
        showScreen("historial");
        loadEstado();
    } else if (tabName === "miqr") {
        stopClock();
        stopGpsWatch();
        showScreen("miqr");
        loadMiQR();
    }
}

// ======================================================================
// Empresa Selector
// ======================================================================
async function checkEmpresas() {
    try {
        const data = await api("/mis-empresas", { auth: false });
        if (data.ok && data.empresas && data.empresas.length > 1) {
            renderEmpresas(data.empresas);
            showScreen("empresas");
        } else if (data.ok && data.empresas && data.empresas.length === 1) {
            // 1 empresa: auto-seleccionar pero mostrar "Cambiar empresa"
            selectEmpresa(data.empresas[0].id, data.empresas[0].nombre);
        } else {
            // Sin empresas: login limpio sin badge
            state.empresaId = null;
            document.getElementById("login-empresa-badge").classList.add("hidden");
            document.getElementById("btn-cambiar-empresa").classList.add("hidden");
            showScreen("login");
        }
    } catch {
        state.empresaId = null;
        document.getElementById("login-empresa-badge").classList.add("hidden");
        document.getElementById("btn-cambiar-empresa").classList.add("hidden");
        showScreen("login");
    }
}

function renderEmpresas(empresas) {
    const list = document.getElementById("empresas-list");
    // Mark companies where we have a saved session
    list.innerHTML = empresas.map(e => {
        const hasSess = Storage.getJSON(sessionKey(e.id));
        const savedClass = (hasSess && hasSess.token && !isTokenExpired(hasSess.token)) ? " empresa-saved" : "";
        return `<button class="empresa-btn${savedClass}" data-id="${e.id}" data-nombre="${e.nombre}">
            <span class="empresa-nombre">${e.nombre}</span>
            ${savedClass ? '<span class="empresa-sesion">Sesión activa</span>' : ''}
        </button>`;
    }).join("");

    list.querySelectorAll(".empresa-btn").forEach(btn => {
        btn.addEventListener("click", () => {
            const id = parseInt(btn.dataset.id);
            const nombre = btn.dataset.nombre;
            // Try to restore existing session for this company
            if (loadSession(id)) {
                if (state.deviceRegistered) {
                    qrTokenLoaded = false; // Reset QR for new company
                    enterHome();
                    return;
                }
            }
            selectEmpresa(id, nombre);
        });
    });
}

function selectEmpresa(id, nombre) {
    state.empresaId = id;
    Storage.set("activeEmpresaId", String(id));
    const badge = document.getElementById("login-empresa-badge");
    badge.textContent = nombre;
    badge.classList.remove("hidden");
    document.getElementById("btn-cambiar-empresa").classList.remove("hidden");
    showScreen("login");
}

// Switch company from home screen (preserves sessions)
async function switchEmpresa() {
    stopClock();
    stopGpsWatch();
    qrTokenLoaded = false;
    // Don't clear current session — just go to selector
    checkEmpresas();
}

// ======================================================================
// Logout (clears current company session only, or all)
// ======================================================================
function logout() {
    if (state.empresaId) {
        clearSession(state.empresaId);
    }
    qrTokenLoaded = false;
    stopClock();
    stopGpsWatch();
    document.getElementById("login-empresa-badge").classList.add("hidden");
    document.getElementById("btn-cambiar-empresa").classList.add("hidden");
    checkEmpresas();
}

function logoutAll() {
    clearAllSessions();
    Storage.remove("activeEmpresaId");
    Storage.remove("pendingFichada");
    qrTokenLoaded = false;
    stopClock();
    stopGpsWatch();
    document.getElementById("login-empresa-badge").classList.add("hidden");
    document.getElementById("btn-cambiar-empresa").classList.add("hidden");
    state.empresaId = null;
    checkEmpresas();
}

// ======================================================================
// Init
// ======================================================================
function init() {
    // Forms
    document.getElementById("form-login").addEventListener("submit", handleLogin);
    document.getElementById("form-activar").addEventListener("submit", handleActivar);

    // Buttons
    document.getElementById("btn-fichar").addEventListener("click", handleFichar);
    document.getElementById("btn-swap-tipo").addEventListener("click", toggleTipoFichada);
    document.getElementById("btn-refresh").addEventListener("click", handleRefresh);
    document.getElementById("btn-logout").addEventListener("click", logout);
    document.getElementById("btn-logout2").addEventListener("click", logout);
    document.getElementById("btn-logout3").addEventListener("click", logout);

    // "Agregar otra empresa" from selector screen
    document.getElementById("btn-otra-empresa").addEventListener("click", () => {
        state.empresaId = null;
        document.getElementById("login-empresa-badge").classList.add("hidden");
        document.getElementById("btn-cambiar-empresa").classList.add("hidden");
        showScreen("activar");
    });

    // "Cambiar empresa" from login screen — clear selection, allow free login
    document.getElementById("btn-cambiar-empresa").addEventListener("click", () => {
        state.empresaId = null;
        document.getElementById("login-empresa-badge").classList.add("hidden");
        document.getElementById("btn-cambiar-empresa").classList.add("hidden");
        // Show login without empresa preselected — backend will resolve cross-tenant
    });

    // Switch empresa from home (preserves sessions)
    const switchBtn = document.getElementById("btn-switch-empresa");
    if (switchBtn) {
        switchBtn.addEventListener("click", switchEmpresa);
    }

    // Tabs
    document.querySelectorAll(".tab").forEach(tab => {
        tab.addEventListener("click", () => handleTab(tab.dataset.tab));
    });

    // Online / Offline listeners
    window.addEventListener("online", updateOnlineStatus);
    window.addEventListener("offline", updateOnlineStatus);
    updateOnlineStatus();

    // Check for activation code in URL (deep link)
    const urlParams = new URLSearchParams(window.location.search);
    const activateCode = urlParams.get("code");
    if (activateCode) {
        pendingActivationCode = activateCode;
        const codeInput = document.getElementById("input-codigo");
        if (codeInput) codeInput.value = activateCode;
        window.history.replaceState({}, "", window.location.pathname);
    }

    // Restore session — try last active company first
    const lastEmpresaId = parseInt(Storage.get("activeEmpresaId"));
    if (lastEmpresaId && loadSession(lastEmpresaId)) {
        if (state.deviceRegistered) {
            enterHome();
        } else {
            showScreen("activar");
        }
    } else {
        // No valid saved session — check registered companies
        checkEmpresas();
    }

    // Check for pending fichada to retry
    const savedFichada = Storage.getJSON("pendingFichada");
    if (savedFichada) {
        pendingFichada = savedFichada;
        if (isOnline) {
            setTimeout(() => retryPendingFichada(), 2000);
        }
    }

    // Register service worker
    if ("serviceWorker" in navigator) {
        navigator.serviceWorker.register("sw.js").catch(() => {});
    }

    // Migrate old localStorage format (one-time)
    migrateOldStorage();
}

// Migrate from old single-session format to per-company format
function migrateOldStorage() {
    const oldToken = Storage.get("token");
    const oldUser = Storage.getJSON("user");
    const oldRegistered = Storage.get("deviceRegistered");
    if (oldToken && oldUser && oldUser.empresaId) {
        // Migrate to new format
        saveSession(oldToken, oldUser, oldRegistered === "true");
        // Clean up old keys
        Storage.remove("token");
        Storage.remove("user");
        Storage.remove("deviceRegistered");
    }
}

document.addEventListener("DOMContentLoaded", init);
