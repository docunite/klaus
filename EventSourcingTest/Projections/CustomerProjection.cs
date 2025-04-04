/*using System.Threading.Tasks;
using MongoDB.Driver;

public class CustomerProjection
{
    private readonly IMongoCollection<CustomerReadModel> _collection;

    public CustomerProjection(IMongoDatabase db)
    {
        _collection = db.GetCollection<CustomerReadModel>("customer_readmodels");
    }

    public async Task ApplyEvent(object @event)
    {
        switch (@event)
        {
            case CustomerCreated e:
                await _collection.InsertOneAsync(new CustomerReadModel
                {
                    Id = e.Id.Value.ToString(),
                    Name = e.Name.Value
                });
                break;

            case CustomerRenamed e:
                var filter = Builders<CustomerReadModel>.Filter.Eq(rm => rm.Id, e.Id.Value.ToString());
                var update = Builders<CustomerReadModel>.Update.Set(rm => rm.Name, e.NewName.Value);
                await _collection.UpdateOneAsync(filter, update);
                break;
        }
    }
}*/