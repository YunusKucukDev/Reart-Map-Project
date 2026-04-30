using AutoMapper;
using MapProject.Api.DTOs.VideoDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Driver;

namespace MapProject.Api.Services.VideoService
{
    public class VideoService : IVideoService
    {
        private readonly IMongoCollection<Video> _collection;
        private readonly IMapper _mapper;

        public VideoService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _collection = database.GetCollection<Video>(databaseSettings.VideoCollectionName);
            _mapper = mapper;
        }

        public async Task<List<ResultVideoDto>> GetAllVideos()
        {
            var values = await _collection.Find(_ => true).SortBy(x => x.Order).ToListAsync();
            return _mapper.Map<List<ResultVideoDto>>(values);
        }

        public async Task<ResultVideoDto?> GetByIdVideo(string id)
        {
            var value = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return value is null ? null : _mapper.Map<ResultVideoDto>(value);
        }

        public async Task<ResultVideoDto?> GetFeaturedVideo()
        {
            var value = await _collection.Find(x => x.IsFeatured).FirstOrDefaultAsync();
            return value is null ? null : _mapper.Map<ResultVideoDto>(value);
        }

        public async Task CreateVideo(CreateVideoDto dto)
        {
            if (dto.IsFeatured)
            {
                var unsetAll = Builders<Video>.Update.Set(x => x.IsFeatured, false);
                await _collection.UpdateManyAsync(_ => true, unsetAll);
            }
            await _collection.InsertOneAsync(_mapper.Map<Video>(dto));
        }

        public async Task UpdateVideo(UpdateVideoDto dto)
        {
            await _collection.ReplaceOneAsync(x => x.Id == dto.Id, _mapper.Map<Video>(dto));
        }

        public async Task DeleteVideo(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task SetFeatured(string id)
        {
            var unsetAll = Builders<Video>.Update.Set(x => x.IsFeatured, false);
            await _collection.UpdateManyAsync(_ => true, unsetAll);

            var setOne = Builders<Video>.Update.Set(x => x.IsFeatured, true);
            await _collection.UpdateOneAsync(x => x.Id == id, setOne);
        }
    }
}
