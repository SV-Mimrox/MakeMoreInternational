using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class ContactMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string cntId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string cntName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string cntEmail { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        public string cntContact { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string cntMessage { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime cntDate { get; set; } = DateTime.Now;
    }
}
