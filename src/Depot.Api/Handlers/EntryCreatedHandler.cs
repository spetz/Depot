using System;
using System.Threading.Tasks;
using Depot.Messages.Events;
using RawRabbit;

namespace Depot.Api.Handlers
{
    public class EntryCreatedHandler : IEventHandler<EntryCreated>
    {
        private readonly IBusClient _busClient;

        public EntryCreatedHandler(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public async Task HandleAsync(EntryCreated command)
        {
            Console.WriteLine($"New entry with key: '{command.Key}' was created.");
            await Task.CompletedTask;
        }
    }
}