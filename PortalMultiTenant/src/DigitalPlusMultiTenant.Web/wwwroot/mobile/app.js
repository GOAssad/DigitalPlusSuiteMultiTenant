"use strict";

const API_BASE = "/api/mobile";

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
    token: Storage.get("token"),
    deviceId: Storage.get("deviceId") || generateDeviceId(),
    user: Storage.getJSON("user"),
    deviceRegistered: Storage.get("deviceRegistered") === "true",
};

let clockInterval = null;
let gpsWatchId = null;
let lastGpsCoords = null;

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

// --- API ---
async function api(endpoint, options = {}) {
    const headers = { "Content-Type": "application/json" };
    if (state.deviceId) headers["X-Device-Id"] = state.deviceId;
    if (state.token && options.auth !== false) {
        headers["Authorization"] = "Bearer " + state.token;
    }

    const res = await fetch(API_BASE + endpoint, {
        method: options.method || "GET",
        headers,
        body: options.body ? JSON.stringify(options.body) : undefined,
    });

    const data = await res.json();

    if (res.status === 401 && options.auth !== false) {
        logout();
        throw new Error(data.mensaje || "Sesión expirada");
    }
    if (!res.ok) {
        throw new Error(data.mensaje || "Error del servidor");
    }
    return data;
}

// --- Auth ---
function saveSession(token, user, deviceRegistered) {
    state.token = token;
    state.user = user;
    state.deviceRegistered = deviceRegistered;
    Storage.set("token", token);
    Storage.setJSON("user", user);
    Storage.set("deviceRegistered", String(deviceRegistered));
}

function logout() {
    state.token = null;
    state.user = null;
    state.deviceRegistered = false;
    Storage.remove("token");
    Storage.remove("user");
    Storage.remove("deviceRegistered");
    stopClock();
    stopGpsWatch();
    showScreen("login");
}

// --- Screens ---
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

// --- Live Clock ---
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

// --- GPS Watch ---
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

            // Show accuracy badge
            accEl.classList.remove("hidden");
            accEl.querySelector(".badge-text").textContent = "Precisión: " + Math.round(pos.coords.accuracy) + "m";

            // Show distance badge
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

// --- Location (for fichada) ---
function getLocation() {
    // Use cached GPS coords if available
    if (lastGpsCoords) {
        return Promise.resolve({ latitud: lastGpsCoords.latitud, longitud: lastGpsCoords.longitud });
    }
    // Fallback: one-shot
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

// --- Time formatting ---
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

// --- Login ---
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
        const data = await api("/login", {
            method: "POST",
            body: { legajo, password: pin },
            auth: false,
        });

        if (data.ok && data.token) {
            saveSession(data.token, {
                legajoId: data.legajoId,
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

// --- Activar ---
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
            Storage.set("deviceRegistered", "true");
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

// --- Home ---
function enterHome() {
    updateHomeHeader();
    showScreen("home");
    startClock();
    startGpsWatch();
    loadEstado();
}

function updateHomeHeader() {
    if (!state.user) return;
    document.getElementById("home-nombre").textContent = state.user.nombreEmpleado;
    document.getElementById("home-empresa").textContent = state.user.nombreEmpresa;
    document.getElementById("home-legajo").textContent = "Legajo: " + state.user.legajoId;
}

async function loadEstado() {
    try {
        const data = await api("/estado");
        if (data.ok) {
            const statusEl = document.getElementById("home-status");
            const ficharText = document.getElementById("fichar-text");

            if (data.ultimaFichada) {
                const isEntrada = data.ultimaFichada.tipo === "Entrada";
                const tipo = isEntrada ? "Entrada" : "Salida";
                const hora = formatTime(data.ultimaFichada.fechaHora);
                document.getElementById("status-info").textContent = tipo + " — " + hora;
                ficharText.textContent = isEntrada ? "FICHAR SALIDA" : "FICHAR ENTRADA";
                statusEl.classList.remove("hidden");
            } else {
                statusEl.classList.add("hidden");
                ficharText.textContent = "FICHAR ENTRADA";
            }

            renderHistorial(data.fichadasHoy || []);
        }
    } catch {
        // Silent fail
    }
}

// --- Fichar ---
async function handleFichar() {
    const btn = document.getElementById("btn-fichar");
    const result = document.getElementById("fichar-result");
    btn.disabled = true;
    result.classList.add("hidden");

    try {
        const location = await getLocation();

        const data = await api("/fichada", {
            method: "POST",
            body: {
                timestamp: new Date().toISOString(),
                latitud: location?.latitud,
                longitud: location?.longitud,
                tipoFichada: "Auto",
            },
        });

        result.textContent = data.mensaje || (data.tipo + " registrada");
        result.className = "result-banner success";
        await loadEstado();
    } catch (err) {
        result.textContent = err.message;
        result.className = "result-banner error";
    } finally {
        btn.disabled = false;
        setTimeout(() => result.classList.add("hidden"), 5000);
    }
}

// --- Historial ---
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

// --- Tabs ---
function handleTab(tabName) {
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
    }
}

// --- Init ---
function init() {
    // Forms
    document.getElementById("form-login").addEventListener("submit", handleLogin);
    document.getElementById("form-activar").addEventListener("submit", handleActivar);

    // Buttons
    document.getElementById("btn-fichar").addEventListener("click", handleFichar);
    document.getElementById("btn-refresh").addEventListener("click", handleRefresh);
    document.getElementById("btn-logout").addEventListener("click", logout);
    document.getElementById("btn-logout2").addEventListener("click", logout);

    // Tabs
    document.querySelectorAll(".tab").forEach(tab => {
        tab.addEventListener("click", () => handleTab(tab.dataset.tab));
    });

    // Check for activation code in URL (deep link)
    const urlParams = new URLSearchParams(window.location.search);
    const activateCode = urlParams.get("code");
    if (activateCode) {
        const codeInput = document.getElementById("input-codigo");
        if (codeInput) codeInput.value = activateCode;
        // Clean URL without reload
        window.history.replaceState({}, "", window.location.pathname);
    }

    // Restore session
    if (state.token && state.user) {
        if (state.deviceRegistered) {
            enterHome();
        } else {
            showScreen("activar");
        }
    } else {
        showScreen("login");
    }

    // Register service worker
    if ("serviceWorker" in navigator) {
        navigator.serviceWorker.register("sw.js").catch(() => {});
    }
}

document.addEventListener("DOMContentLoaded", init);
