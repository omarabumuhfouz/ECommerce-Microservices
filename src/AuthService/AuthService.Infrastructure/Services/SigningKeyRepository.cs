using AuthService.Application.Interfaces;
using AuthService.Domain.SigningKey;
using AuthService.Infrastructure.Data;

namespace AuthService.Infrastructure.Services;


public class SigningKeyRepository(AppDbContext context) : ISigningKeyRepository
{
    public List<SigningKey> GetAllActiveSigningKeys()
    {
        return context.SigningKeys.Where(sk => sk.IsActive).ToList();
    }
}