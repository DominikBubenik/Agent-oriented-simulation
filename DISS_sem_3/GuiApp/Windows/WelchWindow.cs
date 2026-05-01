using System.Diagnostics;

namespace DISS_sem_3.GuiApp.Windows;

public partial class WelchWindow : Form
{
    public event EventHandler? OnPauseRequested;
    public event EventHandler? OnResumeRequested;
    public event EventHandler? OnStopRequested;
    public event EventHandler? OnRunRequested;
    
    private readonly Dictionary<string, ScottPlot.WinForms.FormsPlot> _plotMapping;
    private readonly Stopwatch _renderThrottle = Stopwatch.StartNew();
    private const int MIN_RENDER_MS = 200;
    
    public WelchWindow()
    {
        InitializeComponent();

        // Wire buttons safely to events
        btnRun.Click += (s, e) => OnRunRequested?.Invoke(this, EventArgs.Empty);
        btnPause.Click += (s, e) => OnPauseRequested?.Invoke(this, EventArgs.Empty);
        btnResume.Click += (s, e) => OnResumeRequested?.Invoke(this, EventArgs.Empty);
        btnStop.Click += (s, e) => OnStopRequested?.Invoke(this, EventArgs.Empty);

        // MAP KEYS: These must match the keys used in your SimulationModel[cite: 11]
        _plotMapping = new Dictionary<string, ScottPlot.WinForms.FormsPlot>
        {
            { "CurrentPatientCount", formsPlot1 },
            { "CurrentWalkInPatientCount", formsPlot2 },
            { "CurrentAmbulancePatientCount", formsPlot3 },
            { "CurrentEntryQueueLength", formsPlot4 },
            { "CurrentMedicalTreatWaitingCount", formsPlot5 },
            { "CurrentAllDoctorsUtil", formsPlot6 },
            { "CurrentAllNursesUtil", formsPlot7 },
            { "CurrentAllRoomAUtil", formsPlot8 },
            { "CurrentAllRoomBUtil", formsPlot9 },
        };

        SetupGraphStyles();
    }

    private void SetupGraphStyles()
    {
        string[] titles = { "Current Patient Count", "Current WalkIn Patient Count", "Current Ambulance Patient Count", "Current EntryQueue Length", 
            "Current MedicalTreat Waiting Count", "Current All Doctors Util", "Current All Nurses Util", "Current AllRoom A Util", "Current AllRoom B Util", 
        };
        var plots = new[]
        {
            formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6, formsPlot7, formsPlot8, formsPlot9
        };
        
        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].Plot.Title(titles[i]);
            plots[i].Plot.XLabel("Replication");
            plots[i].Plot.YLabel("Value");
            // Disable interactions to improve performance during high-speed updates
            plots[i].UserInputProcessor.Disable(); 
        }
    }
    
    public void RenderSnapshot(double[] xs, double[][] snapshots)
    {
        if (_renderThrottle.ElapsedMilliseconds < MIN_RENDER_MS) return;
        _renderThrottle.Restart();
        
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => RenderSnapshot(xs, snapshots)));
            return;
        }
 
        if (xs == null || snapshots == null) return;
 
        var plots = new[]
        {
            formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6, 
        };
        for (int i = 0; i < plots.Length && i < snapshots.Length; i++)
        {
            if (snapshots[i] != null && snapshots[i].Length == xs.Length)
                UpdatePlot(plots[i], xs, snapshots[i]);
        }
    }
 
    // ── Private helpers ───────────────────────────────────────────────
 
    private void UpdatePlot(ScottPlot.WinForms.FormsPlot plot, double[] xs, double[] ys)
    {
        if (plot == null || xs == null || ys == null || xs.Length == 0) return;
        try
        {
            plot.Plot.Clear();
            plot.Plot.Add.Scatter(xs, ys);
 
            double xmin = xs.Min(), xmax = xs.Max();
            double ymin = ys.Min(), ymax = ys.Max();
            if (xmin == xmax) { xmin -= 0.5; xmax += 0.5; }
            double span = ymax - ymin;
            if (span == 0) { ymin -= 1; ymax += 1; }
            else { double pad = span * 0.05; ymin -= pad; ymax += pad; }
 
            plot.Plot.Axes.SetLimits(xmin, xmax, ymin, ymax);
            plot.Refresh();
        }
        catch { /* keep UI alive even on bad data */ }
    }
}