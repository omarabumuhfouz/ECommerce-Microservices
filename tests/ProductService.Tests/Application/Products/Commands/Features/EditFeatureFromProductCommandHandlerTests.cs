
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
    public class EditFeatureFromProductCommandHandlerTests : TestFixtureBase<EditFeatureFromProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldEditFeature_WhenProductAndFeatureExist()
        {
            // Arrange
            var product = Context.Products.First(p => p.Features.Any());
            var featureToEdit = product.Features.First();
            var newName = "Edited Feature Name";
            var newValue = "Edited Feature Value";
            var command = new EditFeatureFromProductCommand(product.Id, featureToEdit.Name, newName, newValue);
            var handler = new EditFeatureFromProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            var editedFeature = updatedProduct.Features.FirstOrDefault(f => f.Name == newName);
            editedFeature.Should().NotBeNull();
            editedFeature.Value.Should().Be(newValue);
            updatedProduct.Features.Should().NotContain(f => f.Name == featureToEdit.Name);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new EditFeatureFromProductCommand(Guid.NewGuid(), "any_feature", "new_feature", "new_value");
            var handler = new EditFeatureFromProductCommandHandler(ProductRepository, Logger);

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
            var command = new EditFeatureFromProductCommand(product.Id, "non_existent_feature", "new_feature", "new_value");
            var handler = new EditFeatureFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
