using System.Text.Json;

namespace PackagingTenderTool.Core.Services;

public sealed class CategoryMapper
{
    private const double DefaultFuzzyThreshold = 0.82;

    private readonly IReadOnlyList<CategoryMapping> mappings;
    private readonly IReadOnlyDictionary<string, string> exactLookup;

    public CategoryMapper()
        : this(LoadMappingsFromDefaultJson())
    {
    }

    public CategoryMapper(IReadOnlyList<CategoryMapping> mappings)
    {
        this.mappings = mappings ?? [];
        exactLookup = this.mappings
            .Where(m => !string.IsNullOrWhiteSpace(m.SupplierTerm) && !string.IsNullOrWhiteSpace(m.SystemCategory))
            .GroupBy(m => NormalizeKey(m.SupplierTerm))
            .ToDictionary(group => group.Key, group => group.First().SystemCategory, StringComparer.OrdinalIgnoreCase);
    }

    public CategoryMapper(IReadOnlyDictionary<string, string> mappings)
        : this(mappings
            .Where(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value))
            .Select(pair => new CategoryMapping { SupplierTerm = pair.Key, SystemCategory = pair.Value })
            .ToList())
    {
    }

    public string? MapToSystemCategory(string? supplierTerm)
    {
        if (string.IsNullOrWhiteSpace(supplierTerm))
        {
            return null;
        }

        var normalized = NormalizeKey(supplierTerm);
        if (exactLookup.TryGetValue(normalized, out var mapped))
        {
            return mapped;
        }

        return FindBestFuzzyMatch(normalized);
    }

    private string? FindBestFuzzyMatch(string normalizedSupplierTerm)
    {
        string? bestCategory = null;
        var bestScore = 0.0;

        foreach (var mapping in mappings)
        {
            if (string.IsNullOrWhiteSpace(mapping.SupplierTerm) || string.IsNullOrWhiteSpace(mapping.SystemCategory))
            {
                continue;
            }

            var candidateKey = NormalizeKey(mapping.SupplierTerm);
            var score = Similarity(normalizedSupplierTerm, candidateKey);
            if (score > bestScore)
            {
                bestScore = score;
                bestCategory = mapping.SystemCategory;
            }
        }

        return bestScore >= DefaultFuzzyThreshold ? bestCategory : null;
    }

    private static string NormalizeKey(string value)
    {
        var trimmed = value.Trim();
        var chars = new List<char>(trimmed.Length);
        var previousWasSpace = false;

        foreach (var ch in trimmed)
        {
            if (char.IsWhiteSpace(ch))
            {
                if (!previousWasSpace)
                {
                    chars.Add(' ');
                }
                previousWasSpace = true;
                continue;
            }

            chars.Add(char.ToUpperInvariant(ch));
            previousWasSpace = false;
        }

        return new string(chars.ToArray());
    }

    private static double Similarity(string a, string b)
    {
        if (a.Length == 0 && b.Length == 0) return 1.0;
        if (a.Length == 0 || b.Length == 0) return 0.0;

        var distance = LevenshteinDistance(a, b);
        var maxLen = Math.Max(a.Length, b.Length);
        return 1.0 - (double)distance / maxLen;
    }

    private static int LevenshteinDistance(string a, string b)
    {
        var n = a.Length;
        var m = b.Length;
        var d = new int[n + 1, m + 1];

        for (var i = 0; i <= n; i++) d[i, 0] = i;
        for (var j = 0; j <= m; j++) d[0, j] = j;

        for (var i = 1; i <= n; i++)
        {
            for (var j = 1; j <= m; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[n, m];
    }

    private static IReadOnlyList<CategoryMapping> LoadMappingsFromDefaultJson()
    {
        try
        {
            var path = FindSettingsFilePath("epr-settings.json");
            if (path is null)
            {
                return [];
            }

            var json = File.ReadAllText(path);
            var settings = JsonSerializer.Deserialize<EprSettings>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (settings?.Mappings is null || settings.Mappings.Count == 0)
            {
                return [];
            }

            return settings.Mappings;
        }
        catch
        {
            return [];
        }
    }

    private static string? FindSettingsFilePath(string fileName)
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        for (var depth = 0; depth < 6 && current is not null; depth++)
        {
            var candidate = Path.Combine(current.FullName, fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            current = current.Parent;
        }

        return null;
    }

    private sealed class EprSettings
    {
        public List<CategoryMapping> Mappings { get; set; } = [];
    }
}

public sealed class CategoryMapping
{
    public string SupplierTerm { get; set; } = string.Empty;

    public string SystemCategory { get; set; } = string.Empty;
}

