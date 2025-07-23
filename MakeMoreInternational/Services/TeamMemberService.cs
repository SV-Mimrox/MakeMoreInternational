using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class TeamMemberService 
{
    private readonly IMongoCollection<TeamMemberMaster> _collection;
    private readonly IMongoCollection<TeamMemberPageBanner> _bannerCollection;
    private readonly IWebHostEnvironment _env;

    public TeamMemberService(IOptions<MongoDBSettings> settings, IWebHostEnvironment env)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _collection = db.GetCollection<TeamMemberMaster>("TeamMemberMaster");
        _bannerCollection = db.GetCollection<TeamMemberPageBanner>("TeamMemberPageBanner");
        _env = env;
    }

    public List<TeamMemberMaster> GetAll() => _collection.Find(_ => true).ToList();

    public TeamMemberMaster GetById(string id) => _collection.Find(x => x.Id == id).FirstOrDefault();

    public void Create(TeamMemberMaster member) => _collection.InsertOne(member);

    public void Update(string id, TeamMemberMaster member) =>
        _collection.ReplaceOne(x => x.Id == id, member);

    public void Delete(string id) => _collection.DeleteOne(x => x.Id == id);

    public TeamMemberPageBanner GetBanner()
    {
        return _bannerCollection.Find(_ => true).FirstOrDefault();
    }

    public void UpsertBanner(IFormFile bannerFile)
    {
        var existing = GetBanner();

        string fileName = null;
        if (bannerFile != null)
        {
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(bannerFile.FileName);
            var savePath = Path.Combine(_env.WebRootPath, "images/team", fileName);
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                bannerFile.CopyTo(stream);
            }
        }

        if (existing == null)
        {
            var banner = new TeamMemberPageBanner
            {
                BannerImage = fileName
            };
            _bannerCollection.InsertOne(banner);
        }
        else
        {
            var update = Builders<TeamMemberPageBanner>.Update
                .Set(b => b.BannerImage, fileName ?? existing.BannerImage);

            _bannerCollection.UpdateOne(b => b.Id == existing.Id, update);
        }
    }
}
