using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Addresses.Commands.SetDefaultAddress;  

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


    async Task<Result<Unit>> IRequestHandler<SetDefaultAddressCommand, Result<Unit>>.Handle(
    SetDefaultAddressCommand request, 
    CancellationToken cancellationToken)
{
    _logger.LogInformation("Setting default address {AddressId} for Customer {CustomerId}", 
        request.AddressId, request.CustomerId);

    return await _unitOfWork.GetRepository<Customer>()
        .FirstOrDefaultAsync(new GetCustomerByIdSpec(request.CustomerId, true), cancellationToken)
        .ToResult(DomainErrors.Customer.NotFound(request.CustomerId))

        .TapError(error => _logger.LogWarning("Set default address failed: {Error}", error.Message))

        .Bind(customer => customer.SetDefaultAddress(request.AddressId))

        .Tap(async () => await _unitOfWork.SaveChangesAsync(cancellationToken))

        .Tap(() => _logger.LogInformation("Successfully set default address {AddressId} for Customer {CustomerId}", 
            request.AddressId, request.CustomerId))

        .Map(_ => Unit.Value);
}
}
