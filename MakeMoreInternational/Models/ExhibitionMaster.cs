using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
    public class ExhibitionMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Display(Name = "Seq No")]
        public int ebmSeqNo { get; set; }
        [Display(Name = "Image")]
        public string ebmImage { get; set; }
    }
}
