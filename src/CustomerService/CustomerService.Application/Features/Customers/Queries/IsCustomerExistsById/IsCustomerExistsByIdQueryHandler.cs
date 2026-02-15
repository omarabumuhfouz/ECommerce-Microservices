namespace CustomerService.Application.Customers.Queries;

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


  async Task<Result<Unit>> IRequestHandler<IsCustomerExistsByIdQuery, Result<Unit>>.Handle(IsCustomerExistsByIdQuery request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Checking if customer with ID: {CustomerId} exists", request.CustomerId);

    bool isExists = await _unitOfWork.GetRepository<Customer>()
                              .IsExistsAsync(c => c.Id == request.CustomerId, cancellationToken);

    return isExists ? Unit.Value : DomainErrors.Customer.NotFound(request.CustomerId);
  }
}
