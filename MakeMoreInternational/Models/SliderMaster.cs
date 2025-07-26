using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeMoreInternational.Models
{
    public class SliderMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Main image is required")]
        public string? MainImage { get; set; }

        public string Slogan { get; set; }
        public bool Status { get; set; }

        

        public List<string> SubImages { get; set; } = new List<string>(); // Max 7 images
    }
}