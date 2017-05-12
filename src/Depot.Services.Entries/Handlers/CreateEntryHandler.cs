using System;
using System.Linq;
using System.Threading.Tasks;
using Depot.Messages.Commands;
using Depot.Messages.Events;
using Depot.Services.Entries.Models;
using Depot.Services.Entries.Repositories;
using RawRabbit;

namespace Depot.Services.Entries.Handlers
{
    public class CreateEntryHandler : ICommandHandler<CreateEntry>
    {
        private readonly IBusClient _busClient;
        private readonly IEntryRepository _repository;

        public CreateEntryHandler(IBusClient busClient, IEntryRepository repository)
        {
            _busClient = busClient;
            _repository = repository;
        }

        public async Task HandleAsync(CreateEntry command)
        {
            if(string.IsNullOrWhiteSpace(command.Key))
            {
                await PublishError(command.Key, "Entry key can not be empty.");
                return;
            }
            var entry = _repository.Entries.SingleOrDefault(x => 
                x.Key == command.Key.Trim().ToLowerInvariant());
            if(entry != null)
            {
                await PublishError(command.Key, $"Entry with key: '{command.Key}' already exists.");
                return;
            }
            _repository.Entries.Add(new Entry(command.Key, command.Value));
            Console.WriteLine($"Created a new entry with key: '{command.Key}'.");
            await _busClient.PublishAsync(new EntryCreated(command.Key));
        }

        private async Task PublishError(string key, string reason)
        {
            Console.WriteLine(reason);
            await _busClient.PublishAsync(new CreateEntryRejected(key, reason));

            return;            
        }
    }
}