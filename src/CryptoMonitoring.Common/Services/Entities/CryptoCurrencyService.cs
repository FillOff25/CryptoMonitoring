using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.Common.Interfaces.Caching;
using CryptoMonitoring.Common.Interfaces.Entities;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Services.Entities;

public class CryptoCurrencyService : ICryptoCurrencyService
{
    private readonly IRedisCacherService _redisCacherService;
    private readonly IUnitOfWork _unitOfWork;

    public CryptoCurrencyService(
        IRedisCacherService redisCacherService,
        IUnitOfWork unitOfWork)
    {
        _redisCacherService = redisCacherService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CryptoCurrency> GetCryptoCurrencyWithCache(string name, string symbol)
    {
        var cryptoCurrency = await _redisCacherService.GetAsync<CryptoCurrency>(_redisCacherService.GenerateCryptoCurrencyKey(name, symbol));

        if (cryptoCurrency == null)
        {
            cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(name, symbol)
                ?? throw new HttpException($"Cryptocurrency (name: {name}, symbol: {symbol}) does not exist in database", 400);

            await _redisCacherService.SetAsync(_redisCacherService.GenerateCryptoCurrencyKey(name, symbol), cryptoCurrency);
        }

        return cryptoCurrency;
    }
}
