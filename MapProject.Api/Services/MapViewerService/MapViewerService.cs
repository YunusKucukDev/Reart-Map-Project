using AutoMapper;
using MapProject.Api.DTOs.MapViewerDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MapProject.Api.Services.MapViewerService
{
    public class MapViewerService : IMapViewerService
    {
        private string WebUiUrl = "https://ajansreart.com/MapViewer/Viewer/";
        private readonly IMongoCollection<MapViewer> _collection;
        private readonly IMapper _mapper;

        public MapViewerService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<MapViewer>(_databaseSettings.MapViewerCollectionName);
            _mapper = mapper;

        }

        public async Task CreateMapViewer(CreateMapViewerDto dto)
        {
            // MongoDB'de kayıt oluşmadan önce Id'yi manuel üretelim (veya sistemin üretmesini bekleyelim)
            var map = _mapper.Map<MapViewer>(dto);
            map.Id = ObjectId.GenerateNewId().ToString(); // Manuel Id üretimi

            // URL'yi burada oluştur
            map.Url = $"{WebUiUrl}{map.Id}";

            await _collection.InsertOneAsync(map);
        }

        public async Task DeleteMapViewer(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultMapViewerDto>> GetAllMapViewers()
        {
            var value = await _collection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultMapViewerDto>>(value);
        }

        public async Task<GetByIdMapViewerDto> GetByIsMapViewer(string id)
        {
            var value = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdMapViewerDto>(value);
        }

        public async Task<int> IncrementViewCountAsync(string id)
        {
            var update = Builders<MapViewer>.Update.Inc(x => x.ViewCount, 1);
            var result = await _collection.FindOneAndUpdateAsync(
                x => x.Id == id,
                update,
                new FindOneAndUpdateOptions<MapViewer> { ReturnDocument = ReturnDocument.After });
            return result?.ViewCount ?? 0;
        }

        public async Task UpdateMapViewer(UpdateMapViewerDto dto)
        {
            // Önce mevcut kaydı al
            var existingMap = await _collection.Find(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (existingMap == null) return;

            // Sadece değişmesi gereken alanları güncelle
            existingMap.Title = dto.Title;
            existingMap.ImageUrl = dto.ImageUrl;

            // Url'yi güncelleme, o sabit kalmalı (veya gerekiyorsa yeniden oluştur)

            await _collection.ReplaceOneAsync(x => x.Id == dto.Id, existingMap);
        }
    }
}
