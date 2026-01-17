using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Extensions;
using HunterPie.Core.Scan.Service;
using HunterPie.UI.Architecture.Graphs;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;

public class TelemetricsViewModel : WidgetViewModel
{
    private readonly Timer _dispatcher;

    private PerformanceCounter CpuPerfCounter
    {
        get
        {
            // Lazy initializer
            if (field is null)
            {
                using var self = Process.GetCurrentProcess();
                field = new("Process", "% Processor Time", self.ProcessName, true);
            }

            return field;
        }
    }

    public long Memory
    {
        get;
        set => SetValue(ref field, value);
    }

    public float CPU
    {
        get;
        set => SetValue(ref field, value);
    }

    public int Threads
    {
        get;
        set => SetValue(ref field, value);
    }

    public SeriesCollection CPUSeries { get; private set; }
    public SeriesCollection RAMSeries { get; private set; }

    private readonly ChartValues<ObservablePoint> CPUPoints = new();
    private readonly ChartValues<ObservablePoint> WorkingSetPoints = new();
    private readonly ChartValues<ObservablePoint> PrivateSetPoints = new();

    public Func<double, string> BytesFormatter { get; } =
        new Func<double, string>((value) => ((long)value).FormatBytes());

    public Func<double, string> PercentageFormatter { get; } =
        new Func<double, string>((value) => $"{value:0.0}%");

    public Observable<long> ScanTime { get; }

    public TelemetricsViewModel(
        TelemetricsWidgetConfig config,
        IScanService scanService
    ) : base(config, "Metrics Widget", WidgetType.Window)
    {
        ScanTime = scanService.ElapsedTime;
        // TODO: Make the graphs easier to code
        CPUSeries = new LinearSeriesCollectionBuilder()
            .AddSeries(CPUPoints, "CPU", Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
            .Build();

        RAMSeries = new LinearSeriesCollectionBuilder()
            .AddSeries(WorkingSetPoints, "Working", (Color)ColorConverter.ConvertFromString("#5C97F8"))
            .AddSeries(PrivateSetPoints, "Private", (Color)ColorConverter.ConvertFromString("#7B65F0"))
            .Build();

        _dispatcher = new Timer(5000)
        {
            AutoReset = true
        };
        _dispatcher.Elapsed += UpdateInformation;
        _dispatcher.Start();
    }

    private double start = double.MaxValue;
    public void UpdateInformation(object source, ElapsedEventArgs e)
    {
        double elapsed = TimeSpan.FromTicks(e.SignalTime.Ticks).TotalSeconds;
        start = Math.Min(start, elapsed);

        elapsed -= start;

        using (var self = Process.GetCurrentProcess())
        {
            Memory = self.WorkingSet64 / 1024 / 1024;
            Threads = self.Threads.Count;
            WorkingSetPoints.Add(new ObservablePoint(elapsed, self.WorkingSet64));
            PrivateSetPoints.Add(new ObservablePoint(elapsed, self.PrivateMemorySize64));
        }

        CPU = CpuPerfCounter.NextValue() / Environment.ProcessorCount;
        CPUPoints.Add(new ObservablePoint(elapsed, CPU));

        if (CPUPoints.Count > 50)
        {
            CPUPoints.RemoveAt(0);
            WorkingSetPoints.RemoveAt(0);
            PrivateSetPoints.RemoveAt(0);
        }
    }

    public void ExecuteGarbageCollector() => GC.Collect();

}