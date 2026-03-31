using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    Task RegisterAsync(string username, string password, string role);
}