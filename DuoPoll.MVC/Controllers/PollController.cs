using Microsoft.AspNetCore.Mvc;

namespace DuoPoll.MVC.Controllers
{
    public class PollController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        // GET
        // public IActionResult Show()
        // {
            // return View();
        // }

        public IActionResult Statistics()
        {
            return View();
        }
    }
}