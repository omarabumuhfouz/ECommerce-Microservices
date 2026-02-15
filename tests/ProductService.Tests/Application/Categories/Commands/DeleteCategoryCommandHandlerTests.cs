using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Categories.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Categories.Commands
{
    public class DeleteCategoryCommandHandlerTests : TestFixtureBase<DeleteCategoryCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldDeactivateCategory_WhenCategoryExistsAndHasNoProducts()
        {
            // Arrange
            var newCategory = Category.Create(Guid.NewGuid(), "Category to delete", "Description");
            await CategoryRepository.AddAsync(newCategory, CancellationToken.None);
            await CategoryRepository.SaveChangesAsync(CancellationToken.None);

            var command = new DeleteCategoryCommand(newCategory.Id);
            var handler = new DeleteCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deactivatedCategory = await CategoryRepository.GetByIdAsync(newCategory.Id, CancellationToken.None);
            deactivatedCategory.Should().NotBeNull();
            deactivatedCategory.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldThrowCategoryNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new DeleteCategoryCommand(Guid.NewGuid());
            var handler = new DeleteCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowCategoryHasProductsException_WhenCategoryHasProducts()
        {
            // Arrange
            var categoryWithProducts = Context.Categories.First(c => c.Products.Any());
            var command = new DeleteCategoryCommand(categoryWithProducts.Id);
            var handler = new DeleteCategoryCommandHandler(CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<CategoryHasProductsException>();
        }
    }
}