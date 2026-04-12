namespace MapProject.Api.DTOs.VisitorLogDto
{
    public class CreateVisitorLogDto
    {
        public DateTime VisitedAt { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
