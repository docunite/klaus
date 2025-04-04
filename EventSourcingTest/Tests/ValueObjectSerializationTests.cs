using Xunit;
using EventSourcingTest.Domain;

namespace EventSourcingTest.Tests;

public class ValueObjectSerializationTests
{
    [Fact]
    public void Should_Serialize_And_Deserialize_Nested_ValueObject()
    {
        var original = new CreditsBalance(100, new LimitThreshold(50));
        var json = ObjectConverterEngine.Serialize(original);
        var restored = ObjectConverterEngine.Deserialize<CreditsBalance>(json);
        Assert.Equal(original.Value, restored!.Value);
        Assert.Equal(original.LimitThreshold.Value, restored.LimitThreshold.Value);
    }
}