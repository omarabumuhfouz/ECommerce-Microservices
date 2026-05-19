using ProductService.Domain.Errors;
using ProductService.Domain.Tags;
using ProductService.Domain.ValueObjects;
using SharedKernel.Primitives;
using SharedKernel.Primitives.Results;

namespace ProductService.Domain.Products;

public sealed class Product : AggregateRoot, IAuditableEntity, ISoftDeletable
{
    public IReadOnlyCollection<Image> RelatedImages => _relatedImages.AsReadOnly();
    public IReadOnlyCollection<Feature> Features => _features.AsReadOnly();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

    internal Product(
        Guid id,
        Guid categoryId,
        string name,
        string description,
        Money price,
        Discount discount,
        Image mainImage,
        List<Image>? relatedImages = null,
        List<Feature>? features = null,
        List<Tag>? tags = null
    ) : base(id)
    {
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Price = price;
        Discount = discount;
        MainImage = mainImage;
        _relatedImages = relatedImages?.ToList() ?? new();
        _features = features?.ToList() ?? new();
        _tags = tags?.ToList() ?? new();
    }



    #pragma warning disable CS8618 
    private Product() { }
    #pragma warning restore CS8618

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; } = Money.Zero();
    public Discount Discount { get; private set; } = Discount.Zero();
    public Guid CategoryId { get; private set; }
    public Image MainImage { get; private set; }

    public DateTime CreatedOnUtc { get ; set ; }
    public DateTime? ModifiedOnUtc { get ; set ; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    private List<Image> _relatedImages;
    private readonly List<Feature> _features;
    private readonly List<Tag> _tags;


    public static Result<Product> Create(
        Guid id,
        Guid categoryId,
        string name,
        string description,
        Money price,
        Discount discount,
        Image mainImage,
        List<Image>? relatedImages = null,
        List<Feature>? features = null,
        List<Tag>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Product>(DomainErrors.Product.EmptyName);

        if (price is null || price.Amount < 0)
            return Result.Failure<Product>(DomainErrors.Product.InvalidPrice);

        var product = new Product(
            id,
            categoryId,
            name,
            description,
            price,
            discount,
            mainImage,
            relatedImages,
            features,
            tags);

        return Result.Success(product);
    }

    #region Discount Management

    public Result EditDiscount(int percentage, DateTime? endDate = null)
    {
        if (!IsDisountExists()) return DomainErrors.Discount.NotFound;

        var discountResult = Discount.Create(percentage, endDate);
        if (discountResult.IsFailure) return discountResult.TopError;

        Discount = discountResult.Value;

        return Result.Success();
    }

    public Result RemoveDiscount()
    {
        if(!IsDisountExists()) return DomainErrors.Discount.NotFound;

        Discount = Discount.Create(0, null).Value;
        return Result.Success();    
    }

    public bool IsDisountExists() => Discount.Percentage > 0;

    #endregion

    #region Main Info Management


    public Result EditMainInfo(string name, string description)
    {
        var isValid = Result.Success()
                                .FailIf(_ValidateName(name), DomainErrors.Product.EmptyName)
                                .FailIf(_ValidateDescription(description), DomainErrors.Product.DescriptionRequired);

        if (isValid.IsFailure) return isValid.TopError;

        if (Name != name) RaiseDomainEvent(new ChangeProductNameDomainEvent(Id, name));

        Name = name;
        Description = description;

        return Result.Success();
    }
 
    public Result EditCategory(Guid newCategoryId)
    {
        if (_ValidateCategoryId(newCategoryId)) return DomainErrors.Category.IdRequired;

        CategoryId = newCategoryId;

        return Result.Success();
    }

    public Result EditPrice(decimal price, string currency)
    {
        var priceResult = Money.Create(price, currency);
        if (priceResult.IsFailure) return priceResult.TopError;

        Price = priceResult.Value;
        return Result.Success();
    }

    #endregion

    #region Validation Management

    private bool _ValidateName(string name) => string.IsNullOrWhiteSpace(name);
    private bool _ValidateDescription(string description) => string.IsNullOrWhiteSpace(description);
    private bool _ValidateCategoryId(Guid categoryId) => categoryId == Guid.Empty;

    #endregion

    #region Image Management


    public Result AddRelatedImages(List<Image> addedRelatedImages)
    {
        if (addedRelatedImages is null) return Result.Failure(DomainErrors.Image.ListNull);

        var existingUrls = _relatedImages.Select(ri => ri.Url).ToHashSet().Distinct();

        foreach (var newImageDto in addedRelatedImages)
        {
            if (newImageDto is null) return DomainErrors.Image.ItemNull;

            if (existingUrls.Contains(newImageDto.Url)) return DomainErrors.Image.DuplicateUrl(newImageDto.Url);
        }

        var imagesToAdd = new List<Image>();

        foreach (var addedimage in addedRelatedImages)
        {
            Result<Image> creationResult = Image.Create(addedimage.Url, addedimage.AltText);

            if (creationResult.IsFailure) return creationResult.TopError;

            imagesToAdd.Add(creationResult.Value);
        }

        _relatedImages.AddRange(imagesToAdd);

        return Result.Success();
    }

    public Result RemoveRelatedImage(string url)
    {
        if(string.IsNullOrWhiteSpace(url)) return DomainErrors.Image.UrlRequired;

        var desiredImage = RelatedImages.FirstOrDefault(i => i.Url == url);

        if (desiredImage is null) return DomainErrors.Image.NotFound(url);

        _relatedImages.Remove(desiredImage);

        return Result.Success();
    }

    public Result ReplaceRelatedImage(string oldUrl, string newUrl, string? newAltText)
    {
        if(string.IsNullOrWhiteSpace(oldUrl)) return DomainErrors.Image.UrlRequired;

        var existingImage = _relatedImages.FirstOrDefault(ri => ri.Url == oldUrl);

        if (existingImage is null) return DomainErrors.Image.NotFound(oldUrl);

        var newImageValue = Image.Create(newUrl, newAltText);
        if (newImageValue.IsFailure) return newImageValue.TopError;

        if (oldUrl != newUrl && RelatedImages.Any(i => i.Url == newUrl)) return DomainErrors.Image.AlreadyExists(newUrl);


        _relatedImages.Remove(existingImage);
        _relatedImages.Add(newImageValue.Value);

        return Result.Success();    
    }

    public Result ReplaceMainImage(string newUrl, string? altText)
    {
        var newMainImageResult = Image.Create(newUrl, altText);
        if (newMainImageResult.IsFailure) return newMainImageResult.TopError;

        MainImage = newMainImageResult.Value;

        return Result.Success();
    }

    public void ClearImages() => _relatedImages.Clear();
    
    #endregion

    #region Feature Management 
    public Result AddFeature(string name, string value)
    {
        var addedFeatureResult = Feature.Create(name, value);
        if (addedFeatureResult.IsFailure) return addedFeatureResult.TopError;

        _features.Add(addedFeatureResult.Value);

        return Result.Success();
    }

    public Result AddFeatures(List<Feature> featuresToAdd)
    {
        foreach (var addedfeature in featuresToAdd)
            if (_features.Any(f => f.Name.Contains(addedfeature.Name))) return DomainErrors.Feature.AlreadyExists(addedfeature.Name);

        _features.AddRange(featuresToAdd);

        return Result.Success();
    }

    public Result RemoveFeature(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return DomainErrors.Feature.NameRequired;

        var desiredFeature = Features.FirstOrDefault(f => f.Name == name);

        if (desiredFeature is null) return DomainErrors.Feature.NotFound(name);

        _features.Remove(desiredFeature);
        return Result.Success();
    }

    public Result EditFeature(string oldName, string newName, string newValue)
    {
        if (string.IsNullOrWhiteSpace(oldName)) return DomainErrors.Feature.NameRequired;

        var existingFeature = Features.FirstOrDefault(f => f.Name == oldName);
        if (existingFeature is null) return DomainErrors.Feature.NotFound(oldName);

        var newFeatureResult = Feature.Create(newName, newValue);
        if (newFeatureResult.IsFailure) return newFeatureResult.TopError;

        _features.Remove(existingFeature);
        _features.Add(newFeatureResult.Value);

        return Result.Success();
    }

    public void ClearFeatures() => _features.Clear();

    #endregion

    #region Tag Management

    public Result AddTag(Tag tag)
    {
        if (_tags.Any(t => t.Id == tag.Id)) return DomainErrors.Tag.AlreadyExists(tag.Id);

        _tags.Add(tag);
        return Result.Success();
    }

    public Result RemoveTag(Tag tag)
    {
        var tagToRemove = Tags.FirstOrDefault(t => t.Id == tag.Id);

        if (tagToRemove is null) return DomainErrors.Tag.NotFound(tag.Id);

        _tags.Remove(tagToRemove);

        return Result.Success();
    }

    public void ClearTags() => _tags.Clear();



    #endregion

    public Result Delete()
    {
        IsDeleted = true;
        DeletedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Restore()
    {
        IsDeleted = false;
        DeletedOnUtc = null;

        return Result.Success();
    }

}