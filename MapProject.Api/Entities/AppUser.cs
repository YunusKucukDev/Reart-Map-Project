using AspNetCore.Identity.MongoDbCore.Models;

namespace MapProject.Api.Entities
{
    public class AppUser : MongoIdentityUser<Guid>
    {
        public string? Name { get; set; }
    }
}
