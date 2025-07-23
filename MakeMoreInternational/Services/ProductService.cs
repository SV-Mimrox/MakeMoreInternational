using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class ProductService
{
    private readonly IMongoCollection<ProductMaster> _collection;

    public ProductService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _collection = db.GetCollection<ProductMaster>("ProductMaster");
    }

    public List<ProductMaster> GetAll() => _collection.Find(_ => true).ToList();

    public ProductMaster? GetById(string id) => _collection.Find(p => p.prdId == id).FirstOrDefault();
    public ProductMaster? GetBySlug(string id) => _collection.Find(p => p.prdSlug == id).FirstOrDefault();

    public void Create(ProductMaster model) => _collection.InsertOne(model);

    public void Update(string id, ProductMaster model) =>
        _collection.ReplaceOne(p => p.prdId == id, model);

    public void Delete(string id) =>
        _collection.DeleteOne(p => p.prdId == id);
}
