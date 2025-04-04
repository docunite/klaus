using EventSourcingTest.Snapshots;

namespace EventSourcingTest.Domain;

public class CustomerSnapshot
{
    [Persisted]
    public CustomerId Id { get; set; }

    [Persisted]
    public CustomerName Name { get; set; }
}