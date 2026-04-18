using AutoMapper;
using MapProject.Api.DTOs.CoureselDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Driver;

namespace MapProject.Api.Services.CourselService
{
    public class CoureselService : ICoureselService
    {

        private readonly IMongoCollection<Couresel> _collection;
        private readonly IMapper _mapper;

        public CoureselService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<Couresel>(_databaseSettings.CoureselCollectionName);
            _mapper = mapper;

        }

        public async Task CreateCoureselService(CreateCoureselDto dto)
        {
            await _collection.InsertOneAsync(_mapper.Map<Couresel>(dto));
        }

        public async Task DeleteCoureselService(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultCoureselDto>> GetAllCouresel()
        {
            var value = await _collection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCoureselDto>>(value);
        }

        public async Task<ResultCoureselDto> GetByIdCouresel(string id)
        {
            var value = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultCoureselDto>(value);
        }

        public async Task UpdateCoureselDto(UpdateCoureselDto dto)
        {
            await _collection.ReplaceOneAsync(x => x.Id == dto.Id, _mapper.Map<Couresel>(dto));
        }

        
    }
}
