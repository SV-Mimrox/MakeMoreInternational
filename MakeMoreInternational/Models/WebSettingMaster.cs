using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MakeMoreInternational.Models
{
    public class WebSettingMaster
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? wsmId { get; set; }

        [DisplayName("Website Title")]
        [Required(ErrorMessage = "Title is required")]
        public string wstTitle { get; set; } = string.Empty;

        [DisplayName("Subtitle")]
        [Required(ErrorMessage = "Subtitle is required")]
        public string wsmSubtitle { get; set; } = string.Empty;

        [DisplayName("Logo URL")]
        
        public string? wsmLogo { get; set; }

        [DisplayName("Contact Information")]
        [Required(ErrorMessage = "Contact is required")]
        public string wsmContact { get; set; } = string.Empty;

        [DisplayName("Email Information")]
        [Required(ErrorMessage = "Email is required")]
        public string wsmEmail { get; set; } = string.Empty;

        [DisplayName("Office Address")]
        [Required(ErrorMessage = "Address is required")]
        public string wsmAddress { get; set; } = string.Empty;

        [DisplayName("Slogan")]
        [Required(ErrorMessage = "Slogan is required")]
        public string wsmSlogan { get; set; } = string.Empty;

        [DisplayName("Short Description")]
        public string? wsmShortDesc { get; set; } = string.Empty;

        [DisplayName("Map Link")]
        [Url(ErrorMessage = "Enter a valid Map URL")]
        public string? wsmMapLink { get; set; } = string.Empty;

        [DisplayName("Social Media Links")]
        public List<SocialMedia> wsmSocialMedia { get; set; } = new();

        public bool wsmIsAbout { get; set; }
        public bool wsmIsTeam { get; set; }
        public bool wsmIsInfrastructure { get; set; }

        public string? wsmBroucher { get; set; }

        [Url(ErrorMessage = "Enter a valid URL")]
        [Display(Name = "Home Page Video URL")]
        public string? wsmHomePageVideo { get; set; }

        [Url(ErrorMessage = "Enter a valid URL")]
        [Display(Name = "About Page Video URL")]
        public string? wsmAboutPageVideo { get; set; }

        [Display(Name = "Home Section")]
        public string? wsmHomeSec1 { get; set; }
        [Display(Name = "About Section")]
        public string? wsmAboutSec1 { get; set; }
    }



    public class WebSettingMasterCreate
    {
        
        public string? wsmId { get; set; }

        [DisplayName("Website Title")]
        [Required(ErrorMessage = "Title is required")]
        public string wstTitle { get; set; } = string.Empty;

        [DisplayName("Subtitle")]
        [Required(ErrorMessage = "Subtitle is required")]
        public string wsmSubtitle { get; set; } = string.Empty;

        [DisplayName("Logo URL")]

        [Url(ErrorMessage = "Enter a valid URL")]
        public string wsmLogo { get; set; } = string.Empty;

        [DisplayName("Contact Information")]
        [Required(ErrorMessage = "Contact is required")]
        public string wsmContact { get; set; } = string.Empty;

        [DisplayName("Office Address")]
        [Required(ErrorMessage = "Address is required")]
        public string wsmAddress { get; set; } = string.Empty;

        [DisplayName("Slogan")]
        [Required(ErrorMessage = "Slogan is required")]
        public string wsmSlogan { get; set; } = string.Empty;

        [DisplayName("Social Media Links")]
        public List<SocialMedia> wsmSocialMedia { get; set; } = new();

        [DisplayName("About Page")]
        public bool wsmIsAbout { get; set; }
        [DisplayName("Team Page")]
        public bool wsmIsTeam { get; set; }
        [DisplayName("Infrastructure Page")]
        public bool wsmIsInfrastructure { get; set; }
        [Display(Name = "Broucher")]
        public string wsmBroucher { get; set; }
    }

    public class SocialMedia
	{
		[DisplayName("Social Media Name")]
		[Required(ErrorMessage = "Social media name is required")]
		public string Name { get; set; } = string.Empty;

		[DisplayName("Icon (FontAwesome class or Image URL)")]
		[Required(ErrorMessage = "Icon is required")]
		public string Icon { get; set; } = string.Empty;

		[DisplayName("Social Media Link")]
		[Required(ErrorMessage = "Link is required")]
		
		public string Link { get; set; } = string.Empty;
	}
}
