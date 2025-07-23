using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace MakeMoreInternational.Models
{ 
public class AboutUsMaster
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	public string MainImage { get; set; }
	public string MainDescription { get; set; }
	public string OurMission { get; set; }
	public string OurVision { get; set; }

	public List<AboutValue> Values { get; set; } = new();
	public AboutItem WhyWeAre { get; set; } = new();
	public AboutItem WhatWeDo { get; set; } = new();
}

public class AboutValue
{
	public string Image { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}

public class AboutItem
{
	public string Image { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}
}