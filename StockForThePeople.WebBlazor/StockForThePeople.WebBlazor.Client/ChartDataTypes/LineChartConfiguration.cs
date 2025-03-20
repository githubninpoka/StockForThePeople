namespace StockForThePeople.WebBlazor.Client.ChartDataTypes;

public class LineChartConfiguration
{
    public string Type { get; set; } = "line";
    public LineChartOptions LineChartOptions { get; set; }
    public ChartDataSets Data { get; set; }

}
