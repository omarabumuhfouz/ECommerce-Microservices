using MediatR;
using SharedKernel.Shared;

namespace SharedKernel.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}