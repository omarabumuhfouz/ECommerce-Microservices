using ShoppingCartService.Application.Carts.Commands;

namespace ShoppingCartService.Tests.Application.Carts.Commands;

public class AddToCartCommandHandlerTests : IClassFixture<TestFixtureBase<AddToCartCommandHandler>>
{
    private readonly AddToCartCommandHandler _handler;
    private readonly TestFixtureBase<AddToCartCommandHandler> _fixture;


    public AddToCartCommandHandlerTests(TestFixtureBase<AddToCartCommandHandler> fixture)
    {
        _fixture = fixture;


        _handler = new(
            _fixture.ProductServiceClient,
            _fixture.CartRepository,
            _fixture.ValidationService,
            _fixture.CartMapper
        );
    }
}
