using ProductService.Domain.Errors;
using SharedKernel.Primitives;
using SharedKernel.Primitives.Results;

namespace ProductService.Domain.Categories;

public sealed class Category : AggregateRoot, IAuditableEntity, ISoftDeletable
{
    private Category(Guid id, CategoryName name, string description , List<Guid> products ) : base(id)
    {
        Name = name;
        Description = description;
        ProductIds = products;
    }

    #pragma warning disable CS8618
    private Category() { }
    #pragma warning restore CS8618

    public CategoryName Name { get; private set; }
    public string Description { get; private set; }

    public List<Guid> ProductIds { get; private set; }

    public DateTime CreatedOnUtc { get ; set ; }
    public DateTime? ModifiedOnUtc { get ; set ; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    // Factory method
    public static  Result<Category> Create(Guid id, string name, string description,  List<Guid> products = null)
    {
        var isValid = Result.Combine(
            ValidateId(id),
            ValidateDescription(description)
        );

        if(isValid.IsFailure) return isValid.TopError;
        
        var nameResult = CategoryName.Create(name);
        if (nameResult.IsFailure) return nameResult.TopError;

        return new Category(id, nameResult.Value, description,  products?.ToList() ?? new List<Guid>());
    }

    private static Result ValidateId(Guid id) => 
        id == Guid.Empty ? Result.Failure(DomainErrors.Category.IdRequired) : Result.Success();

    private static Result ValidateDescription(string desc) =>
        string.IsNullOrWhiteSpace(desc) ? Result.Failure(DomainErrors.Category.DescriptionRequired) : Result.Success();

    public Result EditName(string name)
    {
        var nameResult = CategoryName.Create(name);

        if(nameResult.IsFailure) return nameResult.TopError;

        Name = nameResult.Value;

        return Result.Success(); 
    }

    public Result EditDescription(string description)
    {
        var isValid = ValidateDescription(description);
        if(isValid.IsFailure) return isValid;

        Description = description;

        return Result.Success();
    }


    public Result Edit(string name, string description)
    {
        var descResult = ValidateDescription(description);
        if (descResult.IsFailure) return descResult;

        var nameResult = CategoryName.Create(name);
        if (nameResult.IsFailure) return nameResult.TopError;

        Name = nameResult.Value;
        Description = description;

        return Result.Success();
    }

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
