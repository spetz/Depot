using System.Collections.Generic;
using System.Threading.Tasks;
using Depot.Services.Entries.Models;

namespace Depot.Services.Entries.Repositories
{
    public interface IEntryRepository
    {
        Task<Entry> GetAsync(string key);
        Task<IEnumerable<Entry>> BrowseAsync();
        Task AddAsync(Entry entry);
    }
}