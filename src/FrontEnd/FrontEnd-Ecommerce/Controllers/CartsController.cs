using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    public class CartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add(Guid Id)
        {
            // Do Logic Here
            return View();
        }
    }
}
