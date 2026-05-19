using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Addresses.Commands.EditAddress;

public class EditAddressCommandHandler : ICommandHandler<EditAddressCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditAddressCommandHandler> _logger;
    private readonly IMapper _mapper;


    public EditAddressCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditAddressCommandHandler> logger,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    async Task<Result<Unit>> IRequestHandler<EditAddressCommand, Result<Unit>>.Handle(
    EditAddressCommand request, 
    CancellationToken ct)
{
    _logger.LogInformation("Updating Address {AddressId} for Customer {CustomerId}", 
        request.AddressId, request.CustomerId);

    return await _unitOfWork.GetRepository<Customer>()
        .FirstOrDefaultAsync(new GetCustomerByIdSpec(request.CustomerId, true), ct)
        .ToResult(DomainErrors.Customer.NotFound(request.CustomerId))

        .TapError(error => _logger.LogWarning("Edit address failed: {Error}", error.Message))

        .Bind(customer => customer.UpdateAddress(request.AddressId, _mapper.Map<Address>(request)))

        .Tap(async () => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(() => _logger.LogInformation("Address {AddressId} updated successfully for Customer {CustomerId}", 
            request.AddressId, request.CustomerId))

        .Map(_ => Unit.Value);
}
}
