
using ProductService.Domain.Constants;
using ProductService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace ProductService.Domain.ValueObjects;


public class Feature
{
    private Feature(string name, string value)
    {
        Value = value;
        Name = name;
    }

    #pragma warning disable CS8618 
    private Feature() { }
    #pragma warning restore CS8618
    

    public string Name { get; private set; }
    public string Value { get; private set; }

    public static Result<Feature> Create(string name, string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return DomainErrors.Feature.ValueRequired;
        if (string.IsNullOrWhiteSpace(name)) return DomainErrors.Feature.NameRequired;
        if (name.Length > ProductConstants.FeatureNameMaxLength) return DomainErrors.Feature.NameTooLong;
        if (value.Length > ProductConstants.FeatureValueMaxLength) return DomainErrors.Feature.ValueTooLong;

        return new Feature(name, value);
    }
}