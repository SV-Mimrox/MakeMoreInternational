using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class HarvestCategoryMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? hcatId { get; set; }

        [DisplayName("Category Name")]
        [Required, StringLength(100)]
        public string hcatName { get; set; } = string.Empty;

        [DisplayName("Slug")]
        public string? hcatSlug { get; set; }

        [DisplayName("Status")]
        public bool hcatStatus { get; set; } = true;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
