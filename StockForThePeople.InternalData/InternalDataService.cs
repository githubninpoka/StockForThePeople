using Microsoft.EntityFrameworkCore;
using StockForThePeople.Data;
using StockForThePeople.InternalData.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.InternalData;

public class InternalDataService : IInternalDataService
{
    private readonly StockForThePeopleSqliteContext _stockForThePeopleSqliteContext;
    public InternalDataService(StockForThePeopleSqliteContext context)
    {
        _stockForThePeopleSqliteContext = context;
    }
    public async Task<List<AssetGetDtoList>> GetAllAssetsAsync()
    {
        List<AssetGetDtoList> returnable = new();
        var domainAssets = _stockForThePeopleSqliteContext.Assets;
        foreach (var asset in domainAssets)
        {
            returnable.Add(new AssetGetDtoList()
            {
                Id = asset.Id,
                Name = asset.Name,
                Ticker = asset.Ticker
            });
        }
        return returnable;
    }

    public async Task<AssetGetDto> GetAssetByTickerAsync(string ticker)
    {
        var returnable = await _stockForThePeopleSqliteContext.Assets.Where(x => x.Ticker == ticker).Select(y => new AssetGetDto()
        {
            Id = y.Id,
            Name = y.Name,
            Ticker = y.Ticker,
            Exchange = y.Exchange,
            Symbol = y.Symbol,
            Currency = y.Currency,
            Created = y.CreatedAt
        }).FirstOrDefaultAsync(); ;
        return returnable;
    }

    public async Task<AssetWithMarketGetDto> GetMarketForAssetAsync(string ticker)
    {
        int magicNumber = 60;
        DateTime fromDateTime = DateTime.Now - TimeSpan.FromDays(magicNumber);
        DateOnly fromDate = DateOnly.FromDateTime(fromDateTime);
        AssetGetDto asset = await GetAssetByTickerAsync(ticker);
        List<MarketGetDto> market = await _stockForThePeopleSqliteContext.MarketData
            .Where(x => x.AssetId == asset.Id && x.Date >= fromDate)
            .OrderBy(z => z.Date)
            .Select(y => new MarketGetDto()
            {
                Date = y.Date,
                Volume = y.Volume,
                Value = y.Value,
                Open = y.Open,
                Close = y.Close,
                Low = y.Low,
                High = y.High
            })
            .ToListAsync();

        var averageVolume = market.Average(x => x.Volume);
        var percent = averageVolume / 100;
        foreach (var item in market)
        {
            var deviation = item.Volume - averageVolume;
            item.VolumeComparedToAverage = Math.Round(item.Volume / percent, 2);
        }

        return new AssetWithMarketGetDto() { Asset = asset, MarketHistory = market };
    }
}
