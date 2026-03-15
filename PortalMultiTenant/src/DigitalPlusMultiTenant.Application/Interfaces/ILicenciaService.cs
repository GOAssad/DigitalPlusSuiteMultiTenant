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

public interface ILicenciaService
{
    Task<LicenciaInfo> GetLicenciaInfoAsync(int empresaId);
    Task<(bool permitido, string? mensaje)> PuedeCrearLegajoAsync(int empresaId, int legajosActuales);
    Task<(bool permitido, string? mensaje)> PuedeCrearSucursalAsync(int empresaId, int sucursalesActuales);
    Task<(bool permitido, string? mensaje)> PuedeRegistrarFichadaAsync(int empresaId, int fichadasUlt30d);
    Task<(bool permitido, string? mensaje)> PuedeRegistrarTerminalMovilAsync(int empresaId, int terminalesActivas);
    void InvalidarCache(int empresaId);
}
