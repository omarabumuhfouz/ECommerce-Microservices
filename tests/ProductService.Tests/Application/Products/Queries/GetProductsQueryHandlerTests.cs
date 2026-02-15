using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Queries;
using Xunit;

namespace ProductService.Tests.Application.Products.Queries
{
    public class GetProductsQueryHandlerTests : TestFixtureBase<GetProductsQueryHandler>
    {
        [Fact]
        public async Task Handle_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            var query = new GetProductsQuery();
            var handler = new GetProductsQueryHandler(ProductRepository, Mapper, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(Context.Products.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            Context.Products.RemoveRange(Context.Products);
            await Context.SaveChangesAsync();

            var query = new GetProductsQuery();
            var handler = new GetProductsQueryHandler(ProductRepository, Mapper, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}