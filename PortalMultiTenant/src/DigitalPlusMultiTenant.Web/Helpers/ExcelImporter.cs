using ClosedXML.Excel;
using DigitalPlusMultiTenant.Domain.Entities;

namespace DigitalPlusMultiTenant.Web.Helpers;

public class ImportRow
{
    public int Fila { get; set; }
    public string NumeroLegajo { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string SectorNombre { get; set; } = "";
    public string CategoriaNombre { get; set; } = "";
    public string? HorarioNombre { get; set; }
    public string SucursalesTexto { get; set; } = "";
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public DateOnly? FechaIngreso { get; set; }

    // Resolved IDs
    public int? SectorId { get; set; }
    public int? CategoriaId { get; set; }
    public int? HorarioId { get; set; }
    public List<int> SucursalIds { get; set; } = [];

    // Validation
    public bool IsValid => Errores.Count == 0;
    public List<string> Errores { get; set; } = [];
}

public static class ExcelImporter
{
    /// <summary>
    /// Busca un item por codigo o nombre (case-insensitive).
    /// Primero intenta matchear por codigo, si no encuentra, por nombre.
    /// </summary>
    private static int? ResolverId<T>(string valor, Dictionary<string, int> porCodigo, Dictionary<string, int> porNombre)
    {
        var key = valor.Trim().ToLowerInvariant();
        if (porCodigo.TryGetValue(key, out var id)) return id;
        if (porNombre.TryGetValue(key, out id)) return id;
        return null;
    }

