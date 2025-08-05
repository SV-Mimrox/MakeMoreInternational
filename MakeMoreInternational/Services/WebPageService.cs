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

        public void savePage(string page,string pageData)
        {
            var data = _collection.Find(t => true).FirstOrDefault();
            if (data == null) {
                var newData = new WebPageMaster();
                if(page == "terms")
                {
                    newData.wpmTerms = pageData;
                }
                if (page == "privacy")
                {
                    newData.wpmPrivacy = pageData;
                }
                if (page == "infrastructure")
                {
                    newData.wpmInfrastructure = pageData;
                }
                _collection.InsertOne(newData);
            }
            else
            {
                if (page == "terms")
                {
                    data.wpmTerms = pageData;
                }
                if (page == "privacy")
                {
                    data.wpmPrivacy = pageData;
                }
                if (page == "infrastructure")
                {
                    data.wpmInfrastructure = pageData;
                }
                _collection.ReplaceOne(t => t.wpmId == data.wpmId, data);
            }
        }

        public string getPageData(string page)
        {
            var response = "";
            var data = _collection.Find(t => true).FirstOrDefault();
            if (data != null)
            {
                if (page == "terms")
                {
                    response = data.wpmTerms;
                }
                if (page == "privacy")
                {
                    response = data.wpmPrivacy;
                }
                if (page == "infrastructure")
                {
                    response = data.wpmInfrastructure;
                }
            }
            return response;
        }
    }
}
