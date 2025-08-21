using AutoMapper;
using CryptoMonitoring.Common.DTOs.Auth;
using CryptoMonitoring.Common.Services.Auth;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Mapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequestDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => HasherService.Generate(src.Password)));
    }
}