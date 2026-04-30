using System;
using System.Linq;
using System.Windows.Forms;
using DISS_SEM_GUI;
using DISS_SEM_GUI.EventsArguments;
using MainLogic;

namespace Airport_GUI;

public partial class TurboWindow : Form
{
    public event EventHandler? OnPauseRequested;
    public event EventHandler? OnResumeRequested;
    public event EventHandler? OnStopRequested;
    public event EventHandler? OnRunRequested;
    private readonly Dictionary<string, ScottPlot.WinForms.FormsPlot> _plotMapping;

    // Stats update method will accept StatisticsList-like data
    public TurboWindow()
    {
        InitializeComponent();

        // Wire designer buttons to controller events
        try
        {
            if (btnRun != null) btnRun.Click += (s, e) => OnRunRequested?.Invoke(this, EventArgs.Empty);
            if (btnPause != null) btnPause.Click += (s, e) => OnPauseRequested?.Invoke(this, EventArgs.Empty);
            if (btnResume != null) btnResume.Click += (s, e) => OnResumeRequested?.Invoke(this, EventArgs.Empty);
            if (btnStop != null) btnStop.Click += (s, e) => OnStopRequested?.Invoke(this, EventArgs.Empty);
        }
        catch { }

        this.FormClosed += (s, e) => { /* owner will unsubscribe */ };
        
        _plotMapping = new Dictionary<string, ScottPlot.WinForms.FormsPlot>
        {
            { "TotalPatientsInSystem", formsPlot1 },
            { "TotalWalkInInSystem", formsPlot2 },
            { "TotalAmbulancedInSystem",    formsPlot3 },
            { "TotalTimeInSystem",formsPlot4 },
            { "TotalTimeInSystemWalkIn",  formsPlot5 },
            { "TotalTimeInSystemAmbulanced",  formsPlot6 }
        };
    }

