using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace EventSourcingTest.Converters;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ValueObjectJsonConverter<TVO> : JsonConverter<TVO>
{
    public override TVO? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeof(TVO).BaseType?.GetGenericArguments()[0];
        if (valueType == null)
            throw new JsonException($"Could not determine value type for {typeof(TVO).Name}");

        var value = JsonSerializer.Deserialize(ref reader, valueType, options);
        var ctor = typeof(TVO).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { valueType }, null);
        
        if (ctor == null)
            throw new JsonException($"No matching constructor found for type {typeof(TVO).Name}");

        return (TVO?)ctor.Invoke(new[] { value });
    }

    public override void Write(Utf8JsonWriter writer, TVO value, JsonSerializerOptions options)
    {
        var valueProperty = value.GetType().GetProperty("Value");
        if (valueProperty == null)
            throw new JsonException($"Value property not found on type {typeof(TVO).Name}");
            
        var propertyValue = valueProperty.GetValue(value);
        JsonSerializer.Serialize(writer, propertyValue, options);
    }
}