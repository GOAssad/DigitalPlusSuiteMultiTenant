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

    // Identidad
    [MaxLength(500)]
    public string? PaginaWeb { get; set; }

    [MaxLength(500)]
    public string? Facebook { get; set; }

    [MaxLength(500)]
    public string? Instagram { get; set; }

    [MaxLength(500)]
    public string? LinkedIn { get; set; }

    [MaxLength(500)]
    public string? Twitter { get; set; }

    [MaxLength(500)]
    public string? YouTube { get; set; }

    [MaxLength(500)]
    public string? TikTok { get; set; }

    // Portal web de la empresa (ej: https://digitalplusportalmt.azurewebsites.net/)
    [MaxLength(500)]
    public string? UrlPortal { get; set; }

    // Logo
    public byte[]? Logo { get; set; }

    [MaxLength(100)]
    public string? LogoContentType { get; set; }

    [MaxLength(50)]
    public string? CodigoActivacion { get; set; }

    [MaxLength(50)]
    public string Estado { get; set; } = "activa"; // activa, suspendida, baja

    // Módulos opcionales
    public bool MobileHabilitado { get; set; }
    public bool KioskoHabilitado { get; set; }

    // Lemon Squeezy
    [MaxLength(50)]
    public string? LsqCustomerId { get; set; }

    [MaxLength(50)]
    public string? LsqSubscriptionId { get; set; }

    [MaxLength(50)]
    public string? LsqVariantId { get; set; }

    [MaxLength(500)]
    public string? LsqUpdatePaymentUrl { get; set; }

    [MaxLength(500)]
    public string? LsqCustomerPortalUrl { get; set; }

    public DateTime? PlanVencimiento { get; set; }

    [MaxLength(20)]
    public string? PlanOrigen { get; set; } = "manual";

    [MaxLength(50)]
    public string? LsqVariantIdCustom { get; set; }

    [MaxLength(20)]
    public string? LsqStatus { get; set; }

    // MercadoPago
    [MaxLength(50)]
    public string? MpCustomerId { get; set; }

    [MaxLength(50)]
    public string? MpSubscriptionId { get; set; }

    [MaxLength(20)]
    public string? MpStatus { get; set; }

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