    public static List<ImportRow> ParsearExcel(
        byte[] fileBytes,
        List<Sector> sectores,
        List<Categoria> categorias,
        List<Horario> horarios,
        List<Sucursal> sucursales,
        HashSet<string> legajosExistentes)
    {
        var rows = new List<ImportRow>();

        using var stream = new MemoryStream(fileBytes);
        using var wb = new XLWorkbook(stream);

        var ws = wb.Worksheets.First();
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
        if (lastRow < 2) return rows; // Solo header o vacío

        // Detectar fila de encabezados
        var headerRow = 1;
        for (int r = 1; r <= Math.Min(5, lastRow); r++)
        {
            var val = ws.Cell(r, 1).GetString().Trim().ToLowerInvariant();
            if (val == "numerolegajo" || val == "legajo" || val == "nro legajo" || val == "nro. legajo")
            {
                headerRow = r;
                break;
            }
        }

        // Build lookup dictionaries (case-insensitive) — por codigo y por nombre
        var sectorPorCodigo = sectores.Where(s => !string.IsNullOrEmpty(s.Codigo)).ToDictionary(s => s.Codigo.Trim().ToLowerInvariant(), s => s.Id);
        var sectorPorNombre = sectores.ToDictionary(s => s.Nombre.Trim().ToLowerInvariant(), s => s.Id);
        var catPorCodigo = categorias.Where(c => !string.IsNullOrEmpty(c.Codigo)).ToDictionary(c => c.Codigo.Trim().ToLowerInvariant(), c => c.Id);
        var catPorNombre = categorias.ToDictionary(c => c.Nombre.Trim().ToLowerInvariant(), c => c.Id);
        var horPorCodigo = horarios.Where(h => !string.IsNullOrEmpty(h.Codigo)).ToDictionary(h => h.Codigo.Trim().ToLowerInvariant(), h => h.Id);
        var horPorNombre = horarios.ToDictionary(h => h.Nombre.Trim().ToLowerInvariant(), h => h.Id);
        var sucPorCodigo = sucursales.Where(s => !string.IsNullOrEmpty(s.Codigo)).ToDictionary(s => s.Codigo.Trim().ToLowerInvariant(), s => s.Id);
        var sucPorNombre = sucursales.ToDictionary(s => s.Nombre.Trim().ToLowerInvariant(), s => s.Id);

        for (int r = headerRow + 1; r <= lastRow; r++)
        {
            var legajo = ws.Cell(r, 1).GetString().Trim();
            if (string.IsNullOrEmpty(legajo)) continue; // Skip empty rows

            var row = new ImportRow
            {
                Fila = r,
                NumeroLegajo = legajo,
                Apellido = ws.Cell(r, 2).GetString().Trim(),
                Nombre = ws.Cell(r, 3).GetString().Trim(),
                SectorNombre = ws.Cell(r, 4).GetString().Trim(),
                CategoriaNombre = ws.Cell(r, 5).GetString().Trim(),
                HorarioNombre = ws.Cell(r, 6).GetString().Trim(),
                SucursalesTexto = ws.Cell(r, 7).GetString().Trim(),
                Email = ws.Cell(r, 8).GetString().Trim(),
                Telefono = ws.Cell(r, 9).GetString().Trim()
            };

            // Parse FechaIngreso (col 10)
            var fechaCell = ws.Cell(r, 10);
            if (!fechaCell.IsEmpty())
            {
                if (fechaCell.DataType == XLDataType.DateTime)
                    row.FechaIngreso = DateOnly.FromDateTime(fechaCell.GetDateTime());
                else if (DateOnly.TryParse(fechaCell.GetString().Trim(), out var fd))
                    row.FechaIngreso = fd;
            }

            // Nullify empty optional strings
            if (string.IsNullOrEmpty(row.HorarioNombre)) row.HorarioNombre = null;
            if (string.IsNullOrEmpty(row.Email)) row.Email = null;
            if (string.IsNullOrEmpty(row.Telefono)) row.Telefono = null;

            // --- Validations ---
            if (string.IsNullOrEmpty(row.NumeroLegajo))
                row.Errores.Add("NumeroLegajo vacío");
            else if (legajosExistentes.Contains(row.NumeroLegajo))
                row.Errores.Add("Legajo ya existe");

            if (string.IsNullOrEmpty(row.Apellido))
                row.Errores.Add("Apellido vacío");
            if (string.IsNullOrEmpty(row.Nombre))
                row.Errores.Add("Nombre vacío");

            // Sector (por codigo o nombre)
            if (string.IsNullOrEmpty(row.SectorNombre))
                row.Errores.Add("Sector vacío");
            else
            {
                var id = ResolverId<Sector>(row.SectorNombre, sectorPorCodigo, sectorPorNombre);
                if (id.HasValue)
                    row.SectorId = id.Value;
                else
                    row.Errores.Add($"Sector '{row.SectorNombre}' no existe");
            }

            // Categoria (por codigo o nombre)
            if (string.IsNullOrEmpty(row.CategoriaNombre))
                row.Errores.Add("Categoría vacía");
            else
            {
                var id = ResolverId<Categoria>(row.CategoriaNombre, catPorCodigo, catPorNombre);
                if (id.HasValue)
                    row.CategoriaId = id.Value;
                else
                    row.Errores.Add($"Categoría '{row.CategoriaNombre}' no existe");
            }

            // Horario (optional, por codigo o nombre)
            if (row.HorarioNombre != null)
            {
                var id = ResolverId<Horario>(row.HorarioNombre, horPorCodigo, horPorNombre);
                if (id.HasValue)
                    row.HorarioId = id.Value;
                else
                    row.Errores.Add($"Horario '{row.HorarioNombre}' no existe");
            }

            // Sucursales (required, separated by ;, por codigo o nombre)
            if (string.IsNullOrEmpty(row.SucursalesTexto))
                row.Errores.Add("Sucursal vacía (mínimo 1)");
            else
            {
                var valores = row.SucursalesTexto.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var val in valores)
                {
                    var id = ResolverId<Sucursal>(val, sucPorCodigo, sucPorNombre);
                    if (id.HasValue)
                        row.SucursalIds.Add(id.Value);
                    else
                        row.Errores.Add($"Sucursal '{val}' no existe");
                }
                if (row.SucursalIds.Count == 0 && row.Errores.All(e => !e.StartsWith("Sucursal '")))
                    row.Errores.Add("Sucursal vacía (mínimo 1)");
            }

            // Check duplicate NumeroLegajo within the file
            if (rows.Any(existing => existing.NumeroLegajo == row.NumeroLegajo))
                row.Errores.Add("NumeroLegajo duplicado en el archivo");

            rows.Add(row);
        }

        return rows;
    }

