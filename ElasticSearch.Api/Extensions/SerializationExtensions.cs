using System.Text.Json;

namespace ElasticSearch.Api.Extensions;

public static class SerializationExtensions
{
    public static string Serialize<T>(this T obj, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(obj, options ?? new JsonSerializerOptions { WriteIndented = true });
    }
}
