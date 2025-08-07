using AutoMapper;
using CryptoMonitoring.Common.DTOs.CoinCapApi;
using CryptoMonitoring.Common.Mapper.Converters;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Mapper.Profiles;

public class PriceHistoryDataProfile : Profile
{
    public PriceHistoryDataProfile()
    {
        CreateMap<CoinCapHistoryDataResponseDto, PriceHistoryData>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.PriceUsd, opt => opt.ConvertUsing(new NullableStringToNullableDecimalConverter(), src => src.PriceUsd))
            .ForMember(dest => dest.CryptoCurrencyId, opt => opt.Ignore())
            .ForMember(dest => dest.CryptoCurrency, opt => opt.Ignore());

        CreateMap<List<decimal>, PriceHistoryData>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)src[0])))
            .ForMember(dest => dest.PriceUsd, opt => opt.MapFrom(src => src[1]))
            .ForMember(dest => dest.CryptoCurrencyId, opt => opt.Ignore())
            .ForMember(dest => dest.CryptoCurrency, opt => opt.Ignore());
    }
}
