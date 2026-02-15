using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Categories.Queries;
using Xunit;

namespace ProductService.Tests.Application.Categories.Queries
{
    public class GetCategoriesQueryHandlerTests : TestFixtureBase<GetCategoriesQueryHandler>
    {
        [Fact]
        public async Task Handle_ShouldReturnAllCategories_WhenCategoriesExist()
        {
            // Arrange
            var query = new GetCategoriesQuery();
            var handler = new GetCategoriesQueryHandler(CategoryRepository, Mapper, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(Context.Categories.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            Context.Categories.RemoveRange(Context.Categories);
            await Context.SaveChangesAsync();

            var query = new GetCategoriesQuery();
            var handler = new GetCategoriesQueryHandler(CategoryRepository, Mapper, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}