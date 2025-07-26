using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class CertificateMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? crtId { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string? crtImage { get; set; }

        [Required(ErrorMessage = "Certificate image is required")]
        public string? crtCertiImage { get; set; }

        public bool crtStatus { get; set; }
    }
}
