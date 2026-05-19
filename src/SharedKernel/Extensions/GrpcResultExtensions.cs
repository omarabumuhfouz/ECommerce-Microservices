using Grpc.Core;
using SharedKernel.Primitives.Results;
namespace SharedKernel.Extensions;

public static class GrpcResultExtensions
{
    // For Result<T>: Returns T or Throws
    public static T ValueOrThrowRpc<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw CreateRpcException(result.Errors);
    }

    // For Result (void): Just checks failure or Throws
    public static void ThrowRpcIfFailure(this Result result)
    {
        if (!result.IsSuccess)
        {
            throw CreateRpcException(result.Errors);
        }
    }

    private static RpcException CreateRpcException(List<Error> errors)
    {
        var firstError = errors.FirstOrDefault();
        if (firstError == null)
        {
            return new RpcException(new Status(StatusCode.Internal, "An unknown error occurred."));
        }

        // MAP YOUR DOMAIN ERROR TYPES TO GRPC STATUS CODES
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCode.NotFound,
            ErrorType.Validation => StatusCode.InvalidArgument,
            ErrorType.Conflict => StatusCode.AlreadyExists,
            ErrorType.Unauthorized => StatusCode.Unauthenticated,
            ErrorType.Forbidden => StatusCode.PermissionDenied,
            _ => StatusCode.Internal
        };

        return new RpcException(new Status(statusCode, firstError.Message));
    }
}