using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Depot.Api;
using Depot.Messages.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace Depot.Tests.EndToEnd.Controllers
{
    public class EntriesControllerTests
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        public EntriesControllerTests()
        {
            Server = new TestServer(new WebHostBuilder()
                          .UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        [Fact]
        public async Task given_unique_key_entry_should_be_created()
        {
            //Arrange
            var command = new CreateEntry 
            {
                Key = "test",
                Value = "test-entry"
            };
            var payload = GetPayload(command);

            //Act
            var response = await Client.PostAsync("entries", payload);

            //Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Accepted);
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }          
    }
}