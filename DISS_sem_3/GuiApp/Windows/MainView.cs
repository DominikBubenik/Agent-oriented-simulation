using DISS_SEM_GUI.EventsArguments;

namespace DISS_SEM_GUI;

/**
 * Kod vygenerovany s pomocou AI, zdokumentovane v kapitole 2
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 3
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 4
 * Kod bol upraveny s pomocou AI, zdokumentovane v kapitole 7
 * Kod bol upraveny s pomocou AI, zdokumentovane v kapitole 11
 * Kod bol upraveny s pomocou AI, zdokumentovane v kapitole 13
 * Kod bol upraveny s pomocou AI, zdokumentovane v kapitole 21
 */
public partial class MainView : Form
{
    public event EventHandler<StartSimulationArgs>? OnRunRequested;
    public event EventHandler<EventArgs>? OnPauseRequested;
    public event EventHandler? OnOpenSensitivityRequested;
    public event EventHandler? OnOpenObservationRequested;
    public event EventHandler? OnOpenTurboRequested;
    public event EventHandler? OnOpenWelchRequested;
    private bool _paused;

    // Charts moved to separate ChartsWindow. MainView no longer stores plot data.

    // Note: LaneWindow is created by the controller. MainView only raises OnRunRequested.

    public MainView()
    {
        InitializeComponent();

        // MainView initialization
        // Wire browse button for CSV directory if present
        try
        {
            if (btnBrowseCsvDirectory != null)
                btnBrowseCsvDirectory.Click += BrowseCsvDirectoryClick;
        }
        catch { }

        // Wire random seed checkbox: when checked generate a random seed and place it into txtSeed
        try
        {
            if (chkRandomSeed != null)
                chkRandomSeed.CheckedChanged += ChkRandomSeed_CheckedChanged;
        }
        catch { }
    }

    private void ChkRandomSeed_CheckedChanged(object? sender, EventArgs e)
    {
        try
        {
            if (chkRandomSeed.Checked)
            {
                // generate a new random positive int seed
                var rnd = new System.Random();
                int seedVal = rnd.Next(1, int.MaxValue);
                txtSeed.Text = seedVal.ToString();
                txtSeed.ReadOnly = true;
            }
            else
            {
                txtSeed.ReadOnly = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to generate random seed: " + ex.Message);
        }
    }

    private void BrowseCsvDirectoryClick(object? sender, EventArgs e)
    {
        using (var dlg = new FolderBrowserDialog())
        {
            dlg.Description = "Select folder to save CSV file";
            if (!string.IsNullOrWhiteSpace(txtCsvDirectory.Text))
                dlg.SelectedPath = txtCsvDirectory.Text;
            var res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                txtCsvDirectory.Text = dlg.SelectedPath;
            }
        }
    }

    public async void RunBtnClick(object? sender, EventArgs e)
    {

        // If random-seed mode is enabled, generate a new seed for this run
        try
        {
            if (chkRandomSeed != null && chkRandomSeed.Checked)
            {
                var rnd = new System.Random();
                txtSeed.Text = rnd.Next(1, int.MaxValue).ToString();
            }
        }
        catch { }

        var args = GetCurrentArguments();
        OnRunRequested?.Invoke(this, args);
    }

    public void PauseBtnClick(object? sender, EventArgs e)
    {
        _paused = !_paused;
        btnPause.Text = _paused ? "Resume" : "Pause";
        OnPauseRequested?.Invoke(this, EventArgs.Empty);
    }

    // Raised by the designer button to open sensitivity window
    public void SensitivityBtnClick(object? sender, EventArgs e)
    {
        OnOpenSensitivityRequested?.Invoke(this, EventArgs.Empty);
    }

    // Open observation window
    public void ObservationBtnClick(object? sender, EventArgs e)
    {
        OnOpenObservationRequested?.Invoke(this, EventArgs.Empty);
    }

    // Open turbo window
    public void TurboBtnClick(object? sender, EventArgs e)
    {
        OnOpenTurboRequested?.Invoke(this, EventArgs.Empty);
    } 
    
    public void WelchBtnClick(object? sender, EventArgs e)
    {
        OnOpenWelchRequested?.Invoke(this, EventArgs.Empty);
    }

