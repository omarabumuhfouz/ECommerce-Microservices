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
    public class DeleteProductCommandHandlerTests : TestFixtureBase<DeleteProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var product = Context.Products.First();
            var command = new DeleteProductCommand(product.Id);
            var handler = new DeleteProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, false);
            deletedProduct.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteProductCommand(Guid.NewGuid());
            var handler = new DeleteProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }
    }
}