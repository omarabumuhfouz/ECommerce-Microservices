using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Commands.Features.AddFeatures;
public record AddFeaturesToProductCommand(Guid ProductId, List<FeatureDto> Features) : ICommand<Unit>;