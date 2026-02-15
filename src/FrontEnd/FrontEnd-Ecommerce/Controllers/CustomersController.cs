using Microsoft.AspNetCore.Mvc;
namespace FrontEnd_Ecommerce.Controllers;


public class CustomersController : Controller
{
    public IActionResult Index() => View();
}