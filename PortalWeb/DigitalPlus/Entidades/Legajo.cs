using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Legajo
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string LegajoId { get; set; }
        public int SectorId { get; set; }
        public Sector Sector { get; set; }
        public bool Activo { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public List<LegajoSucursal> LegajoSucursal { get; set; } = new List<LegajoSucursal>();
        public List<LegajoHuella> LegajoHuella { get; set; } = new List<LegajoHuella>();
        public int? HorarioId { get; set; }
        public Horario Horario { get; set; }
        public bool CalendarioPersonalizado { get; set; }
    }
}
