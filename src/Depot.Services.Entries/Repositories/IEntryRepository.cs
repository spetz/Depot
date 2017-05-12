using System.Collections.Generic;
using Depot.Services.Entries.Models;

namespace Depot.Services.Entries.Repositories
{
    public interface IEntryRepository
    {
        ICollection<Entry> Entries { get; }
    }
}