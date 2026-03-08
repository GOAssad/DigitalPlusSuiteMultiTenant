using ClosedXML.Excel;

namespace DigitalPlusMultiTenant.Web.Helpers;

public class ExcelExportConfig
{
    public string Titulo { get; set; } = "Reporte";
    public string? NombreEmpresa { get; set; }
    public string? FechaDesde { get; set; }
    public string? FechaHasta { get; set; }
    public List<string> Filtros { get; set; } = new();
    public string[] Encabezados { get; set; } = [];
    public ExcelColFormat[]? Formatos { get; set; }
}

public enum ExcelColFormat
{
    Texto,
    Numero,
    Fecha,
    Hora,
    Decimal
}

public static class ExcelExporter
{
    public static byte[] Generar<T>(
        IEnumerable<T> items,
        ExcelExportConfig config,
        Func<T, object?[]> rowSelector)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Datos");

        var totalCols = config.Encabezados.Length;
        var row = 1;

        // --- Fila 1: Logo/marca a la izquierda + Nombre empresa a la derecha ---
        ws.Cell(row, 1).Value = "DigitalPlus";
        ws.Cell(row, 1).Style
            .Font.SetBold(true)
            .Font.SetFontSize(16)
            .Font.SetFontColor(XLColor.FromHtml("#2c3e50"))
            .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        ws.Row(row).Height = 30;

        // Nombre de la empresa arriba a la derecha
        if (!string.IsNullOrEmpty(config.NombreEmpresa))
        {
            var empresaCell = ws.Cell(row, totalCols);
            empresaCell.Value = config.NombreEmpresa;
            empresaCell.Style
                .Font.SetFontSize(9)
                .Font.SetFontColor(XLColor.FromHtml("#7f8c8d"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        }
        row++;

        // --- TITULO del reporte ---
        ws.Cell(row, 1).Value = config.Titulo;
        ws.Cell(row, 1).Style
            .Font.SetBold(true)
            .Font.SetFontSize(13)
            .Font.SetFontColor(XLColor.FromHtml("#34495e"));
        ws.Range(row, 1, row, totalCols).Merge();
        row++;

        // --- Rango de fechas ---
        if (!string.IsNullOrEmpty(config.FechaDesde) || !string.IsNullOrEmpty(config.FechaHasta))
        {
            var rango = "Periodo: ";
            if (!string.IsNullOrEmpty(config.FechaDesde) && !string.IsNullOrEmpty(config.FechaHasta))
                rango += $"{config.FechaDesde} al {config.FechaHasta}";
            else if (!string.IsNullOrEmpty(config.FechaDesde))
                rango += $"desde {config.FechaDesde}";
            else
                rango += $"hasta {config.FechaHasta}";

            ws.Cell(row, 1).Value = rango;
            ws.Cell(row, 1).Style.Font.SetItalic(true).Font.SetFontSize(10).Font.SetFontColor(XLColor.Gray);
            ws.Range(row, 1, row, totalCols).Merge();
            row++;
        }

        // --- Filtros aplicados ---
        if (config.Filtros.Any())
        {
            var filtroTexto = "Filtros: " + string.Join(" | ", config.Filtros);
            ws.Cell(row, 1).Value = filtroTexto;
            ws.Cell(row, 1).Style.Font.SetItalic(true).Font.SetFontSize(10).Font.SetFontColor(XLColor.Gray);
            ws.Range(row, 1, row, totalCols).Merge();
            row++;
        }

        // --- Fecha de generacion ---
        ws.Cell(row, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cell(row, 1).Style.Font.SetFontSize(9).Font.SetFontColor(XLColor.LightGray);
        ws.Range(row, 1, row, totalCols).Merge();
        row++;

        // --- Fila separadora ---
        row++;

        // --- ENCABEZADOS de la tabla ---
        var headerRow = row;
        for (int i = 0; i < totalCols; i++)
        {
            var cell = ws.Cell(headerRow, i + 1);
            cell.Value = config.Encabezados[i];
            cell.Style
                .Font.SetBold(true)
                .Font.SetFontColor(XLColor.White)
                .Font.SetFontSize(10)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#2c3e50"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorderColor(XLColor.FromHtml("#1a252f"));
        }
        row++;

        // --- DATOS ---
        var dataStartRow = row;
        var isEvenRow = false;
        foreach (var item in items)
        {
            var values = rowSelector(item);
            for (int i = 0; i < Math.Min(values.Length, totalCols); i++)
            {
                var cell = ws.Cell(row, i + 1);
                var val = values[i];

                if (val is DateTime dt)
                    cell.Value = dt;
                else if (val is DateOnly d)
                    cell.Value = d.ToDateTime(TimeOnly.MinValue);
                else if (val is TimeOnly t)
                    cell.Value = t.ToTimeSpan();
                else if (val is TimeSpan ts)
                    cell.Value = ts;
                else if (val is int intVal)
                    cell.Value = intVal;
                else if (val is double dblVal)
                    cell.Value = dblVal;
                else if (val is decimal decVal)
                    cell.Value = (double)decVal;
                else
                    cell.Value = val?.ToString() ?? "";

                // Formato segun tipo de columna
                if (config.Formatos != null && i < config.Formatos.Length)
                {
                    cell.Style.NumberFormat.Format = config.Formatos[i] switch
                    {
                        ExcelColFormat.Fecha => "dd/MM/yyyy",
                        ExcelColFormat.Hora => "HH:mm",
                        ExcelColFormat.Decimal => "#,##0.00",
                        ExcelColFormat.Numero => "#,##0",
                        _ => "@"
                    };
                }

                cell.Style.Font.SetFontSize(10);
                cell.Style.Border.SetBottomBorder(XLBorderStyleValues.Hair);
                cell.Style.Border.SetBottomBorderColor(XLColor.LightGray);
            }

            // Zebra striping
            if (isEvenRow)
            {
                ws.Range(row, 1, row, totalCols).Style
                    .Fill.SetBackgroundColor(XLColor.FromHtml("#f8f9fa"));
            }
            isEvenRow = !isEvenRow;
            row++;
        }

        // --- Auto-ajustar columnas ---
        ws.Columns(1, totalCols).AdjustToContents(headerRow, row - 1);

        // Ancho minimo 10, maximo 50
        for (int i = 1; i <= totalCols; i++)
        {
            var col = ws.Column(i);
            if (col.Width < 10) col.Width = 10;
            if (col.Width > 50) col.Width = 50;
        }

        // --- Freeze panes: fijar encabezados ---
        ws.SheetView.FreezeRows(headerRow);

        // --- AutoFilter en encabezados ---
        if (row > headerRow + 1)
        {
            ws.Range(headerRow, 1, row - 1, totalCols).SetAutoFilter();
        }

        // Configuracion de impresion
        ws.PageSetup.PageOrientation = totalCols > 5
            ? XLPageOrientation.Landscape
            : XLPageOrientation.Portrait;
        ws.PageSetup.FitToPages(1, 0);
        ws.PageSetup.PrintAreas.Add(1, 1, row - 1, totalCols);

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
