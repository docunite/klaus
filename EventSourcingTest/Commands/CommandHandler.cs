/*
using System;
using System.Threading.Tasks;

public abstract class CommandHandler<TAggregate>(IDataStoreCache dataStoreCache)
    where TAggregate : RootAggregate
{
    private async void QueueSaveSnapshot(TAggregate agg, TenantName tenantName, int version, string key)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var json = SnapshotSerializer.Serialize(agg);
                await dataStoreCache.GetAggregateSnapShotStore(tenantName)
                    .SaveRaw(json, agg.Id, version);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Snapshot error: {ex.Message}");
            }
            finally
            {
                await ConcurrentOperationQueue.ReleaseLockWithDelay(key);
            }
        });
    }

    protected async Task UnitOfWork(DomainCommand command, string id, Func<TAggregate, Task> unitAction)
    {
        if (command.TenantName == null)
            throw new Exception("Tenant is not set.");

        var eventStore = dataStoreCache.GetEventStore(command.TenantName);

        await LockHelper.LockAsync(command.TenantName.Value, id, async () =>
        {
            var snapStore = dataStoreCache.GetAggregateSnapShotStore(command.TenantName);
            var raw = snapStore.GetRaw<TAggregate>(id, out var version);

            TAggregate aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), id)!;

            if (raw is not null)
                SnapshotSerializer.DeserializeInto(aggregate, raw);

            var (events, currentVersion) = await eventStore.GetEvents(id, version);
            if (events.Any())
            {
                new OnDemandRehydrator(aggregate).Rehydrate(events);
                version = currentVersion;
            }

            await unitAction(aggregate);

            if (aggregate.CollectedEvents.Count > 0)
                await eventStore.SaveEvents(aggregate.Id, aggregate.CollectedEvents, ++version);

            var key = $"aggregate-{command.TenantName}_{typeof(TAggregate).FullName}_{id}";
            if (events.Count > 5 && !ConcurrentOperationQueue.TryAcquireLock(key))
                QueueSaveSnapshot(aggregate, command.TenantName, version, key);
        });
    }
}
*/
