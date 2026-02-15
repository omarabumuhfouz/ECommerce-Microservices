using System.Security.Claims;

namespace SharedKernel.Extensions;

public static class ClaimsPrincipalExtensions
{

    public static Guid GetUserId(this ClaimsPrincipal user)
    {

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
    }

}