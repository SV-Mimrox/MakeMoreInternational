using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class SliderService 
{
    private readonly IMongoCollection<SliderMaster> _collection;

   
    public SliderService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _collection = db.GetCollection<SliderMaster>("SliderMaster");
    }

    public List<SliderMaster> GetAll() => _collection.Find(s => true).ToList();

    public SliderMaster GetById(string id) => _collection.Find(s => s.Id == id).FirstOrDefault();

    public void Create(SliderMaster slider) => _collection.InsertOne(slider);

    public void Update(string id, SliderMaster slider) =>
        _collection.ReplaceOne(s => s.Id == id, slider);

    public void Delete(string id) => _collection.DeleteOne(s => s.Id == id);
}
