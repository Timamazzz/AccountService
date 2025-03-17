using AutoMapper;
using AccountService.API.Contracts.Requests;
using AccountService.API.Contracts.Responses;
using AccountService.Application.Dto;

namespace AccountService.API.Mappers;

public class UserApiProfile : Profile
{
    public UserApiProfile()
    {
        CreateMap<UserDto, UserResponse>();
        CreateMap<CreateUserRequest, UserDto>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}