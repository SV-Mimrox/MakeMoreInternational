using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class ProductMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? prdId { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Category is required")]
        public string catId { get; set; }

        [DisplayName("Product Name")]
        [Required(ErrorMessage = "Product name is required")]
        public string prdName { get; set; }

        [DisplayName("Slug")]
        public string? prdSlug { get; set; }

        [DisplayName("Description")]
        public string? prdDescription { get; set; }

        [DisplayName("Banner Image")]
        public string? prdBannerImage { get; set; } // Stores file name only

        [DisplayName("Image")]
        public string? prdImage { get; set; }

        [DisplayName("Icon")]
        public string? prdIcon { get; set; } // Stores file name only

        [DisplayName("Product Details")]
        public Dictionary<string, string>? ProductDetails { get; set; } = new();

        [DisplayName("Varieties")]
        public Dictionary<string, string>? Varieties { get; set; } = new();

        [DisplayName("Specification")]
        public Dictionary<string, string>? PhysicalSpecification { get; set; } = new();
        [DisplayName("Grades")]
        public Dictionary<string, string>? Grades { get; set; } = new();
        [DisplayName("Other Details")]
        public Dictionary<string, string>? OtherDetails { get; set; } = new();

        [DisplayName("Status")]
        [Required]
        public bool prdStatus { get; set; } = true;

        [DisplayName("Show in Home")]
        [Required]
        public bool prdIsHome { get; set; } = false;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [DisplayName("Sequence No")]
        [Required(ErrorMessage = "Sequence number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Sequence number must be greater than 0")]
        public int prdSeqNo { get; set; }
        [DisplayName("Uses")]
        public string? prdUses { get; set; }
    }
}
