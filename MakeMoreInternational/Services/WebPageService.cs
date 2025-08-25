using MakeMoreInternational.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MakeMoreInternational.Services
{
    public class WebPageService
    {
        private readonly IMongoCollection<WebPageMaster> _collection;

        public WebPageService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _collection = db.GetCollection<WebPageMaster>("WebPageMaster");
        }

        public void savePage(string page,string pageData, Terms terms, Infra infra)
        {
            var data = _collection.Find(t => true).FirstOrDefault();
            if (data == null) {
                var newData = new WebPageMaster();
                if(page == "terms")
                {
                    newData.wpmTerms = terms;
                }
                if (page == "privacy")
                {
                    newData.wpmPrivacy = terms;
                }
                if (page == "infrastructure")
                {
                    newData.wpmInfrastructure = infra;
                }
                _collection.InsertOne(newData);
            }
            else
            {
                if (page == "terms")
                {
                    data.wpmTerms = terms;
                }
                if (page == "privacy")
                {
                    data.wpmPrivacy = terms;
                }
                if (page == "infrastructure")
                {
                    data.wpmInfrastructure = infra;
                }
                _collection.ReplaceOne(t => t.wpmId == data.wpmId, data);
            }
        }

        public void deleteInfraImage(string img)
        {
            var data = _collection.Find(t => true).FirstOrDefault();
            if (data != null)
            {
                var infraData = data.wpmInfrastructure;
                if (infraData != null)
                {
                    var images = infraData.infraImages;
                    if (images.Count >= 1)
                    {
                        images.Remove(img);
                    }
                    infraData.infraImages = images;
                    data.wpmInfrastructure = infraData;
                }

                _collection.ReplaceOne(t => t.wpmId == data.wpmId, data);
            }
        }

        public Infra getInfra()
        {
            
            var data = _collection.Find(t => true).FirstOrDefault();
            if (data != null)
            {
                return data.wpmInfrastructure;
            }
            else
            {
                return new Infra();
            }
        }

        public Terms getTerms()
        {
            
            var data = _collection.Find(t => true).FirstOrDefault();
            if (data != null)
            {
                return data.wpmTerms;
            }
            else
            {
                return new Terms();
            }
        }
        public Terms getPrivacy()
        {

            var data = _collection.Find(t => true).FirstOrDefault();
            if (data != null)
            {
                return data.wpmPrivacy;
            }
            else
            {
                return new Terms();
            }
        }
    }
}
