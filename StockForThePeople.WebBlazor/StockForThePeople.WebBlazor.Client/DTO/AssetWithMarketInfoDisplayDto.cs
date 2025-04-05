using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.WebBlazor.Client.DTO;

public class AssetWithMarketInfoDisplayDto
{
    public AssetGetDto Asset { get; init; }
    public List<MarketWithInfoDisplayDto> Information { get; init; } = new();
}
