using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands
{
    public class EditProductStatusCommandHandlerTests : TestFixtureBase<EditProductStatusCommandHandler>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_ShouldEditProductStatus_WhenProductExists(bool isAvailable)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditProductStatusCommand(product.Id, isAvailable);
            var handler = new EditProductStatusCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.IsAvailable.Should().Be(isAvailable);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new EditProductStatusCommand(Guid.NewGuid(), true);
            var handler = new EditProductStatusCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }
    }
}