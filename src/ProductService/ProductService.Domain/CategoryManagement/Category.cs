using ProductService.Domain.Category;
using ProductService.Domain.Errors;
using SharedKernel.Primitives.Result;
using SharedKernel.Shared;

namespace ProductService.Domain.CategoryManagement;

public record Category : AggregateRoot
{
    private Category(Guid id, CategoryName name, string description, bool isActive , List<Guid> products ) : base(id)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        ProductIds = products;
    }

    private Category() { }

    public CategoryName Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }

    public List<Guid> ProductIds { get; private set; }

    // Factory method
    public static  Result<Category> Create(Guid id, string name, string description, bool isActive = true, List<Guid> products = null)
    {
        var isValid = Result.Combine(
            ValidateId(id),
            ValidateDescription(description)
        );

        if(isValid.IsFailure) return isValid.TopError;
        
        var nameResult = CategoryName.Create(name);
        if (nameResult.IsFailure) return nameResult.TopError;

        return new Category(id, nameResult.Value, description, isActive, products?.ToList() ?? new List<Guid>());
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

    public void EditStatus(bool isActive) => IsActive = isActive;

    public Result Edit(string name, string description)
    {
        var descResult = ValidateDescription(description);
        if(descResult.IsFailure) return descResult;

        var nameResult = CategoryName.Create(name);
        if(nameResult.IsFailure) return nameResult.TopError;

        Name = nameResult.Value;
        Description = description;

        return Result.Success();
    }

}
