using System;
using System.Threading.Tasks;
using Depot.Messages.Events;
using RawRabbit;

namespace Depot.Api.Handlers
{
    public class CreateEntryRejectedHandler : IEventHandler<CreateEntryRejected>
    {
        private readonly IBusClient _busClient;

        public CreateEntryRejectedHandler(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public async Task HandleAsync(CreateEntryRejected command)
        {
            Console.WriteLine($"Could not create an entry with key: '{command.Key}'. {command.Reason}");
            await Task.CompletedTask;
        }
    }
}