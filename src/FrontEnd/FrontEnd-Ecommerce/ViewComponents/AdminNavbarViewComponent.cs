using Microsoft.AspNetCore.Mvc;

    public class AdminNavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // If later you want to send notifications, user info, etc. -> pass a model here
            return View();
        }
    }