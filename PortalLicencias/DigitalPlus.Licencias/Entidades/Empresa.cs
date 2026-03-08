using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("Empresas")]
public class Empresa
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(100)]
    public string CompanyId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string DatabaseName { get; set; } = string.Empty;

    // Datos fiscales
    public int? PaisId { get; set; }
    public int? TipoIdentificacionFiscalId { get; set; }

    [MaxLength(50)]
    public string? IdentificacionFiscal { get; set; }

    [MaxLength(200)]
    public string? RazonSocial { get; set; }

    [MaxLength(500)]
    public string? DomicilioFiscal { get; set; }

    // Contacto
    [MaxLength(100)]
    public string? Contacto { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Telefono { get; set; }

    [MaxLength(500)]
    public string? Direccion { get; set; }

    public string? Notas { get; set; }

    // Logo
    public byte[]? Logo { get; set; }

    [MaxLength(100)]
    public string? LogoContentType { get; set; }

    [MaxLength(50)]
    public string? CodigoActivacion { get; set; }

    [MaxLength(50)]
    public string Estado { get; set; } = "activa"; // activa, suspendida, baja

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navegacion
    [ForeignKey(nameof(PaisId))]
    public Pais? Pais { get; set; }

    [ForeignKey(nameof(TipoIdentificacionFiscalId))]
    public TipoIdentificacionFiscal? TipoIdentificacionFiscal { get; set; }

    [NotMapped]
    public string EstadoDisplay => Estado switch
    {
        "activa" => "Activa",
        "suspendida" => "Suspendida",
        "baja" => "Baja",
        _ => Estado
    };
}
