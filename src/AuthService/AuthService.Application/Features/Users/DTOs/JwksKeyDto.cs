namespace AuthService.Application.Features.Users.DTOs;

/// <summary>
    /// Represents an individual JSON Web Key (JWK).
    /// </summary>
    public class JwksKeyDto
    {
        /// <summary>
        /// Key type (e.g., "RSA").
        /// </summary>
        public string Kty { get; set; }

        /// <summary>
        /// The intended use of the key (e.g., "sig" for signature).
        /// </summary>
        public string Use { get; set; }

        /// <summary>
        /// Key ID, used to identify the key.
        /// </summary>
        public string Kid { get; set; }

        /// <summary>
        /// Algorithm used with this key (e.g., "RS256").
        /// </summary>
        public string Alg { get; set; }

        /// <summary>
        /// RSA modulus, Base64URL-encoded.
        /// </summary>
        public string N { get; set; }

        /// <summary>
        /// RSA exponent, Base64URL-encoded.
        /// </summary>
        public string E { get; set; }
    }