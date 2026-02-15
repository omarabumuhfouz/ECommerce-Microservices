// Controllers/ErrorController.cs
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd_Ecommerce.Controllers
{
    public class ErrorController : Controller
    {
        // Route to capture any error status code from the URL
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            // Clear any pre-existing response content
            Response.Clear();
            // Explicitly set the response status code
            Response.StatusCode = statusCode;
            // Render the appropriate error view based on the status code
            switch (statusCode)
            {
                case 401:
                    return View("Unauthorized");        // For Unauthorized Access
                case 403:
                    return View("Forbidden");           // For Forbidden Access 
                case 404:
                    return View("NotFound");        // For Not Found errors
                case 500:
                    return View("InternalServer");      // For Internal Server errors
                case 503:
                    return View("ServiceUnavailable");  // For Service Unavailable 
                default:
                    return View("InternalServer");
            }
        }
    }
}