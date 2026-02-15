
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
    public class DeleteTagFromProductCommandHandlerTests : TestFixtureBase<DeleteTagFromProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldDeleteTag_WhenProductAndTagExistAndTagIsAssociated()
        {
            // Arrange
            var product = Context.Products.First(p => p.Tags.Any());
            var tag = product.Tags.First();
            var command = new DeleteTagFromProductCommand(tag.Id, product.Id);
            var handler = new DeleteTagFromProductCommandHandler(ProductRepository, TagRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Tags.Should().NotContain(t => t.Id == tag.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var tag = Context.Tags.First();
            var command = new DeleteTagFromProductCommand(tag.Id, Guid.NewGuid());
            var handler = new DeleteTagFromProductCommandHandler(ProductRepository, TagRepository, Logger);

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
            var command = new DeleteTagFromProductCommand(Guid.NewGuid(), product.Id);
            var handler = new DeleteTagFromProductCommandHandler(ProductRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<TagNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenTagIsNotAssociated()
        {
            // Arrange
            var product = Context.Products.First();
            var tag = Context.Tags.First(t => !product.Tags.Contains(t));
            var command = new DeleteTagFromProductCommand(tag.Id, product.Id);
            var handler = new DeleteTagFromProductCommandHandler(ProductRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
