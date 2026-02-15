using FrontEnd_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontEnd_Ecommerce.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

 public IActionResult About()
    {
        // Pass any dummy data if needed
        ViewData["Message"] = "This is the About page for testing.";
        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Message"] = "This is the Contact page for testing.";
        return View();
    }

    public IActionResult Error()
    {
          var feature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = feature?.Error;

        // You can log the error here or pass it to the view
        ViewBag.ErrorMessage = exception?.Message;
        ViewBag.StackTrace = exception?.StackTrace;

        return View();
    }
}
