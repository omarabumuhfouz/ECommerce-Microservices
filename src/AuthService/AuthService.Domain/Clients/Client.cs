using AuthService.Domain.Extensions;
using AuthService.Domain.RefreshTokens;

namespace AuthService.Domain.Clients; 

public record Client : AggregateRoot
{
    public string ClientId { get; private set; }
    public string Name { get; private set; }
    public string ClientURL { get; private set; }

    private Client() { } 

    private Client(Guid id, string clientId, string name, string clientURL) : base(id)
    {
        ClientId = clientId;
        Name = name;
        ClientURL = clientURL;
    }

    public static Result<Client> Create(string clientId, string name, string clientURL)
    {
        if (string.IsNullOrWhiteSpace(clientId)) return DomainErrors.Client.ClientIdRequired;

        if (string.IsNullOrWhiteSpace(name)) return DomainErrors.Client.NameRequired;

        if (!clientURL.IsValidUrl()) return DomainErrors.Client.UrlRequired;

        if (!Uri.TryCreate(clientURL, UriKind.Absolute, out _)) return DomainErrors.Client.UrlInvalid;

        var client = new Client(Guid.NewGuid(), clientId, name, clientURL);
        return Result.Success(client);
    }


    public Result WithName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return DomainErrors.Client.NameRequired;

        Name = name;
        return Result.Success();
    }

    public Result WithClientURL(string clientURL)
    {
        if (string.IsNullOrWhiteSpace(clientURL)) return DomainErrors.Client.UrlRequired;

        if (!Uri.TryCreate(clientURL, UriKind.Absolute, out _)) return DomainErrors.Client.UrlInvalid;

        ClientURL = clientURL;
        return Result.Success();
    }
}
