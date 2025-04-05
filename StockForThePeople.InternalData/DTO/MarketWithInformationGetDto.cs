using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.InternalData.DTO;

public class MarketWithInformationGetDto
{
    public DateOnly Date { get; init; }
    public int Volume { get; init; }
    public decimal Value { get; init; }
    public decimal Open { get; init; }
    public decimal Close { get; init; }
    public decimal Low { get; init; }
    public decimal High { get; init; }

    // from here are values that are calculated by the service,
    // not stored in the database, because they need to be calculated
    // on a per call basis
    public double? VolumeComparedToAverageInPercentage { get; set; }
    public double? ValueComparedToMeanInPercentage { get; set; }


}
