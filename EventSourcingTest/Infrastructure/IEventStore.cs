using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcingTest.Infrastructure;

public interface IEventStore
{
    Task SaveEvents(string streamId, IReadOnlyCollection<object> events, int expectedVersion);
    Task<(List<object> Events, int Version)> GetEvents(string streamId, int fromVersion);
    bool IsVersionConflict(Exception ex);
}