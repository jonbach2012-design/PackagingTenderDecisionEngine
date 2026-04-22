using System.Text.Json;
using System.Text.Json.Serialization;
using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services;

public sealed class TenderStorageService
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public void Save(TenderProject project, string filePath)
    {
        ArgumentNullException.ThrowIfNull(project);
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is required.", nameof(filePath));

        var json = JsonSerializer.Serialize(project, SerializerOptions);
        File.WriteAllText(filePath, json);
    }

    public TenderProject Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is required.", nameof(filePath));
        var json = File.ReadAllText(filePath);
        var project = JsonSerializer.Deserialize<TenderProject>(json, SerializerOptions);
        return project ?? new TenderProject();
    }
}

