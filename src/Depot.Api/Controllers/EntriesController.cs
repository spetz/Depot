using System;
using System.Threading.Tasks;
using Depot.Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Depot.Api.Controllers
{
    [Route("[controller]")]
    public class EntriesController : Controller
    {
        private readonly IBusClient _busClient;

        public EntriesController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateEntry command)
        {   
            if(string.IsNullOrWhiteSpace(command.Key))
            {
               throw new ArgumentException("Entry key can not be empty.", nameof(command.Key));
            }
            await _busClient.PublishAsync(command);

            return Accepted();
        }        
    }
}