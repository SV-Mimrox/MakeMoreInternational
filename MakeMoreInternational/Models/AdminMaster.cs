using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MakeMoreInternational.Models
{
    public class AdminMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? admId { get; set; }

        
        public string admName { get; set; } = string.Empty;
        public string admEmail { get; set; } = string.Empty;
        public string admContact { get; set; } = string.Empty;
        public string admPassword { get; set; } = string.Empty;
        public bool admStatus { get; set; } = true;
    }
}
