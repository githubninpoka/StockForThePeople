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
    private readonly IStockForThePeopleContext _stockForThePeopleContext;
    public InternalDataService(IStockForThePeopleContext context)
    {
        _stockForThePeopleContext = context;
    }
    public async Task<List<AssetGetDtoList>> GetAllAssetsAsync()
    {
        List<AssetGetDtoList> returnable = new();
        var domainAssets = _stockForThePeopleContext.Assets;
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
        var returnable = await _stockForThePeopleContext.Assets.Where(x => x.Ticker == ticker).Select(y => new AssetGetDto()
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
        List<MarketWithVolumeGetDto> market = await _stockForThePeopleContext.MarketData
            .Where(x => x.AssetId == asset.Id && x.Date >= fromDate)
            .OrderBy(z => z.Date)
            .Select(y => new MarketWithVolumeGetDto()
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

    public async Task<SingleAssetWithMarketAndInformationListGetDto> GetMarketWithInformationForAssetAsync(
        string ticker,
        InformationOptions informationOptions,
        int numberOfDays = 30,
        DateTime lastDateTime = default(DateTime)

        )
    {

        if (lastDateTime == default(DateTime))
        {
            lastDateTime = DateTime.Now;
        }
        DateOnly lastDate = DateOnly.FromDateTime(lastDateTime);

        DateTime fromDateTime = lastDateTime - TimeSpan.FromDays(numberOfDays);
        DateOnly fromDate = DateOnly.FromDateTime(fromDateTime);

        AssetGetDto asset = await GetAssetByTickerAsync(ticker);
        List<MarketWithInformationGetDto> marketWithInformation = await _stockForThePeopleContext.MarketData
            .Where(x => x.AssetId == asset.Id && x.Date >= fromDate && x.Date <= lastDate)
            .OrderBy(z => z.Date)
            .Select(y => new MarketWithInformationGetDto()
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

        if (informationOptions.VolumeComparedToAverageInPercentage)
        {
            double averageVolume = marketWithInformation.Average(x => x.Volume);
            double onePercent = averageVolume / 100;
            foreach (var item in marketWithInformation)
            {
                double delta = item.Volume - averageVolume;
                double percentageDeviation = delta / onePercent;
                item.VolumeComparedToAverageInPercentage = Math.Round( 100 + percentageDeviation, 2);
            }
        }
        if (informationOptions.ValueComparedToMedianInPercentage)
        {
            decimal medianValue = GetMedian(marketWithInformation.Select(x => x.Value).ToArray());
            double onePercent = (double)medianValue / 100;
            foreach (var item in marketWithInformation)
            {
                double delta = (double)item.Value - (double)medianValue;
                double percentageDeviation = delta / onePercent;
                item.ValueComparedToMeanInPercentage = Math.Round( 100 + percentageDeviation, 2);
            }
        }
        return new SingleAssetWithMarketAndInformationListGetDto() { 
            Asset = asset, 
            Information = marketWithInformation 
        };
    }

    // if I ever have a successful application with plenty of other numerical types, I might switch to generics..
    private static decimal GetMedian(decimal[] sourceArray)
    {
        // https://www.mytecbits.com/microsoft/dot-net/calculate-median-in-c-sharp

        if (sourceArray == null || sourceArray.Length == 0)
        {
            throw new ArgumentException("Cannot calculate median value for empty array");
        }
        Array.Sort(sourceArray);
        int middle = sourceArray.Length / 2;
        if (sourceArray.Length % 2 != 0)
        {
            return sourceArray[middle];
        }
        decimal value1 = sourceArray[middle];
        decimal value2 = sourceArray[middle - 1];
        return (value1 + value2) / 2;
    }

}
