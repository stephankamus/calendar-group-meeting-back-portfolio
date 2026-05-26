using System.Security.Cryptography;
using System.Text;

namespace CalendarGroupMeeting.Api.Application.Services;

public class TokenService : ITokenService
{
    private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public string GenerateEventToken()
    {
        return GenerateBase62Token(8);
    }

    public string GenerateResponseToken()
    {
        return GenerateBase62Token(32);
    }

    private static string GenerateBase62Token(int length)
    {
        var bytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var result = new StringBuilder(length);
        foreach (var b in bytes)
        {
            result.Append(Base62Chars[b % Base62Chars.Length]);
        }

        return result.ToString();
    }
}
