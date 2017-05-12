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
        {
            var entries = await _repository.BrowseAsync();

            return Json(entries.Select(x => x.Key)); 
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var entry = await _repository.GetAsync(key);
            if(entry == null)
            {
                return NotFound();
            }
            return Json(entry.Value);
        }            
    }
}