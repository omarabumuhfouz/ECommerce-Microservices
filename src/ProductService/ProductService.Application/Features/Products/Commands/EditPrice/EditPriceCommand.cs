namespace ProductService.Application.Features.Products.Commands.EditPrice;

public record EditPriceCommand(Guid ProductId, decimal NewPrice, string NewCurrency) : ICommand<Unit>;

