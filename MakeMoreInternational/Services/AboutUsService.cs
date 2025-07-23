using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class AboutUsService
{
    private readonly IMongoCollection<AboutUsMaster> _collection;

    public AboutUsService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<AboutUsMaster>("AboutUsMaster");
    }

    public AboutUsMaster Get()
		=> _collection.Find(_ => true).FirstOrDefault();

	public void Create(AboutUsMaster data)
		=> _collection.InsertOne(data);

	public void Update(string id, AboutUsMaster data)
		=> _collection.ReplaceOne(x => x.Id == id, data);
}
