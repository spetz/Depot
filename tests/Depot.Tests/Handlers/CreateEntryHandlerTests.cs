using System;
using System.Threading.Tasks;
using Depot.Messages.Commands;
using Depot.Messages.Events;
using Depot.Services.Entries.Handlers;
using Depot.Services.Entries.Models;
using Depot.Services.Entries.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using RawRabbit;
using RawRabbit.Configuration.Publish;
using Xunit;

namespace Depot.Tests.Handlers
{
    public class CreateEntryHandlerTests
    {
        public CreateEntryHandler Handler;
        public Mock<IBusClient> BusClientMock;
        public Mock<IEntryRepository> EntryRepositoryMock;
        public Mock<IDistributedCache> CacheMock;
        public CreateEntry Command;

        public CreateEntryHandlerTests()
        {
            BusClientMock = new Mock<IBusClient>();
            EntryRepositoryMock = new Mock<IEntryRepository>();
            CacheMock = new Mock<IDistributedCache>();
            Handler = new CreateEntryHandler(BusClientMock.Object,
                EntryRepositoryMock.Object, CacheMock.Object);
        }

        [Fact]
        public async Task Test()
        {
            //Arrange
            Command = new CreateEntry
            {
                Key = "test",
                Value = "test-value"
            };

            //Act
            await Handler.HandleAsync(Command);

            //Assert
            EntryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Entry>()), Times.Once());
            BusClientMock.Verify(x => x.PublishAsync(It.IsAny<EntryCreated>(), It.IsAny<Guid>(), 
                It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Once());
        }
    }
}