const CACHE_NAME = "digitalone-v2.1.0";
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
    // Network-first for everything (fall back to cache if offline)
    e.respondWith(
        fetch(e.request)
            .then(res => {
                // Update cache with fresh response
                if (res.ok && e.request.method === "GET") {
                    const clone = res.clone();
                    caches.open(CACHE_NAME).then(cache => cache.put(e.request, clone));
                }
                return res;
            })
            .catch(() => caches.match(e.request))
    );
});
