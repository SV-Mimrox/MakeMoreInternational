using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class CountryMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? cntId { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image")]
        public string? cntImage { get; set; }

        [Required(ErrorMessage = "Country name is required")]
        [Display(Name = "Country")]
        public string cntName { get; set; }
        

        [Required(ErrorMessage = "Latitude is required")]
        [Display(Name = "Latitude")]
        public string cntLat { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Display(Name = "Longitude")]
        public string cntLng { get; set; }

        [Display(Name = "Status")]
        public bool cntStatus { get; set; }
    }
}
