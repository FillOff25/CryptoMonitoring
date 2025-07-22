using AutoMapper;
using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Business.Mapper.Profiles;

public class CryptoCurrencyProfile : Profile
{
    public CryptoCurrencyProfile()
    {
        CreateMap<CoinCapAssetResponseDto, CryptoCurrency>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
            .ForMember(dest => dest.CoinCapId, opt => opt.MapFrom(src => src.Id));
    }
}