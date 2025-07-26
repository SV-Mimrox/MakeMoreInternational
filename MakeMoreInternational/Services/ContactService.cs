using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class ContactService
    {
        private readonly IMongoCollection<ContactMaster> _contactCollection;

        public ContactService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _contactCollection = db.GetCollection<ContactMaster>("ContactMaster");
        }

        public void Create(ContactMaster contact)
        {
            _contactCollection.InsertOne(contact);
        }

        public List<ContactMaster> GetAll()
        {
            return _contactCollection.Find(c => true).SortByDescending(c => c.cntDate).ToList();
        }

        public ContactMaster GetById(string id)
        {
            return _contactCollection.Find(c => c.cntId == id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            _contactCollection.DeleteOne(c => c.cntId == id);
        }
    }
}
