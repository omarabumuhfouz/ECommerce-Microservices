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
    public class EditProductCommandHandlerTests : TestFixtureBase<EditProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldEditProduct_WhenProductExistsAndNameIsNotDuplicate()
        {
            // Arrange
            var product = Context.Products.First();
            var newName = "Edited Product Name";
            var newDescription = "Edited Product Description";
            var command = new EditProductCommand(product.Id, newName, newDescription);
            var handler = new EditProductCommandHandler(ProductRepository, CategoryRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Name.Should().Be(newName);
            updatedProduct.Description.Should().Be(newDescription);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new EditProductCommand(Guid.NewGuid(), "Any Name", "Any Description");
            var handler = new EditProductCommandHandler(ProductRepository, CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateProductNameException_WhenNameIsDuplicate()
        {
            // Arrange
            var product1 = Context.Products.First();
            var product2 = Context.Products.Skip(1).First();
            var command = new EditProductCommand(product1.Id, product2.Name, "Any Description");
            var handler = new EditProductCommandHandler(ProductRepository, CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateProductNameException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenNameIsInvalid(string name)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditProductCommand(product.Id, name, "Any Description");
            var handler = new EditProductCommandHandler(ProductRepository, CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenDescriptionIsInvalid(string description)
        {
            // Arrange
            var product = Context.Products.First();
            var command = new EditProductCommand(product.Id, "Any Name", description);
            var handler = new EditProductCommandHandler(ProductRepository, CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}