namespace ProductService.Application.Features.Products.Commands.AddDiscount;
public record AddDiscountToProductCommand(Guid ProductId, int DiscountPercentage, DateTime DiscountEndDate) 
    : ICommand<Unit>;
