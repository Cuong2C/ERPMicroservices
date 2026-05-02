using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace AuthService.Api.Apis.OpenIdConnect.JsonWebKeySet;

public class JwksProvider
{
    public JsonWebKeySetResult Jwks { get; }

    public JwksProvider(IOptions<JwtOptions> options)
    {
        using var rsa = RSA.Create();

        var publicKeyBytes = Convert.FromBase64String(options.Value.PublicKey);
        rsa.ImportRSAPublicKey(publicKeyBytes, out _);

        var parameters = rsa.ExportParameters(false);

        var jwk = new JwkKey(
            Kty: "RSA",
            Use: "sig",
            Kid: options.Value.KeyId,
            N: Base64UrlEncode(parameters.Modulus!),
            E: Base64UrlEncode(parameters.Exponent!),
            Alg: "RS256"
        );

        Jwks = new JsonWebKeySetResult(new List<JwkKey> { jwk });
    }

    private static string Base64UrlEncode(byte[] input)
    {
        var output = Convert.ToBase64String(input);
        return output.Replace('+', '-')
                     .Replace('/', '_')
                     .TrimEnd('=');
    }
}
