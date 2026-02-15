
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands.Images;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands.ImagesManagement
{
    public class ReplaceMainImageCommandHandlerTests : TestFixtureBase<ReplaceMainImageCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldReplaceMainImage_WhenProductExists()
        {
            // Arrange
            var product = Context.Products.First();
            var newImageUrl = "http://example.com/new_main_image.jpg";
            var newAltText = "New main image alt text";
            var command = new ReplaceMainImageCommand(product.Id, newImageUrl, newAltText);
            var handler = new ReplaceMainImageCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.MainImage.Url.Should().Be(newImageUrl);
            updatedProduct.MainImage.AltText.Should().Be(newAltText);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new ReplaceMainImageCommand(Guid.NewGuid(), "http://example.com/any.jpg", "alt");
            var handler = new ReplaceMainImageCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenNewUrlIsInvalid(string newUrl)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new ReplaceMainImageCommand(product.Id, newUrl, "alt");
            var handler = new ReplaceMainImageCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
