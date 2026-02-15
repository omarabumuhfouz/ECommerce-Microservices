using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Queries;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Queries
{
    public class GetProductNamesQueryHandlerTests : TestFixtureBase<GetProductNamesQueryHandler>
    {
        [Fact]
        public async Task Handle_ShouldReturnProductNames_WhenAllProductsExist()
        {
            // Arrange
            var products = Context.Products.Take(2).ToList();
            var productIds = products.Select(p => p.Id).ToList();
            var query = new GetProductNamesQuery(productIds);
            var handler = new GetProductNamesQueryHandler(ProductRepository, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Select(r => r.Id).Should().BeEquivalentTo(productIds);
        }

        [Fact]
        public async Task Handle_ShouldThrowProductsNotFoundException_WhenSomeProductsDoNotExist()
        {
            // Arrange
            var existingProduct = Context.Products.First();
            var nonExistingId = Guid.NewGuid();
            var productIds = new List<Guid> { existingProduct.Id, nonExistingId };
            var query = new GetProductNamesQuery(productIds);
            var handler = new GetProductNamesQueryHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductsNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductIdsAreProvided()
        {
            // Arrange
            var query = new GetProductNamesQuery(new List<Guid>());
            var handler = new GetProductNamesQueryHandler(ProductRepository, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}