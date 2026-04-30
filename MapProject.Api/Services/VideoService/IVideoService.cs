using MapProject.Api.DTOs.VideoDto;

namespace MapProject.Api.Services.VideoService
{
    public interface IVideoService
    {
        Task<List<ResultVideoDto>> GetAllVideos();
        Task<ResultVideoDto?> GetByIdVideo(string id);
        Task<ResultVideoDto?> GetFeaturedVideo();
        Task CreateVideo(CreateVideoDto dto);
        Task UpdateVideo(UpdateVideoDto dto);
        Task DeleteVideo(string id);
        Task SetFeatured(string id);
    }
}
