using DigitalPlus.DTOs;
using DigitalPlus.Entidades.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Fichada
    {
        public int Id { get; set; }
        public int SucursalId { get; set; }
        public int Legajoid { get; set; }
        [DataType(DataType.DateTime)]

        [Fichada_FechaHoraMenorAlPresente]
        public DateTime Registro { get; set; }
        public string EntraSale{ get; set; }

        public Sucursal Sucursal { get; set; }

        public Legajo Legajo { get; set; }

        

    }
}
