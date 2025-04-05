using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.InternalData.DTO;

public class SingleAssetWithMarketAndInformationListGetDto
{
    public AssetGetDto Asset { get; init; }

    public List<MarketWithInformationGetDto> Information { get; init; }

}
