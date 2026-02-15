
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Tags.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Tags.Commands
{
    public class DeleteTagCommandHandlerTests : TestFixtureBase<DeleteTagCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldDeleteTag_WhenTagExistsAndIsNotAssociatedWithProducts()
        {
            // Arrange
            var newTag = Tag.Create("New Tag to be deleted");
            await TagRepository.AddAsync(newTag, CancellationToken.None);
            await TagRepository.SaveChangesAsync(CancellationToken.None);

            var command = new DeleteTagCommand(newTag.Id);
            var handler = new DeleteTagCommandHandler(ProductRepository, TagRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedTag = await TagRepository.GetByIdAsync(newTag.Id, CancellationToken.None);
            deletedTag.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldThrowTagNotFoundException_WhenTagDoesNotExist()
        {
            // Arrange
            var command = new DeleteTagCommand(Guid.NewGuid());
            var handler = new DeleteTagCommandHandler(ProductRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<TagNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenTagIsAssociatedWithProducts()
        {
            // Arrange
            var product = Context.Products.First(p => p.Tags.Any());
            var tag = product.Tags.First();
            var command = new DeleteTagCommand(tag.Id);
            var handler = new DeleteTagCommandHandler(ProductRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
