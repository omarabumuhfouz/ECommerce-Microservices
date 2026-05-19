using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDetailsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

    public GetCustomerByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetCustomerByIdQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }



    async Task<Result<CustomerDetailsDto>> IRequestHandler<GetCustomerByIdQuery, Result<CustomerDetailsDto>>.Handle(GetCustomerByIdQuery request, CancellationToken ct)
{
    _logger.LogInformation("Fetching details for CustomerId: {CustomerId}", request.CustomerId);

    return await _unitOfWork.GetRepository<Customer>()
        .FirstOrDefaultAsync(new GetCustomerByIdSpec(request.CustomerId), ct)
        .ToResult(DomainErrors.Customer.NotFound(request.CustomerId))

        .TapError(error => _logger.LogWarning("Search failed for CustomerId: {CustomerId}. Error: {@Error}", 
            request.CustomerId, error))

        .Tap(customer => _logger.LogInformation("Successfully retrieved Customer: {CustomerId}", customer.Id))

        .Map(customer => _mapper.Map<CustomerDetailsDto>(customer));
}
}
