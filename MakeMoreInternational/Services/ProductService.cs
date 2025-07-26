using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class ProductService
{
    private readonly IMongoCollection<ProductMaster> _collection;
    private readonly IMongoCollection<CategoryMaster> _catCollection;

    public ProductService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _collection = db.GetCollection<ProductMaster>("ProductMaster");
        _catCollection = db.GetCollection<CategoryMaster>("CategoryMaster");
    }

    public List<ProductMaster> GetAll() => _collection.Find(_ => true).ToList();
    public List<ProductMaster> GetAllActive() => _collection.Find(t=>t.prdStatus==true).ToList();
    public List<ProductMaster> GetForHome() => _collection.Find(t=>t.prdIsHome==true).ToList();
    public List<ProductMaster> GetRelatedProducts(string id)
    {
       var data = _collection.Find(t => t.catId == id).ToList();
        return data;
    }
    public List<ProductMaster> GetProductByCat(string id)
    {
        var response= new List<ProductMaster>();
        var catData = _catCollection.Find(t=>t.catSlug==id).FirstOrDefault();
        if(catData == null)
        {
            return response;
        }
        else
        {
            var subcategories = _catCollection.Find(c => c.catParentId == catData.catId).ToList();
            foreach(var s in subcategories)
            {
                var products = _collection.Find(t=>t.catId== s.catId).ToList();
                response.AddRange(products);
            }
        }
        return response;
    }

    public ProductMaster? GetById(string id) => _collection.Find(p => p.prdId == id).FirstOrDefault();
    public ProductMaster? GetBySlug(string id) => _collection.Find(p => p.prdSlug == id).FirstOrDefault();

    public void Create(ProductMaster model) => _collection.InsertOne(model);

    public void Update(string id, ProductMaster model) =>
        _collection.ReplaceOne(p => p.prdId == id, model);

    public void Delete(string id) =>
        _collection.DeleteOne(p => p.prdId == id);
}
