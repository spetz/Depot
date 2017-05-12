using Microsoft.AspNetCore.Mvc;

namespace Depot.Api.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Welcome to the Depot API!");
        }        
    }
}