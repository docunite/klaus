/*using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

public class EventStreamListener
{
    private readonly IMongoCollection<BsonDocument> _collection;
    private readonly IDocuniteObjectConverter _converter;
    private readonly CustomerProjection _projection;

    public EventStreamListener(IMongoDatabase db, IDocuniteObjectConverter converter, CustomerProjection projection)
    {
        _collection = db.GetCollection<BsonDocument>("events");
        _converter = converter;
        _projection = projection;
    }

    public async Task ListenAsync(CancellationToken cancellationToken = default)
    {
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
            .Match(change => change.OperationType == ChangeStreamOperationType.Insert);

        using var cursor = await _collection.WatchAsync(pipeline, cancellationToken: cancellationToken);

        await cursor.ForEachAsync(async change =>
        {
            var doc = change.FullDocument;
            var typeName = doc["eventType"].AsString;
            var json = doc["payload"].ToJson();

            var evt = _converter.Deserialize(json, typeName);
            if (evt != null)
            {
                await _projection.ApplyEvent(evt);
                Console.WriteLine($"[Projection] Applied {typeName}");
            }
        }, cancellationToken);
    }
}*/