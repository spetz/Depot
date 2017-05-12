using System.Collections.Generic;
using Depot.Services.Entries.Models;

namespace Depot.Services.Entries.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private static readonly ICollection<Entry> _entries = new List<Entry>();
        public ICollection<Entry> Entries => _entries;        
    }
}