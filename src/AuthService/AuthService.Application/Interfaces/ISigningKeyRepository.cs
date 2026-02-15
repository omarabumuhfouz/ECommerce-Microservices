using AuthService.Domain.SigningKey;

namespace AuthService.Application.Interfaces;

public interface ISigningKeyRepository
{
    List<SigningKey> GetAllActiveSigningKeys();
}