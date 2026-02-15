using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Categories.Queries;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Categories.Queries
{
    public class GetCategoryByIdQueryHandlerTests : TestFixtureBase<GetCategoryByIdQueryHandler>
    {
        [Fact]
        public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var category = Context.Categories.First();
            var query = new GetCategoryByIdQuery(category.Id);
            var handler = new GetCategoryByIdQueryHandler(CategoryRepository, Mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowCategoryNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var query = new GetCategoryByIdQuery(Guid.NewGuid());
            var handler = new GetCategoryByIdQueryHandler(CategoryRepository, Mapper);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<CategoryNotFoundException>();
        }
    }
}