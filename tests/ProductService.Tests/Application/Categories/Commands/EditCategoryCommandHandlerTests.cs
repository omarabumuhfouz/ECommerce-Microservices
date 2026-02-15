using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Categories.Commands;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Categories.Commands
{
    public class EditCategoryCommandHandlerTests : TestFixtureBase<EditCategoryCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldEditCategory_WhenCategoryExistsAndNameIsNotDuplicate()
        {
            // Arrange
            var category = Context.Categories.First();
            var newName = "Edited Category Name";
            var newDescription = "Edited Category Description";
            var command = new EditCategoryCommand(category.Id, newName, newDescription);
            var handler = new EditCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedCategory = await CategoryRepository.GetByIdAsync(category.Id, CancellationToken.None);
            updatedCategory.Should().NotBeNull();
            updatedCategory.Name.Should().Be(newName);
            updatedCategory.Description.Should().Be(newDescription);
        }

        [Fact]
        public async Task Handle_ShouldThrowCategoryNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new EditCategoryCommand(Guid.NewGuid(), "Any Name", "Any Description");
            var handler = new EditCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateCategoryNameException_WhenNameIsDuplicate()
        {
            // Arrange
            var category1 = Context.Categories.First();
            var category2 = Context.Categories.Skip(1).First();
            var command = new EditCategoryCommand(category1.Id, category2.Name, "Any Description");
            var handler = new EditCategoryCommandHandler(CategoryRepository, Logger);

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
            var category = Context.Categories.First();
            var command = new EditCategoryCommand(category.Id, name, "Any Description");
            var handler = new EditCategoryCommandHandler(CategoryRepository, Logger);

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
            var category = Context.Categories.First();
            var command = new EditCategoryCommand(category.Id, "Any Name", description);
            var handler = new EditCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}