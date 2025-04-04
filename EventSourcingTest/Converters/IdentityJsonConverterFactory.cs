using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventSourcingTest.Converters;

public class IdentityJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.BaseType is { IsGenericType: true } &&
               typeToConvert.BaseType.GetGenericTypeDefinition() == typeof(Identity<>);
    }

    public override JsonConverter? CreateConverter(Type type, JsonSerializerOptions options)
    {
        var idType = type.BaseType!.GetGenericArguments()[0];
        var converterType = typeof(IdentityJsonConverter<,>).MakeGenericType(type, idType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }

    private class IdentityJsonConverter<TIdentity, TId> : JsonConverter<TIdentity>
        where TIdentity : Identity<TId>
    {
        public override TIdentity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = JsonSerializer.Deserialize<TId>(ref reader, options);
            var constructor = typeof(TIdentity).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { typeof(TId) }, null);
            return (TIdentity?)constructor?.Invoke(new object[] { value! });
        }

        public override void Write(Utf8JsonWriter writer, TIdentity value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}