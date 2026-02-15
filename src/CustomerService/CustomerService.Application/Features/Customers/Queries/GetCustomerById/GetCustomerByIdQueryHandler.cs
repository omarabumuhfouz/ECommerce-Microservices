namespace CustomerService.Application.Customers.Queries;

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



    async Task<Result<CustomerDetailsDto>> IRequestHandler<GetCustomerByIdQuery, Result<CustomerDetailsDto>>.Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customerRepository = _unitOfWork.GetRepository<Customer>();
        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByIdSpec(request.CustomerId), cancellationToken);

        if (customer is not null) return _mapper.Map<CustomerDetailsDto>(customer);

        _logger.LogWarning("Customer Not foound with Id : {@CustomerId}", request.CustomerId);
        return DomainErrors.Customer.NotFound(request.CustomerId);
    }
}
