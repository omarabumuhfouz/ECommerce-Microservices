using SharedKernel.Common;

namespace CustomerService.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, PagedList<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCustomersQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task<Result<PagedList<CustomerDto>>> IRequestHandler<GetAllCustomersQuery, Result<PagedList<CustomerDto>>>.Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customerRepository = _unitOfWork.GetRepository<Customer>();

        var customers = await customerRepository.ListAsync(
            new GetCustomersSpec(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.PagingParams),
            cancellationToken); 

        if (!customers.Any()) return new PagedList<CustomerDto>(new List<CustomerDto>(), request.PagingParams.Page, request.PagingParams.PageSize, 0);


        return new PagedList<CustomerDto>(
            _mapper.Map<List<CustomerDto>>(customers),
            request.PagingParams.Page,
            request.PagingParams.PageSize,
            await customerRepository.CountAsync(cancellationToken));
    }
}
