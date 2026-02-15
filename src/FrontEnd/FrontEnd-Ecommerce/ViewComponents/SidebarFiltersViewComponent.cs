using FrontEnd_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

public class SidebarFiltersViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<CategoryFilter> categories, int minPrice, int maxPrice, string selectedSort)
    {
        return View("Default", new SidebarFiltersModel
        {
            Categories = categories,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SelectedSort = selectedSort
        });
    }
}

    

    
