using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

public class GeneralValueObjectJsonConverter<TVO> : JsonConverter<TVO>
{
    public override TVO? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDoc = JsonDocument.ParseValue(ref reader);
        var jsonElement = jsonDoc.RootElement;

        // Versuche alle Konstruktoren (public + non-public)
        var ctors = typeof(TVO).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var ctor in ctors.OrderBy(c => c.GetParameters().Length))
        {
            var parameters = ctor.GetParameters();
            var args = new object?[parameters.Length];
            var success = true;

            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                if (!jsonElement.TryGetProperty(p.Name!, out var prop))
                {
                    success = false;
                    break;
                }

                var val = prop.Deserialize(p.ParameterType, options);
                args[i] = val;
            }

            if (success)
            {
                return (TVO?)ctor.Invoke(args);
            }
        }

        throw new JsonException($"No matching constructor found for {typeof(TVO).Name}");
    }

    public override void Write(Utf8JsonWriter writer, TVO value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
