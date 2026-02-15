namespace CustomerService.Application.Addresses.Commands;

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

    async Task<Result<Unit>> IRequestHandler<EditAddressCommand, Result<Unit>>.Handle(EditAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Updating Address : {@AddressId} For CustomerId : {@CustomerId}",
             request.AddressId,
            request.CustomerId);

        var customerRepository = _unitOfWork.GetRepository<Customer>();

        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByIdSpec(request.CustomerId, true), cancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("Customer not found with Id : {@CustomerId}", request.CustomerId);
            return DomainErrors.Customer.NotFound(request.CustomerId);
        }

        var result = customer.UpdateAddress(request.AddressId, _mapper.Map<Address>(request));

        if (result.IsFailure) return result.TopError;


        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Address Updated Successfully belong to Customer Id :{@CustomerId}, with Address Id {@AddressId}",
                    request.CustomerId,
                    request.AddressId);

        return Unit.Value; 
    }
}
