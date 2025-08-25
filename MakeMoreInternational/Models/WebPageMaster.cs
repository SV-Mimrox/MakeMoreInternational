using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class WebPageMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string wpmId { get; set; }

        [Display(Name = "Terms & Condition")]
        public Terms wpmTerms { get; set; }

        [Display(Name = "Privacy Policy")]
        public Terms wpmPrivacy { get; set; }

        [Display(Name = "Infrastructure")]
        public Infra wpmInfrastructure { get; set; }

    }

    public class Terms
    {
        public string tcTitle { get; set; }

        // Multiple key–value pairs
        public Dictionary<string, string> tcDesc { get; set; } = new Dictionary<string, string>();

        public DateTime tcUpdateDate { get; set; }
    }

    public class Infra { 
        public string? infraTitle { get; set; }
        public string? infraDesc { get; set; }
        public List<string> infraImages { get; set; }

    }

}
