using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using StockForThePeople.Data;
using StockForThePeople.Domain.Models;
using StockForThePeople.ExternalData.DTO;
using StockForThePeople.WebApiExecuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.ExternalData;

public class ExternalDataService : IExternalDataService
{
    private ILogger<ExternalDataService> _logger;
    private IWebApiExecuter _webApiExecuter;
    private ExternalDataConfigurationOptions _externalDataConfigurationOptions;
    private IStockForThePeopleContext _dbContext;

    public ExternalDataService(
        ILogger<ExternalDataService> logger,
        IWebApiExecuter webApiExecuter,
        IOptions<ExternalDataConfigurationOptions> options,
        IStockForThePeopleContext dbContext
        )
    {
        _logger = logger;
        _dbContext = dbContext;

        _webApiExecuter = webApiExecuter;
        _webApiExecuter.BaseUrl = options.Value.BaseUrl;
        _externalDataConfigurationOptions = options.Value;

    }

    public async Task LoadHistoricalDataAsync(int numberOfDays)
    {
        // clear MarketData table
        // repopulate MarketData table -1000 days
        _logger.LogInformation("{var1} - {var2} - Initializing", nameof(ExternalDataService), nameof(LoadHistoricalDataAsync));
        //await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM MarketData");
        _dbContext.MarketData.RemoveRange(_dbContext.MarketData);
        await _dbContext.SaveChangesAsync();
        await UpdateDataAsync(numberOfDays);
    }
    public async Task UpdateDataAsync(int numberOfDays = 30)
    {
        _logger.LogInformation("{var1} - {var2} - Updating", nameof(ExternalDataService), nameof(UpdateDataAsync));
        DateTime thirtyDayAgo = DateTime.Now.AddDays(-numberOfDays); // yes yes magic number. have to fix later
        DateTime tomorrow = DateTime.Now.AddDays(1);
        DateOnly start_dateDateOnly = DateOnly.FromDateTime(thirtyDayAgo);
        string start_date = thirtyDayAgo.ToString("yyyy-MM-dd");
        string end_date = tomorrow.ToString("yyyy-MM-dd");

        var alreadyInDb = _dbContext.MarketData.Where(x => x.Date >= start_dateDateOnly).Select(y => new AlreadyInDb() { Id = y.AssetId, Date = y.Date }).ToList();

        var tickers = _dbContext.Assets.Select(x => new { x.Ticker, x.Id }).ToList();
        foreach (var ticker in tickers)
        {
            _logger.LogInformation("{var1} - {var2} - Calling external api for {var3}",
                nameof(ExternalDataService),
                nameof(UpdateDataAsync),
                ticker.Ticker
                );

            List<DailyMarketDto> tickerData = await _webApiExecuter.InvokeGetAsync<List<DailyMarketDto>>(_externalDataConfigurationOptions.MarketHistoricalDataDailyEndpoint +
            ticker.Ticker +
            $"?token={_externalDataConfigurationOptions.ApiToken}" +
            $"&start_date={start_date}&end_date={end_date}");

            List<MarketData> marketDataList = ConvertToMarketData(tickerData, ticker.Id, alreadyInDb);
            await _dbContext.MarketData.AddRangeAsync(marketDataList);
        }
        // batch store everything that's now being tracked by EF
        // all from the same HttpContext
        await _dbContext.SaveChangesAsync();


    }

    private List<MarketData> ConvertToMarketData(List<DailyMarketDto> tickerData, Guid assetId, List<AlreadyInDb> alreadyInDbs)
    {
        List<MarketData> returnable = new();
        foreach (var item in tickerData)
        {
            DateTime measurementDateTime = DateTime.UnixEpoch.AddSeconds(item.timeStampStartOfDay);
            DateOnly entryDate = DateOnly.FromDateTime(measurementDateTime);
            if (alreadyInDbs.Any(x => x.Id == assetId && x.Date == entryDate))
            {
                _logger.LogInformation("Skipping {var} - {var2}", assetId, entryDate);
            }
            else
            {
                _logger.LogInformation("Adding {var} - {var2}", assetId, entryDate);
                returnable.Add(
                    new MarketData
                    {
                        AssetId = assetId,
                        Date = entryDate,
                        Volume = item.volume,
                        Value = (decimal)item.value,
                        Open = (decimal)item.open,
                        Close = (decimal)item.close,
                        Low = (decimal)item.low,
                        High = (decimal)item.high

                    }
                    );
            }
        }
        return returnable;
    }

    internal class AlreadyInDb()
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
    };

}
