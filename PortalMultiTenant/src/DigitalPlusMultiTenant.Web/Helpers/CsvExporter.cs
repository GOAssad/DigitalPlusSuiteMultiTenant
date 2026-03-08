using System.Text;

namespace DigitalPlusMultiTenant.Web.Helpers;

public static class CsvExporter
{
    public static string ToCsv<T>(IEnumerable<T> items, string[] headers, Func<T, string[]> rowSelector)
    {
        var sb = new StringBuilder();
        // BOM for Excel UTF-8 detection
        sb.Append('\uFEFF');
        sb.AppendLine(string.Join(";", headers.Select(EscapeCsv)));
        foreach (var item in items)
        {
            sb.AppendLine(string.Join(";", rowSelector(item).Select(EscapeCsv)));
        }
        return sb.ToString();
    }

    public static byte[] ToCsvBytes<T>(IEnumerable<T> items, string[] headers, Func<T, string[]> rowSelector)
    {
        return Encoding.UTF8.GetBytes(ToCsv(items, headers, rowSelector));
    }

    private static string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value.Contains(';') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}
