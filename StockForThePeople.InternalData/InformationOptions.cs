using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.InternalData;

public class InformationOptions
{
    public bool VolumeComparedToAverageInPercentage { get; set; } = false;
    public bool ValueComparedToMedianInPercentage { get; set; } = false; 
}
