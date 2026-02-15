// using AuthService.Queries;
// using AuthService.Users.Queries;
// using Contracts.User;
// using Grpc.Core;

// namespace AuthService.GrpcServices;

// public class GrpcUserServer(ISender sender) : UserProtoService.UserProtoServiceBase
// {
//     public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
//     {
//         try
//         {
//             return (await sender.Send(
//                             new GetUserByIdQuery(Guid.Parse(request.UserId)))
//                    )
//                    .ToGrpcModel();
//         }
//         catch (UserNotFoundException ex)
//         {
//             throw new RpcException(new Status(StatusCode.NotFound, $"User with id {ex.UserId} not found."));
//         }
//         catch(Exception ex)
//         {
//             throw new RpcException(new Status(StatusCode.Internal, ex.Message));
//         }
//     }

// }