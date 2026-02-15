namespace CustomerService.Application.Customers.Queries;

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

    async Task<Result<CustomerDetailsDto>> IRequestHandler<GetCustomerByUserIdQuery, Result<CustomerDetailsDto>>.Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
    {
        var customerRepository = _unitOfWork.GetRepository<Customer>();
        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByUserIdSpec(request.UserId), cancellationToken);

        if (customer is not null) return _mapper.Map<CustomerDetailsDto>(customer);

        _logger.LogWarning("Customer Not found with UserId : {@UserId}", request.UserId);
        return DomainErrors.Customer.NotFoundByUser(request.UserId);
    }
}