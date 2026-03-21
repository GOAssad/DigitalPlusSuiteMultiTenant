"use strict";

const API_BASE = "/api/mobile";

// --- Storage ---
const Storage = {
    get(key) { try { return localStorage.getItem("kiosko_" + key); } catch { return null; } },
    set(key, val) { try { localStorage.setItem("kiosko_" + key, val); } catch {} },
    remove(key) { try { localStorage.removeItem("kiosko_" + key); } catch {} },
};

// --- State ---
let deviceId = Storage.get("deviceId");
let empresaNombre = Storage.get("empresaNombre");
let sucursalNombre = Storage.get("sucursalNombre");
let clockInterval = null;
let scanner = null;
let lastScannedToken = null;
let lastScanTime = 0;
const COOLDOWN_MS = 5000; // 5s cooldown between same token (UI side)

// --- Screens ---
function showScreen(name) {
    document.querySelectorAll(".screen").forEach(s => s.classList.remove("active"));
    const el = document.getElementById("screen-" + name);
    if (el) el.classList.add("active");
}

// --- Clock ---
function startClock() {
    updateClock();
    clockInterval = setInterval(updateClock, 1000);
}
function updateClock() {
    const el = document.getElementById("header-clock");
    if (!el) return;
    const now = new Date();
    el.textContent = now.toLocaleTimeString("es-AR", { hour: "2-digit", minute: "2-digit" });
}

// --- API ---
async function apiPost(endpoint, body) {
    const headers = {
        "Content-Type": "application/json",
        "X-Device-Id": deviceId,
    };
    const res = await fetch(API_BASE + endpoint, {
        method: "POST",
        headers,
        body: JSON.stringify(body),
    });
    return await res.json();
}

async function apiGet(endpoint) {
    const headers = { "X-Device-Id": deviceId };
    const res = await fetch(API_BASE + endpoint, { headers });
    return await res.json();
}

// --- Setup ---
async function handleSetup(e) {
    e.preventDefault();
    const errorEl = document.getElementById("setup-error");
    errorEl.classList.add("hidden");

    const id = document.getElementById("input-deviceid").value.trim();
    if (!id) {
        errorEl.textContent = "Ingrese el Device ID.";
        errorEl.classList.remove("hidden");
        return;
    }

    deviceId = id;
    Storage.set("deviceId", id);

    // Verify kiosko exists
    try {
        const data = await apiGet("/kiosko-info");
        if (!data.ok) {
            errorEl.textContent = data.mensaje || "Kiosko no encontrado.";
            errorEl.classList.remove("hidden");
            Storage.remove("deviceId");
            deviceId = null;
            return;
        }

        empresaNombre = data.empresaNombre;
        sucursalNombre = data.sucursalNombre;
        Storage.set("empresaNombre", empresaNombre);
        Storage.set("sucursalNombre", sucursalNombre);

        enterScanner();
    } catch (err) {
        errorEl.textContent = "Error de conexion: " + err.message;
        errorEl.classList.remove("hidden");
    }
}

// --- Scanner ---
function enterScanner() {
    document.getElementById("header-sucursal").textContent = sucursalNombre || "";
    document.getElementById("footer-empresa").textContent = empresaNombre || "";
    showScreen("scanner");
    startClock();
    startScanner();
}

function startScanner() {
    if (scanner) return;

    scanner = new Html5Qrcode("qr-reader");

    // Calcular qrbox proporcional al contenedor
    var readerEl = document.getElementById("qr-reader");
    var w = readerEl.offsetWidth || 400;
    var qrSize = Math.min(Math.floor(w * 0.7), 300);

    scanner.start(
        { facingMode: "environment" },
        {
            fps: 15,
            qrbox: { width: qrSize, height: qrSize },
            disableFlip: false,
        },
        onQrDecoded,
        () => {} // ignore scan failures
    ).then(() => {
        document.querySelector(".scanner-instruction div").textContent =
            "Camara activa - Muestre su QR frente a la camara";
    }).catch(err => {
        console.error("Camera error:", err);
        document.querySelector(".scanner-instruction div").textContent =
            "Error de camara: " + err;
    });
}

async function onQrDecoded(decodedText) {
    // Cooldown: ignore same token within COOLDOWN_MS
    const now = Date.now();
    if (decodedText === lastScannedToken && (now - lastScanTime) < COOLDOWN_MS) {
        return;
    }
    lastScannedToken = decodedText;
    lastScanTime = now;

    try {
        const data = await apiPost("/fichar-qr", {
            qrToken: decodedText,
            deviceId: deviceId,
        });

        if (data.ok) {
            showResult(data);
        } else {
            showScanError(data.mensaje || "Error al fichar.");
        }
    } catch (err) {
        showScanError("Error de conexion.");
    }
}

function showResult(data) {
    const el = document.getElementById("scan-result");
    const fotoEl = document.getElementById("result-foto");
    const iconEl = document.getElementById("result-icon");

    if (data.foto) {
        fotoEl.src = "data:image/jpeg;base64," + data.foto;
        fotoEl.classList.remove("hidden");
        iconEl.classList.add("hidden");
    } else {
        fotoEl.classList.add("hidden");
        iconEl.classList.remove("hidden");
    }

    document.getElementById("result-nombre").textContent = data.nombre;

    const tipoEl = document.getElementById("result-tipo");
    tipoEl.textContent = data.tipo.toUpperCase();
    tipoEl.className = "result-tipo " + (data.tipo === "Entrada" ? "entrada" : "salida");

    const hora = new Date(data.fechaHora);
    document.getElementById("result-hora").textContent =
        hora.toLocaleTimeString("es-AR", { hour: "2-digit", minute: "2-digit", second: "2-digit" });
    document.getElementById("result-sucursal").textContent = data.sucursalNombre || "";

    el.classList.remove("hidden");

    // Auto-hide after 3 seconds
    setTimeout(() => {
        el.classList.add("hidden");
    }, 3000);
}

function showScanError(msg) {
    const el = document.getElementById("scan-error");
    document.getElementById("error-mensaje").textContent = msg;
    el.classList.remove("hidden");
    setTimeout(() => el.classList.add("hidden"), 3000);
}

// --- Reset ---
function handleReset() {
    if (!confirm("Desea reconfigurar el kiosko?")) return;
    if (scanner) {
        scanner.stop().catch(() => {});
        scanner = null;
    }
    if (clockInterval) { clearInterval(clockInterval); clockInterval = null; }
    Storage.remove("deviceId");
    Storage.remove("empresaNombre");
    Storage.remove("sucursalNombre");
    deviceId = null;
    empresaNombre = null;
    sucursalNombre = null;
    lastScannedToken = null;
    showScreen("setup");
}

// --- Init ---
function init() {
    document.getElementById("form-setup").addEventListener("submit", handleSetup);
    document.getElementById("btn-reset").addEventListener("click", handleReset);

    // Restore session
    if (deviceId) {
        // Verify kiosko still valid
        apiGet("/kiosko-info").then(data => {
            if (data.ok) {
                empresaNombre = data.empresaNombre;
                sucursalNombre = data.sucursalNombre;
                enterScanner();
            } else {
                showScreen("setup");
            }
        }).catch(() => {
            // Offline? Try with cached data
            if (empresaNombre && sucursalNombre) {
                enterScanner();
            } else {
                showScreen("setup");
            }
        });
    } else {
        showScreen("setup");
    }
}

document.addEventListener("DOMContentLoaded", init);
