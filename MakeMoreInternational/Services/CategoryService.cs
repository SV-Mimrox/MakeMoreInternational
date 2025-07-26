using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace MakeMoreInternational.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<CategoryMaster> _collection;

        public CategoryService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _collection = db.GetCollection<CategoryMaster>("CategoryMaster");
        }

        // Get all categories
        public List<CategoryMaster> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public List<CategoryMaster> GetActiveAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public List<CategoryMaster> GetParentActiveAll()
        {
            return _collection.Find(t=>t.catParentId==null).ToList();
        }

        public List<CategoryMaster> GetHome()
        {
            return _collection.Find(t => t.catParentId == null && t.catStatus==true && t.isHome==true).ToList();
        }

        // Get main categories only (for dropdown)
        public List<CategoryMaster> GetMainCategories()
        {
            return _collection.Find(c => c.catParentId == null).ToList();
        }

        // Get by Id
        public CategoryMaster? GetById(string id)
        {
            return _collection.Find(c => c.catId == id).FirstOrDefault();
        }

        public CategoryMaster? GetBySlug(string id)
        {
            return _collection.Find(c => c.catSlug == id).FirstOrDefault();
        }

        // Create new category
        public void Create(CategoryMaster model)
        {
            // Enforce unique name
            if (_collection.Find(c => c.catName.ToLower() == model.catName.ToLower()).Any())
                throw new Exception("Category name must be unique.");

            model.catSlug = GenerateUniqueSlug(model.catName);
            _collection.InsertOne(model);
        }

        // Update category
        public void Update(string id, CategoryMaster model)
        {
            var existing = GetById(id);
            if (existing == null) throw new Exception("Record not found.");

            // Check if name changed, then validate uniqueness
            if (existing.catName.ToLower() != model.catName.ToLower())
            {
                if (_collection.Find(c => c.catName.ToLower() == model.catName.ToLower() && c.catId != id).Any())
                    throw new Exception("Category name must be unique.");
            }

            model.catSlug = GenerateUniqueSlug(model.catName, id);
            _collection.ReplaceOne(c => c.catId == id, model);
        }

        // Delete category
        public void Delete(string id)
        {
            _collection.DeleteOne(c => c.catId == id);
        }

        // Slug Generator
        private string GenerateSlug(string text)
        {
            text = text.ToLower().Trim();
            text = Regex.Replace(text, @"[^a-z0-9\s-]", ""); // remove special chars
            text = Regex.Replace(text, @"\s+", "-"); // replace spaces with dashes
            return text;
        }

        // Unique Slug Generator
        private string GenerateUniqueSlug(string name, string? currentId = null)
        {
            string baseSlug = GenerateSlug(name);
            string slug = baseSlug;
            int counter = 1;

            while (_collection.Find(c => c.catSlug == slug && c.catId != currentId).Any())
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }
    }
}
