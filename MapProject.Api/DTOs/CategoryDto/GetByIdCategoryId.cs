namespace MapProject.Api.DTOs.CategoryDto
{
    public class GetByIdCategoryId
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string? ImageUrl1 { get; set; }
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }
        public bool IsFavorite { get; set; }
    }
}
