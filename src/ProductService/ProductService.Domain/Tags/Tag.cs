using ProductService.Domain.Errors;
using ProductService.Domain.Products;
using SharedKernel.Primitives;
using SharedKernel.Primitives.Results;

namespace ProductService.Domain.Tags;

public sealed class Tag : Entity, IAuditableEntity, ISoftDeletable
{
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Tag(Guid id, string name) : base(id)
    {
        Name = name;
    }

    #pragma warning disable CS8618
    private Tag() { }
    #pragma warning restore CS8618
    public string Name { get; private set; }
    public DateTime CreatedOnUtc { get ; set ; }
    public DateTime? ModifiedOnUtc { get ; set ; }

    public bool IsDeleted { get ; private set ; }

    public DateTime? DeletedOnUtc { get ; private set ; }

    private readonly List<Product> _products = new();

    public static Result<Tag> Create(string name)
    {
        if (_ValidateName(name)) return DomainErrors.Tag.NameRequired;

        return new Tag(Guid.NewGuid(), name);
    }

    public Result EditName(string name)
    {
        if(_ValidateName(name)) return DomainErrors.Tag.NameRequired;

        Name = name;

        return Result.Success();
    }
    private static bool _ValidateName(string name) => string.IsNullOrWhiteSpace(name);

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