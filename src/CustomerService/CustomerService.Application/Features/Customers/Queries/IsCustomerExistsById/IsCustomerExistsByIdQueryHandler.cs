using CustomerService.Domain.Customers;

namespace CustomerService.Application.Features.Customers.Queries.IsCustomerExistsById;

public class IsCustomerExistsByIdQueryHandler : IQueryHandler<IsCustomerExistsByIdQuery, Unit>
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ILogger _logger;

  public IsCustomerExistsByIdQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<IsCustomerExistsByIdQueryHandler> logger
    )
  {
    _unitOfWork = unitOfWork;
    _logger = logger;
  }


  async Task<Result<Unit>> IRequestHandler<IsCustomerExistsByIdQuery, Result<Unit>>.Handle(
      IsCustomerExistsByIdQuery request,
      CancellationToken ct)
  {
    _logger.LogInformation("Checking existence of Customer: {CustomerId}", request.CustomerId);

    return await Result.Success()
        // 1. Use EnsureAsync to validate existence
        .Ensure(async () => await _unitOfWork.GetRepository<Customer>()
            .AnyAsync(c => c.Id == request.CustomerId, ct),
            DomainErrors.Customer.NotFound(request.CustomerId))

        // 2. Log if the "NotFound" error was triggered
        .TapError(error => _logger.LogWarning("Existence check failed: Customer {CustomerId} not found.",
            request.CustomerId))

        // 3. Map to Unit.Value on success
        .Map(_ => Unit.Value);
  }
}
