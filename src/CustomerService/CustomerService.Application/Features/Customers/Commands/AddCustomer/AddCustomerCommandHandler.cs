using MassTransit;

namespace CustomerService.Application.Customers.Commands.AddCustomer;

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
        var customerRepository = _unitOfWork.GetRepository<Customer>();

        if (await customerRepository.IsExistsAsync(c => c.UserId == request.UserId, ct))
        {
            _logger.LogWarning("Failed to create customer. A customer with UserId {UserId} already exists.", request.UserId);
            return DomainErrors.Customer.UserIdAlreadyExists(request.UserId);
        }

        var addedCustomer = Customer.Create(request.UserId, request.FirstName, request.LastName, request.PhoneNumber, null);

        if (addedCustomer.IsFailure) return addedCustomer.TopError;

        await customerRepository.AddAsync(addedCustomer.Value, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Customer Added Succesfully relater to UserId : {@UserId}", request.UserId);

        return addedCustomer.Value.Id;
    }
}