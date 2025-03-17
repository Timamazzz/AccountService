using AutoMapper;
using AccountService.Application.Dto;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Core.Entities;
using AccountService.Core.Errors.Codes;
using ApplicationException = AccountService.Core.Errors.Exceptions.ApplicationException;

namespace AccountService.Application.Services;

/// <summary>
/// Service for managing user operations.
/// </summary>
public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper) : IUserService
{
    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id)
                   ?? throw new ApplicationException(ApplicationErrors.NotFound);

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username)
                   ?? throw new ApplicationException(ApplicationErrors.NotFound);

        return mapper.Map<UserDto>(user);
    }

    public async Task CreateUserAsync(UserDto userDto)
    {
        if (await userRepository.GetByUsernameAsync(userDto.Username) is not null)
        {
            throw new ApplicationException(ApplicationErrors.AlreadyTaken);
        }

        userDto.PasswordHash = passwordHasher.HashPassword(userDto.PasswordHash);
        var user = mapper.Map<User>(userDto);

        await userRepository.AddAsync(user);
    }
}