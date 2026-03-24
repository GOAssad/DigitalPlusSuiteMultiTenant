// Leaflet map interop for SucursalForm
window.LeafletMap = {
    _map: null,
    _marker: null,
    _circle: null,
    _dotnetRef: null,

    init: function (elementId, dotnetRef, lat, lng, radio) {
        this._dotnetRef = dotnetRef;

        // Default: Buenos Aires center if no coords
        const defaultLat = lat || -34.6037;
        const defaultLng = lng || -58.3816;
        const zoom = lat ? 16 : 12;

        this._map = L.map(elementId, { zoomControl: true }).setView([defaultLat, defaultLng], zoom);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors',
            maxZoom: 19
        }).addTo(this._map);

        // Marker
        this._marker = L.marker([defaultLat, defaultLng], { draggable: true }).addTo(this._map);

        // Radius circle
        this._circle = L.circle([defaultLat, defaultLng], {
            radius: radio || 100,
            color: '#00C9A7',
            fillColor: '#00C9A7',
            fillOpacity: 0.12,
            weight: 2
        }).addTo(this._map);

        // Hide marker/circle if no initial coords
        if (!lat) {
            this._marker.setOpacity(0);
            this._circle.setStyle({ opacity: 0, fillOpacity: 0 });
        }

        // Drag end
        this._marker.on('dragend', () => {
            const pos = this._marker.getLatLng();
            this._circle.setLatLng(pos);
            this._map.panTo(pos);
            this._notifyPosition(pos.lat, pos.lng);
            this._reverseGeocode(pos.lat, pos.lng);
        });

        // Click on map
        this._map.on('click', (e) => {
            this._marker.setLatLng(e.latlng);
            this._marker.setOpacity(1);
            this._circle.setLatLng(e.latlng);
            this._circle.setStyle({ opacity: 1, fillOpacity: 0.12 });
            this._notifyPosition(e.latlng.lat, e.latlng.lng);
            this._reverseGeocode(e.latlng.lat, e.latlng.lng);
        });

        // Fix map size after render
        setTimeout(() => this._map.invalidateSize(), 200);
    },

    updateRadius: function (radio) {
        if (this._circle) {
            this._circle.setRadius(radio);
        }
    },

    setPosition: function (lat, lng) {
        if (!this._map) return;
        const latlng = L.latLng(lat, lng);
        this._marker.setLatLng(latlng);
        this._marker.setOpacity(1);
        this._circle.setLatLng(latlng);
        this._circle.setStyle({ opacity: 1, fillOpacity: 0.12 });
        this._map.setView(latlng, 16);
    },

    searchAddress: async function (query) {
        if (!query || query.length < 3) return;
        try {
            const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)}&limit=5&countrycodes=ar&addressdetails=1`;
            const res = await fetch(url, {
                headers: { 'Accept-Language': 'es' }
            });
            const results = await res.json();
            if (results.length > 0 && this._dotnetRef) {
                const items = results.map(r => ({
                    displayName: r.display_name,
                    lat: parseFloat(r.lat),
                    lng: parseFloat(r.lon),
                    address: r.address || {}
                }));
                await this._dotnetRef.invokeMethodAsync('OnSearchResults', JSON.stringify(items));
            }
        } catch (e) {
            console.error('Geocoding error:', e);
        }
    },

    selectResult: function (lat, lng) {
        this.setPosition(lat, lng);
        this._notifyPosition(lat, lng);
        this._reverseGeocode(lat, lng);
    },

    _notifyPosition: function (lat, lng) {
        if (this._dotnetRef) {
            this._dotnetRef.invokeMethodAsync('OnMapPositionChanged', lat, lng);
        }
    },

    _reverseGeocode: async function (lat, lng) {
        try {
            const url = `https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}&addressdetails=1`;
            const res = await fetch(url, {
                headers: { 'Accept-Language': 'es' }
            });
            const data = await res.json();
            if (data.address && this._dotnetRef) {
                const addr = data.address;
                const info = {
                    direccion: [addr.road, addr.house_number].filter(Boolean).join(' ') || '',
                    localidad: addr.city || addr.town || addr.village || addr.municipality || '',
                    provincia: addr.state || ''
                };
                await this._dotnetRef.invokeMethodAsync('OnReverseGeocode', JSON.stringify(info));
            }
        } catch (e) {
            console.error('Reverse geocoding error:', e);
        }
    },

    dispose: function () {
        if (this._map) {
            this._map.remove();
            this._map = null;
        }
        this._dotnetRef = null;
    }
};
