using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class HarvestProductMaster
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? hprdId { get; set; }

        [DisplayName("Harvest Category")]
        [Required(ErrorMessage = "Category is required")]
        public string hcatId { get; set; }

        [DisplayName("Product Name")]
        [Required, StringLength(120)]
        public string hprdName { get; set; } = string.Empty;

        [DisplayName("Icon (file name)")]
        public string? hprdIcon { get; set; }

        [DisplayName("Sequence No")]
        [Required, Range(1, int.MaxValue)]
        public int seqNo { get; set; }

        [DisplayName("Status")]
        public bool hprdStatus { get; set; } = true;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
