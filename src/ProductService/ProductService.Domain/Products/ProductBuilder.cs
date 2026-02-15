using ProductService.Domain.ProductManagement;
using ProductService.Domain.TagManagement;
using ProductService.Domain.ValueObjects;

public class ProductBuilder
{
    private Guid _id;
    private string _name;
    private string _description;
    private Money _price;
    private Discount _discount;
    private int _stockQuantity;
    private bool _isAvailable;
    private Guid _categoryId;
    private Image _mainImage;
    private readonly List<Image> _relatedImages;
    private readonly List<Feature> _features;
    private readonly List<Tag> _tags;

    private List<string>_builderMessageErrors;

    public ProductBuilder()
    {
        // Set defaults
        _id = Guid.NewGuid();
        _stockQuantity = 0;
        _isAvailable = false;
        _price = Money.Zero();
        _discount = Discount.Zero();
        _relatedImages = new();
        _features = new();
        _tags = new();
        _builderMessageErrors = new();
    }

    #region  MaiInfo Management
    public ProductBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = Money.Create(price).Value;
        return this;
    }

    public ProductBuilder WithPrice(decimal price, string currency)
    {
        _price = Money.Create(price, currency).Value;
        return this;
    }

    public ProductBuilder WithDiscount(int percentage, DateTime? expiryDate = null)
    {
        _discount = Discount.Create(percentage, expiryDate).Value;
        return this;
    }

    public ProductBuilder WithStockQuantity(int quantity)
    {
        _stockQuantity = quantity;
        _isAvailable = quantity > 0;
        return this;
    }

    public ProductBuilder WithAvailability(bool isAvailable)
    {
        _isAvailable = isAvailable;
        return this;
    }

    public ProductBuilder WithCategoryId(Guid categoryId)
    {
        _categoryId = categoryId;
        return this;
    }

    #endregion

    #region  Image Methods
    public ProductBuilder WithMainImage(Image image)
    {
        _mainImage = image;
        return this;
    }

    public ProductBuilder WithMainImage(string imageUrl, string? mainAltText )
    {
        _mainImage = Image.Create(imageUrl, mainAltText).Value;
        return this;
    }

    public ProductBuilder AddRelatedImage(Image image)
    {
        if (image is not null)
        {
            _relatedImages.Add(image);
        }
        return this;
    }

    public ProductBuilder AddRelatedImage(string imageUrl)
    {
        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            _relatedImages.Add(Image.Create(imageUrl).Value);
        }
        return this;
    }

    public ProductBuilder AddRelatedImages(IEnumerable<Image>? images)
    {
        if (images is not null && images.Any())
        {
            _relatedImages.AddRange(images);
        }
        return this;
    }

    public ProductBuilder AddRelatedImages(params string[] imageUrls)
    {
        if (imageUrls != null)
        {
            foreach (var url in imageUrls.Where(u => !string.IsNullOrWhiteSpace(u)))
            {
                _relatedImages.Add(Image.Create(url).Value);
            }
        }
        return this;
    }

    #endregion

   #region  Feature Methods
    public ProductBuilder AddFeature(Feature feature)
    {
        if (feature != null)
        {
            _features.Add(feature);
        }
        return this;
    }

    public ProductBuilder AddFeature(string name, string value)
    {
        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
        {
            _features.Add(Feature.Create(name, value).Value);
        }
        return this;
    }

    public ProductBuilder AddFeatures(IEnumerable<Feature>? features)
    {
        if (features is not null && features.Any())
        {
            _features.AddRange(features);
        }
        return this;
    }

    #endregion

    #region  Tags methods
    public ProductBuilder AddTag(Tag tag)
    {
        if (tag != null)
        {
            _tags.Add(tag);
        }
        return this;
    }

    public ProductBuilder AddTag(string tagName)
    {
        if (!string.IsNullOrWhiteSpace(tagName))
        {
            _tags.Add(Tag.Create(tagName).Value);
        }
        return this;
    }

    public ProductBuilder AddTags(IEnumerable<Tag> tags)
    {
        if (tags != null)
        {
            _tags.AddRange(tags);
        }
        return this;
    }

    public ProductBuilder AddTags(List<string>? tagNames)
    {
        if (tagNames is not null && tagNames.Any())
        {
            foreach (var name in tagNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                _tags.Add(Tag.Create(name).Value);
            }
        }
        return this;
    }

    #endregion

    public ProductBuilder Reset()
    {
        _id = Guid.Empty;
        _categoryId = Guid.Empty;
        _name = string.Empty;
        _description = string.Empty;
        _price = Money.Zero();
        _discount = Discount.Zero();
        _stockQuantity = 0;
        _isAvailable = false;
        _mainImage = null;
        _tags.Clear();
        _relatedImages.Clear();
        _features.Clear();
        _builderMessageErrors.Clear();

        return this;
    }

    public Product Build()
    {
        try
        {
            _EnsureIdInitialized();
            _ValidateBuildPreconditions();

            return _ConstructProduct();
        }
        finally
        {
            this.Reset();
        }
    }

    public bool  TryBuild(out Product? product, out List<string> errors)
    {
        try
        {
            product = Build();
            errors = _builderMessageErrors;
            return true;
        }
        catch (ProductBuildException ex)
        {
            errors = ex.ValidationErrors.ToList();
            product = null;
            return false;
        }
    }



    private bool _CanBuild()
    {
        _builderMessageErrors.Clear();

        _ValidateId();
        _ValidateName();
        _ValidateDescription();
        _ValidatePrice();
        _ValidateStockQuantity();
        _ValidateCategoryId();
        _ValidateMainImage();

        return !_HasValidationErrors();
    }

    #region  Validation Management
    private void _ValidateBuildPreconditions()
    {
        if (!this._CanBuild())
        {
            throw new ProductBuildException("Product Build Failed", _builderMessageErrors);
        }
    }
    private bool _HasValidationErrors()
    {
        return _builderMessageErrors.Any();
    }
    private void _EnsureIdInitialized()
    {
        if (_id == Guid.Empty)
            _id = Guid.NewGuid();
    }

    private void _ValidateId()
    {
        if (_id == Guid.Empty)
            _builderMessageErrors.Add("Id must be Exists.");
    }

    private void _ValidateName()
    {
        if (string.IsNullOrWhiteSpace(_name))
            _builderMessageErrors.Add("Product Name Must be provided.");
    }

    private void _ValidateDescription()
    {
        if (string.IsNullOrWhiteSpace(_description))
            _builderMessageErrors.Add("Product Description Must be provided.");
    }

    private void _ValidatePrice()
    {
        if (_price is null)
            _builderMessageErrors.Add("Product Price Must be provided.");
    }

    private void _ValidateStockQuantity()
    {
        if (_stockQuantity < 0)
            _builderMessageErrors.Add("Product Stock Quantity must be more than or equal zero.");
    }

    private void _ValidateCategoryId()
    {
        if (_categoryId == Guid.Empty)
            _builderMessageErrors.Add("Category Id Must be provided.");
    }
    
    private void _ValidateMainImage()
    {
        if (_mainImage is null)
            _builderMessageErrors.Add("Product main image must be provied.");
    }

    #endregion
    public static ProductBuilder CreateNew() => new();

    private Product _ConstructProduct()
    {
        return new Product
        (
            _id,
            _categoryId,
            _name,
            _description,
            _stockQuantity,
            _isAvailable,
            _price,
            _discount,
            _mainImage,
            _relatedImages,
            _features,
            _tags
        );
    }
}