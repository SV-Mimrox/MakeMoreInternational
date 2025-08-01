using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
	public class PackagingService
	{
		private readonly IMongoCollection<PackagingMaster> _collection;

		public PackagingService(IOptions<MongoDBSettings> settings)
		{
			var client = new MongoClient(settings.Value.ConnectionString);
			var db = client.GetDatabase(settings.Value.DatabaseName);
			_collection = db.GetCollection<PackagingMaster>("PackagingMaster");
		}

		public List<PackagingMaster> GetAll() => _collection.Find(c => true).ToList();
		public List<PackagingMaster> GetActive() => _collection.Find(c => c.pkgStatus == true).ToList();

		public PackagingMaster GetById(string id) =>
			_collection.Find(c => c.pkgId == id).FirstOrDefault();

		public void Create(PackagingMaster cert) => _collection.InsertOne(cert);

		public void Update(string id, PackagingMaster cert) =>
			_collection.ReplaceOne(c => c.pkgId == id, cert);

		public void Delete(string id) => _collection.DeleteOne(c => c.pkgId == id);
	}
}
