using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapProject.Api.Entities
{
    public class MapIdentityDescription
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Experincenumber { get; set; } = "deneme";
        public string Projectnumber { get; set; } = "deneme";
        public string Description1 { get; set; } = "deneme";
        public string Description2 { get; set; } = "deneme";
        public string Description3 { get; set; } = "deneme";
        public string? ImageUrl1 { get; set; }
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }
    }
}
