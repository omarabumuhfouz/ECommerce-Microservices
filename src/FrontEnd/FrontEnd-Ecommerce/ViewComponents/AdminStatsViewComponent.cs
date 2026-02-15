using Microsoft.AspNetCore.Mvc;
using FrontEnd_Ecommerce.Models;

    public class AdminStatsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Later you can fetch from DB/service instead of hardcoding
            var model = new AdminStatsViewModel
            {
                TotalOrders = 1248,
                Revenue = 54823.00m,
                PendingOrders = 18,
                Cancellations = 7
            };

            return View(model); // this will look for Views/Shared/Components/AdminStats/Default.cshtml
        }
    }
