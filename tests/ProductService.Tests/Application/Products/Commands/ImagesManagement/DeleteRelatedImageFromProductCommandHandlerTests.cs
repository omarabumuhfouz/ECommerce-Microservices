
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands.ImagesManagement
{
    public class DeleteRelatedImageFromProductCommandHandlerTests : TestFixtureBase<DeleteRelatedImageFromProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldDeleteRelatedImage_WhenProductAndImageExist()
        {
            // Arrange
            var product = Context.Products.First(p => p.RelatedImages.Any());
            var imageToDelete = product.RelatedImages.First();
            var command = new DeleteRelatedImageFromProductCommand(product.Id, imageToDelete.Url);
            var handler = new DeleteRelatedImageFromProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.RelatedImages.Should().NotContain(img => img.Url == imageToDelete.Url);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteRelatedImageFromProductCommand(Guid.NewGuid(), "http://example.com/any.jpg");
            var handler = new DeleteRelatedImageFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenImageDoesNotExist()
        {
            // Arrange
            var product = Context.Products.First();
            var command = new DeleteRelatedImageFromProductCommand(product.Id, "http://example.com/non_existent.jpg");
            var handler = new DeleteRelatedImageFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenImageUrlIsInvalid(string imageUrl)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new DeleteRelatedImageFromProductCommand(product.Id, imageUrl);
            var handler = new DeleteRelatedImageFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
