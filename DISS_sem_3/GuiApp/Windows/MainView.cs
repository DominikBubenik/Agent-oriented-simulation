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

        // Wire random seed checkbox
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
                var rnd = new System.Random();
                numSeed.Value = rnd.Next(1, int.MaxValue);
                numSeed.Enabled = false;
            }
            else
            {
                numSeed.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to generate random seed: " + ex.Message);
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
                numSeed.Value = rnd.Next(1, int.MaxValue);
            }
        }
        catch { }

        var args = GetCurrentArguments();
        OnRunRequested?.Invoke(this, args);
    }

    public void PauseBtnClick(object? sender, EventArgs e)
    {
        _paused = !_paused;
        btnPause.Text = _paused ? "RESUME" : "PAUSE";
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
        int seed = (int)numSeed.Value;
        int replications = (int)numReplications.Value;
        bool observation = chkObservationMode.Checked;
        bool allocateBestRoom = chkAllocateBestRoom.Checked;
        double endTime = (double)numEndTime.Value * 3600;
        double warmUp = (double)numWarmUp.Value * 3600;
        double exp4MaxWaitTime = (double)numMaxWaitTimeExp4.Value * 60;
        int nursesCount = (int)numNurses.Value;
        int doctorsCount = (int)numDoctors.Value;
        int entryMax = (int)numMaxEntryQueue.Value;
        int medicalMax = (int)numMaxMedicalQueue.Value;
        
        var experimentVariant = (ResourceAllocatingStrategy)cmbExperimentVariant.SelectedItem;

        return new StartSimulationArgs(
            seed: seed,
            replications: replications,
            observationMode: observation,
            endSimulationTime: endTime,
            nursesCount: nursesCount,
            doctorsCount: doctorsCount,
            entryMax: entryMax,
            medicalMax: medicalMax,
            timeIntervalSeconds: 0,
            experimentVariant: experimentVariant,
            warmUpProof: chkWarmUpProof.Checked,
            allocateBestRoom: allocateBestRoom,
            warmUp: warmUp,
            exp4WaitTime: exp4MaxWaitTime
         );
    }

}
