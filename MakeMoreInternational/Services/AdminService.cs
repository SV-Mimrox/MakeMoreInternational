using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class AdminService
    {
        private readonly IMongoCollection<AdminMaster> _adminCollection;

        public AdminService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _adminCollection = database.GetCollection<AdminMaster>("AdminMaster");
        }

        public void Register(AdminMaster admin)
        {
            var existing = GetByEmail(admin.admEmail);
            if (existing != null) throw new Exception("Email already exists");

            admin.admPassword = BCrypt.Net.BCrypt.HashPassword(admin.admPassword);
            _adminCollection.InsertOne(admin);
        }

        public AdminMaster? Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.admPassword))
            {
                return user;
            }
            return null;
        }

        public AdminMaster? GetByEmail(string email)
        {
            return  _adminCollection.Find(x => x.admEmail == email).FirstOrDefault();
        }
    }
}
