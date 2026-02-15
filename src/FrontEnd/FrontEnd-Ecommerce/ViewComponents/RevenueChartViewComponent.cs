using FrontEnd_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

    // This ViewComponent will render your chart
     public class RevenueChartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Dummy monthly data (replace with DB/service later)
            var model = new RevenueChartViewModel
            {
                Labels = new List<string>
                {
                    "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                },
                Values = new List<int>
                {
                    12500, 19000, 18000, 22000, 25000, 28000,
                    32000, 35000, 40000, 43000, 48000, 54823
                }
            };

            return View(model); // sends data to Default.cshtml
        }
    }
    // ViewModel for chart data
    
