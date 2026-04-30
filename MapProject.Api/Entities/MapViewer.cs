using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapProject.Api.Entities
{
    public class MapViewer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public int ViewCount { get; set; }
    }
}
