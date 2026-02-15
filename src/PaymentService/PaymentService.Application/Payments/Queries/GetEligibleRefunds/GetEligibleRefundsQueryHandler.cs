// using SharedKernel.Abstractions;
// using SharedKernel.Common;
// using SharedKernel.Shared;

// namespace RefundService.Application.Refunds.Queries.GetEligibleRefunds;

// public class GetEligibleRefundsQueryHandler : IQueryHandler<GetEligibleRefundsQuery, PagedList<PendingRefundDto>>
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;

//     public GetEligibleRefundsQueryHandler(
//         IUnitOfWork unitOfWork,
//         IMapper mapper
//     )
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }

//     Task<Result<PagedList<PendingRefundDto>>> IRequestHandler<GetEligibleRefundsQuery, Result<PagedList<PendingRefundDto>>>.Handle(GetEligibleRefundsQuery request, CancellationToken ct)
//     {
//         //var eligible = await _context.Cancellations
//         //            .Include(c => c.Order)
//         //            .ThenInclude(o => o.Payment)
//         //            .Where(c => c.Status == CancellationStatus.Approved && c.Refund == null
//         //            && c.Order.Payment.PaymentMethod.ToLower() != "COD")


//         //            .Select(c => new PendingRefundResponseDTO
//         //            {
//         //                CancellationId = c.Id,
//         //                OrderId = c.OrderId,
//         //                OrderAmount = c.OrderAmount,
//         //                CancellationCharge = c.CancellationCharges ?? 0.00m,
//         //                ComputedRefundAmount = c.OrderAmount - (c.CancellationCharges ?? 0.00m),
//         //                CancellationRemarks = c.Remarks
//         //            }).ToListAsync();


//         //return new ApiResponse<List<PendingRefundResponseDTO>>(200, eligible);
//         return null;
//     }
// }
