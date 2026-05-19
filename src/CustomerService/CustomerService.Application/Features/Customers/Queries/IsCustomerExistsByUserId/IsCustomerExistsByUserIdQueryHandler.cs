using CustomerService.Domain.Customers;

namespace CustomerService.Application.Features.Customers.Queries.IsCustomerExistsByUserId;

public class IsCustomerExistsByUserIdQueryHandler : IQueryHandler<IsCustomerExistsByUserIdQuery, Unit>
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public IsCustomerExistsByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<IsCustomerExistsByUserIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<IsCustomerExistsByUserIdQuery, Result<Unit>>.Handle(
    IsCustomerExistsByUserIdQuery request, 
    CancellationToken ct)
{
    _logger.LogInformation("Checking if customer with UserID: {UserId} exists", request.UserId);

    return await Result.Success()
        .Ensure(async () => await _unitOfWork.GetRepository<Customer>()
            .AnyAsync(c => c.UserId == request.UserId, ct), 
            DomainErrors.Customer.NotFoundByUser(request.UserId))
        
        .TapError(error => _logger.LogWarning("Existence check failed: Customer for User {UserId} not found.", 
            request.UserId))
            
        .Map(_ => Unit.Value);
}
}