using FrontEnd_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

    public class HeroViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string title, string subtitle, string buttonText, string buttonUrl)
        {
            return View("Default", new HeroModel
            {
                Title = title,
                Subtitle = subtitle,
                ButtonText = buttonText,
                ButtonUrl = buttonUrl
            });
        }
    }

    