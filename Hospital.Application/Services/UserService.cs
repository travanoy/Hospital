using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _userRepository.GetByUsernameAsync(username);
    }

    public Task AddAsync(User user)
    {
        return _userRepository.AddAsync(user);
    }
}