using Microsoft.AspNetCore.Mvc;
using SharedKernel.Extensions;

namespace ApiGateway.Endpoints;

public static class ProfileEndpoints
{
    public static RouteGroupBuilder MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var profilApis = app.MapGroup("/api/profile")
                            .WithTags("Profile")
                            .WithOpenApi();

        // profilApis.MapGet("",null)
        // .ProducesProblem
        // profilApis.MapGet("/", GetProfile)
        //    .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
        //    .Produces<ApiResponse<string>>(StatusCodes.Status400BadRequest)
        //    .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)
        //    .Produces<ApiResponse<string>>(StatusCodes.Status422UnprocessableEntity)
        //    .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
        //    .WithSummary("")
        //    .WithDescription("")
        //    .WithName("");

        return profilApis;
    }

    private static async Task<IResult> GetProfile([FromServices] HttpContext context)
    {
        var userId = context.User.GetUserId();

        return null;
    }
}