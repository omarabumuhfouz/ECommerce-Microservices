using ProductService.Domain.Errors;
using ProductService.Domain.TagManagement;
using ProductService.Domain.TagManagement.Specifications;
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

    async Task<Result<Guid>> IRequestHandler<CreateProductCommand, Result<Guid>>.Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to create product with Name : {@ProductName}", request.Name);

        var prodcutRepo = _unitOfWork.GetRepository<Product>();
        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var tagRepo = _unitOfWork.GetRepository<Tag>();

        if (await prodcutRepo.IsExistsAsync(p => p.Name == request.Name, cancellationToken)) 
            return DomainErrors.Product.DuplicateName(request.Name);

        if (!await categoryRepo.IsExistsAsync(c => c.Id == request.CategoryId, cancellationToken))
            return DomainErrors.Category.NotFound(request.CategoryId);

        // Validate Tags
        List<Tag> tags = new List<Tag>();
        if (request.TagIds != null && request.TagIds.Any())
        {
            tags = await tagRepo.ListAsync(new GetTagsByIdsSpec(request.TagIds), cancellationToken);

            if (tags.Count != request.TagIds.Count)
            {
                var foundIds = tags.Select(t => t.Id);
                var missingIds = request.TagIds.Except(foundIds);
                return DomainErrors.Tag.NotFoundList(missingIds.ToList());
            }
        }

        // if Failed will throw ProductBuildException
        var productBuilder = ProductBuilder.CreateNew()
                            .WithId(Guid.NewGuid())
                            .WithCategoryId(request.CategoryId)
                            .WithName(request.Name)
                            .WithDescription(request.Description)
                            .WithStockQuantity(request.StockQuantity)
                            .WithAvailability(true)
                            .WithPrice(request.Price, request.Currency)
                            .WithDiscount(request.DiscountPercentage, request.DiscountEndDate)
                            .WithMainImage(request.MainImage.Url, request.MainImage.AltText)
                            .AddRelatedImages(request.RelatedImages?.Select(rid => Image.Create(rid.Url, rid.AltText).Value))
                            .AddFeatures(request.Features?.Select(fd => Feature.Create(fd.Name, fd.Value).Value))
                            .AddTags(tags)
                            .Build();


        await prodcutRepo.AddAsync(productBuilder, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product Creatd Successfully with Id {@ProductId}", productBuilder.Id);

        return productBuilder.Id;

    }
}
