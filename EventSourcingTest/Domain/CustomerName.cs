using EventSourcingTest.Infrastructure;

namespace EventSourcingTest.Domain;

public class CustomerName : StringValueObject
{
    public CustomerName(string value) : base(value) { }
}