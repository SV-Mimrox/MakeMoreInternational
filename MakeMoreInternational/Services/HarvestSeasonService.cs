using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class HarvestSeasonService
    {
        private readonly IMongoCollection<HarvestSeasonMaster> _col;

        public HarvestSeasonService(IOptions<MongoDBSettings> cfg)
        {
            var cli = new MongoClient(cfg.Value.ConnectionString);
            _col = cli.GetDatabase(cfg.Value.DatabaseName)
                         .GetCollection<HarvestSeasonMaster>("HarvestSeasonMaster");
        }

        /* -------- basic helpers --------- */
        public List<HarvestSeasonMaster> GetAll() => _col.Find(_ => true).ToList();

        public HarvestSeasonMaster? GetById(string id) =>
            _col.Find(s => s.hsesId == id).FirstOrDefault();

        /* ----- bulk replace for one product ----- */
        public void ReplaceSeasonsForProduct(string productId, IEnumerable<int> months)
        {
            // 1. remove prior docs
            _col.DeleteMany(s => s.hprdId == productId);

            // 2. insert distinct months
            var docs = months.Distinct()
                             .Select(m => new HarvestSeasonMaster
                             {
                                 hprdId = productId,
                                 month = m,
                                 hsesStatus = true,
                                 createdAt = DateTime.Now
                             })
                             .ToList();

            if (docs.Count > 0) _col.InsertMany(docs);
        }

        /* single‑row helpers, not used in matrix page */
        public void Create(HarvestSeasonMaster m) => _col.InsertOne(m);
        public void Update(string id, HarvestSeasonMaster m) => _col.ReplaceOne(s => s.hsesId == id, m);
        public void Delete(string id) => _col.DeleteOne(s => s.hsesId == id);

        public bool Exists(string productId, int month, string? excludeId = null) =>
            _col.Find(s => s.hprdId == productId && s.month == month && s.hsesId != excludeId).Any();
    }
}
