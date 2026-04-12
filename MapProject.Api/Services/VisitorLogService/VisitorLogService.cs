using AutoMapper;
using MapProject.Api.DTOs.VisitorLogDto;
using MapProject.Api.Entities;
using MapProject.Api.Settings;
using MongoDB.Driver;

namespace MapProject.Api.Services.VisitorLogService
{
    public class VisitorLogService : IVisitorLogService
    {
        private readonly IMongoCollection<VisitorLog> _collection;
        private readonly IMapper _mapper;

        public VisitorLogService(IMapper mapper, IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<VisitorLog>(settings.VisitorLogCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateVisitorLogDto dto)
        {
            await _collection.InsertOneAsync(_mapper.Map<VisitorLog>(dto));
        }

        public async Task<long> GetTotalCountAsync()
        {
            return await _collection.CountDocumentsAsync(FilterDefinition<VisitorLog>.Empty);
        }

        public async Task<List<ResultVisitorLogDto>> GetRecentAsync(int count)
        {
            var list = await _collection
                .Find(FilterDefinition<VisitorLog>.Empty)
                .SortByDescending(x => x.VisitedAt)
                .Limit(count)
                .ToListAsync();
            return _mapper.Map<List<ResultVisitorLogDto>>(list);
        }

        public async Task<List<DailyVisitorCountDto>> GetDailyCountsAsync(int days)
        {
            var since = DateTime.UtcNow.Date.AddDays(-days + 1);
            var filter = Builders<VisitorLog>.Filter.Gte(x => x.VisitedAt, since);
            var all = await _collection.Find(filter).ToListAsync();

            var grouped = all
                .GroupBy(x => x.VisitedAt.Date)
                .Select(g => new DailyVisitorCountDto
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            // Boş günleri de doldur
            var result = new List<DailyVisitorCountDto>();
            for (int i = 0; i < days; i++)
            {
                var date = since.AddDays(i).ToString("yyyy-MM-dd");
                var found = grouped.FirstOrDefault(x => x.Date == date);
                result.Add(found ?? new DailyVisitorCountDto { Date = date, Count = 0 });
            }
            return result;
        }
    }
}
