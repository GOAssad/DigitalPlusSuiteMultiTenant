const CACHE_NAME = "digitalone-v1";
const ASSETS = [
    "/mobile/",
    "/mobile/index.html",
    "/mobile/style.css",
    "/mobile/app.js",
    "/mobile/icon.svg",
];

self.addEventListener("install", (e) => {
    e.waitUntil(
        caches.open(CACHE_NAME).then(cache => cache.addAll(ASSETS))
    );
    self.skipWaiting();
});

self.addEventListener("activate", (e) => {
    e.waitUntil(
        caches.keys().then(keys =>
            Promise.all(keys.filter(k => k !== CACHE_NAME).map(k => caches.delete(k)))
        )
    );
    self.clients.claim();
});

self.addEventListener("fetch", (e) => {
    // Network-first for API calls
    if (e.request.url.includes("/api/")) {
        e.respondWith(fetch(e.request));
        return;
    }
    // Cache-first for static assets
    e.respondWith(
        caches.match(e.request).then(cached => cached || fetch(e.request))
    );
});
