using System.Collections.Generic;
using System.Threading.Tasks;
using Depot.Services.Entries.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Depot.Services.Entries.Repositories
{
    public class MongoEntryRepository : IEntryRepository
    {
        private readonly IMongoDatabase _database;

        public MongoEntryRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Entry> GetAsync(string key)
            => await Entries.AsQueryable()
                .FirstOrDefaultAsync(x => x.Key == key);

        public async Task<IEnumerable<Entry>> BrowseAsync()
            => await Entries.AsQueryable().ToListAsync();

        public async Task AddAsync(Entry entry)
            => await Entries.InsertOneAsync(entry);

        private IMongoCollection<Entry> Entries 
            => _database.GetCollection<Entry>("Entries");
    }
}