using AutoMapper;
using MapProject.Api.DTOs.MapIdentityDescriptionDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Driver;

namespace MapProject.Api.Services.MapIdentityDescriptionService
{
    public class MapIdentityDescriptionService : IMapIdentityDescriptionService
    {
        private readonly IMongoCollection<MapIdentityDescription> _collection;
        private readonly IMapper _mapper;

        public MapIdentityDescriptionService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<MapIdentityDescription>(_databaseSettings.MapIdentityDescriptionCollectionName);
            _mapper = mapper;
        }

        public async Task CreateMapIdentityDescriptionDto(CreateMapIdentityDescriptionDto dto)
        {
            await _collection.InsertOneAsync(_mapper.Map<MapIdentityDescription>(dto));
        }

        public async Task DeleteMapIdentityDescriptionDto(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<ResultMapIdentityDescriptionDto> GetMapIdentityDescription()
        {
            // Veritabanından veriyi çek
            var value = await _collection.Find(x => true).FirstOrDefaultAsync();

            if (value == null)
            {
                return new ResultMapIdentityDescriptionDto();
            }

            return _mapper.Map<ResultMapIdentityDescriptionDto>(value);
        }

        public async Task<UpdateMapIdentityDescriptionDto> GetByIdMapIdentityDescription(string id)
        {
            var value = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<UpdateMapIdentityDescriptionDto>(value);
        }

        public async Task UpdateMapIdentityDescriptionDto(UpdateMapIdentityDescriptionDto dto)
        {
            var entity = _mapper.Map<MapIdentityDescription>(dto);
            // Filtreleme: Id'si eşleşen dökümanı bul
            var filter = Builders<MapIdentityDescription>.Filter.Eq(x => x.Id, dto.Id);

            // ReplaceOne kullanırken nesnenin ID'sinin filtredekiyle aynı olduğundan emin olun
            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}