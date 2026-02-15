using Microsoft.AspNetCore.Mvc;

namespace FrontEnd_Ecommerce.Controllers;


public class TestingController : Controller
{
    public IActionResult AdmainTest() => View();

    public IActionResult Index() => View();
    public IActionResult ProfileTesting() => View();

}