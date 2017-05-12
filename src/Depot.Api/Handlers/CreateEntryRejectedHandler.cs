using System;
using System.Threading.Tasks;
using Depot.Api.Repositories;
using Depot.Messages.Events;
using RawRabbit;

namespace Depot.Api.Handlers
{
    public class CreateEntryRejectedHandler : IEventHandler<CreateEntryRejected>
    {
        private readonly IBusClient _busClient;
        private readonly ILogRepository _repository;

        public CreateEntryRejectedHandler(IBusClient busClient, ILogRepository repository)
        {
            _busClient = busClient;
            _repository = repository;
        }

        public async Task HandleAsync(CreateEntryRejected command)
        {
            var message = $"{DateTime.UtcNow}: Could not create an entry with key: '{command.Key}'. {command.Reason}";
            Console.WriteLine(message);
            _repository.Logs.Add(message);
            await Task.CompletedTask;
        }
    }
}