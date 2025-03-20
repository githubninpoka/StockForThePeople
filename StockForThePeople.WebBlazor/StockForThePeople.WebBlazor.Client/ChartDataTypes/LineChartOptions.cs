namespace StockForThePeople.WebBlazor.Client.ChartDataTypes;

public class LineChartOptions
{
    public string ChartTitle { get; set; } = "";
    public bool Responsive { get; set; } = true;
    public ChartAxes Scales { get; set; }
}
