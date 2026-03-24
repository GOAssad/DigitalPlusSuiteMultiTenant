// Dashboard Charts - Chart.js interop
window.dashboardCharts = {
    _instances: {},

    destroy(canvasId) {
        if (this._instances[canvasId]) {
            this._instances[canvasId].destroy();
            delete this._instances[canvasId];
        }
    },

    renderAsistenciaSemanal(canvasId, labels, presentes, ausentes, tardes) {
        this.destroy(canvasId);
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;
        this._instances[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'Presentes',
                        data: presentes,
                        backgroundColor: 'rgba(25, 135, 84, 0.8)',
                        borderRadius: 4
                    },
                    {
                        label: 'Tarde',
                        data: tardes,
                        backgroundColor: 'rgba(255, 193, 7, 0.8)',
                        borderRadius: 4
                    },
                    {
                        label: 'Ausentes',
                        data: ausentes,
                        backgroundColor: 'rgba(220, 53, 69, 0.8)',
                        borderRadius: 4
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'bottom', labels: { boxWidth: 12, padding: 15 } }
                },
                scales: {
                    x: { stacked: true, grid: { display: false } },
                    y: { stacked: true, beginAtZero: true, ticks: { stepSize: 1, precision: 0 } }
                }
            }
        });
    },

    renderEstadoHoy(canvasId, presentes, ausentes, sinFichada) {
        this.destroy(canvasId);
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;
        this._instances[canvasId] = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: ['Presentes', 'Ausentes', 'Sin fichada'],
                datasets: [{
                    data: [presentes, ausentes, sinFichada],
                    backgroundColor: [
                        'rgba(25, 135, 84, 0.85)',
                        'rgba(220, 53, 69, 0.85)',
                        'rgba(108, 117, 125, 0.5)'
                    ],
                    borderWidth: 2,
                    borderColor: '#fff'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '65%',
                plugins: {
                    legend: { position: 'bottom', labels: { boxWidth: 12, padding: 10 } }
                }
            }
        });
    },

    renderFichadasPorHora(canvasId, labels, entradas, salidas) {
        this.destroy(canvasId);
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;
        this._instances[canvasId] = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'Entradas',
                        data: entradas,
                        borderColor: 'rgba(25, 135, 84, 1)',
                        backgroundColor: 'rgba(25, 135, 84, 0.1)',
                        fill: true,
                        tension: 0.3,
                        pointRadius: 3
                    },
                    {
                        label: 'Salidas',
                        data: salidas,
                        borderColor: 'rgba(220, 53, 69, 1)',
                        backgroundColor: 'rgba(220, 53, 69, 0.1)',
                        fill: true,
                        tension: 0.3,
                        pointRadius: 3
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'bottom', labels: { boxWidth: 12, padding: 15 } }
                },
                scales: {
                    x: { grid: { display: false } },
                    y: { beginAtZero: true, ticks: { stepSize: 1, precision: 0 } }
                }
            }
        });
    }
};
