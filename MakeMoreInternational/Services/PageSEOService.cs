using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class PageSEOService
    {
        private readonly IMongoCollection<PageSeo> _collection;

        public PageSEOService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _collection = db.GetCollection<PageSeo>("PageSeo");
        }
        public List<PageSeo> GetAll() => _collection.Find(FilterDefinition<PageSeo>.Empty).ToList();

        public PageSeo GetById(string id) =>
            _collection.Find(x => x.psId == id).FirstOrDefault();

        public void Create(PageSeo seo) =>
            _collection.InsertOne(seo);

        public void Update(PageSeo seo) =>
            _collection.ReplaceOne(x => x.psId == seo.psId, seo);

        public void Delete(string id) =>
            _collection.DeleteOne(x => x.psId == id);
    }

}
