namespace FrontEnd_Ecommerce.Controllers;

using Microsoft.AspNetCore.Mvc;

public class CategoriesController : Controller
{
    public IActionResult Index()
    {
        // later you can load categories from database/service
        return View();
    }
}
