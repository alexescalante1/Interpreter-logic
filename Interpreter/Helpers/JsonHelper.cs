using System.Text.Json;

namespace Interpreter.Helpers
{
    internal static class JsonHelper
    {
        public static object? Parse(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return ConvertElement(doc.RootElement);
        }

        public static string Stringify(object? value)
        {
            return JsonSerializer.Serialize(value);
        }

        private static object? ConvertElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var dict = new Dictionary<string, object?>(element.GetRawText().Length / 16);
                    foreach (var property in element.EnumerateObject())
                    {
                        dict[property.Name] = ConvertElement(property.Value);
                    }
                    return dict;
                case JsonValueKind.Array:
                    var list = new List<object?>();
                    foreach (var item in element.EnumerateArray())
                    {
                        list.Add(ConvertElement(item));
                    }
                    return list;
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    if (element.TryGetInt32(out var i32)) return i32;
                    if (element.TryGetInt64(out var i64) && i64 <= int.MaxValue && i64 >= int.MinValue) return (int)i64;
                    if (element.TryGetDouble(out var d)) return d;
                    return element.GetRawText();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                default:
                    return null;
            }
        }
    }
}
