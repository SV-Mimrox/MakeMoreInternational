using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class HarvestCategoryService
    {
        private readonly IMongoCollection<HarvestCategoryMaster> _col;

        public HarvestCategoryService(IOptions<MongoDBSettings> cfg)
        {
            var client = new MongoClient(cfg.Value.ConnectionString);
            var database = client.GetDatabase(cfg.Value.DatabaseName);
            _col = database.GetCollection<HarvestCategoryMaster>("HarvestCategoryMaster");
        }

        public List<HarvestCategoryMaster> GetAll() => _col.Find(_ => true).ToList();
        public HarvestCategoryMaster? GetById(string id) => _col.Find(c => c.hcatId == id).FirstOrDefault();
        public void Create(HarvestCategoryMaster m) => _col.InsertOne(m);
        public void Update(string id, HarvestCategoryMaster m) => _col.ReplaceOne(c => c.hcatId == id, m);
        public void Delete(string id) => _col.DeleteOne(c => c.hcatId == id);
        public HarvestCategoryMaster? GetBySlug(string slug) =>
    _col.Find(c => c.hcatSlug == slug).FirstOrDefault();
    }
}
