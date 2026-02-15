using FrontEnd_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;


public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(int currentPage, int totalPages)
    {
        return View("Default", new PaginationModel
        {
            CurrentPage = currentPage,
            TotalPages = totalPages
        });
    }
}


