using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class BlogMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string blmId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string blmTitle { get; set; }
        public string blmUrl { get; set; }

        [Display(Name = "Image")]
        public string blmImage { get; set; }
        [Display(Name = "Image Text")]
        public string blmImageAlt { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string blmDescription { get; set; }

        public DateTime blmDate { get; set; } = DateTime.Now;
        [Display(Name = "Status")]
        public bool blmStatus { get; set; }

        [Display(Name = "Meta Title")]
        public string blmMetaTitle { get; set; }
        [Display(Name = "Meta Description")]
        public string blmMetaDescription { get; set; }
        [Display(Name = "Meta Keywords")]
        public string blmMetaKeywords { get; set; }
    }
}
