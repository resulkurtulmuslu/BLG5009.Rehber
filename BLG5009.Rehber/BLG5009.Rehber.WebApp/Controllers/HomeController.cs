using Microsoft.AspNetCore.Mvc;

namespace BLG5009.Rehber.WebApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
