using System;
using Xunit;

public class ObjectConverterTests
{
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
        var original = new CustomerCreated
        {
            Id = new CustomerId(Guid.NewGuid()),
            Name = new CustomerName("Bob")
        };

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