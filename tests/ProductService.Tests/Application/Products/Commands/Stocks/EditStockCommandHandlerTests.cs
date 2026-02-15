
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands.Stocks;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands.Stocks
{
    public class EditStockCommandHandlerTests : TestFixtureBase<EditStockCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldIncreaseStock_WhenOperationIsIncrease()
        {
            // Arrange
            var product = Context.Products.First();
            var initialStock = product.StockQuantity;
            var quantity = 10;
            var command = new EditStockCommand(product.Id, quantity, "increase");
            var handler = new EditStockCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.StockQuantity.Should().Be(initialStock + quantity);
        }

        [Fact]
        public async Task Handle_ShouldDecreaseStock_WhenOperationIsDecrease()
        {
            // Arrange
            var product = Context.Products.First(p => p.StockQuantity > 10);
            var initialStock = product.StockQuantity;
            var quantity = 5;
            var command = new EditStockCommand(product.Id, quantity, "decrease");
            var handler = new EditStockCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.StockQuantity.Should().Be(initialStock - quantity);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new EditStockCommand(Guid.NewGuid(), 10, "increase");
            var handler = new EditStockCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenOperationIsInvalid()
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditStockCommand(product.Id, 10, "invalid_operation");
            var handler = new EditStockCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_ShouldThrowArgumentException_WhenQuantityIsInvalidForIncrease(int quantity)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditStockCommand(product.Id, quantity, "increase");
            var handler = new EditStockCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_ShouldThrowArgumentException_WhenQuantityIsInvalidForDecrease(int quantity)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditStockCommand(product.Id, quantity, "decrease");
            var handler = new EditStockCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInsufficientStockException_WhenDecreasingMoreThanAvailable()
        {
            // Arrange
            var product = Context.Products.First();
            var quantity = product.StockQuantity + 1;
            var command = new EditStockCommand(product.Id, quantity, "decrease");
            var handler = new EditStockCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InsufficientStockException>();
        }
    }
}
