using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Addresses.Commands.DeleteAddress;
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

    async Task<Result<Unit>> IRequestHandler<DeleteAddressCommand, Result<Unit>>.Handle(
    DeleteAddressCommand request, 
    CancellationToken ct)
{
    _logger.LogInformation("Deleting Address {AddressId} for Customer {CustomerId}", 
        request.AddressId, request.CustomerId);

    return await _unitOfWork.GetRepository<Customer>()
        .FirstOrDefaultAsync(new GetCustomerByIdSpec(request.CustomerId, true), ct)
        .ToResult(DomainErrors.Customer.NotFound(request.CustomerId))
        
        .TapError(error => _logger.LogWarning("Delete address failed: {Error}", error.Message))

        .Bind(customer => customer.DeleteAddress(request.AddressId))

        .Tap(async () => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(() => _logger.LogInformation("Address {AddressId} deleted successfully for Customer {CustomerId}", 
            request.AddressId, request.CustomerId))

        .Map(_ => Unit.Value);
}
}
