
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
    public class EditRelatedImageCommandHandlerTests : TestFixtureBase<EditRelatedImageCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldEditRelatedImage_WhenProductAndImageExist()
        {
            // Arrange
            var product = Context.Products.First(p => p.RelatedImages.Count > 1);
            var imageToEdit = product.RelatedImages.First();
            var newUrl = "http://example.com/new_image.jpg";
            var newAltText = "New alt text";
            var command = new EditRelatedImageCommand(product.Id, imageToEdit.Url, newUrl, newAltText);
            var handler = new EditRelatedImageCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            var editedImage = updatedProduct.RelatedImages.FirstOrDefault(i => i.Url == newUrl);
            editedImage.Should().NotBeNull();
            editedImage.AltText.Should().Be(newAltText);
            updatedProduct.RelatedImages.Should().NotContain(i => i.Url == imageToEdit.Url);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new EditRelatedImageCommand(Guid.NewGuid(), "http://example.com/any.jpg", "http://example.com/new.jpg", "alt");
            var handler = new EditRelatedImageCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenImageToEditDoesNotExist()
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditRelatedImageCommand(product.Id, "http://example.com/non_existent.jpg", "http://example.com/new.jpg", "alt");
            var handler = new EditRelatedImageCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenNewUrlAlreadyExists()
        {
            // Arrange
            var product = Context.Products.First(p => p.RelatedImages.Count > 1);
            var imageToEdit = product.RelatedImages.First();
            var existingUrl = product.RelatedImages.Last().Url;
            var command = new EditRelatedImageCommand(product.Id, imageToEdit.Url, existingUrl, "alt");
            var handler = new EditRelatedImageCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenOldImageUrlIsInvalid(string oldUrl)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditRelatedImageCommand(product.Id, oldUrl, "http://example.com/new.jpg", "alt");
            var handler = new EditRelatedImageCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
