using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class InquiryService
    {
        private readonly IMongoCollection<InquiryMaster> _collection;

        public InquiryService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _collection = db.GetCollection<InquiryMaster>("InquiryMaster");
        }

        public void Create(InquiryMaster inq)
        {
            _collection.InsertOne(inq);
        }

        public List<InquiryMaster> GetAll()
        {
            return _collection.Find(c => true).SortByDescending(c => c.inqDate).ToList();
        }

        public InquiryMaster GetById(string id)
        {
            return _collection.Find(c => c.inqId == id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            _collection.DeleteOne(c => c.inqId == id);
        }
    }
}
