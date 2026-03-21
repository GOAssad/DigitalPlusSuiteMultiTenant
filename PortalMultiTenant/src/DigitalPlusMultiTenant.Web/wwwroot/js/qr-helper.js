// QR Helper para Blazor - usa qrcode-generator
window.qrHelper = {
    // Renderiza un QR como SVG dentro del elemento con el id dado
    render: function (elementId, text, cellSize) {
        var el = document.getElementById(elementId);
        if (!el || !text) return;
        var qr = qrcode(0, 'M');
        qr.addData(text);
        qr.make();
        el.innerHTML = qr.createSvgTag(cellSize || 4, 0);
    },

    // Genera un QR como data URL (para impresion)
    toDataUrl: function (text, cellSize) {
        var qr = qrcode(0, 'M');
        qr.addData(text);
        qr.make();
        return qr.createDataURL(cellSize || 6, 0);
    },

    // Imprime credenciales QR (abre ventana de impresion)
    printCredenciales: function (legajos) {
        var html = '<html><head><title>Credenciales QR</title><style>';
        html += 'body{font-family:Segoe UI,Arial,sans-serif;margin:0;padding:10px}';
        html += '.card{display:inline-block;width:240px;border:1px solid #ccc;border-radius:8px;padding:16px;margin:8px;text-align:center;page-break-inside:avoid}';
        html += '.card img.foto{width:60px;height:60px;border-radius:50%;object-fit:cover;margin-bottom:8px}';
        html += '.card .nombre{font-weight:600;font-size:14px;margin-bottom:2px}';
        html += '.card .legajo{color:#666;font-size:12px;margin-bottom:8px}';
        html += '.card .qr{margin:8px auto}';
        html += '.card .qr svg{display:block;margin:0 auto}';
        html += '@media print{body{margin:0}.card{border:1px solid #999}}';
        html += '</style></head><body>';

        for (var i = 0; i < legajos.length; i++) {
            var l = legajos[i];
            var qr = qrcode(0, 'M');
            qr.addData(l.qrToken);
            qr.make();
            html += '<div class="card">';
            if (l.foto) {
                html += '<img class="foto" src="data:image/jpeg;base64,' + l.foto + '"/>';
            }
            html += '<div class="nombre">' + l.nombre + '</div>';
            html += '<div class="legajo">Legajo: ' + l.numeroLegajo + '</div>';
            html += '<div class="qr">' + qr.createSvgTag(3, 0) + '</div>';
            html += '</div>';
        }

        html += '</body></html>';
        var w = window.open('', '_blank');
        w.document.write(html);
        w.document.close();
        w.onload = function () { w.print(); };
    }
};
