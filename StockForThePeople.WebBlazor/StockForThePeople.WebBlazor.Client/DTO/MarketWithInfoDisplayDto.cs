namespace StockForThePeople.WebBlazor.Client.DTO
{
    public class MarketWithInfoDisplayDto
    {
        public DateOnly Date { get; init; }
        public int Volume { get; init; }
        public decimal Value { get; init; }
        public decimal Open { get; init; }
        public decimal Close { get; init; }
        public decimal Low { get; init; }
        public decimal High { get; init; }


        public double? VolumeComparedToAverageInPercentage { get; set; }
        public double? ValueComparedToMeanInPercentage { get; set; }


    }
}
