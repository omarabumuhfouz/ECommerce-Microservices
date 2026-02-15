using Microsoft.AspNetCore.Mvc;

namespace FrontEnd_Ecommerce.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            return View();
        }

        public IActionResult Delete(Guid id)
        {
            return View();
        }


        public IActionResult Details(Guid id)
        {
            return View();
        }
    }
}
