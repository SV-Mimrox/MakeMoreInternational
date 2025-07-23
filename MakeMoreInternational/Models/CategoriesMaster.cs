using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MakeMoreInternational.Models
{
    public class CategoryMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? catId { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Category Name is required")]
        public string catName { get; set; }

        [DisplayName("Slug")]
        public string? catSlug { get; set; }

        [DisplayName("Parent Category")]
        public string? catParentId { get; set; }   // null = main category, value = sub-category

        [DisplayName("Description")]
        public string? catDescription { get; set; }

        [DisplayName("Image")]
        public string? catImage { get; set; }      // store only filename

        /* ➕ NEW FIELDS ------------------------------------------------------ */
        [DisplayName("Sequence No.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seq No. must be ≥ 1")]
        public int seqNo { get; set; } = 1;

        [DisplayName("Show on Home?")]
        public bool isHome { get; set; } = false;
        /* ------------------------------------------------------------------- */

        [DisplayName("Status")]
        [Required]
        public bool catStatus { get; set; } = true;
    }
}
