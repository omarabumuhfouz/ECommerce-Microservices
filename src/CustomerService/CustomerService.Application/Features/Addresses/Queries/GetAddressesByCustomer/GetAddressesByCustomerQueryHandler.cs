using CustomerService.Application.Addresses.DTOs;
using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Addresses.Queries.GetAddressesByCustomer;
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

    async Task<Result<List<AddressDto>>> IRequestHandler<GetAddressesByCustomerQuery, Result<List<AddressDto>>>.Handle(
    GetAddressesByCustomerQuery request, 
    CancellationToken ct)
{
    _logger.LogInformation("Retrieving addresses for Customer: {CustomerId}", request.CustomerId);

    return await _unitOfWork.GetRepository<Customer>()
        .FirstOrDefaultAsync(new GetCustomerByIdSpec(request.CustomerId), ct)
        .ToResult(DomainErrors.Customer.NotFound(request.CustomerId))
        
        .TapError(error => _logger.LogWarning("Address retrieval failed: {@Error}", error))
        
        .Map(customer => {
            var addresses = customer.Addresses ?? new List<Address>();
            
            _logger.LogInformation("Found {Count} addresses for Customer {CustomerId}", 
                addresses.Count, request.CustomerId);
                
            return _mapper.Map<List<AddressDto>>(addresses);
        });
}
}
