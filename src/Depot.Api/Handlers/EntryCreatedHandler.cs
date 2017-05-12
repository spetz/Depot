using System;
using System.Threading.Tasks;
using Depot.Api.Repositories;
using Depot.Messages.Events;
using RawRabbit;

namespace Depot.Api.Handlers
{
    public class EntryCreatedHandler : IEventHandler<EntryCreated>
    {
        private readonly IBusClient _busClient;
        private readonly ILogRepository _repository;

        public EntryCreatedHandler(IBusClient busClient, ILogRepository repository)
        {
            _busClient = busClient;
            _repository = repository;
        }

        public async Task HandleAsync(EntryCreated command)
        {
            var message = $"{DateTime.UtcNow}: New entry with key: '{command.Key}' was created.";
            Console.WriteLine(message);
            _repository.Logs.Add(message);
            await Task.CompletedTask;
        }
    }
}