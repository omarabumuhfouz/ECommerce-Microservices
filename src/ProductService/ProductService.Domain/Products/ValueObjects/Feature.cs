
using ProductService.Domain.Constants;
using ProductService.Domain.Errors;
using SharedKernel.Shared;

namespace ProductService.Domain.ValueObjects;


public class Feature
{
    private Feature(string name, string value)
    {
        Value = value;
        Name = name;
    }

    private Feature() { }

    public string Name { get; private set; }
    public string Value { get; private set; }

    public static Result<Feature> Create(string name, string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return DomainErrors.Feature.ValueRequired;
        if (string.IsNullOrWhiteSpace(name)) return DomainErrors.Feature.NameRequired;
        if (name.Length > ProductConstants.FEATURE_NAME_MAX_LENGTH) return DomainErrors.Feature.NameTooLong;
        if (value.Length > ProductConstants.FEATURE_VALUE_MAX_LENGTH) return DomainErrors.Feature.ValueTooLong;

        return new Feature(name, value);
    }
}