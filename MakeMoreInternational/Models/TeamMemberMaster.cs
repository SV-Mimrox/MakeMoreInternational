using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MakeMoreInternational.Models
{
	public class TeamMemberMaster
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		public string Name { get; set; }
		public string Position { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string? Photo { get; set; }

		public bool ShowOnTopRow { get; set; }  // To highlight certain members
	}

	public class TeamMemberPageBanner
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string BannerImage { get; set; }
	}

    public class TeamPageViewModel
    {
        public string BannerImage { get; set; }
        public List<TeamMemberMaster> Members { get; set; }
    }
}
