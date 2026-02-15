using FrontEnd_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

    public class RecentOrdersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var orders = new List<OrderViewModel>
            {
                new OrderViewModel { Id="#ORD-7842", Customer="John Smith", Date="2023-10-15", Amount="$128.50", Status="Completed" },
                new OrderViewModel { Id="#ORD-7841", Customer="Emma Johnson", Date="2023-10-15", Amount="$242.00", Status="Processing" },
                new OrderViewModel { Id="#ORD-7840", Customer="Michael Brown", Date="2023-10-14", Amount="$87.99", Status="Pending" },
                new OrderViewModel { Id="#ORD-7839", Customer="Sarah Wilson", Date="2023-10-14", Amount="$356.75", Status="Cancelled" },
                new OrderViewModel { Id="#ORD-7838", Customer="David Miller", Date="2023-10-13", Amount="$54.25", Status="Completed" }
            };

            return View(orders);
        }
    }

    