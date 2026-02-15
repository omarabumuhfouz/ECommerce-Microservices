using AuthService.Api.Contracts.Users;
using AuthService.Application.Features.RefreshTokens.DTOs;
using AuthService.Application.Features.RefreshTokens.Refresh;
using AuthService.Application.Features.Users.Commands.ChangeEmail;
using AuthService.Application.Features.Users.Commands.ChangePassword;
using AuthService.Application.Features.Users.Commands.ChangeRole;
using AuthService.Application.Features.Users.Commands.Login;
using AuthService.Application.Features.Users.DTOs;
using AuthService.Application.Features.Users.Queries.GetUserById;
using AuthService.Features.Users.Commands.Logout;
using AuthService.Features.Users.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Extensions;
using SharedKernel.Constants;

namespace AuthService.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var authApi = app.MapGroup("/api/auth")
            .WithTags("Auth")
            .WithOpenApi();

        authApi.MapPost("/login", Login)
            .Accepts<LoginCommand>("application/json")
            .Produces<TokenResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Authenticate user and issue tokens")
            .WithName("Login");

        authApi.MapPost("/register", Register)
            .Accepts<RegisterCommand>("application/json")
            .Produces<RegisterResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Register a new user")
            .WithName("Register");

        authApi.MapPost("/refresh-token", RefreshToken)
            .Accepts<RefreshTokenCommand>("application/json")
            .Produces<TokenResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Refresh access token")
            .WithName("RefreshToken");

        authApi.MapPost("/logout", Logout)
            .RequireAuthorization(AuthConstants.Policies.Shared)
            .Accepts<LogoutRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Logout user");

        authApi.MapPost("/change-password", ChangePassword)
            .RequireAuthorization(AuthConstants.Policies.Shared)
            .Accepts<ChangePasswordRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Change user password");

        authApi.MapPut("/users/email", ChangeEmail)
            .RequireAuthorization(AuthConstants.Policies.Shared)
            .Accepts<ChangeEmailRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Change user email");

        authApi.MapGet("/users/{userId:guid}", GetUserById)
            .RequireAuthorization(AuthConstants.Policies.Shared)
            .Produces<UserDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get user by ID");

        authApi.MapPut("/users/{userId:guid}/role", ChangeRole)
            .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<ChangeRoleRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Change user role")
            .WithDescription("Allows an admin to change another user's role.");

        return authApi;
    }

    private static async Task<IResult> Register([FromBody] RegisterCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);
        return result.Match(v => Results.Ok(v), e => e.ToProblem());
    }

    private static async Task<IResult> Login([FromBody] LoginCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);
        return result.Match(v => Results.Ok(v), e => e.ToProblem());
    }

    private static async Task<IResult> RefreshToken([FromBody] RefreshTokenCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);
        return result.Match(v => Results.Ok(v), e => e.ToProblem());
    }

    private static async Task<IResult> Logout([FromBody] LogoutRequest request, [FromServices] IHttpContextAccessor context, [FromServices] ISender sender)
    {
        Guid userId = context.HttpContext?.User.GetUserId() ?? Guid.Empty;
        // Note: Ensure your LogoutCommand logic handles the userId correctly as discussed
        var result = await sender.Send(new LogoutCommand(userId, request.RefreshToken, request.ClientId, request.IsLogoutFromAllDevices));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }

    private static async Task<IResult> ChangePassword([FromBody] ChangePasswordRequest request, [FromServices] IHttpContextAccessor context, [FromServices] ISender sender)
    {
        Guid userId = context.HttpContext?.User.GetUserId() ?? Guid.Empty;
        var result = await sender.Send(new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword, request.ConfirmNewPassword));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }

    private static async Task<IResult> ChangeEmail([FromBody] ChangeEmailRequest request, [FromServices] IHttpContextAccessor context, [FromServices] ISender sender)
    {
        Guid userId = context.HttpContext?.User.GetUserId() ?? Guid.Empty;
        var result = await sender.Send(new ChangeEmailCommand(userId, request.NewEmail));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }

    private static async Task<IResult> GetUserById([FromRoute] Guid userId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetUserByIdQuery(userId));
        return result.Match(v => Results.Ok(v), e => e.ToProblem());
    }

    private static async Task<IResult> ChangeRole([FromRoute] Guid userId, [FromBody] ChangeRoleRequest request, [FromServices] ISender sender)
    {
        var result = await sender.Send(new ChangeRoleCommand(userId, request.Roles));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }
}