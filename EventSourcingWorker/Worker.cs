using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

public class Worker : BackgroundService
{
    private readonly IMongoCollection<BsonDocument> _eventCollection;
    private readonly IDocuniteObjectConverter _converter;
    private readonly CustomerProjection _projection;

    public Worker(IMongoClient mongoClient)
    {
        var db = mongoClient.GetDatabase("neo_demo");
        _eventCollection = db.GetCollection<BsonDocument>("events");

        _converter = new DocuniteObjectConverter(
            typeof(CustomerCreated).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract));

        _projection = new CustomerProjection(db);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Mongo Change Stream Listener active...");

        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
            .Match(change => change.OperationType == ChangeStreamOperationType.Insert);

        using var cursor = await _eventCollection.WatchAsync(pipeline, cancellationToken: stoppingToken);

        await cursor.ForEachAsync(async change =>
        {
            var doc = change.FullDocument;
            var typeName = doc["eventType"].AsString;
            var json = doc["payload"].ToJson();

            var evt = _converter.Deserialize(json, typeName);
            if (evt != null)
            {
                await _projection.ApplyEvent(evt);
                Log.Information($"[Worker] Applied {typeName}");
            }
        }, stoppingToken);
    }
}