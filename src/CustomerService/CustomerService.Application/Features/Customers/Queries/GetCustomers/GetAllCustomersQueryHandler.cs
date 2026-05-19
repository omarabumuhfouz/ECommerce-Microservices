using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, PagedList<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken ct)
{
    var repo = _unitOfWork.GetRepository<Customer>();
    var spec = new GetCustomersSpec(request.FirstName, request.LastName, request.PhoneNumber, request.PagingParams);

    var customers = await repo.GetListAsync(spec, ct);
    var totalCount = await repo.CountAsync(spec, ct); 

    return Result.Success(customers)
        .Map(items => {
            var dtos = _mapper.Map<List<CustomerDto>>(items);
            return new PagedList<CustomerDto>(
                dtos, 
                request.PagingParams.Page, 
                request.PagingParams.PageSize, 
                totalCount);
        });
}
}
