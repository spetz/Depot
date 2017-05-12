using System;
using System.Linq;
using System.Threading.Tasks;
using Depot.Messages.Commands;
using Depot.Messages.Events;
using Depot.Services.Entries.Models;
using Depot.Services.Entries.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RawRabbit;

namespace Depot.Services.Entries.Handlers
{
    public class CreateEntryHandler : ICommandHandler<CreateEntry>
    {
        private readonly IBusClient _busClient;
        private readonly IEntryRepository _repository;
        private readonly IDistributedCache _cache;

        public CreateEntryHandler(IBusClient busClient, IEntryRepository repository,
            IDistributedCache cache)
        {
            _busClient = busClient;
            _repository = repository;
            _cache = cache;
        }

        public async Task HandleAsync(CreateEntry command)
        {
            if(string.IsNullOrWhiteSpace(command.Key))
            {
                await PublishError(command.Key, "Entry key can not be empty.");
                return;
            }
            var entry = await _repository.GetAsync(command.Key);
            if(entry != null)
            {
                await PublishError(command.Key, $"Entry with key: '{command.Key}' already exists.");
                return;
            }
            await _repository.AddAsync(new Entry(command.Key, command.Value));
            await _cache.SetStringAsync(command.Key, JsonConvert.SerializeObject(command.Value),
                new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10)));
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