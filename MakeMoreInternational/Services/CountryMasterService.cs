using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class CountryMasterService
    {
        private readonly IMongoCollection<CountryMaster> _collection;

        public CountryMasterService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _collection = db.GetCollection<CountryMaster>("CountryMaster");
        }

        public List<CountryMaster> GetAll() => _collection.Find(c => true).ToList();
        public List<CountryMaster> GetActive() => _collection.Find(c => c.cntStatus == true).ToList();

        public CountryMaster GetById(string id) =>
            _collection.Find(c => c.cntId == id).FirstOrDefault();

        public void Create(CountryMaster cert) => _collection.InsertOne(cert);

        public void Update(string id, CountryMaster cert) =>
            _collection.ReplaceOne(c => c.cntId == id, cert);

        public void Delete(string id) => _collection.DeleteOne(c => c.cntId == id);
    }
}
