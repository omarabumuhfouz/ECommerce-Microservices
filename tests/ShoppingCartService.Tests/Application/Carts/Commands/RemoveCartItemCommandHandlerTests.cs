using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ShoppingCartService.Tests.CartTests.CommandsTests;

public class RemoveCartItemCommandHandlerTests : IClassFixture<TestFixtureBase<RemoveCartItemCommandHandler>>
{
    private readonly TestFixtureBase<RemoveCartItemCommandHandler> _fixture;
    private readonly RemoveCartItemCommandHandler _handler;

    public RemoveCartItemCommandHandlerTests(TestFixtureBase<RemoveCartItemCommandHandler> fixture)
    {
        _fixture = fixture;

        _handler = new(
            _fixture.CartRepository,
            _fixture.CartMapper,
            _fixture.CustomerServiceClient
        );
    }
}