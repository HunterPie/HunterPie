using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Architecture.Colors;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Graphs;

public class LinearSeriesCollectionBuilder
{
    private readonly SeriesCollection _instance = new();

    public LinearSeriesCollectionBuilder AddSeries(ChartValues<ObservablePoint> points, string title, Color color)
    {
        Brush fill = ColorFadeGradient.FromColor(
            color: AnalogousColor.NegativeFrom(
                main: color,
                angle: 41.5
            )
        );

        var series = new LineSeries
        {
            Title = title,
            Stroke = new SolidColorBrush(color),
            Fill = fill,
            PointGeometry = null,
            StrokeThickness = 1,
            LineSmoothness = 0,
            Values = points
        };

        _instance.Add(series);

        return this;
    }

    public SeriesCollection Build() => _instance;

}