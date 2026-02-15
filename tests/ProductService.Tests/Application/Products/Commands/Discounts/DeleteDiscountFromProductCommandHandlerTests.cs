
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
    public class DeleteDiscountFromProductCommandHandlerTests : TestFixtureBase<DeleteDiscountFromProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldRemoveDiscount_WhenProductExistsAndHasDiscount()
        {
            // Arrange
            var productWithDiscount = Context.Products.First(p => p.Discount.Percentage > 0);
            var command = new DeleteDiscountFromProductCommand(productWithDiscount.Id);
            var handler = new DeleteDiscountFromProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(productWithDiscount.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Discount.Percentage.Should().Be(0);
            updatedProduct.Discount.EndDate.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldDoNothing_WhenProductExistsAndHasNoDiscount()
        {
            // Arrange
            var productWithoutDiscount = Context.Products.First(p => p.Discount.Percentage == 0);
            var command = new DeleteDiscountFromProductCommand(productWithoutDiscount.Id);
            var handler = new DeleteDiscountFromProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(productWithoutDiscount.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Discount.Percentage.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteDiscountFromProductCommand(Guid.NewGuid());
            var handler = new DeleteDiscountFromProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }
    }
}
