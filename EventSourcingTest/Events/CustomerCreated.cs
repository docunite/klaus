using System.Text.Json.Serialization;
using EventSourcingTest.Domain;

namespace EventSourcingTest.Events;

public class CustomerCreated
{
    [JsonConstructor]
    public CustomerCreated(CustomerId id, CustomerName name)
    {
        Id = id;
        Name = name;
    }
    public CustomerId Id { get;  } = default!;
    public CustomerName Name { get;  } = default!;
}