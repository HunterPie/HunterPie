using HunterPie.Core.Architecture;
using System;
using System.Diagnostics;
using System.Timers;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using LiveCharts.Defaults;

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

        public SeriesCollection Series { get; private set; } = new();

        private ChartValues<ObservablePoint> CPUSeries = new();
        private ChartValues<ObservablePoint> RAMSeries = new();

        public TelemetricsViewModel()
        {
            // TODO: Make the graphs easier to code
            var newSeries = new LineSeries()
            {
                Title = "CPU",
                Stroke = Brushes.White,
                Fill = new SolidColorBrush(Color.FromArgb(0x50, 255, 255, 255)),
                PointGeometrySize = 0,
                StrokeThickness = 2,
                LineSmoothness = 1
            };
            newSeries.Values = CPUSeries;

            var series2 = new LineSeries()
            {
                Title = "Memory",
                Stroke = new SolidColorBrush(Color.FromArgb(255, 127, 160, 244)),
                Fill = new SolidColorBrush(Color.FromArgb(0x50, 127, 160, 244)),
                PointGeometrySize = 0,
                StrokeThickness = 2,
                LineSmoothness = 1
            };
            series2.Values = RAMSeries;

            Series.Add(newSeries);
            //Series.Add(series2);
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
                RAMSeries.Add(new ObservablePoint(elapsed, Memory));
            }
            CPU = CpuPerfCounter.NextValue() / Environment.ProcessorCount;
            CPUSeries.Add(new ObservablePoint(elapsed, CPU));

            if (CPUSeries.Count > 50)
            {
                CPUSeries.RemoveAt(0);
                RAMSeries.RemoveAt(0);
            }
        }

        public void ExecuteGarbageCollector() => GC.Collect();

    }
}
