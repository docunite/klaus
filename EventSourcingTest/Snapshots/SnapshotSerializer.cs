using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace EventSourcingTest.Snapshots;

public static class SnapshotSerializer
{
    public static string Serialize(object aggregate)
    {
        var type = aggregate.GetType();
        var persisted = GetPersistedMembers(type);

        var dict = new Dictionary<string, object>();
        foreach (var member in persisted)
        {
            var value = GetValue(aggregate, member);
            dict[member.Name] = value;
        }

        return JsonSerializer.Serialize(dict);
    }

    public static void DeserializeInto(object target, string json)
    {
        var type = target.GetType();
        var persisted = GetPersistedMembers(type);
        var values = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)!;

        foreach (var member in persisted)
        {
            if (values.TryGetValue(member.Name, out var element))
            {
                var memberType = GetMemberType(member);
                var value = element.Deserialize(memberType);
                SetValue(target, member, value);
            }
        }
    }

    private static IEnumerable<MemberInfo> GetPersistedMembers(Type type) =>
        type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(m => m.GetCustomAttribute<PersistedAttribute>() != null &&
                        (m is FieldInfo || m is PropertyInfo { CanWrite: true }));

    private static object? GetValue(object obj, MemberInfo m) =>
        m switch
        {
            FieldInfo f => f.GetValue(obj),
            PropertyInfo p => p.GetValue(obj),
            _ => null
        };

    private static void SetValue(object obj, MemberInfo m, object? value)
    {
        switch (m)
        {
            case FieldInfo f: f.SetValue(obj, value); break;
            case PropertyInfo p: p.SetValue(obj, value); break;
        }
    }

    private static Type GetMemberType(MemberInfo m) =>
        m switch
        {
            FieldInfo f => f.FieldType,
            PropertyInfo p => p.PropertyType,
            _ => throw new InvalidOperationException()
        };
}