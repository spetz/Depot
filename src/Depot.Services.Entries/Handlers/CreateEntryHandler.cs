using System;
using System.Threading.Tasks;
using Depot.Messages.Commands;
using Depot.Messages.Events;
using RawRabbit;

namespace Depot.Services.Entries.Handlers
{
    public class CreateEntryHandler : ICommandHandler<CreateEntry>
    {
        private readonly IBusClient _busClient;

        public CreateEntryHandler(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public async Task HandleAsync(CreateEntry command)
        {
            if(string.IsNullOrWhiteSpace(command.Key))
            {
                var reason = "Entry key can not be empty";
                Console.WriteLine(reason);
                await _busClient.PublishAsync(new CreateEntryRejected(command.Key, reason));

                return;
            }
            Console.WriteLine($"Created a new entry with key: '{command.Key}'.");
            await _busClient.PublishAsync(new EntryCreated(command.Key));
        }
    }
}