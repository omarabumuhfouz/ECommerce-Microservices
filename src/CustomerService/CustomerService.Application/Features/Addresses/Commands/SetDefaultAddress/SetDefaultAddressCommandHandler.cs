namespace CustomerService.Application.Addresses.Commands.SetDefaultAddress;  

public sealed class SetDefaultAddressCommandHandler : ICommandHandler<SetDefaultAddressCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetDefaultAddressCommandHandler> _logger;

    public SetDefaultAddressCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<SetDefaultAddressCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }


    async Task<Result<Unit>> IRequestHandler<SetDefaultAddressCommand, Result<Unit>>.Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Set Default Address : {@AddressId} for Customer : {@CustomerId}",
                        request.AddressId,
                        request.CustomerId);

        var customerRepository = _unitOfWork.GetRepository<Customer>();
        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByIdSpec(request.CustomerId, true), cancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("Customer not found with Id {@CustomerId}", request.CustomerId);
            return DomainErrors.Customer.NotFound(request.CustomerId);
        }

        customer.SetDefaultAddress(request.AddressId);

        customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
        "Set Defualt Address Successfully for Customer : {@CustomerId}, with Address : {@AddressId}",
        request.CustomerId,
        request.AddressId);

        return Unit.Value;
    }
}
