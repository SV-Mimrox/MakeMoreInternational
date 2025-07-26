using MakeMoreInternational.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MakeMoreInternational.Services
{
    public class BlogService
    {
        private readonly IMongoCollection<BlogMaster> _blogCollection;

        public BlogService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _blogCollection = db.GetCollection<BlogMaster>("BlogMaster");
        }

        public List<BlogMaster> GetAll() =>
            _blogCollection.Find(blog => true).SortByDescending(b => b.blmDate).ToList();

        public List<BlogMaster> GetActive() =>
            _blogCollection.Find(blog => true).SortByDescending(b => b.blmDate).ToList();

        public BlogMaster GetById(string id) =>
            _blogCollection.Find(blog => blog.blmId == id).FirstOrDefault();
        public BlogMaster GetByUrl(string id) =>
            _blogCollection.Find(blog => blog.blmUrl == id).FirstOrDefault();

        public void Create(BlogMaster blog) =>
            _blogCollection.InsertOne(blog);

        public void Update(string id, BlogMaster blog) =>
            _blogCollection.ReplaceOne(b => b.blmId == id, blog);

        public void Delete(string id) =>
            _blogCollection.DeleteOne(b => b.blmId == id);

        public string GenerateUniqueSlug(string title)
        {
            string baseSlug = GenerateSlug(title);
            string slug = baseSlug;
            int count = 1;

            while (_blogCollection.Find(x => x.blmUrl == slug).Any())
            {
                slug = $"{baseSlug}-{count}";
                count++;
            }

            return slug;
        }

        private string GenerateSlug(string title)
        {
            string slug = title.ToLower().Replace(" ", "-").Replace("---", "-").Replace("--", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            return slug;
        }

    }
}
