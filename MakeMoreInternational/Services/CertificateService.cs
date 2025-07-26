using MakeMoreInternational.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class CertificateService
    {
        private readonly IMongoCollection<CertificateMaster> _collection;

        public CertificateService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _collection = db.GetCollection<CertificateMaster>("CertificateMaster");
        }

        public List<CertificateMaster> GetAll() => _collection.Find(c => true).ToList();
        public List<CertificateMaster> GetActive() => _collection.Find(c => c.crtStatus == true).ToList();

        public CertificateMaster GetById(string id) =>
            _collection.Find(c => c.crtId == id).FirstOrDefault();

        public void Create(CertificateMaster cert) => _collection.InsertOne(cert);

        public void Update(string id, CertificateMaster cert) =>
            _collection.ReplaceOne(c => c.crtId == id, cert);

        public void Delete(string id) => _collection.DeleteOne(c => c.crtId == id);
    }
}
