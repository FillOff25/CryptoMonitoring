using AutoMapper;
using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Mapper.Converters;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Business.Mapper.Profiles;

public class PriceHistoryDataProfile : Profile
{
    public PriceHistoryDataProfile()
    {
        CreateMap<CoinCapHistoryDataResponseDto, PriceHistoryData>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.PriceUsd, opt => opt.ConvertUsing(new NullableStringToNullableDecimalConverter(), src => src.PriceUsd))
            .ForMember(dest => dest.CirculatingSupply, opt => opt.ConvertUsing(new NullableStringToNullableDecimalConverter(), src => src.CirculatingSupply))
            .ForMember(dest => dest.CryptoCurrencyId, opt => opt.Ignore())
            .ForMember(dest => dest.CryptoCurrency, opt => opt.Ignore());
    }
}
