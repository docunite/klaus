using System;
using System.Text.Json;

namespace EventSourcingTest.Converters;

    public static class ObjectConverterEngine
    {
        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, obj.GetType(), JsonOptions());
        }

        public static object? Deserialize(string json, Type targetType)
        {
            return JsonSerializer.Deserialize(json, targetType, JsonOptions());
        }

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, JsonOptions());
        }

        public static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new ValueObjectJsonConverterFactory(),  
                    new IdentityJsonConverterFactory()    ,
                    new StandardTypeJsonConverterFactory()
                }
            };
        }
    }