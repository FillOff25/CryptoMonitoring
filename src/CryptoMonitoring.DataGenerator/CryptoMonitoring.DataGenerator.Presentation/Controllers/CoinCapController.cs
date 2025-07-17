using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/coincap")]
public class CoinCapController : ControllerBase
{
    private readonly ICoinCapApiService _coinCapApiService;

    public CoinCapController(ICoinCapApiService coinCapApiService)
    {
        _coinCapApiService = coinCapApiService;
    }

    [HttpPost("crypto-currencies")]
    public async Task<IActionResult> GetCryptoCurrencies()
    {
        await _coinCapApiService.GetCryptoCurrenciesAsync();

        return Ok();
    }
}
