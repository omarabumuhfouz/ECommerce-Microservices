using ShoppingCartService.Application.Carts.Commands;

namespace ShoppingCartService.Tests.Application.Carts.Commands;

public class ClearCartCommandHandlerTests : IClassFixture<TestFixtureBase<ClearCartCommandHandler>>
{
    private readonly ClearCartCommandHandler _handler;
    private readonly TestFixtureBase<ClearCartCommandHandler> _fixture;

    public ClearCartCommandHandlerTests(TestFixtureBase<ClearCartCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CartRepository,
            _fixture.CustomerServiceClient
        );
    }
}