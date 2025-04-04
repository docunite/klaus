using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcingTest.Converters;

namespace EventSourcingTest.Infrastructure;

public class DocuniteObjectConverter : IDocuniteObjectConverter
{
    private readonly Dictionary<string, Type> _knownTypes;

    public DocuniteObjectConverter(IEnumerable<Type> knownTypes)
    {
        _knownTypes = knownTypes.ToDictionary(t => t.Name, t => t);
    }

    public string Serialize(object obj)
    {
        return ObjectConverterEngine.Serialize(obj);
    }

    public object? Deserialize(string json, string eventTypeName)
    {
        if (_knownTypes.TryGetValue(eventTypeName, out var type))
        {
            return ObjectConverterEngine.Deserialize(json, type);
        }
        return null;
    }

    public T Deserialize<T>(string json)
    {
        return ObjectConverterEngine.Deserialize<T>(json)!;
    }
}