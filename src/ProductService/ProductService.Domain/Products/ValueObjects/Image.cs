using ProductService.Domain.Errors;
using SharedKernel.Shared;

namespace ProductService.Domain.ValueObjects;

public record Image

{
    private Image(string url, string? altText)
    {
        Url = url;
        AltText = altText;
    }

    private Image() { }

    public string Url { get; init; }
    public string? AltText { get; private set; }

    public static Result<Image> Create(string url, string? altText = null)
    {
        if (string.IsNullOrWhiteSpace(url)) return DomainErrors.Image.UrlRequired;

        return new Image(url,  altText);
    }
}
