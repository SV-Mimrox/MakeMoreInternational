using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class LanguageMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? lmId { get; set; }

        [Required(ErrorMessage = "Language Name is required")]
        public string? lmName { get; set; }

        [Required(ErrorMessage = "Language Code is required")]
        public string? lmCode { get; set; }

        public bool lmStatus { get; set; } = true;
    }
}
