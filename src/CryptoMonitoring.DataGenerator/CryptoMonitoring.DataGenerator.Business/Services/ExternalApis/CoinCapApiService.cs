using AutoMapper;
using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.Extensions.Configuration;

namespace CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;

public class CoinCapApiService : ICoinCapApiService
{
    private readonly IExternalApiHttpClient _externalApiHttpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoinCapApiService(
        IExternalApiHttpClient externalApiHttpClient,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _externalApiHttpClient = externalApiHttpClient;
        _externalApiHttpClient.SetBaseAddress(configuration["COINCAP_BASE_URL_ADDRESS"]!);
        _externalApiHttpClient.SetBearerApiKey(configuration["COINCAP_API_KEY"]!);

        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task GetCryptoCurrenciesAsync()
    {
        var entities = await _externalApiHttpClient.GetDataAsync<CoinCapAssetsResponse>("assets");

        if (entities !=  null)
        {
            var cryptoCurrencies = _mapper.Map<List<CryptoCurrency>>(entities.Data);

            foreach (var cryptoCurrency in cryptoCurrencies)
            {
                if (!await _unitOfWork.CryptoCurrencies.IsNameExistAsync(cryptoCurrency.Name))
                {
                    cryptoCurrency.Id = Guid.NewGuid();

                    await _unitOfWork.CryptoCurrencies.AddAsync(cryptoCurrency);
                    await _unitOfWork.SaveAsync();
                }
            }
        }
    }
}