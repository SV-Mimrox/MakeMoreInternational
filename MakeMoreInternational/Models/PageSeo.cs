using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class PageSeo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? psId { get; set; }

        [Required]
        [Display(Name = "Page")]
        public string psName { get; set; }
        
        [Display(Name = "Meta Title")]
        public string? psMetaTitle { get; set; }
        
        [Display(Name = "Meta Description")]
        public string? psMetaDescription { get; set; }
        
        [Display(Name = "Meta Keywords")]
        public string? psMetaKeywords { get; set; }
    }
}
