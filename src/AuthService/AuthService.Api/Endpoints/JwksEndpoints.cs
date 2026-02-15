using System.Security.Cryptography;
using AuthService.Application.Features.Users.DTOs;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Api.Endpoints;


public static class JwksEndpoints
{
    public static RouteGroupBuilder MapJwksEndpoints(this IEndpointRouteBuilder app)
    {
        var jwksApi = app.MapGroup(".well-known")
            .WithTags("JWKS")
            .WithOpenApi();

        jwksApi.MapGet("/jwks.json", GetJWKS)
            .WithSummary("Retrieve JSON Web Key Set (JWKS)")
            .WithDescription("Returns the current active JSON Web Keys (JWKs) used for verifying JWT signatures. " +
                             "Clients can use this endpoint to obtain the public keys and validate JWT tokens issued by the server.")
            .WithName("GetJwks");



        return jwksApi;
    }

    private static IResult GetJWKS([FromServices] ISigningKeyRepository signingKeyRepository)
    {
        var keys = signingKeyRepository.GetAllActiveSigningKeys();

        var jwksDto = new JwksDto
        {
            Keys = keys.Select(k => new JwksKeyDto
            {
                Kty = "RSA",
                Use = "sig",
                Kid = k.KeyId,
                Alg = "RS256",
                N = Base64UrlEncoder.Encode(GetModulus(k.PublicKey)),
                E = Base64UrlEncoder.Encode(GetExponent(k.PublicKey))
            }).ToList()
        };


        return Results.Json(jwksDto);

    }

    // Helper method to extract the modulus component from a Base64-encoded public key
    private static byte[] GetModulus(string publicKey)
    {
        // Create a new RSA instance for cryptographic operations
        var rsa = RSA.Create();

        // Import the RSA public key from its Base64-encoded representation
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

        // Export the RSA parameters without including the private key
        var parameters = rsa.ExportParameters(false);
        // Define the JWKS endpoint that responds to GET requests at '/.well-known/jwks.json'
        // Dispose of the RSA instance to free up resources and prevent memory leaks
        rsa.Dispose();

        if (parameters.Modulus == null)
        {
            throw new InvalidOperationException("RSA parameters are not valid.");
        }

        // Return the modulus component of the RSA key
        return parameters.Modulus;
    }

    // Helper method to extract the exponent component from a Base64-encoded public key
    private static byte[] GetExponent(string publicKey)
    {
        // Create a new RSA instance for cryptographic operations
        var rsa = RSA.Create();

        // Import the RSA public key from its Base64-encoded representation
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

        // Export the RSA parameters without including the private key
        var parameters = rsa.ExportParameters(false);

        // Dispose of the RSA instance to free up resources and prevent memory leaks
        rsa.Dispose();

        if (parameters.Exponent == null)
        {
            throw new InvalidOperationException("RSA parameters are not valid.");
        }

        // Return the exponent component of the RSA key
        return parameters.Exponent;
    }

}