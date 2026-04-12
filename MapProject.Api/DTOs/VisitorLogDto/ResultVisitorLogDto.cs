namespace MapProject.Api.DTOs.VisitorLogDto
{
    public class ResultVisitorLogDto
    {
        public string Id { get; set; }
        public DateTime VisitedAt { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
