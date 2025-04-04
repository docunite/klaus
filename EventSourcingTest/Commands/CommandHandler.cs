using System;
using System.Linq;
using System.Threading.Tasks;

public abstract class CommandHandler<TAggregate, TSnapShot>
    where TAggregate : AggregateRoot
{
    private readonly IEventStore _eventStore;
    private readonly ISnapshotStore<TSnapShot> _snapshotStore;

    protected CommandHandler(IEventStore eventStore, ISnapshotStore<TSnapShot> snapshotStore)
    {
        _eventStore = eventStore;
        _snapshotStore = snapshotStore;
    }

    protected abstract TSnapShot ToSnapshot(TAggregate aggregate);
    protected abstract TAggregate FromSnapshot(TSnapShot snapshot, string id);
    protected abstract TAggregate CreateEmpty(string id);

    public async Task UnitOfWork(string streamId, Func<TAggregate, Task> action)
    {
        var snapshot = _snapshotStore.Get(streamId, out var version);
        var aggregate = version == -1 ? CreateEmpty(streamId) : FromSnapshot(snapshot, streamId);

        var (events, latestVersion) = await _eventStore.GetEvents(streamId, version);
        foreach (var e in events) aggregate.Apply(e);

        await action(aggregate);

        if (aggregate.CollectedEvents.Any())
            await _eventStore.SaveEvents(streamId, aggregate.CollectedEvents, ++latestVersion);

        if (events.Count > 3)
        {
            var snap = ToSnapshot(aggregate);
            await _snapshotStore.Save(snap, streamId, latestVersion);
        }
    }
}