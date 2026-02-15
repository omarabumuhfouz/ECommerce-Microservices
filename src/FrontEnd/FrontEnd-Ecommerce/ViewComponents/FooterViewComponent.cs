namespace FrontEnd_Ecommerce.ViewComponent;

using Microsoft.AspNetCore.Mvc;

public class FooterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // For now, no dynamic data is needed. You can extend this later (e.g., dynamic year, links, etc.)
        return View(); // Views/Shared/Components/Footer/Default.cshtml
    }
}
