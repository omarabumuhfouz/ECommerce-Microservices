using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Queries;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Queries
{
    public class GetProductByIdQueryHandlerTests : TestFixtureBase<GetProductByIdQueryHandler>
    {
        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = Context.Products.First();
            var query = new GetProductByIdQuery(product.Id);
            var handler = new GetProductByIdQueryHandler(ProductRepository, Mapper, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(product.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var query = new GetProductByIdQuery(Guid.NewGuid());
            var handler = new GetProductByIdQueryHandler(ProductRepository, Mapper, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }
    }
}