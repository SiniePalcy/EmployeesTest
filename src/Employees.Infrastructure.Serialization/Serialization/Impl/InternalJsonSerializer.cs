using System.Text.Json;

namespace Employees.Infrastructure.Serialization.Impl;

internal class InternalJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        IncludeFields = true,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = true,
    };

    public T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _options);

    public string? Serialize<T>(T? obj) => JsonSerializer.Serialize(obj, _options);
}
