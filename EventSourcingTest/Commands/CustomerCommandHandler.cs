/*public class CustomerCommandHandler : CommandHandler<CustomerAggregate, CustomerSnapshot>
{
    public CustomerCommandHandler(IEventStore eventStore, ISnapshotStore<CustomerSnapshot> snapshotStore)
        : base(eventStore, snapshotStore) { }

    protected override CustomerSnapshot ToSnapshot(CustomerAggregate aggregate)
    {
        return new CustomerSnapshot
        {
            Id = aggregate.Id,
            Name = aggregate.Name
        };
    }

    protected override CustomerAggregate FromSnapshot(CustomerSnapshot snapshot, string id)
    {
        var agg = new CustomerAggregate(id);
        agg.Apply(new CustomerCreated(snapshot.Id, snapshot.Name));
        return agg;
    }

    protected override CustomerAggregate CreateEmpty(string id)
    {
        return new CustomerAggregate(id);
    }
}*/