using ShoppingCartService.Application.Queries;

namespace ShoppingCartService.Tests.CartTests.QueriesTests;

public class GetCartByCustomerIdQueryHandlerTests : IClassFixture<TestFixtureBase<GetCartByCustomerIdQueryHandler>>
{
    private readonly TestFixtureBase<GetCartByCustomerIdQueryHandler> _fixture;
    private readonly GetCartByCustomerIdQueryHandler _handler;
    public GetCartByCustomerIdQueryHandlerTests(TestFixtureBase<GetCartByCustomerIdQueryHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CartRepository,
            _fixture.CartMapper,
            _fixture.CustomerServiceClient
        );
    }
}