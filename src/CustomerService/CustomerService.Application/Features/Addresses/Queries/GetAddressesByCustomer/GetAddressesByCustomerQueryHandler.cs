using CustomerService.Application.Addresses.DTOs;

namespace CustomerService.Application.Addresses.Queries.GetAddressesByCustomer;
public class GetAddressesByCustomerQueryHandler : IQueryHandler<GetAddressesByCustomerQuery, List<AddressDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAddressesByCustomerQueryHandler> _logger;

    public GetAddressesByCustomerQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetAddressesByCustomerQueryHandler> logger
        )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    async Task<SharedKernel.Shared.Result<List<AddressDto>>> IRequestHandler<GetAddressesByCustomerQuery, SharedKernel.Shared.Result<List<AddressDto>>>.Handle(GetAddressesByCustomerQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Retrived All Address for Customer : {@CustomerId}", request.CustomerId);

        var customerRepository = _unitOfWork.GetRepository<Customer>();
        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByIdSpec(request.CustomerId), cancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("Customer Not found with Id: {@CustomerId}", request.CustomerId);
            return DomainErrors.Customer.NotFound(request.CustomerId);
        }

        if (!customer.Addresses.Any()) return new List<AddressDto>();

        return _mapper.Map<List<AddressDto>>(customer.Addresses);
    }
}
