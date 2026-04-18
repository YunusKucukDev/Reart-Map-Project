using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapProject.Api.Entities
{
    public class Couresel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Descripton { get; set; }
        public string? ImageUrl { get; set; }
    }
}
