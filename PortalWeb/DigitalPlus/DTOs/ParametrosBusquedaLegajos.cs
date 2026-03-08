using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalPlus.DTOs
{
    public class ParametrosBusquedaLegajos
    {
        public int Pagina { get; set; } = 1;
        public int CantidadRegistros { get; set; } = 25;
        public Paginacion Paginacion
        {
            get { return new Paginacion() { Pagina = Pagina, CantidadRegistros = CantidadRegistros }; }
        }
        public string Nombre { get; set; }
        public int SectorId { get; set; }
        public string UsuarioId { get; set; }
        public bool Activo { get; set; }

    }
}
