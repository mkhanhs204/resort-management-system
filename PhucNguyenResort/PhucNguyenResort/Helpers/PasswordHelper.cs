using Microsoft.AspNetCore.Identity;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Helpers;

public static class PasswordHelper
{
    private static readonly PasswordHasher<User> Hasher = new();

    public static string HashPassword(User user, string password)
    {
        return Hasher.HashPassword(user, password);
    }

    public static bool VerifyPassword(User user, string hashedPassword, string inputPassword)
    {
        var result = Hasher.VerifyHashedPassword(user, hashedPassword, inputPassword);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}