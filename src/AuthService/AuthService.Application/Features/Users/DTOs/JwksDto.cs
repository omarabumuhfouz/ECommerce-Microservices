namespace AuthService.Application.Features.Users.DTOs;

 /// <summary>
    /// Represents a JSON Web Key Set (JWKS) containing one or more keys.
    /// </summary>
    public class JwksDto
    {
        /// <summary>
        /// The list of active JSON Web Keys.
        /// </summary>
        public required List<JwksKeyDto> Keys { get; set; }
    }