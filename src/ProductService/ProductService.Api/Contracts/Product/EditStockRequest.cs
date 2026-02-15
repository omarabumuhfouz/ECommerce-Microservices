using ProductService.Application.Enums;

namespace ProductService.Api.Contracts.Product;
public record EditStockRequest(int Quantity, StockOperation Operation);
