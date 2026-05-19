namespace ProductService.Application.Features.Products.DTOs;

public record ImageDto(string Url, string? AltText)
{
    public static ImageDto Empty() => new(string.Empty, string.Empty);
}