using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Categories.Commands;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Categories.Commands
{
    public class CreateCategoryCommandHandlerTests : TestFixtureBase<CreateCategoryCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldCreateCategory_WhenNameIsUniqueAndValid()
        {
            // Arrange
            var command = new CreateCategoryCommand("New Category", "New Description");
            var handler = new CreateCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            var categoryId = await handler.Handle(command, CancellationToken.None);

            // Assert
            var category = await CategoryRepository.GetByIdAsync(categoryId, CancellationToken.None);
            category.Should().NotBeNull();
            category.Name.Should().Be(command.Name);
            category.Description.Should().Be(command.Description);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateCategoryNameException_WhenNameExists()
        {
            // Arrange
            var existingCategory = Context.Categories.First();
            var command = new CreateCategoryCommand(existingCategory.Name, "Any Description");
            var handler = new CreateCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateCategoryNameException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Handle_ShouldThrowArgumentException_WhenNameIsInvalid(string name)
        {
            // Arrange
            var command = new CreateCategoryCommand(name, "Any Description");
            var handler = new CreateCategoryCommandHandler(CategoryRepository, Logger);

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
            var command = new CreateCategoryCommand("Any Name", description);
            var handler = new CreateCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}