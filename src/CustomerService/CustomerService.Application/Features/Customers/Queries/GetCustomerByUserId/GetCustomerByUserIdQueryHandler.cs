using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerByUserId;

public class GetCustomerByUserIdQueryHandler : IQueryHandler<GetCustomerByUserIdQuery, CustomerDetailsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerByUserIdQueryHandler> _logger;

    public GetCustomerByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetCustomerByUserIdQueryHandler> logger

    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<Result<CustomerDetailsDto>> IRequestHandler<GetCustomerByUserIdQuery, Result<CustomerDetailsDto>>.Handle(
    GetCustomerByUserIdQuery request, 
    CancellationToken ct)
{
    _logger.LogInformation("Querying Customer by UserId: {UserId}", request.UserId);

    return await _unitOfWork.GetRepository<Customer>()
        .FirstOrDefaultAsync(new GetCustomerByUserIdSpec(request.UserId), ct)
        .ToResult(DomainErrors.Customer.NotFoundByUser(request.UserId))
        
        .TapError(error => _logger.LogWarning("Customer lookup failed for UserId: {UserId}. Error: {@Error}", 
            request.UserId, error))
            
        .Tap(customer => _logger.LogInformation("Successfully found Customer {Id} for User {UserId}", 
            customer.Id, request.UserId))
            
        .Map(customer => _mapper.Map<CustomerDetailsDto>(customer));
}
}