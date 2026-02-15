// namespace ShoppingCartService.Application.Common.Behaviors;


// public class IdempotentBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IIdempotentCommand
//     where TResponse : Result
// {
//     private readonly IIdempotencyService _idempotencyService;

//     public IdempotentBehavior(IIdempotencyService idempotencyService)
//     {
//         _idempotencyService = idempotencyService;
//     }

//     public async Task<TResponse> Handle(
//         TRequest request,
//         RequestHandlerDelegate<TResponse> next,
//         CancellationToken cancellationToken)
//     {
//         if (await _idempotencyService.ExistsAsync(request.Id))
//         {
//             return (TResponse)Result.Success();
//         }

//         TResponse response = await next();

//         if (!response.IsSuccess)
//         {
//             return response;
//         }

//         await _idempotencyService.MarkAsProcessedAsync(
//             request.Id,
//             typeof(TRequest).Name);

//         return response;
//     }
// }