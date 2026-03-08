using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades.Validaciones
{
    public class Fichada_FechaHoraMenorAlPresente : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var fichada = validationContext.ObjectInstance as Fichada;
            if (fichada != null)
            {
                if (fichada.Registro >= DateTime.Now)
                {
                    return new ValidationResult($"La fecha ingresada todavia no llego");
                }
            }

            return ValidationResult.Success;
        }
    }
}
