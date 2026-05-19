using CustomerService.Application.Features.Specifications.Customers;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Customers.Commands.EditCustomer;

public class EditCustomerCommandHandler : ICommandHandler<EditCustomerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditCustomerCommandHandler> _logger;

    public EditCustomerCommandHandler(
     IUnitOfWork unitOfWork,
     ILogger<EditCustomerCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditCustomerCommand, Result<Unit>>.Handle(EditCustomerCommand request, CancellationToken ct)
{
    _logger.LogInformation("Beginning update for Customer: {CustomerId}. New Data: {@Request}", 
        request.CustomerId, new { request.FirstName, request.LastName });

    var customerRepo = _unitOfWork.GetRepository<Customer>();

    return await customerRepo.FirstOrDefaultAsync(new GetCustomerByIdSpec(request.CustomerId), ct)
        .ToResult(DomainErrors.Customer.NotFound(request.CustomerId))
        .Bind(customer => customer.Update(request.FirstName, request.LastName, request.PhoneNumber)
            .Map(_ => customer)) 
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Tap(customer => _logger.LogInformation("Successfully updated Customer {CustomerId}", customer.Id))
        .TapError(error => _logger.LogWarning("Update failed for Customer {CustomerId}. Error: {@Error}", 
            request.CustomerId, error))
        .Map(_ => Unit.Value);
}
}