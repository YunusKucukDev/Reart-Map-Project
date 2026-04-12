using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapProject.Api.Entities
{
    public class UserInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Slogan { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LinkednLink { get; set; }
        public string InstagramLink { get; set; }
        public string XLink { get; set; }
        public string WPLink { get; set; }
    }
}
