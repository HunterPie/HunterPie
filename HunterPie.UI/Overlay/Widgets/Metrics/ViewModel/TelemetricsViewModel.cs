using HunterPie.Core.Architecture;
using System;
using System.Diagnostics;
using System.Timers;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using LiveCharts.Defaults;
using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Architecture.Graphs;
using HunterPie.Core.Extensions;

namespace HunterPie.UI.Overlay.Widgets.Metrics.ViewModel
{
    public class TelemetricsViewModel : Bindable
    {

        private long _memoryUsage;
        private float _cpuUsage;
        private int _threads;

        private PerformanceCounter _cpuPerfCounter;
        private PerformanceCounter CpuPerfCounter
        {
            get
            {
                // Lazy initializer
                if (_cpuPerfCounter is null)
                    using (Process self = Process.GetCurrentProcess())
                        _cpuPerfCounter = new("Process", "% Processor Time", self.ProcessName, true);

                return _cpuPerfCounter;
            }
        }

        public long Memory
        {
            get => _memoryUsage;
            set { SetValue(ref _memoryUsage, value); }
        }

        public float CPU
        {
            get => _cpuUsage;
            set { SetValue(ref _cpuUsage, value); }
        }

        public int Threads
        {
            get => _threads;
            set { SetValue(ref _threads, value); }
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

        public TelemetricsViewModel()
        {
            // TODO: Make the graphs easier to code
            CPUSeries = new LinearSeriesCollectionBuilder()
                .AddSeries(CPUPoints, "CPU", Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
                .Build();

            RAMSeries = new LinearSeriesCollectionBuilder()
                .AddSeries(WorkingSetPoints, "Working", (Color)ColorConverter.ConvertFromString("#5C97F8"))
                .AddSeries(PrivateSetPoints, "Private", (Color)ColorConverter.ConvertFromString("#7B65F0"))
                .Build();

            var dispatcher = new Timer(5000)
            {
                AutoReset = true
            };
            dispatcher.Elapsed += UpdateInformation;
            dispatcher.Start();
        }
        double start = double.MaxValue;
        public void UpdateInformation(object source, ElapsedEventArgs e)
        {
            double elapsed = TimeSpan.FromTicks(e.SignalTime.Ticks).TotalSeconds;
            start = Math.Min(start, elapsed);

            elapsed -= start;

            using (Process self = Process.GetCurrentProcess())
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
}
