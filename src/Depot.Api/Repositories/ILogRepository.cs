using System.Collections.Generic;

namespace Depot.Api.Repositories
{
    public interface ILogRepository
    {
         ICollection<string> Logs { get; }
    }
}