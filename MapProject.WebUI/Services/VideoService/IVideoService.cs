using MapProject.DtoLayer.DTOs.VideoDto;

namespace MapProject.WebUI.Services.VideoService
{
    public interface IVideoService
    {
        Task<List<ResultVideoDto>> GetAllVideos();
        Task<ResultVideoDto?> GetFeaturedVideo();
        Task<UpdateVideoDto?> GetByIdVideo(string id);
        Task CreateVideo(CreateVideoDto dto, IFormFile? videoFile, IFormFile? thumbnailFile);
        Task UpdateVideo(UpdateVideoDto dto, IFormFile? videoFile, IFormFile? thumbnailFile);
        Task DeleteVideo(string id);
        Task SetFeatured(string id);
    }
}
