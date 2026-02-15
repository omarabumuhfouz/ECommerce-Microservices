using System.ComponentModel;
using FrontEnd_Ecommerce.Models;
using FrontEnd_Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;


public class NavBarViewComponent(ICartService cartService, ICookieManager cookieManager) : ViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Dummy authentication for testing
        bool isAuthenticated = !string.IsNullOrWhiteSpace(cookieManager.Get(CookieKeys.AccessToken)); // set true to simulate logged in
        string userName = "Omar";    // dummy username
        int count = 10;

        var model = new NavBarViewModel
        {
            // CartCount = await _cartService.GetCartCountAsync(User.Identity?.Name ?? "guest"),
            CartCount = count,
            IsAuthenticated = isAuthenticated,
            UserName = userName
        };

        return View(model);
    }
}
