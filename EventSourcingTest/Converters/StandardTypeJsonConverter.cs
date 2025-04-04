using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventSourcingTest.Converters
{
    public class StandardTypeJsonConverter<T> : JsonConverter<T>
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (typeToConvert.IsEnum)
                {
                    var str = reader.GetString();
                    if (str != null && Enum.TryParse(typeToConvert, str, ignoreCase: true, out var result) && Enum.IsDefined(typeToConvert, result))
                    {
                        return (T)result!;
                    }
                    throw new JsonException($"Invalid enum value '{str}' for type '{typeToConvert.Name}'");
                }

                if (typeToConvert == typeof(string))
                {
                    return (T)(object)(reader.GetString() ?? "");
                }

                if (typeToConvert == typeof(int))
                {
                    return (T)(object)reader.GetInt32();
                }

                if (typeToConvert == typeof(long))
                {
                    return (T)(object)reader.GetInt64();
                }

                if (typeToConvert == typeof(double))
                {
                    return (T)(object)reader.GetDouble();
                }

                if (typeToConvert == typeof(DateTime))
                {
                    return (T)(object)reader.GetDateTime();
                }

                var raw = JsonSerializer.Deserialize(ref reader, typeToConvert, options);
                return (T?)raw;
            }
            catch (Exception ex)
            {
                throw new JsonException($"Failed to deserialize type '{typeToConvert.Name}'", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            var type = value.GetType();
            if (type.IsEnum)
            {
                writer.WriteStringValue(value.ToString());
            }
            else if (type == typeof(string))
            {
                writer.WriteStringValue((string)(object)value);
            }
            else if (type == typeof(int))
            {
                writer.WriteNumberValue((int)(object)value);
            }
            else if (type == typeof(long))
            {
                writer.WriteNumberValue((long)(object)value);
            }
            else if (type == typeof(double))
            {
                writer.WriteNumberValue((double)(object)value);
            }
            else if (type == typeof(DateTime))
            {
                writer.WriteStringValue(((DateTime)(object)value).ToString("O"));
            }
            else
            {
                JsonSerializer.Serialize(writer, value, type, options);
            }
        }
    }
}
