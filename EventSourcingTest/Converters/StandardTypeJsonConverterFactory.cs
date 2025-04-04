using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventSourcingTest.Converters;

public class StandardTypeJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsPrimitive ||
               typeToConvert == typeof(string) ||
               typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(StandardTypeJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}