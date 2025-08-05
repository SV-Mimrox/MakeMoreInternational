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
        public string? wpmTerms { get; set; }

        [Display(Name = "Privacy Policy")]
        public string wpmPrivacy { get; set; }

        [Display(Name = "Infrastructure")]
        public string wpmInfrastructure { get; set; }

    }
}
