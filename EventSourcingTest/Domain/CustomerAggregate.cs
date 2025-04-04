using System;

public class CustomerAggregate 
{
    [Persisted]
    private CustomerId _id;
    [Persisted]
    private CustomerName _name;

    public CustomerAggregate(string id)
    {
        _id = new CustomerId(Guid.Parse(id));
    }

    public void Apply(CustomerCreated @event)
    {
        _id = @event.Id;
        _name = @event.Name;
    }

  
}