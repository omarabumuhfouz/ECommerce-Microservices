using AutoMapper;
using Microsoft.Extensions.Logging;
using CancellationService.Application.Enums;
using CancellationService.Application.Services;
using SharedKernel.Common;
using CancellationService.Application.Cancellations.Specifications;

namespace CancellationService.Application.Cancellations.Commands.CreateCancellation;

public class CreateCancellationCommandHandler : ICommandHandler<CreateCancellationCommand, CancellationDto>
{
    private readonly IRepository<Cancellation> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCancellationCommandHandler> _logger; 

    public CreateCancellationCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderService orderService,
        IMapper mapper,
        ILogger<CreateCancellationCommandHandler> logger)
    {

        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.GetRepository<Cancellation>();
        _orderService = orderService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CancellationDto>> Handle(CreateCancellationCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting cancellation request for Order {OrderId}", request.OrderId);

        return await _orderService.GetOrderByIdAsync(request.OrderId)
            .Ensure(order => order.Status == OrderStatus.Processing,
                order => DomainErrors.Cancellation.NotEligible(order.Status.ToString()))

            .Bind(async order =>
            {
                var existing = await _repository.FirstOrDefaultAsync(new GetCancellationByOrderIdSpec(order.Id), ct);
                return existing is null
                    ? order
                    : Result.Failure<OrderDto>(DomainErrors.Cancellation.DuplicateCancellation(order.Id));
            })

            .Bind(order => Cancellation.Create(
                orderId: order.Id,
                reasonString: request.Reason,
                orderAmount: order.TotalAmount))

            .Tap(async cancellation =>
            {
                await _repository.AddAsync(cancellation);
                await _unitOfWork.SaveChangesAsync(ct);
            })

            .Tap(cancellation => _logger.LogInformation(
                "Cancellation successfully created for Order {OrderId}. ID: {CancellationId}",
                request.OrderId, cancellation.Id))
            .Map(cancellation => _mapper.Map<CancellationDto>(cancellation))

            .TapError(error => _logger.LogWarning(
                "Cancellation process failed for Order {OrderId}. Error: {Error}",
                request.OrderId, error));
    }
}