using ProductService.Domain.Errors;
using ProductService.Domain.ProductManagement;
using SharedKernel.Primitives;
using SharedKernel.Shared;

namespace ProductService.Domain.TagManagement;

public record Tag : Entity
{
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Tag(Guid id, string name) : base(id)
    {
        Name = name;
    }
    private Tag() { }

    public string Name { get; private set; }
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
}