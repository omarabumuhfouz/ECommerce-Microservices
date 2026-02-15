using SharedKernel.Shared;
using ShoppingCartService.Application.Carts.DTOs;
using ShoppingCartService.Application.DTOs;
using ShoppingCartService.Application.Services;
using ShoppingCartService.Domain.CartManagement;

namespace ShoppingCartService.Infrastructure.Services;

public class CartMapper : ICartMapper
{
    private readonly IProductService _productService;

    public CartMapper(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<CartDto>> MapToDTOAsync(Cart cart)
    {
        var productIds = cart.GetProductIds();
        List<ProductNameDto> productNames = new();
        if (productIds.Any())
        {
            var productNamesResult = await _productService.GetProductNamesByIdsAsync(productIds);
            if (productNamesResult.IsFailure) return productNamesResult.TopError;

            productNames = productNamesResult.Value;
        }

        
        var cartItemsDto = cart.CartItems?.Select(ci => new CartItemDto
        {
            Id = ci.Id,
            ProductId = ci.ProductId,
            ProductName = productNames.FirstOrDefault(p => p.Id == ci.ProductId)?.Name ?? "Unknown Product",
            Quantity = ci.Quantity?.Value ?? 0,
            UnitPrice = ci.UnitPrice?.Value ?? 0m,
            Discount = ci.Discount?.Value ?? 0m,
            TotalPrice = ci.TotalPrice?.Value ?? 0m
        }).ToList() ?? new List<CartItemDto>();

        return new CartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            IsCheckedOut = cart.IsCheckedOut,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt,
            CartItems = cartItemsDto,
            TotalBasePrice = cart.CalculateBasePrice(),
            TotalDiscount = cart.CalculateTotalDiscount(),
            TotalAmount = cart.CalculateTotalAmount()
        };
    }

}
