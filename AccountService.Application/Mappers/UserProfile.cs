using AutoMapper;
using AccountService.Application.Dto;
using AccountService.Core.Entities;

namespace AccountService.Application.Mappers;

/// <summary>
/// AutoMapper profile for user entity and DTO mapping.
/// </summary>
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}