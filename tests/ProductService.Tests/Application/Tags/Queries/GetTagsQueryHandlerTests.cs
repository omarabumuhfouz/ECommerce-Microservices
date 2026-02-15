
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Tags.Queries;
using Xunit;

namespace ProductService.Tests.Application.Tags.Queries
{
    public class GetTagsQueryHandlerTests : TestFixtureBase<GetTagsQueryHandler>
    {
        [Fact]
        public async Task Handle_ShouldReturnAllTags_WhenTagsExist()
        {
            // Arrange
            var query = new GetTagsQuery();
            var handler = new GetTagsQueryHandler(TagRepository, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(Context.Tags.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTagsExist()
        {
            // Arrange
            Context.Tags.RemoveRange(Context.Tags);
            await Context.SaveChangesAsync();

            var query = new GetTagsQuery();
            var handler = new GetTagsQueryHandler(TagRepository, Logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
