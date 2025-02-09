using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AuthLab.Api.Extensions;

public static class SerializationExtensions
{
    public static string Serialize<T>(this T obj, JsonSerializerSettings? settings = null)
    {
        settings ??= new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };
        return JsonConvert.SerializeObject(obj, settings);
    }

    public static T? Deserialize<T>(this string json, JsonSerializerSettings? settings = null)
    {
        settings ??= new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
}
