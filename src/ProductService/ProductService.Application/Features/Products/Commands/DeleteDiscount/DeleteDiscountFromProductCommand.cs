namespace ProductService.Application.Features.Products.Commands.DeleteDiscount;
public record DeleteDiscountFromProductCommand(Guid ProductId) : ICommand<Unit>;