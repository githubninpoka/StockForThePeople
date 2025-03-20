namespace StockForThePeople.WebBlazor.Client.ChartDataTypes;

public class ChartAxes
{
    public ChartAxis? LeftAxis { get; set; }
    public ChartAxis? RightAxis { get; set; }

    // this will make a default invisible Y axis
    public ChartAxis Y { get; set; } = new ChartAxis();
    
}
