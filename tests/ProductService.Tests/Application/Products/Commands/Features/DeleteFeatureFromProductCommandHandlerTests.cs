
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands.Features;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands.Features
{
    public class DeleteFeatureFromProductCommandHandlerTests : TestFixtureBase<DeleteFeatureFromProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldDeleteFeature_WhenProductAndFeatureExist()
        {
            // Arrange
            var product = Context.Products.First(p => p.Features.Any());
            var featureToDelete = product.Features.First();
            var command = new DeleteFeatureFromProductCommand(product.Id, featureToDelete.Name);
            var handler = new DeleteFeatureFromProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Features.Should().NotContain(f => f.Name == featureToDelete.Name);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteFeatureFromProductCommand(Guid.NewGuid(), "any_feature");
            var handler = new DeleteFeatureFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenFeatureDoesNotExist()
        {
            // Arrange
            var product = Context.Products.First();
            var command = new DeleteFeatureFromProductCommand(product.Id, "non_existent_feature");
            var handler = new DeleteFeatureFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenFeatureNameIsInvalid(string featureName)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new DeleteFeatureFromProductCommand(product.Id, featureName);
            var handler = new DeleteFeatureFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
