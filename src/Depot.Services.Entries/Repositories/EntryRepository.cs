using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depot.Services.Entries.Models;

namespace Depot.Services.Entries.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private static readonly ICollection<Entry> _entries = new List<Entry>();

        public async Task<Entry> GetAsync(string key)
            => await Task.FromResult(_entries.SingleOrDefault(x 
                => x.Key == key.Trim().ToLowerInvariant()));

        public async Task<IEnumerable<Entry>> BrowseAsync()
            => await Task.FromResult(_entries);

        public async Task AddAsync(Entry entry)
        {
            _entries.Add(entry);
            await Task.CompletedTask;
        }
    }
}