using AutoMapper;
using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Business.Mapper.Profiles;

public class CryptoCurrencyProfile : Profile
{
    public CryptoCurrencyProfile()
    {
        CreateMap<CoinCapAssetDto, CryptoCurrency>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
