using AutoMapper;
using MapProject.Api.DTOs.UserInformationDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Driver;

namespace MapProject.Api.Services.UserInformationService
{
    public class UserInformationService : IUserInformationService
    {

        private readonly IMongoCollection<UserInformation> _collection;
        private readonly IMapper _mapper;

        public UserInformationService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<UserInformation>(_databaseSettings.UserInformationCollectionName);
            _mapper = mapper;

        }

        public async Task CreateUserInformationDto(CreateUserInformationDto dto)
        {
            await _collection.InsertOneAsync(_mapper.Map<UserInformation>(dto));
        }

        public async Task DeleteUserInformationDto(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<ResultUserInformationDto> GetUserInformation()
        {
            var values = await _collection.Find(x => true).FirstOrDefaultAsync();
            return _mapper.Map<ResultUserInformationDto>(values);
        }

        public async Task UpdateUserInformationDto(UpdateUserInformationDto dto)
        {
            await _collection.ReplaceOneAsync(x => x.Id == dto.Id, _mapper.Map<UserInformation>(dto));
        }
    }
}
