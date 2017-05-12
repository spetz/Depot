using System;
using System.Collections.Generic;

namespace Depot.Api.Repositories
{
    public class LogRepository : ILogRepository
    {
        private static readonly ICollection<string> _logs = new List<string>();
        public ICollection<string> Logs => _logs;
    }
}