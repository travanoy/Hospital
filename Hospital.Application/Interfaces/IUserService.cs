using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces;

public interface IUserService
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}