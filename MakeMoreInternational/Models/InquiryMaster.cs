using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class InquiryMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string inqId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string inqName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string inqEmail { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        public string inqContact { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        public string inqSubject { get; set; }

        public string? inqPage { get; set; } = "-";

        [Required(ErrorMessage = "Message is required")]
        public string inqMessage { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime inqDate { get; set; } = DateTime.Now;
    }
}
