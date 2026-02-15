
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Tags.Commands;
using Xunit;

namespace ProductService.Tests.Application.Tags.Commands
{
    public class AddTagCommandHandlerTests : TestFixtureBase<AddTagCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldCreateTag_WhenNameIsValid()
        {
            // Arrange
            var command = new AddTagCommand("New Tag");
            var handler = new AddTagCommandHandler(TagRepository, Logger);

            // Act
            var tagId = await handler.Handle(command, CancellationToken.None);

            // Assert
            var tag = await TagRepository.GetByIdAsync(tagId, CancellationToken.None);
            tag.Should().NotBeNull();
            tag.Name.Should().Be(command.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenNameIsInvalid(string name)
        {
            // Arrange
            var command = new AddTagCommand(name);
            var handler = new AddTagCommandHandler(TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
