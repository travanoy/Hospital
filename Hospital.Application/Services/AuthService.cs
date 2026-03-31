using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Hospital.Application.Services;

public class AuthService : IAuthService
{
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "Admin",
        "Doctor"
    };

    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var normalizedUsername = username.Trim();
        var user = await _userService.GetByUsernameAsync(normalizedUsername);
        if (user is null)
        {
            return null;
        }

        if (!VerifyPassword(password, user.Password))
        {
            return null;
        }

        return user;
    }

    public async Task RegisterAsync(string username, string password, string role)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required.", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(password));
        }

        if (password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters.", nameof(password));
        }

        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role is required.", nameof(role));
        }

        var normalizedUsername = username.Trim();
        var normalizedRole = role.Trim();

        if (!AllowedRoles.Contains(normalizedRole))
        {
            throw new ArgumentException("Role must be Admin or Doctor.", nameof(role));
        }

        var existingUser = await _userService.GetByUsernameAsync(normalizedUsername);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("User with this username already exists.");
        }

        var user = new User
        {
            Username = normalizedUsername,
            Password = HashPassword(password),
            Role = normalizedRole
        };

        await _userService.AddAsync(user);
    }

    private static string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = SHA256.HashData(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        byte[] expectedHash;

        try
        {
            expectedHash = Convert.FromBase64String(passwordHash);
        }
        catch (FormatException)
        {
            return false;
        }

        var actualHash = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        if (actualHash.Length != expectedHash.Length)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}