using AutoMapper;
using CryptoMonitoring.Common.DTOs.CoinCapApi;
using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Business.Mapper.Profiles;

public class CryptoCurrencyProfile : Profile
{
    public CryptoCurrencyProfile()
    {
        CreateMap<CoinCapAssetResponseDto, CryptoCurrency>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol.ToUpper()))
            .ForMember(dest => dest.CoinCapId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CoinGeckoId, opt => opt.Ignore());

        CreateMap<CoinGeckoCoinResponseDto, CryptoCurrency>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol.ToUpper()))
            .ForMember(dest => dest.CoinCapId, opt => opt.Ignore())
            .ForMember(dest => dest.CoinGeckoId, opt => opt.MapFrom(src => src.Id));
    }
}