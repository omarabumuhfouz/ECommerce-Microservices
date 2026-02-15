using Microsoft.AspNetCore.Mvc;
using FrontEnd_Ecommerce.Services;
using FrontEnd_Ecommerce.Services.Implementations;

public class ProductListingViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public ProductListingViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public IViewComponentResult Invoke()
    {
        var products = _productService.GetProducts();
        return View(products);
    }
}
