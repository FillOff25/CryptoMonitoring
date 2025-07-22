using AutoMapper;
using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Mapper.Converters;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Business.Mapper.Profiles;

public class MarketDataProfile : Profile
{
    public MarketDataProfile()
    {
        CreateMap<CoinCapAssetResponseDto, MarketData>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Volume24hUsd, opt => opt.ConvertUsing(new NullableStringToNullableDecimalConverter(), src => src.VolumeUsd24Hr))
            .ForMember(dest => dest.MarketCapUsd, opt => opt.ConvertUsing(new NullableStringToNullableDecimalConverter(), src => src.MarketCapUsd))
            .ForMember(dest => dest.Vwap24hUsd, opt => opt.ConvertUsing(new NullableStringToNullableDecimalConverter(), src => src.Vwap24Hr))
            .ForMember(dest => dest.Change24hPercent, opt => opt.ConvertUsing(new NullableStringToNullableDecimalConverter(), src => src.ChangePercent24Hr))
            .ForMember(dest => dest.CryptoCurrencyId, opt => opt.Ignore())
            .ForMember(dest => dest.CryptoCurrency, opt => opt.Ignore());
    }
}