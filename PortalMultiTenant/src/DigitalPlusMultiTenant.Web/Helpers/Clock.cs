namespace DigitalPlusMultiTenant.Web.Helpers;

/// <summary>
/// Reloj centralizado que devuelve fecha/hora de Argentina (UTC-3).
/// Usar en lugar de DateTime.Now / DateTime.Today para que funcione
/// correctamente en servidores Azure fuera de Argentina.
/// </summary>
public static class Clock
{
    private static readonly TimeZoneInfo _tz =
        TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows()
                ? "Argentina Standard Time"
                : "America/Argentina/Buenos_Aires");

    /// <summary>Hora actual en Argentina.</summary>
    public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _tz);

    /// <summary>Fecha actual en Argentina (sin hora).</summary>
    public static DateTime Today => Now.Date;

    /// <summary>Fecha actual en Argentina como DateOnly.</summary>
    public static DateOnly TodayDate => DateOnly.FromDateTime(Now);
}
