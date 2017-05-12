using System.Linq;
using System.Threading.Tasks;
using Depot.Services.Entries.Repositories;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Depot.Services.Entries.Controllers
{
    [Route("[controller]")]
    public class EntriesController : Controller
    {
        private readonly IEntryRepository _repository;

        public EntriesController(IEntryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Json(await Task.FromResult(_repository.Entries.Select(x => x.Key)));    

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var entry = _repository.Entries
                .SingleOrDefault(x => x.Key == key.Trim().ToLowerInvariant());
            
            if(entry == null)
            {
                return NotFound();
            }

            return Json(await Task.FromResult(entry.Value));
        }            
    }
}