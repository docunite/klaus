using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace EventSourcingTest.Converters;

public class GeneralValueObjectJsonConverter<TVO> : JsonConverter<TVO>
{
    public override TVO? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        var ctors = typeof(TVO).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var ctor in ctors.OrderBy(c => c.GetParameters().Length))
        {
            var parameters = ctor.GetParameters();
            var args = new object?[parameters.Length];
            var success = true;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (!root.TryGetProperty(parameters[i].Name!, out var prop))
                {
                    success = false;
                    break;
                }

                args[i] = prop.Deserialize(parameters[i].ParameterType, options);
            }

            if (success)
                return (TVO?)ctor.Invoke(args);
        }

        throw new JsonException($"No matching constructor for type {typeof(TVO).Name}");
    }

    public override void Write(Utf8JsonWriter writer, TVO value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}