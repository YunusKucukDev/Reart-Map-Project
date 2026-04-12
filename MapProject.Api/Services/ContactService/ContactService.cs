using AutoMapper;
using MapProject.Api.DTOs.ContactDto;
using MapProject.Api.DTOs.ContactDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Driver;

namespace MapProject.Api.Services.ContactService
{
    public class ContactService : IContactService
    {

        private readonly IMongoCollection<Contact> _collection;
        private readonly IMapper _mapper;

        public ContactService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<Contact>(_databaseSettings.ContactCollectionName);
            _mapper = mapper;

        }

        public async Task CreateContactService(CreateContactDto dto)
        {
            await _collection.InsertOneAsync(_mapper.Map<Contact>(dto));
        }

        public async Task DeleteContactService(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultContactDto>> GetAllContact()
        {
            var value = await _collection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultContactDto>>(value);
        }

        public async Task<GetByIdContactDto> GetByIdContact(string id)
        {
            var value = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdContactDto>(value);
        }

        public async Task UpdateContactDto(UpdateContactDto dto)
        {
            await _collection.ReplaceOneAsync(x => x.Id == dto.Id, _mapper.Map<Contact>(dto));
        }
    }
}
