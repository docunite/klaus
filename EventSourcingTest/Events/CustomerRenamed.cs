using EventSourcingTest.Domain;

namespace EventSourcingTest.Events;

public class CustomerRenamed
{
    public CustomerId Id { get; set; } = default!;
    public CustomerName NewName { get; set; } = default!;
}