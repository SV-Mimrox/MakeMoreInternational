using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MakeMoreInternational.Models
{
	public class PackagingMaster
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? pkgId { get; set; }

		[Required(ErrorMessage = "Image is required")]
		public string? pkgImage { get; set; }

		[Required(ErrorMessage = "Seq No is required")]
		public int pkgSeqNo { get; set; } = 0;

		public bool pkgStatus { get; set; }
	}
}
