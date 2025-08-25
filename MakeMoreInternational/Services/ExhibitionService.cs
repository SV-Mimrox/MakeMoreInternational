using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class ExhibitionService
    {
        private readonly IMongoCollection<ExhibitionMaster> _collection;

        public ExhibitionService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _collection = db.GetCollection<ExhibitionMaster>("ExhibitionMaster");
        }
        public List<ExhibitionMaster> GetAll() =>
            _collection.Find(_ => true).SortBy(e => e.ebmSeqNo).ToList();

        public ExhibitionMaster GetById(string id) =>
            _collection.Find(e => e.Id == id).FirstOrDefault();

        public void Create(ExhibitionMaster model) =>
            _collection.InsertOne(model);

        public void Update(string id, ExhibitionMaster model) =>
            _collection.ReplaceOne(e => e.Id == id, model);

        public void Delete(string id) =>
            _collection.DeleteOne(e => e.Id == id);
    }
}
