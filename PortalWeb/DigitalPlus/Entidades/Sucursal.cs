using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Sucursal
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatirio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage ="Este campo es obligatirio")]
        [MaxLength(5, ErrorMessage ="El codigo puede tener hasta 5 caracteres Alfanumericos")]
        public string CodigoSucursal { get; set; }

        public List<LegajoSucursal> LegajoSucursal { get; set; }


        /// <summary>
        /// Para que funcione la busqueda del typeahead
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is Sucursal s2)
            {
                return Id == s2.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // fin para que funcione la busqueda del typeahead///////////////
    }
}
