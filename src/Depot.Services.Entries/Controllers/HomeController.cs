using Microsoft.AspNetCore.Mvc;

namespace Depot.Services.Entries.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Welcome to the Entries Service!");
        }        
    }
}