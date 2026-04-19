using Isopoh.Cryptography.Argon2;

namespace AuthService.Api.Identity;

public class CustomHasher
{
    public static string HashByArgon2(string input)
    {
        return Argon2.Hash(input);
    }

    public static bool VerifyByArgon2(string hash, string input)
    {
        return Argon2.Verify(hash, input);
    }
}
