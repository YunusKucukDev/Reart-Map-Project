using AutoMapper;
using MapProject.Api.DTOs.CategoryDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Driver;

namespace MapProject.Api.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {

        private readonly IMongoCollection<Category> _collection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;

        }

        public async Task CreateCategoryService(CreateCategoryDto dto)
        {
            await _collection.InsertOneAsync(_mapper.Map<Category>(dto));
        }

        public async Task DeleteCategoryService(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategory()
        {
            var value = await _collection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(value);
        }

        public async Task<List<ResultCategoryDto>> GetFavoriteCategories()
        {
            var value = await _collection.Find(x => x.IsFavorite == true).ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(value);
        }

        public async Task<GetByIdCategoryId> GetByIdCategory(string id)
        {
            var value = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdCategoryId>(value);
        }

        public async Task UpdateCategoryDto(UpdateCategoryDto dto)
        {
            await _collection.ReplaceOneAsync(x => x.Id == dto.Id, _mapper.Map<Category>(dto));
        }
    }
}
