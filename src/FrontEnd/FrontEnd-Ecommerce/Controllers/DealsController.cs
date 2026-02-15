using Microsoft.AspNetCore.Mvc;

namespace FrontEnd_Ecommerce.Controllers;

public class DealsController : Controller
{
    public IActionResult Index()
    {
        System.Console.WriteLine("\nIndex in Deals\n");
        // ViewData["Message"] = "Welcome to the Deals page!";ccc
        
        return View();
    }
}
