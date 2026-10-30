using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Areas.Leader.Controllers
{
    [Area("Leader")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
