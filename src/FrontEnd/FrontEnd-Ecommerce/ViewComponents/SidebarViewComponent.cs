using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FrontEnd_Ecommerce.Models;

public class SidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // Example: dynamic notifications
        var sidebarModel = new SidebarViewModel
        {
            NotificationsCount = 5,
            // You could load menu items from a database here if needed
        };

        return View(sidebarModel);
    }
}