    public static byte[] GenerarPlantilla(
        List<Sector> sectores,
        List<Categoria> categorias,
        List<Horario> horarios,
        List<Sucursal> sucursales)
    {
        using var wb = new XLWorkbook();

        // --- Hoja 1: Datos ---
        var ws = wb.Worksheets.Add("Legajos");
        var headers = new[] { "NumeroLegajo", "Apellido", "Nombre", "Sector", "Categoria", "Horario", "Sucursales", "Email", "Telefono", "FechaIngreso" };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style
                .Font.SetBold(true)
                .Font.SetFontColor(XLColor.White)
                .Font.SetFontSize(10)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#2c3e50"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }

        // Example row — usar codigo del primer item
        var ejemplo = new[] {
            "001", "García", "Juan",
            sectores.FirstOrDefault()?.Codigo ?? "S001",
            categorias.FirstOrDefault()?.Codigo ?? "C001",
            horarios.FirstOrDefault()?.Codigo ?? "",
            sucursales.FirstOrDefault()?.Codigo ?? "0001",
            "juan@ejemplo.com", "1155551234", "01/03/2026"
        };
        for (int i = 0; i < ejemplo.Length; i++)
        {
            var cell = ws.Cell(2, i + 1);
            cell.Value = ejemplo[i];
            cell.Style.Font.SetFontColor(XLColor.Gray).Font.SetItalic(true);
        }

        // Notes row
        var notas = new[] {
            "Requerido, único", "Requerido", "Requerido",
            "Codigo o nombre (ver lista)", "Codigo o nombre (ver lista)", "Codigo o nombre (ver lista)",
            "Codigo o nombre; separar con ;", "Opcional", "Opcional", "Opcional (dd/mm/aaaa)"
        };
        for (int i = 0; i < notas.Length; i++)
        {
            var cell = ws.Cell(3, i + 1);
            cell.Value = notas[i];
            cell.Style.Font.SetFontSize(8).Font.SetFontColor(XLColor.FromHtml("#95a5a6")).Font.SetItalic(true);
        }

        ws.Columns(1, headers.Length).AdjustToContents();
        for (int i = 1; i <= headers.Length; i++)
        {
            if (ws.Column(i).Width < 14) ws.Column(i).Width = 14;
        }
        ws.SheetView.FreezeRows(1);

        // --- Hoja 2: Valores válidos (Codigo | Nombre) ---
        var ref_ = wb.Worksheets.Add("Valores válidos");
        int col = 1;

        void WriteRefSection(string titulo, string color, IEnumerable<(string Codigo, string Nombre)> items)
        {
            // Header: Codigo
            ref_.Cell(1, col).Value = titulo + " (Cod)";
            ref_.Cell(1, col).Style.Font.SetBold(true).Fill.SetBackgroundColor(XLColor.FromHtml(color)).Font.SetFontColor(XLColor.White);
            // Header: Nombre
            ref_.Cell(1, col + 1).Value = titulo + " (Nombre)";
            ref_.Cell(1, col + 1).Style.Font.SetBold(true).Fill.SetBackgroundColor(XLColor.FromHtml(color)).Font.SetFontColor(XLColor.White);

            int row = 2;
            foreach (var item in items)
            {
                ref_.Cell(row, col).Value = item.Codigo;
                ref_.Cell(row, col + 1).Value = item.Nombre;
                row++;
            }
            col += 2;
        }

        // Separador visual entre secciones (columna vacia)
        WriteRefSection("Sectores", "#3498db", sectores.Select(s => (s.Codigo ?? "", s.Nombre)));
        col++; // columna separadora
        WriteRefSection("Categorías", "#2ecc71", categorias.Select(c => (c.Codigo ?? "", c.Nombre)));
        col++;
        WriteRefSection("Horarios", "#e67e22", horarios.Select(h => (h.Codigo ?? "", h.Nombre)));
        col++;
        WriteRefSection("Sucursales", "#9b59b6", sucursales.Select(s => (s.Codigo ?? "", s.Nombre)));

        ref_.Columns(1, col).AdjustToContents();
        for (int i = 1; i <= col; i++)
        {
            if (ref_.Column(i).Width < 12) ref_.Column(i).Width = 12;
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
