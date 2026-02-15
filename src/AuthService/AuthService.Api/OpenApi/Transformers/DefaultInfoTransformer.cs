using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

internal sealed class DefaultInfoTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info.Title = "Project API";
        document.Info.Version = "1.0"; // optional
        return Task.CompletedTask;
    }
}
