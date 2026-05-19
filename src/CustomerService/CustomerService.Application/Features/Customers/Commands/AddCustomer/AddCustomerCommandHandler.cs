using CustomerService.Domain.Customers;

namespace CustomerService.Application.Features.Customers.Commands.AddCustomer;

public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddCustomerCommandHandler> _logger;

    public AddCustomerCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddCustomerCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }


    async Task<Result<Guid>> IRequestHandler<AddCustomerCommand, Result<Guid>>.Handle(AddCustomerCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Processing AddCustomerCommand for UserId: {UserId}", request.UserId);

        var customerRepository = _unitOfWork.GetRepository<Customer>();

        return await Result.Success(request)

            .Ensure(async () => !await customerRepository.AnyAsync(new GetCustomerByUserIdSpec(request.UserId), ct),
                DomainErrors.Customer.UserIdAlreadyExists(request.UserId))

            .Bind(_ => Customer.Create(request.UserId, request.FirstName, request.LastName, request.PhoneNumber, null))

            .Tap(async customer => await customerRepository.AddAsync(customer, ct))

            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

            .Tap(customer => _logger.LogInformation("Customer {CustomerId} created for User {UserId}", customer.Id, request.UserId))
            .TapError(error => _logger.LogWarning("Create Customer failed: {@Error}", error))

            .Map(customer => customer.Id);
    }
}