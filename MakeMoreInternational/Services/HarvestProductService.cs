using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class HarvestProductService
    {
        private readonly IMongoCollection<HarvestProductMaster> _col;

        public HarvestProductService(IOptions<MongoDBSettings> cfg)
        {
            var cli = new MongoClient(cfg.Value.ConnectionString);
            _col = cli.GetDatabase(cfg.Value.DatabaseName)
                         .GetCollection<HarvestProductMaster>("HarvestProductMaster");
        }

        public List<HarvestProductMaster> GetAll() => _col.Find(_ => true).ToList();
        public HarvestProductMaster? GetById(string id) => _col.Find(p => p.hprdId == id).FirstOrDefault();
        public void Create(HarvestProductMaster m) => _col.InsertOne(m);
        public void Update(string id, HarvestProductMaster m) => _col.ReplaceOne(p => p.hprdId == id, m);
        public void Delete(string id) => _col.DeleteOne(p => p.hprdId == id);
        public bool SeqExists(string hcatId, int seq, string? excludeId = null) =>
    _col.Find(p => p.hcatId == hcatId && p.seqNo == seq && p.hprdId != excludeId).Any();

        public List<HarvestProductMaster> GetByCategory(string categoryId)
        {
            return _col.Find(p => p.hcatId == categoryId).ToList();
        }

    }
}
