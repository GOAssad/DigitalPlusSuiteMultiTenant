// Contextual help: updates the help button href based on current portal page
// Uses polling because Blazor Server navigates via SignalR, not pushState
(function () {
    var helpBase = 'https://integraia.tech/digital-one-help.html';

    var routeMap = {
        '/': '#portal-dashboard',
        '/legajos': '#portal-legajos',
        '/fichadas': '#portal-fichadas',
        '/horarios': '#portal-estructura',
        '/categorias': '#portal-estructura',
        '/sectores': '#portal-estructura',
        '/sucursales': '#portal-estructura',
        '/terminales': '#portal-estructura',
        '/terminales-moviles': '#terminales-moviles',
        '/fichado-movil': '#fichada-movil',
        '/incidencias': '#portal-incidencias',
        '/vacaciones': '#portal-incidencias',
        '/feriados': '#portal-estructura',
        '/reportes/asistencia': '#portal-reportes',
        '/reportes/llegadas-tarde': '#portal-reportes',
        '/reportes/ausencias': '#portal-reportes',
        '/reportes/horas-trabajadas': '#portal-reportes',
        '/usuarios': '#portal-usuarios',
        '/configuracion/empresa': '#portal',
        '/configuracion/planes': '#licencias',
        '/configuracion': '#portal',
        '/noticias': '#portal'
    };

    var partialMap = [
        { match: '/legajos/', anchor: '#portal-legajo-form' },
        { match: '/reportes/', anchor: '#portal-reportes' },
        { match: '/configuracion/', anchor: '#portal' }
    ];

    var lastPath = '';

    function updateHelp() {
        var btn = document.getElementById('helpBtn');
        if (!btn) return;

        var path = window.location.pathname.replace(/\/$/, '') || '/';
        if (path === lastPath) return;
        lastPath = path;

        var anchor = routeMap[path] || '';
        if (!anchor) {
            for (var i = 0; i < partialMap.length; i++) {
                if (path.indexOf(partialMap[i].match) === 0) {
                    anchor = partialMap[i].anchor;
                    break;
                }
            }
        }
        btn.href = helpBase + anchor;
    }

    // Poll every 500ms to detect Blazor navigation
    setInterval(updateHelp, 500);
})();
