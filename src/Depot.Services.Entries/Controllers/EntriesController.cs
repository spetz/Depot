using System.Linq;
using System.Threading.Tasks;
using Depot.Services.Entries.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RawRabbit;

namespace Depot.Services.Entries.Controllers
{
    [Route("[controller]")]
    public class EntriesController : Controller
    {
        private readonly IEntryRepository _repository;
        private readonly IDistributedCache _cache;

        public EntriesController(IEntryRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var entries = await _repository.BrowseAsync();

            return Json(entries.Select(x => x.Key)); 
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var cacheItem = await _cache.GetStringAsync(key);
            if(cacheItem != null)
            {
                return Json(JsonConvert.DeserializeObject(cacheItem));
            }
            var entry = await _repository.GetAsync(key);
            if(entry == null)
            {
                return NotFound();
            }
            return Json(entry.Value);
        }            
    }
}