namespace CustomerService.Application.Addresses.Commands.DeleteAddress;
public class DeleteAddressCommandHandler : ICommandHandler<DeleteAddressCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAddressCommandHandler> _logger;

    public DeleteAddressCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteAddressCommandHandler> logger
        )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<DeleteAddressCommand, Result<Unit>>.Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Deleting Address : {AddressId} For CustomerId : {@CustomerId}",
             request.AddressId,
             request.CustomerId);

        var customerRepository = _unitOfWork.GetRepository<Customer>();
        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByIdSpec(request.CustomerId, true), cancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("Customer Not found with Id {@CustomerId}", request.CustomerId);
            return DomainErrors.Customer.NotFound(request.CustomerId);
        }

        var result = customer.DeleteAddress(request.AddressId);

        if (result.IsFailure) return result.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
        "Address Deleted Successfully owns by Customer Id :{@CustomerId}, with Address Id : {@AddressId}",
         request.CustomerId,
         request.AddressId);

        return Unit.Value;
    }
}
