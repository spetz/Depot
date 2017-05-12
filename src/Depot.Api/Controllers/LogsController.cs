using Depot.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Depot.Api.Controllers
{
    [Route("[controller]")]
    public class LogsController : Controller
    {
        private readonly ILogRepository _repository;

        public LogsController(ILogRepository repository)
        {
           _repository = repository;
        }

        [HttpGet]
        public IActionResult Get() => Json(_repository.Logs);        
    }
}