    public void UpdateDashboard(int replication, Dictionary<string, StatHistory> history)
    {
        if (this.InvokeRequired)
        {
            this.BeginInvoke(new Action(() => UpdateDashboard(replication, history)));
            return;
        }

        txtTurboReplication.Text = replication.ToString();
        // Efficiently update only the stats we have plots for
        foreach (var mapping in _plotMapping)
        {
            
            if (history.TryGetValue(mapping.Key, out var data))
            {
                UpdatePlot(mapping.Value, data.X.ToArray(), data.Avg.ToArray(), data.Lower.ToArray(), data.Upper.ToArray());
            }
        }
    }
    
    
    // UI helper to set status label to Running/Paused/Stopped safely
    public void SetStatus(string status)
    {
        try
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => { if (lblStatus != null && !lblStatus.IsDisposed) lblStatus.Text = status; }));
                }
                else
                {
                    if (lblStatus != null && !lblStatus.IsDisposed) lblStatus.Text = status;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("TurboWindow.SetStatus failed: " + ex.Message);
        }
    }

    // New: render snapshot with confidence intervals (avg + lower + upper)
    public void RenderSnapshot(double[] xs, double[][] avgSnapshots, double[][] lowerSnapshots, double[][] upperSnapshots)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => RenderSnapshot(xs, avgSnapshots, lowerSnapshots, upperSnapshots)));
            return;
        }
        try
        {
            if (avgSnapshots.Length > 0 && avgSnapshots[0] != null && avgSnapshots[0].Length == xs.Length)
                UpdatePlot(formsPlot1, xs, avgSnapshots[0], lowerSnapshots?[0], upperSnapshots?[0]);
            if (avgSnapshots.Length > 1 && avgSnapshots[1] != null && avgSnapshots[1].Length == xs.Length)
                UpdatePlot(formsPlot2, xs, avgSnapshots[1], lowerSnapshots?[1], upperSnapshots?[1]);
            if (avgSnapshots.Length > 2 && avgSnapshots[2] != null && avgSnapshots[2].Length == xs.Length)
                UpdatePlot(formsPlot3, xs, avgSnapshots[2], lowerSnapshots?[2], upperSnapshots?[2]);
            if (avgSnapshots.Length > 3 && avgSnapshots[3] != null && avgSnapshots[3].Length == xs.Length)
                UpdatePlot(formsPlot4, xs, avgSnapshots[3], lowerSnapshots?[3], upperSnapshots?[3]);
            if (avgSnapshots.Length > 4 && avgSnapshots[4] != null && avgSnapshots[4].Length == xs.Length)
                UpdatePlot(formsPlot5, xs, avgSnapshots[4], lowerSnapshots?[4], upperSnapshots?[4]);
            if (avgSnapshots.Length > 5 && avgSnapshots[5] != null && avgSnapshots[5].Length == xs.Length)
                UpdatePlot(formsPlot6, xs, avgSnapshots[5], lowerSnapshots?[5], upperSnapshots?[5]);
        }
        catch { }
    }

    // Overload: accepts optional CI bounds arrays to render CI lines (thin bands)
    /**
     * Kod upraveny s pomocou AI, zdokumentovane v kapitole 20
     */
    private void UpdatePlot(ScottPlot.WinForms.FormsPlot plot, double[] xs, double[] ys, double[]? lowerYs, double[]? upperYs)
    {
        if (xs == null || ys == null || xs.Length == 0 || ys.Length == 0) return;

        plot.Plot.Clear();

        // main average line
        var avg = plot.Plot.Add.Scatter(xs, ys);
        avg.LineWidth = 2;
        avg.MarkerSize = 0;

        // compute axis limits using provided data and CI bounds if any
        double xMin = xs.Min();
        double xMax = xs.Max();
        double yMin = ys.Min();
        double yMax = ys.Max();
        if (lowerYs != null && lowerYs.Length == ys.Length)
        {
            yMin = Math.Min(yMin, lowerYs.Min());
            yMax = Math.Max(yMax, lowerYs.Max());
        }
        if (upperYs != null && upperYs.Length == ys.Length)
        {
            yMin = Math.Min(yMin, upperYs.Min());
            yMax = Math.Max(yMax, upperYs.Max());
        }

        double ySpan = yMax - yMin;
        if (ySpan == 0) { yMin -= 1.0; yMax += 1.0; }
        else { double padding = ySpan * 0.05; yMin -= padding; yMax += padding; }
        if (xMin == xMax) { xMin -= 0.5; xMax += 0.5; }

        try { plot.Plot.Axes.SetLimits(xMin, xMax, yMin, yMax); } catch { }

        // draw CI lines (thin) if provided
        try
        {
            if (lowerYs != null && lowerYs.Length == ys.Length)
            {
                var lower = plot.Plot.Add.Scatter(xs, lowerYs);
                lower.LineWidth = 1; lower.MarkerSize = 0;
            }
            if (upperYs != null && upperYs.Length == ys.Length)
            {
                var upper = plot.Plot.Add.Scatter(xs, upperYs);
                upper.LineWidth = 1; upper.MarkerSize = 0;
            }
        }
        catch { }

        plot.Refresh();
    }

    public void UpdateStats(SimulationStatsDto stats)
    {
        if (this.InvokeRequired) { this.Invoke(new Action(() => UpdateStats(stats))); return; }
        try
        {
            // Replication
            if (txtTurboReplication != null) txtTurboReplication.Text = stats.Replication.ToString();

            // Helper to set avg + CI into two textboxes
            void SetAvgCi(OneStat? s, TextBox avgBox, TextBox ciBox, string? fallback)
            {
                if (s != null)
                {
                    try { avgBox.Text = s.Avg.ToString("0.###"); } catch { avgBox.Text = "-"; }
                    try { ciBox.Text = $"[{s.LowerBound:0.###} - {s.UpperBound:0.###}]"; } catch { ciBox.Text = "-"; }
                }
                else
                {
                    avgBox.Text = fallback ?? "-";
                    ciBox.Text = "-";
                }
            }

            // Show Total Passengers using OneStat if available
            // SetAvgCi(stats.SumTotalPassengersInSystem, txtTurboTotal, txtTurboTotal_CI, stats.TotalPassengersInSystem);
            //
            // // Map other StatisticsList.OneStat values to the designer textboxes
            // SetAvgCi(stats.SumAvgTimeInSystem, txtTurboAvgTime, txtTurboAvgTime_CI, stats.AvgTimeInSystem);
            // // Safely format avg time if OneStat present
            // if (stats.SumAvgTimeInSystem != null) txtTurboAvgTime.Text = GlobalLogger.FormatTime(stats.SumAvgTimeInSystem.Avg);
            // SetAvgCi(stats.SumAvgEntryQueue, txtTurboEntryQueue, txtTurboEntryQueue_CI, stats.EntryQueueLength);
            // SetAvgCi(stats.SumAvgDetectorQueue, txtTurboDetectorQueue, txtTurboDetectorQueue_CI, stats.PassengerDetectorQueueLength);
            // SetAvgCi(stats.SumAvgBeforeDetector, txtTurboBeforeDetector, txtTurboBeforeDetector_CI, null);
            // SetAvgCi(stats.SumAvgAfterDetector, txtTurboAfterDetector, txtTurboAfterDetector_CI, null);
            // SetAvgCi(stats.SumAvgWaitingQueue, txtTurboLuggageQueue, txtTurboLuggageQueue_CI, stats.WaitForLuggageQueueLength);
        }
        catch { }
    }
    
    public double GetSkipFractionFromUI()
    {
        try
        {
            var s = txtTurboSkipPercent?.Text?.Trim() ?? "5%";
            if (string.IsNullOrEmpty(s)) return 0.05;
            if (s.EndsWith("%"))
            {
                var num = s.Substring(0, s.Length - 1).Trim();
                if (double.TryParse(num, out var v)) return Math.Max(0.0, Math.Min(0.99, v / 100.0));
            }
            if (double.TryParse(s, out var d))
            {
                // If entered as whole number >1 assume percent
                if (d > 1.0) return Math.Max(0.0, Math.Min(0.99, d / 100.0));
                return Math.Max(0.0, Math.Min(0.99, d));
            }
        }
        catch { }
        return 0.05;
    }
}