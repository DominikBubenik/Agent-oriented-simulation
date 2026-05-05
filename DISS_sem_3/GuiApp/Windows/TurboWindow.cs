using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
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
    private readonly Stopwatch _uiRefreshThrottle = Stopwatch.StartNew();
    private const int REFRESH_MS = 500; // Throttling ensures the UI thread stays responsive
    private readonly Stopwatch _renderThrottle = Stopwatch.StartNew();
    private const int MIN_RENDER_MS = 200;

    public TurboWindow()
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
            { "TotalPatientsInSystem", formsPlot1 },
            { "TotalWalkInInSystem", formsPlot2 },
            { "TotalAmbulancedInSystem", formsPlot3 },
            { "TotalTimeInSystem", formsPlot4 },
            { "TotalTimeInSystemWalkIn", formsPlot5 },
            { "TotalTimeInSystemAmbulanced", formsPlot6 },
            { "EntryQueueWaitWalkIn", formsPlot7 },
            { "EntryQueueWaitAmbulanced", formsPlot8 },
            { "EntryQueueLength", formsPlot9 },
            { "MedicalTreatWaitingTimeA", formsPlot10 },
            { "MedicalTreatWaitingTimeAB", formsPlot11 },
            { "MedicalTreatWaitingTimeB", formsPlot12 },
            { "AllDoctorsUtil", formsPlot13 },
            { "AllNursesUtil", formsPlot14 },
            { "AllRoomAUtil", formsPlot15 },
            { "AllRoomBUtil", formsPlot16 },
            { "FromEntryToMedicalWalkIn", formsPlot17 },
            { "FromEntryToMedicalAmbulance", formsPlot18 },
        };

        SetupGraphStyles();
    }

    private void SetupGraphStyles()
    {
        string[] titles = { "Total Patients", "Walk-In Count", "Ambulance Count", "Avg Time (Total)", "Avg Time (Walk-In)", "Avg Time (Ambulance)", 
            "Avg Entry WaitTime (WalkIn)", "Avg Entry WaitTime (Ambulance)", "AvgEntryQueueLength", "AvgMedicTreatWaitTime TypeA", "AvgMedicTreatWaitTime TypeAB",
            "AvgMedicTreatWaitTime TypeB", "Doctors Utilization", "Nurses Utilization", "Rooms A Utilization", "Rooms B Utilization", "TimeFromEntryToMedicalTreatWalkIn",
            "TimeFromEntryToMedicalTreatAmbulance"
        };
        var plots = new[]
        {
            formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6, formsPlot7, formsPlot8, formsPlot9,
            formsPlot10, formsPlot11, formsPlot12, formsPlot13, formsPlot14, formsPlot15, formsPlot16, formsPlot17, formsPlot18
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

    public void UpdateDashboard(int replication, Dictionary<string, StatHistory> history)
    {
        // THROTTLE: Skip frame if updated too recently to keep the window clickable[cite: 15]
        // if (_uiRefreshThrottle.ElapsedMilliseconds < REFRESH_MS) return;
        // _uiRefreshThrottle.Restart();
        if (this.IsDisposed || this.Disposing) return;
        if ((replication + 1) % 10 != 0)
        {
            return;
        }
        if (this.InvokeRequired)
        {
            this.BeginInvoke(new Action(() => UpdateDashboard(replication, history)));
            return;
        }

        txtTurboReplication.Text = replication.ToString();

        foreach (var mapping in _plotMapping)
        {
            if (history.TryGetValue(mapping.Key, out var data))
            {
                double[] xArr, yArr, lArr, uArr;
                
                // LOCK: Thread-safety prevents crashes if the simulation adds points while we read[cite: 15]
                lock(data) 
                {
                    if (data.X.Count == 0) continue;
                    xArr = data.X.ToArray();
                    yArr = data.Avg.ToArray();
                    lArr = data.Lower.ToArray();
                    uArr = data.Upper.ToArray();
                }

                // DOWNSAMPLING: If data is massive, only plot 400 points to keep rendering fast[cite: 15]
                if (xArr.Length > 400)
                {
                    int step = xArr.Length / 400;
                    xArr = xArr.Where((v, i) => i % step == 0).ToArray();
                    yArr = yArr.Where((v, i) => i % step == 0).ToArray();
                    lArr = lArr.Where((v, i) => i % step == 0).ToArray();
                    uArr = uArr.Where((v, i) => i % step == 0).ToArray();
                }

                UpdatePlot(mapping.Value, xArr, yArr, lArr, uArr);
            }
        }
        Application.DoEvents(); 
    }

    private void UpdatePlot(ScottPlot.WinForms.FormsPlot plot, double[] xs, double[] ys, double[]? lowerYs, double[]? upperYs)
    {
        plot.Plot.Clear();

        // Main line (Average)
        var avg = plot.Plot.Add.Scatter(xs, ys);
        avg.LineWidth = 2;
        avg.MarkerSize = 0;
        avg.Color = ScottPlot.Color.FromHex("#0000FF");

        // CI Lines (Thin boundaries)[cite: 12]
        if (lowerYs != null && lowerYs.Length == ys.Length) {
            var low = plot.Plot.Add.Scatter(xs, lowerYs);
            low.LineWidth = 1; low.MarkerSize = 0;
            low.Color = ScottPlot.Color.FromHex("#880000FF");
        }
        if (upperYs != null && upperYs.Length == ys.Length) {
            var high = plot.Plot.Add.Scatter(xs, upperYs);
            high.LineWidth = 1; high.MarkerSize = 0;
            high.Color = ScottPlot.Color.FromHex("#880000FF");
        }

        // AUTO-LIMITS: Prevent "empty" looking graphs by forcing axes to data range[cite: 12]
        double yMin = ys.Min();
        double yMax = ys.Max();
        if (lowerYs != null) yMin = Math.Min(yMin, lowerYs.Min());
        if (upperYs != null) yMax = Math.Max(yMax, upperYs.Max());

        double yPadding = (yMax - yMin) * 0.05;
        if (yPadding == 0) yPadding = 1.0;

        plot.Plot.Axes.SetLimits(xs.Min(), xs.Max(), yMin - yPadding, yMax + yPadding);
        plot.Refresh();
    }

    public void UpdateStats(SimulationStatsDto stats)
    {
        if (this.IsDisposed || this.Disposing) return;
        if (this.InvokeRequired) { this.BeginInvoke(new Action(() => UpdateStats(stats))); return; }

        void Map(string key, TextBox valBox, TextBox ciBox, bool time = false)
        {
            if (stats.Stats.TryGetValue(key, out var s) && s != null)
            {
                valBox.Text = time ?  $"{TimeSpan.FromSeconds(s.Avg):hh\\:mm\\:ss}": s.Avg.ToString("F3");
                ciBox.Text = $"[{s.LowerBound:F2} - {s.UpperBound:F2}]";
            }
            else 
            {
                valBox.Text = "No Data"; // Diagnostic indicator
            }
        }

        Map("TotalPatientsInSystem", txtTurboTotal, txtTurboTotal_CI);
        Map("TotalWalkInInSystem", txtTurboWalkIn, txtTurboWalkIn_CI);
        Map("TotalAmbulancedInSystem", txtTurboAmbulance, txtTurboAmbulance_CI);
        
        Map("TotalTimeInSystem", txtTurboAvgTime, txtTurboAvgTime_CI, true);
        Map("TotalTimeInSystemWalkIn", txtTurboTimeWalkIn, txtTurboTimeWalkIn_CI, true);
        Map("TotalTimeInSystemAmbulanced", txtTurboTimeAmbulance, txtTurboTimeAmbulance_CI, true);
        
        Map("EntryQueueWaitWalkIn", txtTurboEntryWaitTimeWalkIn, txtTurboEntryWaitTimeWalkIn_CI, true);
        Map("EntryQueueWaitAmbulanced", txtTurboEntryWaitAmbulance, txtTurboEntryWaitTimeAmbulance_CI, true);
        Map("EntryQueueLength", txtTurboEntryQueueLength, txtTurboEntryQueueLength_CI);
        
        Map("MedicalTreatWaitingTimeA", txtTurboMedicalTrWaitingTimeA,txtTurboMedicalTrWaitingTimeA_CI, true);
        Map("MedicalTreatWaitingTimeAB", txtTurboMedicalTrWaitingTimeAB, txtTurboMedicalTrWaitingTimeAB_CI, true);
        Map("MedicalTreatWaitingTimeB", txtTurboMedicalTrWaitingTimeB, txtTurboMedicalTrWaitingTimeB_CI, true);
        
        Map("AllDoctorsUtil", txtTurboAllDoctorsUtil, txtTurboAllDoctorsUtil_CI);
        Map("AllNursesUtil", txtTurboAllNursesUtil, txtTurboAllNursesUtil_CI);
        Map("AllRoomAUtil", txtTurboAllRoomAUtil, txtTurboAllRoomAUtil_CI);
        Map("AllRoomBUtil", txtTurboAllRoomBUtil, txtTurboAllRoomBUtil_CI);
        
        Map("FromEntryToMedicalWalkIn", txtTurboTimeFromEntryToMedicalWalkIn, txtTurboTimeFromEntryToMedicalWalkIn_CI, true);
        Map("FromEntryToMedicalAmbulance", txtTurboTimeFromEntryToMedicalAmbulance, txtTurboTimeFromEntryToMedicalAmbulance_CI, true);
        
        Map("TimeFromEntryToMedicalPriority1", txtTurboTimeFromEntryToMedicalPriority1, txtTurboTimeFromEntryToMedicalPriority1_CI, true);
        Map("TimeFromEntryToMedicalPriority2", txtTurboTimeFromEntryToMedicalPriority2, txtTurboTimeFromEntryToMedicalPriority2_CI, true);
        Map("TimeFromEntryToMedicalPriority3", txtTurboTimeFromEntryToMedicalPriority3, txtTurboTimeFromEntryToMedicalPriority3_CI, true);
        Map("TimeFromEntryToMedicalPriority4", txtTurboTimeFromEntryToMedicalPriority4, txtTurboTimeFromEntryToMedicalPriority4_CI, true);
        Map("TimeFromEntryToMedicalPriority5", txtTurboTimeFromEntryToMedicalPriority5, txtTurboTimeFromEntryToMedicalPriority5_CI, true);
    }

    public void SetStatus(string status)
    {
        if (this.InvokeRequired) { this.BeginInvoke(new Action(() => SetStatus(status))); return; }
        if (lblStatus != null) lblStatus.Text = status;
    }

    public double GetSkipFractionFromUI()
    {
        string text = txtTurboSkipPercent.Text.Replace("%", "").Trim();
        return double.TryParse(text, out double val) ? Math.Clamp(val / 100.0, 0, 0.95) : 0.05;
    }
}