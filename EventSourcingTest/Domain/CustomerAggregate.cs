using System;
using EventSourcingTest.Events;
using EventSourcingTest.Snapshots;

namespace EventSourcingTest.Domain;

public class CustomerAggregate(string id)
{
    [Persisted]
    private CustomerId _id = new(Guid.Parse(id));
    [Persisted]
    private CustomerName _name;

    [Persisted] public CreditsBalance Balance = new(100, new LimitThreshold(11));
    public void Apply(CustomerCreated @event)
    {
        _id = @event.Id;
        _name = @event.Name;
    }

  
}