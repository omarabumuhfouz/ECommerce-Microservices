using ProductService.Domain.Constants;
using ProductService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace ProductService.Domain.Categories;
public record CategoryName
{
    public string Value { get; }

    private CategoryName(string value) => Value = value;

    #pragma warning disable CS8618 
    public CategoryName(){}
    #pragma warning restore CS8618


    public static Result<CategoryName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return DomainErrors.Category.NameRequired;
            
        if (name.Length > CategoryConstants.NameMaxLength) return DomainErrors.Category.NameTooLong;

        return new CategoryName(name);
    }

    // Implicit conversion allows you to treat it like a string easily
    public static implicit operator string(CategoryName name) => name.Value;

    public static CategoryName Empty() => new(string.Empty);
}