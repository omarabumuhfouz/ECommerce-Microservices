
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands.Tags
{
    public class AddTagToProductCommandHandlerTests : TestFixtureBase<AddTagToProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldAddTag_WhenProductAndTagExistAndTagIsNotAssociated()
        {
            // Arrange
            var product = Context.Products.First();
            var tag = Context.Tags.First(t => !product.Tags.Contains(t));
            var command = new AddTagToProductCommand(tag.Id, product.Id);
            var handler = new AddTagToProductCommandHandler(TagRepository, ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Tags.Should().Contain(t => t.Id == tag.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var tag = Context.Tags.First();
            var command = new AddTagToProductCommand(tag.Id, Guid.NewGuid());
            var handler = new AddTagToProductCommandHandler(TagRepository, ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowTagNotFoundException_WhenTagDoesNotExist()
        {
            // Arrange
            var product = Context.Products.First();
            var command = new AddTagToProductCommand(Guid.NewGuid(), product.Id);
            var handler = new AddTagToProductCommandHandler(TagRepository, ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<TagNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenTagIsAlreadyAssociated()
        {
            // Arrange
            var product = Context.Products.First(p => p.Tags.Any());
            var tag = product.Tags.First();
            var command = new AddTagToProductCommand(tag.Id, product.Id);
            var handler = new AddTagToProductCommandHandler(TagRepository, ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
