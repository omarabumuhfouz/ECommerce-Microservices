using ProductService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace ProductService.Domain.ValueObjects;

public record Image

{
    private Image(string url, string? altText)
    {
        Url = url;
        AltText = altText;
    }



    #pragma warning disable CS8618 
    private Image() { }

    #pragma warning restore CS8618








    public string Url { get; init; }
    public string? AltText { get; private set; }

    public static Result<Image> Create(string url, string? altText = null)
    {
        if (string.IsNullOrWhiteSpace(url)) return DomainErrors.Image.UrlRequired;

        return new Image(url,  altText);
    }
}
