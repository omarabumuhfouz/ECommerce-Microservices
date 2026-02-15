
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Tags.Commands;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Tags.Commands
{
    public class EditTagCommandHandlerTests : TestFixtureBase<EditTagCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldEditTag_WhenTagExistsAndNameIsNotDuplicate()
        {
            // Arrange
            var tag = Context.Tags.First();
            var newName = "Edited Tag Name";
            var command = new EditTagCommand(tag.Id, newName);
            var handler = new EditTagCommandHandler(TagRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedTag = await TagRepository.GetByIdAsync(tag.Id, CancellationToken.None);
            updatedTag.Should().NotBeNull();
            updatedTag.Name.Should().Be(newName);
        }

        [Fact]
        public async Task Handle_ShouldThrowTagNotFoundException_WhenTagDoesNotExist()
        {
            // Arrange
            var command = new EditTagCommand(Guid.NewGuid(), "Any Name");
            var handler = new EditTagCommandHandler(TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<TagNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenNameIsDuplicate()
        {
            // Arrange
            var tag1 = Context.Tags.First();
            var tag2 = Context.Tags.Skip(1).First();
            var command = new EditTagCommand(tag1.Id, tag2.Name);
            var handler = new EditTagCommandHandler(TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenNameIsInvalid(string name)
        {
            // Arrange
            var tag = Context.Tags.First();
            var command = new EditTagCommand(tag.Id, name);
            var handler = new EditTagCommandHandler(TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
