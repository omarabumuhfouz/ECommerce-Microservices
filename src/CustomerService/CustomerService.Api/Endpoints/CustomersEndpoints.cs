using CustomerService.Api.Contracts.Address;
using CustomerService.Api.Contracts.Customer;
using CustomerService.Application.Addresses.DTOs;
using CustomerService.Application.Customers.DTOs;
using CustomerService.Application.Features.Addresses.Commands.DeleteAddress;
using CustomerService.Application.Features.Addresses.Commands.SetDefaultAddress;
using CustomerService.Application.Features.Addresses.Queries.GetAddressesByCustomer;
using CustomerService.Application.Features.Customers.Commands.AddCustomer;
using CustomerService.Application.Features.Customers.Queries.GetCustomerById;
using CustomerService.Application.Features.Customers.Queries.GetCustomerByUserId;
using CustomerService.Application.Features.Customers.Queries.IsCustomerExistsById;
using CustomerService.Application.Features.Customers.Queries.IsCustomerExistsByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SharedKernel.Extensions;

namespace CustomerService.Api.Endpoints;

public static class CustomersEndpoints
{
    public static RouteGroupBuilder MapCustomersEndpoints(this IEndpointRouteBuilder app)
    {
        var customersApi = app.MapGroup("/api/customers")
            .WithTags("Customers")
            .WithOpenApi();

        #region Customers Endpoints
        
        customersApi.MapPost("/", AddCustomer)
            // .RequireAuthorization(AuthConstants.Policies.Shared)
            .Accepts<AddCustomerCommand>("application/json")
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a new customer")
            .WithName("AddCustomer");

        customersApi.MapPut("/{userId:Guid}", EditCustomer)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Accepts<EditCustomerRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit an existing customer")
            .WithName("EditCustomer");

        customersApi.MapGet("/{customerId:Guid}/exists", IsExistsCustomer)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Check if a customer exists")
            .WithName("IsExistsCustomer");

        customersApi.MapGet("/user/{userId:Guid}/exists", IsExistsCustomerByUserId)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Check if a customer exists by user ID")
            .WithName("IsExistsCustomerByUserId");

        customersApi.MapGet("/", GetAllCustomers)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces<List<CustomerDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all customers")
            .WithName("GetAllCustomers");

        customersApi.MapGet("/{customerId:Guid}", GetCustomerById)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<CustomerDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Get customer by ID")
            .WithName("GetCustomerById");

        customersApi.MapGet("/user/{userId:Guid}", GetCustomerByUserId)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<CustomerDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Get customer by user ID")
            .WithName("GetCustomerByUserId");

        #endregion

        #region Addresses Endpoints

        customersApi.MapPost("/{customerId:Guid}/addresses", CreateAddress)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Accepts<CreateAddressRequest>("application/json")
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new address")
            .WithName("CreateAddress");

        customersApi.MapDelete("/{customerId:Guid}/addresses/{addressId:Guid}", DeleteAddress)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a customer's address")
            .WithName("DeleteAddress");

        customersApi.MapPut("/{customerId:Guid}/addresses/{addressId:Guid}", EditAddress)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Accepts<EditAddressRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update an address")
            .WithName("EditAddress");

        customersApi.MapPut("/{customerId:Guid}/addresses/{addressId:Guid}/default", SetDefaultAddress)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Set Default Address")
            .WithName("SetDefaultAddress");

        customersApi.MapGet("/{customerId:Guid}/addresses", GetAddressesByCustomer)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<List<AddressDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all addresses by customer ID")
            .WithName("GetAddressesByCustomerId");
            
        #endregion

        return customersApi;
    }
#region Customers Methods
    private static async Task<IResult> AddCustomer([FromBody] AddCustomerCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);

        return result.Match(
           onValue: value => Results.Ok(value),
           onError: e => e.ToProblem());
    }

    private static async Task<IResult> EditCustomer([FromRoute] Guid userId,[FromBody] EditCustomerRequest request, [FromServices] ISender sender , [FromServices] IOutputCacheStore cach)
    {
        var result = await sender.Send(request.ToCommand(userId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> GetCustomerById([FromRoute] Guid customerId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetCustomerByIdQuery(customerId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> GetCustomerByUserId([FromRoute] Guid userId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetCustomerByUserIdQuery(userId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> IsExistsCustomer([FromRoute] Guid customerId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new IsCustomerExistsByIdQuery(customerId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> IsExistsCustomerByUserId([FromRoute] Guid userId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new IsCustomerExistsByUserIdQuery(userId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> GetAllCustomers([AsParameters] GetAllCustomersRequest request, [FromServices] ISender sender)
    {
        var result = await sender.Send(request.ToQuery());

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: e => e.ToProblem()
        );
    }

   
    #endregion
    
    #region  Addresses Methods
    private static async Task<IResult> CreateAddress([FromRoute] Guid customerId,[FromBody] CreateAddressRequest request, [FromServices] ISender sender)
    {
        var result = await sender.Send(request.ToCommand(customerId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> EditAddress(
        [FromRoute] Guid customerId,
        [FromRoute] Guid addressId,
        [FromBody] EditAddressRequest request,
        [FromServices] ISender sender
    )
    {
        var result = await sender.Send(request.ToCommand(customerId, addressId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> DeleteAddress([FromRoute] Guid customerId, [FromRoute] Guid addressId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteAddressCommand(addressId, customerId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: e => e.ToProblem()
        );
    }

    private static async Task<IResult> GetAddressesByCustomer([FromRoute] Guid customerId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetAddressesByCustomerQuery(customerId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: e => e.ToProblem()
        );
    }

     private static async Task<IResult> SetDefaultAddress([FromRoute] Guid customerId, [FromRoute] Guid addressId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new SetDefaultAddressCommand(customerId, addressId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: e => e.ToProblem()
        );
    }
    
    #endregion
}
