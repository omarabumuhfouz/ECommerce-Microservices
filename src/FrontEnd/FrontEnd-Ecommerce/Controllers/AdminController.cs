using Microsoft.AspNetCore.Mvc;

namespace FrontEnd_Ecommerce.Controllers;


public class AdminController : Controller
{
    public IActionResult Index() => View();
}