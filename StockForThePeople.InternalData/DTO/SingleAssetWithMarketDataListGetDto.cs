using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.InternalData.DTO;

public class SingleAssetWithMarketDataListGetDto
{
    public AssetGetDto Asset { get; init; }
    public List<MarketGetDto> MarketHistory { get; init; } = new();
}
