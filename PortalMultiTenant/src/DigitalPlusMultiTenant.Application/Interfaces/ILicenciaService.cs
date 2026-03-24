namespace DigitalPlusMultiTenant.Application.Interfaces;

public class LicenciaInfo
{
    public string Plan { get; set; } = "free";
    public int MaxLegajos { get; set; } = 5;
    public int MaxSucursales { get; set; } = 1;
    public int MaxFichadasMes { get; set; } = 200;
    public int MaxTerminalesMoviles { get; set; } = 1;
    public string LicenseType { get; set; } = "active";
    public DateTime? ExpiresAt { get; set; }

    // Lemon Squeezy
    public string? PlanOrigen { get; set; }
    public DateTime? PlanVencimiento { get; set; }
    public string? LsqUpdatePaymentUrl { get; set; }
    public string? LsqCustomerPortalUrl { get; set; }

    public bool EsLemonSqueezy => PlanOrigen == "lsq";

    public bool EsIlimitado(int valor) => valor == 0;

    public string PlanDisplay => Plan switch
    {
        "free" => "Free",
        "basic" => "Basic",
        "pro" => "Pro",
        "enterprise" => "Enterprise",
        _ => Plan
    };
}

public class PlanParametro
{
    public string Parametro { get; set; } = "";
    public decimal Valor { get; set; }
    public string Categoria { get; set; } = "";
    public string TipoVisualizacion { get; set; } = "cantidad";
    public string LabelAmigable { get; set; } = "";
    public string? Icono { get; set; }
    public int OrdenVisualizacion { get; set; }
    public bool VisibleEnComparacion { get; set; } = true;

    public string ValorFormateado => TipoVisualizacion switch
    {
        "precio" => Valor == 0 ? "Gratis" : Valor == Math.Floor(Valor) ? $"USD {Valor:N0}" : $"USD {Valor:N2}",
        "cantidad" => Valor == 0 ? "Ilimitados" : Valor.ToString("N0"),
        "booleano" => Valor == 1 ? "Sí" : "No",
        _ => Valor == Math.Floor(Valor) ? Valor.ToString("N0") : Valor.ToString("N2")
    };
}

public class PlanComparacion
{
    public string Plan { get; set; } = "";
    public string PlanDisplay => Plan switch { "free" => "Free", "basic" => "Basic", "pro" => "Pro", "enterprise" => "Enterprise", _ => Plan };
    public int Orden { get; set; }
    public List<PlanParametro> Parametros { get; set; } = new();

    // Helpers para acceso rápido a valores conocidos
    public int MaxLegajos => (int)(Parametros.FirstOrDefault(p => p.Parametro == "MaxLegajos")?.Valor ?? 0);
    public int MaxSucursales => (int)(Parametros.FirstOrDefault(p => p.Parametro == "MaxSucursales")?.Valor ?? 0);
    public int MaxFichadasMes => (int)(Parametros.FirstOrDefault(p => p.Parametro == "MaxFichadasRolling30d")?.Valor ?? 0);
    public int MaxTerminalesMoviles => (int)(Parametros.FirstOrDefault(p => p.Parametro == "MaxTerminalesMoviles")?.Valor ?? 0);
    public decimal ImporteMensual => Parametros.FirstOrDefault(p => p.Parametro == "ImporteMensual")?.Valor ?? 0;
    public decimal ImporteAnual => Parametros.FirstOrDefault(p => p.Parametro == "ImporteAnual")?.Valor ?? 0;
    public List<PlanParametro> ParametrosVisibles => Parametros.Where(p => p.VisibleEnComparacion).OrderBy(p => p.OrdenVisualizacion).ToList();
}

public interface ILicenciaService
{
    Task<LicenciaInfo> GetLicenciaInfoAsync(int empresaId);
    Task<List<PlanComparacion>> GetPlanesComparacionAsync();
    Task<(bool permitido, string? mensaje)> PuedeCrearLegajoAsync(int empresaId, int legajosActuales);
    Task<(bool permitido, string? mensaje)> PuedeCrearSucursalAsync(int empresaId, int sucursalesActuales);
    Task<(bool permitido, string? mensaje)> PuedeRegistrarFichadaAsync(int empresaId, int fichadasUlt30d);
    Task<(bool permitido, string? mensaje)> PuedeRegistrarTerminalMovilAsync(int empresaId, int terminalesActivas);
    void InvalidarCache(int empresaId);
}
