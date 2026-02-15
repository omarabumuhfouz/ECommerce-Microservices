namespace CustomerService.Application.Customers.Commands.EditCustomer;

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

    async Task<Result<Unit>> IRequestHandler<EditCustomerCommand, Result<Unit>>.Handle(EditCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerRepository = _unitOfWork.GetRepository<Domain.Entities.Customer>();
        var customer = await customerRepository.GetSingleBySpecAsync(new GetCustomerByIdSpec(request.CustomerId), cancellationToken);

        if (customer is null) return DomainErrors.Customer.NotFound(request.CustomerId);

       customer = customer.Edit(request.FirstName, request.LastName, request.PhoneNumber).Value;

        customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Customer Updated Successfully with Id : Customer Id : {@CustomerId}", customer.Id);

        return Unit.Value;
    }
}