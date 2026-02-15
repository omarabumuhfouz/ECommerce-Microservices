using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Categories.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Categories.Queries
{
    public class GetProductsByCategoryQueryHandlerTests : TestFixtureBase<GetProductsByCategoryQueryHandler>
    {
        [Fact]
        public async Task Handle_ShouldReturnProducts_WhenCategoryExistsAndHasProducts()
        {
            // Arrange
            var categoryWithProducts = Context.Categories.First(c => c.Products.Any());
            var query = new GetProductsByCategoryQuery(categoryWithProducts.Id);
            var handler = new GetProductsByCategoryQueryHandler(ProductRepository, Mapper, CategoryRepository, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(categoryWithProducts.Products.Count);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenCategoryExistsAndHasNoProducts()
        {
            // Arrange
            var newCategory = Category.Create(Guid.NewGuid(), "Empty Category", "Description");
            await CategoryRepository.AddAsync(newCategory, CancellationToken.None);
            await CategoryRepository.SaveChangesAsync(CancellationToken.None);

            var query = new GetProductsByCategoryQuery(newCategory.Id);
            var handler = new GetProductsByCategoryQueryHandler(ProductRepository, Mapper, CategoryRepository, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldThrowCategoryNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var query = new GetProductsByCategoryQuery(Guid.NewGuid());
            var handler = new GetProductsByCategoryQueryHandler(ProductRepository, Mapper, CategoryRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<CategoryNotFoundException>();
        }
    }
}