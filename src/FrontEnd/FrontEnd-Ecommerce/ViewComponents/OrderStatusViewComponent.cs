using Microsoft.AspNetCore.Mvc;
using FrontEnd_Ecommerce.Models;

public class OrderStatusViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        await Task.Delay(100);
        // Dummy data (later: fetch from DB/service)
        var model = new OrderStatusChartViewModel
        {
            Completed = 65,
            Processing = 25,
            Pending = 10
        };

        return View(model); // looks for Views/Shared/Components/OrderStatus/Default.cshtml
    }
}
