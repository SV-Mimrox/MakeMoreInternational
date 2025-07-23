using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class WebSettingService
    {
        private readonly IMongoCollection<WebSettingMaster> _collection;

        public WebSettingService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<WebSettingMaster>("WebSettingMaster");
        }

        public List<WebSettingMaster> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public WebSettingMaster? GetById(string id)
        {
            return _collection.Find(w => w.wsmId == id).FirstOrDefault();
        }

        public void Create(WebSettingMaster model)
        {
            _collection.InsertOne(model);
        }

        public void Update(string id, WebSettingMaster model)
        {
            _collection.ReplaceOne(w => w.wsmId == id, model);
        }

        public void Delete(string id)
        {
            _collection.DeleteOne(w => w.wsmId == id);
        }

        /// <summary>
        /// Create or Update a singleton WebSettingMaster record.
        /// </summary>
        public void CreateOrUpdate(WebSettingMaster model)
        {
            var existing = _collection.Find(_ => true).FirstOrDefault();

            if (existing != null)
            {
                model.wsmId = existing.wsmId;
                _collection.ReplaceOne(w => w.wsmId == model.wsmId, model);
            }
            else
            {
                _collection.InsertOne(model);
            }
        }

        /// <summary>
        /// Gets the first settings record or null.
        /// </summary>
        public WebSettingMaster? GetSingle()
        {
            return _collection.Find(_ => true).FirstOrDefault();
        }
    }
}
