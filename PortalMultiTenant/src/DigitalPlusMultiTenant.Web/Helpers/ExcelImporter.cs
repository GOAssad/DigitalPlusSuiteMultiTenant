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

        // Build lookup dictionaries (case-insensitive)
        var sectorDict = sectores.ToDictionary(s => s.Nombre.Trim().ToLowerInvariant(), s => s.Id);
        var catDict = categorias.ToDictionary(c => c.Nombre.Trim().ToLowerInvariant(), c => c.Id);
        var horDict = horarios.ToDictionary(h => h.Nombre.Trim().ToLowerInvariant(), h => h.Id);
        var sucDict = sucursales.ToDictionary(s => s.Nombre.Trim().ToLowerInvariant(), s => s.Id);

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

            // Sector
            if (string.IsNullOrEmpty(row.SectorNombre))
                row.Errores.Add("Sector vacío");
            else if (sectorDict.TryGetValue(row.SectorNombre.ToLowerInvariant(), out var sid))
                row.SectorId = sid;
            else
                row.Errores.Add($"Sector '{row.SectorNombre}' no existe");

            // Categoria
            if (string.IsNullOrEmpty(row.CategoriaNombre))
                row.Errores.Add("Categoría vacía");
            else if (catDict.TryGetValue(row.CategoriaNombre.ToLowerInvariant(), out var cid))
                row.CategoriaId = cid;
            else
                row.Errores.Add($"Categoría '{row.CategoriaNombre}' no existe");

            // Horario (optional)
            if (row.HorarioNombre != null)
            {
                if (horDict.TryGetValue(row.HorarioNombre.ToLowerInvariant(), out var hid))
                    row.HorarioId = hid;
                else
                    row.Errores.Add($"Horario '{row.HorarioNombre}' no existe");
            }

            // Sucursales (required, separated by ;)
            if (string.IsNullOrEmpty(row.SucursalesTexto))
                row.Errores.Add("Sucursal vacía (mínimo 1)");
            else
            {
                var nombres = row.SucursalesTexto.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var sn in nombres)
                {
                    if (sucDict.TryGetValue(sn.ToLowerInvariant(), out var sucId))
                        row.SucursalIds.Add(sucId);
                    else
                        row.Errores.Add($"Sucursal '{sn}' no existe");
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

        // Example row
        var ejemplo = new[] {
            "001", "García", "Juan",
            sectores.FirstOrDefault()?.Nombre ?? "Administración",
            categorias.FirstOrDefault()?.Nombre ?? "Empleado",
            horarios.FirstOrDefault()?.Nombre ?? "",
            sucursales.FirstOrDefault()?.Nombre ?? "Central",
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
            "Requerido (de lista)", "Requerido (de lista)", "Opcional (de lista)",
            "Requerido; separar con ;", "Opcional", "Opcional", "Opcional (dd/mm/aaaa)"
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

        // --- Hoja 2: Valores válidos ---
        var ref_ = wb.Worksheets.Add("Valores válidos");
        int col = 1;

        // Sectores
        ref_.Cell(1, col).Value = "Sectores";
        ref_.Cell(1, col).Style.Font.SetBold(true).Fill.SetBackgroundColor(XLColor.FromHtml("#3498db")).Font.SetFontColor(XLColor.White);
        for (int i = 0; i < sectores.Count; i++)
            ref_.Cell(i + 2, col).Value = sectores[i].Nombre;
        col++;

        // Categorías
        ref_.Cell(1, col).Value = "Categorías";
        ref_.Cell(1, col).Style.Font.SetBold(true).Fill.SetBackgroundColor(XLColor.FromHtml("#2ecc71")).Font.SetFontColor(XLColor.White);
        for (int i = 0; i < categorias.Count; i++)
            ref_.Cell(i + 2, col).Value = categorias[i].Nombre;
        col++;

        // Horarios
        ref_.Cell(1, col).Value = "Horarios";
        ref_.Cell(1, col).Style.Font.SetBold(true).Fill.SetBackgroundColor(XLColor.FromHtml("#e67e22")).Font.SetFontColor(XLColor.White);
        for (int i = 0; i < horarios.Count; i++)
            ref_.Cell(i + 2, col).Value = horarios[i].Nombre;
        col++;

        // Sucursales
        ref_.Cell(1, col).Value = "Sucursales";
        ref_.Cell(1, col).Style.Font.SetBold(true).Fill.SetBackgroundColor(XLColor.FromHtml("#9b59b6")).Font.SetFontColor(XLColor.White);
        for (int i = 0; i < sucursales.Count; i++)
            ref_.Cell(i + 2, col).Value = sucursales[i].Nombre;

        ref_.Columns(1, col).AdjustToContents();
        for (int i = 1; i <= col; i++)
        {
            if (ref_.Column(i).Width < 18) ref_.Column(i).Width = 18;
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
