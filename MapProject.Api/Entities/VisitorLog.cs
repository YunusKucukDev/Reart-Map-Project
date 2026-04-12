using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapProject.Api.Entities
{
    public class VisitorLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime VisitedAt { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
