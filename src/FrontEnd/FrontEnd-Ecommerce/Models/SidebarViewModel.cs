using System.Collections.Generic;

namespace FrontEnd_Ecommerce.Models
{
    public class SidebarViewModel
    {
        public int NotificationsCount { get; set; } = 5;

        // Example: active page, optional
        // public string ActivePage { get; set; } = "Dashboard";

        // Example: dynamic menu items (optional)
        // public List<SidebarMenuItem> MenuItems { get; set; } = new List<SidebarMenuItem>
        // {
        //     new SidebarMenuItem { Title = "Dashboard", Icon = "fas fa-tachometer-alt", Url = "/", IsActive = true },
        //     new SidebarMenuItem
        //     {
        //         Title = "Orders", Icon = "fas fa-shopping-cart", SubItems = new List<SidebarMenuItem>
        //         {
        //             new SidebarMenuItem { Title = "All Orders", Icon = "fas fa-list", Url = "/Orders/Index" },
        //             new SidebarMenuItem { Title = "New Orders", Icon = "fas fa-plus", Url = "/Orders/New" },
        //             new SidebarMenuItem { Title = "Shipped Orders", Icon = "fas fa-truck", Url = "/Orders/Shipped" },
        //             new SidebarMenuItem { Title = "Cancellations Orders", Icon = "fas fa-ban", Url = "/Orders/Cancellations" },
        //         }
        //     },
        //     new SidebarMenuItem { Title = "Payments", Icon = "fas fa-money-bill-wave", Url = "/Payments/Index" },
        //     new SidebarMenuItem { Title = "Cancellations", Icon = "fas fa-ban", Url = "/Cancellations/Index" },
        //     new SidebarMenuItem { Title = "Feedbacks", Icon = "fas fa-comments", Url = "/Feedbacks", Badge = 5 },
        //     new SidebarMenuItem { Title = "Refunds", Icon = "fas fa-undo-alt", Url = "/Refunds" },
        // };
    }

    public class SidebarMenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; } = false;
        public int? Badge { get; set; } = null;
        public List<SidebarMenuItem> SubItems { get; set; } = new List<SidebarMenuItem>();
    }
}
