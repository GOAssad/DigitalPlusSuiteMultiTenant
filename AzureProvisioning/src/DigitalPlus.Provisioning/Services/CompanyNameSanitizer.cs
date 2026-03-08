using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DigitalPlus.Provisioning.Services;

public class CompanyNameSanitizer
{
    private const int MaxLength = 50;
    private const string DbPrefix = "DP_";

    public (string sanitized, string dbName) Sanitize(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be empty.");

        var result = companyName.Trim();

        result = RemoveDiacritics(result);

        result = Regex.Replace(result, @"[^a-zA-Z0-9]", "");

        if (result.Length > MaxLength)
            result = result[..MaxLength];

        if (string.IsNullOrEmpty(result))
            throw new ArgumentException("Company name results in empty string after sanitization.");

        var dbName = DbPrefix + result;

        return (result, dbName);
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
