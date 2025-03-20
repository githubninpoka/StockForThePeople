namespace StockForThePeople.WebBlazor.Client.ChartDataTypes;

public class ChartAxis
{
    public string Type { get; set; } = "linear";

    // "left" or "right"
    public string Position { get; set; } = "left";

    public ChartAxisTitle Title { get; set; }

    // show axis or no
    public bool Display { get; set; } = false;

    public bool BeginAtZero { get; set; } = true;


    // manipulate axis a bit to not begin or end at min/max value of corresponding data
    public double? Min { get; set; }
    public double? Max { get; set; }

    // should Chart.js display a grid for this axis?
    // represented in chart.js as a separate object
    public ChartAxisGrid? Grid { get; set; }
}
