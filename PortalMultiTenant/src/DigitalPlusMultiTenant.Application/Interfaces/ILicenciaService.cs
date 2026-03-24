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
    public int Valor { get; set; }
    public string Categoria { get; set; } = "";
    public string TipoVisualizacion { get; set; } = "cantidad";
    public string LabelAmigable { get; set; } = "";
    public string? Icono { get; set; }
    public int OrdenVisualizacion { get; set; }
    public bool VisibleEnComparacion { get; set; } = true;

    public string ValorFormateado => TipoVisualizacion switch
    {
        "precio" => Valor == 0 ? "Gratis" : $"${Valor:N0}",
        "cantidad" => Valor == 0 ? "Ilimitados" : Valor.ToString("N0"),
        "booleano" => Valor == 1 ? "Sí" : "No",
        _ => Valor.ToString()
    };
}

public class PlanComparacion
{
    public string Plan { get; set; } = "";
    public string PlanDisplay => Plan switch { "free" => "Free", "basic" => "Basic", "pro" => "Pro", "enterprise" => "Enterprise", _ => Plan };
    public int Orden { get; set; }
    public List<PlanParametro> Parametros { get; set; } = new();

    // Helpers para acceso rápido a valores conocidos
    public int MaxLegajos => Parametros.FirstOrDefault(p => p.Parametro == "MaxLegajos")?.Valor ?? 0;
    public int MaxSucursales => Parametros.FirstOrDefault(p => p.Parametro == "MaxSucursales")?.Valor ?? 0;
    public int MaxFichadasMes => Parametros.FirstOrDefault(p => p.Parametro == "MaxFichadasRolling30d")?.Valor ?? 0;
    public int MaxTerminalesMoviles => Parametros.FirstOrDefault(p => p.Parametro == "MaxTerminalesMoviles")?.Valor ?? 0;
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
