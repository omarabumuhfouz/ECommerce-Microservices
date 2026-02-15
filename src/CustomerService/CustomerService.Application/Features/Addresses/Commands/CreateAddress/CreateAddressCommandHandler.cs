namespace CustomerService.Application.Addresses.Commands.CreateAddress;

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


    async Task<Result<Guid>> IRequestHandler<CreateAddressCommand, Result<Guid>>.Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Address Start with CustomerId : {@CustomerId}", request.CustomerId);

        var customerRepository = _unitOfWork.GetRepository<Customer>();
        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByIdSpec(request.CustomerId, true), cancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("Customer not found with Id : {@CustomerId}", request.CustomerId);
            return DomainErrors.Customer.NotFound(request.CustomerId);
        }

        var address = _mapper.Map<Address>(request);
        var result = customer.AddAddress(address);

        if (result.IsFailure) return result.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Address Created Successfully with Id {@AddressId}", address.Id);
        return address.Id;
    }
}
