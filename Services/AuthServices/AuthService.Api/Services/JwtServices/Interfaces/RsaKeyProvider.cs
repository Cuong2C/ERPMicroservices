using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace AuthService.Api.Services.JwtServices.Interfaces;

public class RsaKeyProvider
{
    public RSA Rsa { get; }

    public RsaKeyProvider(IOptions<JwtOptions> options)
    {
        var rsa = RSA.Create();

        rsa.ImportRSAPrivateKey(
            Convert.FromBase64String(options.Value.PrivateKey),
            out _
        );

        Rsa = rsa;
    }
}
