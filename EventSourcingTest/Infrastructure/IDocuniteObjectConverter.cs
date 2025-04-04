namespace EventSourcingTest.Infrastructure;

public interface IDocuniteObjectConverter
{
    string Serialize(object obj);
    object? Deserialize(string json, string eventTypeName);
    T Deserialize<T>(string json);
}