namespace StockForThePeople.WebBlazor.Client.ChartDataTypes;

public class ChartDataSet
{
    // the actual array. does not need to have the same amount of items as the accompanying Label type
    public double[] Data { get; set; }
    
    // Chart.js uses BorderColor as the line color for the set
    public string BorderColor { get; set; }

    // if not provided, Chart.js will assume false
    public string? Stepped { get; set; }
    // if not provided, Chart.js will assume 'Y' or something that is available
    public string? YAxisId { get; set; }

    // if not provided, Chart.js will not render a label for this dataset
    // otherwise it will render this and make it togglable
    public string? Label { get; set; }

    public bool Fill { get; set; } = false;

    public string? PointStyle { get; set; }

}
