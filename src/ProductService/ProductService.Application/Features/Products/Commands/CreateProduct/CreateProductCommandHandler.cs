using ProductService.Domain.TagManagement.Specifications;
using ProductService.Domain.Tags;
using ProductService.Domain.ValueObjects;

namespace ProductService.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductCommandHandler> _logger;
    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Guid>> IRequestHandler<CreateProductCommand, Result<Guid>>.Handle(CreateProductCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting to create product with Name : {@ProductName}", request.Name);

        var prodcutRepo = _unitOfWork.GetRepository<Product>();
        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var tagRepo = _unitOfWork.GetRepository<Tag>();

        if (await prodcutRepo.AnyAsync(p => p.Name == request.Name, ct)) 
            return DomainErrors.Product.DuplicateName(request.Name);

        if (!await categoryRepo.AnyAsync(c => c.Id == request.CategoryId, ct))
            return DomainErrors.Category.NotFound(request.CategoryId);

        // Validate Tags
        List<Tag> tags = new List<Tag>();
        if (request.TagIds != null && request.TagIds.Any())
        {
            tags = await tagRepo.GetListAsync(new GetTagsByIdsSpec(request.TagIds), ct);

            if (tags.Count != request.TagIds.Count)
            {
                var foundIds = tags.Select(t => t.Id);
                var missingIds = request.TagIds.Except(foundIds);
                return DomainErrors.Tag.NotFoundList(missingIds.ToList());
            }
        }

        var priceResult = Money.Create(request.Price, request.Currency);
        var discountResult = Discount.Create(request.DiscountPercentage, request.DiscountEndDate);
        var mainImageResult = Image.Create(request.MainImage.Url, request.MainImage.AltText);

        if (priceResult.IsFailure) return Result.Failure<Guid>(priceResult.TopError);
        if (discountResult.IsFailure) return Result.Failure<Guid>(discountResult.TopError);
        if (mainImageResult.IsFailure) return Result.Failure<Guid>(mainImageResult.TopError);

        var relatedImages = request.RelatedImages?
            .Select(img => Image.Create(img.Url, img.AltText).Value).ToList();

        var features = request.Features?
            .Select(f => Feature.Create(f.Name, f.Value).Value).ToList();

        return Product.Create(
                        id: Guid.NewGuid(),
                        categoryId: request.CategoryId,
                        name: request.Name,
                        description: request.Description,
                        price: priceResult.Value,
                        discount: discountResult.Value,
                        mainImage: mainImageResult.Value,
                        relatedImages: relatedImages,
                        features: features,
                        tags: tags)

                    // Add to EF Core Change Tracker (Synchronous!)
                    .Tap(product => prodcutRepo.AddAsync(product))
                    .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

                    // Success & Error Logging
                    .Tap(product => _logger.LogInformation(
                        "Product Created Successfully with Id {@ProductId}", product.Id))

                    .TapError(error => _logger.LogError(
                        "Failed to build Product. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                        error.Code, error.Message))

                    .Map(product => product.Id);


    }
}
