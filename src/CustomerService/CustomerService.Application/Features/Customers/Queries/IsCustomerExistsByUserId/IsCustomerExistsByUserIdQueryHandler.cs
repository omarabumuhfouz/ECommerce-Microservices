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

    async Task<Result<Unit>> IRequestHandler<IsCustomerExistsByUserIdQuery, Result<Unit>>.Handle(IsCustomerExistsByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking if customer with UserID: {UserId} exists", request.UserId);

        var isExists = await _unitOfWork.GetRepository<Customer>()
                            .IsExistsAsync(c => c.UserId == request.UserId, cancellationToken);

        return isExists ? Unit.Value : DomainErrors.Customer.NotFound(request.UserId);
    }
}