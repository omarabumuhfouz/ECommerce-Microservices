using ProductService.Domain.Constants;
using ProductService.Domain.Errors;
using SharedKernel.Shared;

namespace ProductService.Domain.Category;
public record CategoryName
{
    public string Value { get; }

    private CategoryName(string value) => Value = value;

    public static Result<CategoryName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return DomainErrors.Category.NameRequired;
            
        if (name.Length > CategoryConstants.NAME_MAX_LENGTH) return DomainErrors.Category.NameTooLong;

        return new CategoryName(name);
    }
    
    // Implicit conversion allows you to treat it like a string easily
    public static implicit operator string(CategoryName name) => name.Value;
}