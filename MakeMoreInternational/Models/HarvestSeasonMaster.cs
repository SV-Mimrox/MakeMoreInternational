using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class HarvestSeasonMaster
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? hsesId { get; set; }

        [DisplayName("Harvest Product")]
        [Required]
        public string hprdId { get; set; } = string.Empty;           // FK to HarvestProductMaster

        [DisplayName("Month (1‑12)")]
        [Range(1, 12)]
        public int month { get; set; }

        [DisplayName("Status")]
        public bool hsesStatus { get; set; } = true;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime createdAt { get; set; } = DateTime.Now;
    }

    public class HarvestSeasonMultiVM
    {
        public string ProductId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Select at least one month")]
        public int[] Months { get; set; } = Array.Empty<int>();

        public bool Status { get; set; } = true;
    }
    public class HarvestSeasonRowVM
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductImage { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public bool[] Months { get; set; } = new bool[12];
    }


}
