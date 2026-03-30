using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }
}