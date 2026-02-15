using System.Threading.Tasks;
using ShoppingCartService.Application.Carts.Commands;

namespace ShoppingCartService.Tests.CartTests.CommandsTests;

public class EditCartItemCommandHandlerTests : IClassFixture<TestFixtureBase<EditCartItemCommandHandler>>
{
    private readonly TestFixtureBase<EditCartItemCommandHandler> _fixture;
    private readonly EditCartItemCommandHandler _handler;

    public EditCartItemCommandHandlerTests(TestFixtureBase<EditCartItemCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CartRepository,
            _fixture.ValidationService,
            _fixture.ProductServiceClient,
            _fixture.CartMapper,
            _fixture.CustomerServiceClient
        );

    }
}