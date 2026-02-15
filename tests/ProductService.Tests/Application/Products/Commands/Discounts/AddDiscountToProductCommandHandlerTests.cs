
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands.Discounts
{
    public class AddDiscountToProductCommandHandlerTests : TestFixtureBase<AddDiscountToProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldAddDiscount_WhenProductExistsAndHasNoDiscount()
        {
            // Arrange
            var productWithoutDiscount = Context.Products.First(p => p.Discount.Percentage == 0);
            var command = new AddDiscountToProductCommand(productWithoutDiscount.Id, 10, DateTime.UtcNow.AddDays(10));
            var handler = new AddDiscountToProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(productWithoutDiscount.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Discount.Percentage.Should().Be(command.DiscountPercentage);
            updatedProduct.Discount.EndDate.Should().Be(command.DiscountEndDate);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new AddDiscountToProductCommand(Guid.NewGuid(), 10, DateTime.UtcNow.AddDays(10));
            var handler = new AddDiscountToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenProductAlreadyHasDiscount()
        {
            // Arrange
            var productWithDiscount = Context.Products.First(p => p.Discount.Percentage > 0);
            var command = new AddDiscountToProductCommand(productWithDiscount.Id, 20, DateTime.UtcNow.AddDays(10));
            var handler = new AddDiscountToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenDiscountPercentageIsInvalid(int percentage)
        {
            // Arrange
            var productWithoutDiscount = Context.Products.First(p => p.Discount.Percentage == 0);
            var command = new AddDiscountToProductCommand(productWithoutDiscount.Id, percentage, DateTime.UtcNow.AddDays(10));
            var handler = new AddDiscountToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenDiscountEndDateIsInThePast()
        {
            // Arrange
            var productWithoutDiscount = Context.Products.First(p => p.Discount.Percentage == 0);
            var command = new AddDiscountToProductCommand(productWithoutDiscount.Id, 10, DateTime.UtcNow.AddDays(-1));
            var handler = new AddDiscountToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
