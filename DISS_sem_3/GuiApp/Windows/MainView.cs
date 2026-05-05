using DISS_sem_3;
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
        cmbExperimentVariant.DataSource = Enum.GetValues(typeof(ResourceAllocatingStrategy));
        cmbExperimentVariant.SelectedItem = ResourceAllocatingStrategy.Exp0FirstAvailable;
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

    public StartSimulationArgs GetCurrentArguments()
    {
        int seed = 12345;
        int replications = 1;
        bool observation;
        bool allocateBestRoom;
        double startTime = 0;
        double endTime = 627;
        int nursesCount = 3;
        int doctorsCount = 2;
        int entryMax = 2;
        int medicalMax = 2;
        double exp4MaxWaitTime = 0.5;
        int after = 2;
        double lambda = 0.08;
        double intervalSeconds = 0;
        double warmUp = 15000; // default warm-up in milliseconds (or whatever unit the simulation expects)

        if (!string.IsNullOrWhiteSpace(txtSeed.Text) && int.TryParse(txtSeed.Text, out var sVal))
            seed = sVal;
        if (!string.IsNullOrWhiteSpace(txtReplications.Text) && int.TryParse(txtReplications.Text, out var rVal))
            replications = rVal;
        observation = chkObservationMode.Checked;  
        
        allocateBestRoom = chkAllocateBestRoom.Checked;
     
        if (!string.IsNullOrWhiteSpace(txtEndTime.Text) && double.TryParse(txtEndTime.Text, out var etVal))
            endTime = etVal * 3600;   
        
        if (!string.IsNullOrWhiteSpace(txtMaxWaitTimeExp4.Text) && double.TryParse(txtMaxWaitTimeExp4.Text, out var exp4Val))
            exp4MaxWaitTime = exp4Val * 60;

        if (!string.IsNullOrWhiteSpace(txtNurses.Text) && int.TryParse(txtNurses.Text, out var lVal))
            nursesCount = lVal;
        if (!string.IsNullOrWhiteSpace(txtDoctors.Text) && int.TryParse(txtDoctors.Text, out var bVal))
            doctorsCount = bVal; 
        if (!string.IsNullOrWhiteSpace(txtMaxEntryQueue.Text) && int.TryParse(txtMaxEntryQueue.Text, out var entry))
            entryMax = entry;
        if (!string.IsNullOrWhiteSpace(txtMaxMedicalQueue.Text) && int.TryParse(txtMaxMedicalQueue.Text, out var medical))
            medicalMax = medical;
        
        var experimentVariant = (ResourceAllocatingStrategy)cmbExperimentVariant.SelectedItem;

        // Read warmUp from UI if provided
        if (!string.IsNullOrWhiteSpace(txtWarmUp?.Text) && int.TryParse(txtWarmUp.Text, out var wu))
            warmUp = wu * 3600;

        return new StartSimulationArgs(
            seed: seed,
            replications: replications,
            observationMode: observation,
            endSimulationTime: endTime,
            nursesCount: nursesCount,
            doctorsCount: doctorsCount,
            entryMax: entryMax,
            medicalMax: medicalMax,
            timeIntervalSeconds: intervalSeconds,
            experimentVariant: experimentVariant,
            warmUpProof: chkWarmUpProof.Checked,
            allocateBestRoom: allocateBestRoom,
            warmUp: warmUp,
            exp4WaitTime: exp4MaxWaitTime
         );
    }

}
