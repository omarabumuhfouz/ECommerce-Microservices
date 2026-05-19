using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateAddressCommandHandler> _logger;


    public CreateAddressCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateAddressCommandHandler> logger
        )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }


    async Task<Result<Guid>> IRequestHandler<CreateAddressCommand, Result<Guid>>.Handle(
    CreateAddressCommand request,
    CancellationToken ct)
    {
        _logger.LogInformation("Creating Address for CustomerId: {CustomerId}", request.CustomerId);

        var customerRepository = _unitOfWork.GetRepository<Customer>();

        return await customerRepository
            .FirstOrDefaultAsync(new GetCustomerByIdSpec(request.CustomerId, true), ct)
            .ToResult(DomainErrors.Customer.NotFound(request.CustomerId))

            .TapError(error => _logger.LogWarning("Create Address failed: {Error}", error.Message))

            .Bind(customer =>
            {
                var address = _mapper.Map<Address>(request);
                return customer.AddAddress(address).Map(_ => (customer, address));
            })

            .Tap(async x => await _unitOfWork.SaveChangesAsync(ct))

            .Tap(x => _logger.LogInformation("Address {AddressId} created for Customer {CustomerId}",
                x.address.Id, x.customer.Id))

            .Map(x => x.address.Id);
    }
}