    // Thread-safe method to update the current simulation time textbox.
    public void UpdateSimulationTime(double time)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(() => UpdateSimulationTime(time));
            return;
        }

        // If the time represents seconds, format it as hh:mm:ss
        try
        {
            var ts = TimeSpan.FromSeconds(time);
            txtCurrentTime.Text = ts.ToString();
        }
        catch
        {
            // Fallback: show numeric value
            txtCurrentTime.Text = time.ToString("F2");
        }
    }

    public StartSimulationArgs GetCurrentArguments()
    {
        int seed = 12345;
        int replications = 1;
        bool observation;
        double startTime = 0;
        double endTime = 627;
        int nursesCount = 3;
        int doctorsCount = 2;
        int after = 2;
        int experimentVariant = 1000;
        double lambda = 0.08;
        double intervalSeconds = 0;
        double warmUp = 15000; // default warm-up in milliseconds (or whatever unit the simulation expects)

        if (!string.IsNullOrWhiteSpace(txtSeed.Text) && int.TryParse(txtSeed.Text, out var sVal))
            seed = sVal;
        if (!string.IsNullOrWhiteSpace(txtReplications.Text) && int.TryParse(txtReplications.Text, out var rVal))
            replications = rVal;
        observation = chkObservationMode.Checked;
     
        if (!string.IsNullOrWhiteSpace(txtEndTime.Text) && double.TryParse(txtEndTime.Text, out var etVal))
            endTime = etVal * 3600;

        // Parse Time Interval if provided. Accepts either a numeric seconds value or HH:MM:SS (or HH:MM) format.
        if (!string.IsNullOrWhiteSpace(txtTimeInterval.Text))
        {
            var intervalText = txtTimeInterval.Text.Trim();

            // 1. Try numeric parse first (e.g., "3600")
            if (double.TryParse(intervalText, out var numSec))
            {
                intervalSeconds = numSec;
            }
            else if (intervalText.Contains(":"))
            {
                // 2. Manual split for HH:MM:SS or HH:MM
                // This treats the first part strictly as HOURS, even if it's 24, 48, or 100.
                var parts = intervalText.Split(':');
                try
                {
                    double h = 0, m = 0, s = 0;

                    if (parts.Length >= 2) // We have at least HH and MM
                    {
                        h = double.Parse(parts[0]);
                        m = double.Parse(parts[1]);

                        if (parts.Length == 3) // We also have SS
                        {
                            s = double.Parse(parts[2]);
                        }

                        // TotalSeconds = (Hours * 3600) + (Minutes * 60) + Seconds
                        intervalSeconds = (h * 3600) + (m * 60) + s;
                    }
                }
                catch
                {
                    // If parsing fails (e.g., user typed "AA:BB"), intervalSeconds stays 0
                    intervalSeconds = 0;
                }
            }

            // if (intervalSeconds > 0)
            // {
            //     endTime = startTime + intervalSeconds;
            // }
        }

        if (!string.IsNullOrWhiteSpace(txtNurses.Text) && int.TryParse(txtNurses.Text, out var lVal))
            nursesCount = lVal;
        if (!string.IsNullOrWhiteSpace(txtDoctors.Text) && int.TryParse(txtDoctors.Text, out var bVal))
            doctorsCount = bVal;
        if (cmbSystemCapacity.SelectedItem != null &&
            int.TryParse(cmbSystemCapacity.SelectedItem.ToString(), out var capVal))
        {
            experimentVariant = capVal;
        }

        // Read warmUp from UI if provided
        if (!string.IsNullOrWhiteSpace(txtWarmUp?.Text) && int.TryParse(txtWarmUp.Text, out var wu))
            warmUp = wu * 3600;

        // Read warming-proof options (RefreshRate)
        int refreshRate = 100;
        if (!string.IsNullOrWhiteSpace(txtRefreshRate.Text) && int.TryParse(txtRefreshRate.Text, out var rr))
            refreshRate = rr;

        // Find sensitivity inputs
        bool findSensitivityRequested = false;
        double findComfortSeconds = 600.0; // default 10 minutes
        int findDetectorAvg = 20;
        int findLuggageAvg = 10;

        if (chkFindSensitivityGenerateCsv != null)
            findSensitivityRequested = chkFindSensitivityGenerateCsv.Checked;

        if (!string.IsNullOrWhiteSpace(txtFindComfortTime.Text))
        {
            var t = txtFindComfortTime.Text.Trim();
            if (double.TryParse(t, out var numeric))
            {
                findComfortSeconds = numeric;
            }
            else if (t.Contains(':'))
            {
                var parts = t.Split(':');
                try
                {
                    int h = 0, m = 0, s = 0;
                    if (parts.Length >= 1) h = int.Parse(parts[0]);
                    if (parts.Length >= 2) m = int.Parse(parts[1]);
                    if (parts.Length >= 3) s = int.Parse(parts[2]);
                    findComfortSeconds = h * 3600 + m * 60 + s;
                }
                catch { findComfortSeconds = 600.0; }
            }
        }

        if (!string.IsNullOrWhiteSpace(txtFindDetectorQueueAvg.Text) && int.TryParse(txtFindDetectorQueueAvg.Text, out var fdet))
            findDetectorAvg = fdet;
        if (!string.IsNullOrWhiteSpace(txtFindLuggageQueueAvg.Text) && int.TryParse(txtFindLuggageQueueAvg.Text, out var flag))
            findLuggageAvg = flag;

        return new StartSimulationArgs(
            seed: seed,
            replications: replications,
            observationMode: observation,
            endSimulationTime: endTime,
            nursesCount: nursesCount,
            doctorsCount: doctorsCount,
            afterDetectorCount: after,
            timeIntervalSeconds: intervalSeconds,
            systemCapacity: experimentVariant,
            refreshRate: refreshRate,
            warmUpProof: chkWarmUpProof.Checked,
            warmUp: warmUp,
            sensibilityRequested: chkSensRun.Checked,
            sensCapacity: !string.IsNullOrWhiteSpace(txtSensCapacity.Text) && int.TryParse(txtSensCapacity.Text, out var sc) ? sc : 1000,
            sensReplications: !string.IsNullOrWhiteSpace(txtSensReplications.Text) && int.TryParse(txtSensReplications.Text, out var sr) ? sr : 10,
            sensGraphPoints: !string.IsNullOrWhiteSpace(txtSensGraphPoints.Text) && int.TryParse(txtSensGraphPoints.Text, out var gp) ? gp : 10,
            findSensitivityRequested: findSensitivityRequested,
            findComfortSeconds: findComfortSeconds,
            findEntryQueueAvg: findDetectorAvg,
            findLuggageQueueAvg: findLuggageAvg
            ,
            csvGenerateRequested: chkFindSensitivityGenerateCsv != null && chkFindSensitivityGenerateCsv.Checked,
            csvDirectory: !string.IsNullOrWhiteSpace(txtCsvDirectory.Text) ? txtCsvDirectory.Text : null,
            csvFileName: !string.IsNullOrWhiteSpace(txtCsvFileName.Text) ? txtCsvFileName.Text : null
         );
    }

}
