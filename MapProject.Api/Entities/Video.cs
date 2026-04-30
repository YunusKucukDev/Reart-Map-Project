using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapProject.Api.Entities
{
    public class Video
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? YoutubeVideoId { get; set; }
        public string? VideoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Tag { get; set; }
        public string? Duration { get; set; }
        public bool IsFeatured { get; set; }
        public int Order { get; set; }
    }
}
