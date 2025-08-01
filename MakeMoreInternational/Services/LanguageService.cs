using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class LanguageService
    {
        private readonly IMongoCollection<LanguageMaster> _collection;

        public LanguageService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<LanguageMaster>("LanguageMaster");
        }

        public List<LanguageMaster> GetAll() => _collection.Find(x => true).ToList();
        public List<LanguageMaster> GetActive() => _collection.Find(x => x.lmStatus == true).ToList();

        public LanguageMaster GetById(string id) => _collection.Find(x => x.lmId == id).FirstOrDefault();

        public void Create(LanguageMaster data) => _collection.InsertOne(data);

        public void Update(string id, LanguageMaster data) =>
            _collection.ReplaceOne(x => x.lmId == id, data);

        public void Delete(string id) => _collection.DeleteOne(x => x.lmId == id);
    }
}
