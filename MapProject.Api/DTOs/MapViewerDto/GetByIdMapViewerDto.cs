namespace MapProject.Api.DTOs.MapViewerDto
{
    public class GetByIdMapViewerDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; } // Sunucuda kayıtlı ismi
        public string Url { get; set; }
    }
}
