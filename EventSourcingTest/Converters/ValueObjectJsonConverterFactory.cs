using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventSourcingTest.Converters;

public class ValueObjectJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.BaseType is { IsGenericType: true } &&
               typeToConvert.BaseType.GetGenericTypeDefinition() == typeof(ValueObject<>);
    }

    public override JsonConverter? CreateConverter(Type type, JsonSerializerOptions options)
    {
        var converterType = typeof(GeneralValueObjectJsonConverter<>).MakeGenericType(type);
        return Activator.CreateInstance(converterType) as JsonConverter;
    }
}