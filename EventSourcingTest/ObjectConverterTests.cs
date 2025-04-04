using System;
using EventSourcingTest.Converters;
using EventSourcingTest.Domain;
using EventSourcingTest.Events;
using EventSourcingTest.Snapshots;
using Xunit;

namespace EventSourcingTest;

public class SnapshotTests
{
   

    [Fact]
    public void Should_Serialize_And_Deserialize_CustomerAggregate_Using_PersistedAttribute()
    {
        // Arrange
        var customerId = new CustomerId(Guid.NewGuid());
        var customerName = new CustomerName("Testkunde");

        var aggregate = new CustomerAggregate(customerId.Value.ToString());
        aggregate.Apply(new CustomerCreated(customerId, customerName));

        // Act
        var json = SnapshotSerializer.Serialize(aggregate);
        var restored = new CustomerAggregate(customerId.Value.ToString());
        SnapshotSerializer.DeserializeInto(restored, json);

        // Assert
        Assert.Equal(customerId.Value, GetPrivateField<CustomerId>(restored, "_id").Value);
        Assert.Equal(customerName.Value, GetPrivateField<CustomerName>(restored, "_name").Value);
        
        Assert.True(aggregate.Balance.IsTheSameBalance(restored.Balance));
    }

    private static T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (T)field!.GetValue(obj)!;
    }
}
public class ObjectConverterTests
{
    [Fact]
    public void Should_Serialize_And_Deserialize_Nested_ValueObject()
    {
        var original = new CreditsBalance(100, new LimitThreshold(50));
        var json = ObjectConverterEngine.Serialize(original);
        var restored = ObjectConverterEngine.Deserialize<CreditsBalance>(json);
        Assert.Equal(original.Value, restored!.Value);
        Assert.Equal(original.TheLimit.Value, restored.TheLimit.Value);
    }
    [Fact]
    public void Should_Serialize_And_Deserialize_ValueObject_And_Identity()
    {
        var original = new CustomerSnapshot
        {
            Id = new CustomerId(Guid.NewGuid()),
            Name = new CustomerName("Alice")
        };

        var json = ObjectConverterEngine.Serialize(original);
        var deserialized = ObjectConverterEngine.Deserialize<CustomerSnapshot>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(original.Id.Value, deserialized!.Id.Value);
        Assert.Equal(original.Name.Value, deserialized.Name.Value);
    }
    
    [Fact]
    public void Should_Serialize_And_Deserialize_CustomerCreated_Event()
    {
        var original = new CustomerCreated(new CustomerId(Guid.NewGuid()), new CustomerName("Bob"));

        var json = ObjectConverterEngine.Serialize(original);
        var deserialized = ObjectConverterEngine.Deserialize<CustomerCreated>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(original.Id.Value, deserialized!.Id.Value);
        Assert.Equal(original.Name.Value, deserialized.Name.Value);
    }

    [Fact]
    public void Should_Serialize_And_Deserialize_CustomerRenamed_Event()
    {
        var original = new CustomerRenamed
        {
            Id = new CustomerId(Guid.NewGuid()),
            NewName = new CustomerName("Charlie")
        };

        var json = ObjectConverterEngine.Serialize(original);
        var deserialized = ObjectConverterEngine.Deserialize<CustomerRenamed>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(original.Id.Value, deserialized!.Id.Value);
        Assert.Equal(original.NewName.Value, deserialized.NewName.Value);
    }

}