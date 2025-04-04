using System;
using System.Collections.Generic;

public class CustomerAggregate : AggregateRoot
{
    public CustomerId Id { get; private set; } = default!;
    public CustomerName Name { get; private set; } = default!;

    public CustomerAggregate(string id)
    {
        Id = new CustomerId(Guid.Parse(id));
    }

    public void Create(CustomerName name)
    {
        Apply(new CustomerCreated { Id = Id, Name = name });
    }

    public void Rename(CustomerName newName)
    {
        Apply(new CustomerRenamed { Id = Id, NewName = newName });
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case CustomerCreated e:
                Id = e.Id;
                Name = e.Name;
                break;
            case CustomerRenamed e:
                Name = e.NewName;
                break;
        }
    }
}

public abstract class AggregateRoot
{
    public List<object> CollectedEvents { get; } = new();

    public void Apply(object @event)
    {
        When(@event);
        CollectedEvents.Add(@event);
    }

    protected abstract void When(object @event);
}