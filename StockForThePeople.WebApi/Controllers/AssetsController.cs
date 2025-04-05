using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockForThePeople.InternalData;
using StockForThePeople.InternalData.DTO;
using System.Runtime.CompilerServices;

namespace StockForThePeople.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssetsController : ControllerBase
{
    private readonly IInternalDataService _internalDataService;
    private readonly ILogger<AssetsController> _logger;
    public AssetsController(
        IInternalDataService internalDataService, 
        ILogger<AssetsController> logger
        )
    {
        _internalDataService = internalDataService;
        _logger = logger;
    }

    [OutputCache(PolicyName = "Expire300")]
    [HttpGet]
    public async Task<IActionResult>GetAsync()
    {
        return Ok(await _internalDataService.GetAllAssetsAsync());
    }

    // Get api/<Assets>/Tickercode
    [OutputCache(PolicyName = "Expire300")]
    [HttpGet("{ticker}")]
    public async Task<IActionResult> GetByTickerAsync(string ticker)
    {
        return Ok(await _internalDataService.GetAssetByTickerAsync(ticker));
    }

    [OutputCache(PolicyName = "Expire300")]
    [HttpGet("market/{ticker}")]
    public async Task<IActionResult> GetMarketByTickerAsync(string ticker)
    {
        return Ok(await _internalDataService.GetMarketForAssetAsync(ticker));
    }

    [OutputCache(PolicyName = "Expire300")]
    [HttpGet("{ticker}/Information")]
    public async Task<IActionResult> GetInformationByTickerAsync(
        [FromRoute] string ticker,
        [FromQuery] bool ValueComparedToMedianInPercentage = false,
        [FromQuery] bool VolumeComparedToAverageInPercentage = false,
        [FromQuery] int NumberOfDays = 30,
        [FromQuery] DateTime EndDate = new DateTime()
        )
    {
        if (EndDate == new DateTime())
        {
            EndDate = DateTime.Now;
        }
        InformationOptions options = new()
        {
            ValueComparedToMedianInPercentage = ValueComparedToMedianInPercentage,
            VolumeComparedToAverageInPercentage = VolumeComparedToAverageInPercentage
        };
        SingleAssetWithMarketAndInformationListGetDto singleAssetWithMarketAndInformationList = await _internalDataService.GetMarketWithInformationForAssetAsync(ticker, options,NumberOfDays,EndDate);
        return Ok(singleAssetWithMarketAndInformationList);
    }
}
