using MediatR;
using SharedKernel.Primitives.Results;

namespace SharedKernel.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}