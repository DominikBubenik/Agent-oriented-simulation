using System.IO;
using DISS_SEM_GUI;
using DISS_SEM_GUI.EventsArguments;

namespace DISS_sem_3;

public class TurboController
{
    private TurboWindow? _turboWindow;
    private SimulationModel? _currentModel;
    private StartSimulationArgs? _args;
    private volatile bool _isStarted = false;
    private int _skipCount;
    private List<double> xs;
    private List<double>[] valsAvg;
    private List<double>[] valsLower;
    private List<double>[] valsUpper;

    private Dictionary<string, StatHistory> _history;
    private SimulationStatsDto? _latestStats;

    
    public void ShowWindow(StartSimulationArgs args)
    {
        _args = args;
        _turboWindow = new TurboWindow();
        _currentModel = new SimulationModel();
        _history = new();
        
        _turboWindow.OnRunRequested += OnRunRequested;
        _turboWindow.OnPauseRequested += OnPauseRequested;

        _currentModel.OnTurboUI += OnTurboRefresh;
        _turboWindow.OnStopRequested += OnStopRequested;
        _turboWindow.FormClosed += OnWindowClosed;
        
        _turboWindow.Show();
    }
      
    private void OnRunRequested(object? sender, EventArgs e)
    {
        if (_args == null || _currentModel == null) return;
        InitBeforeRun();
        _args.ObservationMode = false;
        _args.TurboMode = true;
        _latestStats = null;
        _currentModel.StartSimulation(_args);
        try { _turboWindow?.SetStatus("Running"); } catch { }
    }

    private void InitBeforeRun()
    {
        var skipFrac = 0.05; // default 5%
        try { if (_turboWindow != null) skipFrac = _turboWindow.GetSkipFractionFromUI(); } catch { }
        _skipCount = Math.Max(0, (int)Math.Floor(_args.Replications * skipFrac));
        
        xs = new List<double>();
        valsAvg = new List<double>[6];
        valsLower = new List<double>[6];
        valsUpper = new List<double>[6];
        for (int m = 0; m < 6; m++) { valsAvg[m] = new List<double>(); valsLower[m] = new List<double>(); valsUpper[m] = new List<double>(); }
    }

    private void OnTurboRefresh(SimulationStatsDto st)
    {
         _latestStats = st;
         if (st.Replication % 10 == 0)
         {
             Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " - " + st.Replication);
         }
         if (st.Replication <= _skipCount) return;

       
         foreach (var kvp in st.Stats)
         {
             if (kvp.Value == null) continue;

             if (!_history.ContainsKey(kvp.Key))
                 _history[kvp.Key] = new StatHistory();

             var h = _history[kvp.Key];
             h.X.Add(st.Replication);
             h.Avg.Add(kvp.Value.Avg);
             h.Lower.Add(kvp.Value.LowerBound);
             h.Upper.Add(kvp.Value.UpperBound);
         }

         // Send the complete history to the window
         _turboWindow?.UpdateDashboard(st.Replication, _args.Replications, _history);
         _turboWindow?.UpdateStats(st);
         
         if (_args != null && st.Replication >= _args.Replications -1)
         {
             ExportStatsToCsv();
         }
    }
    
    private void OnPauseRequested(object? sender, EventArgs e)
    {
        if (_currentModel.IsPaused())
        {
            _currentModel.ResumeSimulation();
        }
        else
        {
            _currentModel.PauseSimulation();
        }
    }
    
    private void OnStopRequested(object? sender, EventArgs e)
    {
        _currentModel?.StopSimulation();
        _turboWindow?.SetStatus("Stopped");
        ExportStatsToCsv();
    }
    
    /**
     * Kod vygenerovany s pomocou AI, zdokumentovane v kapitole 5
     */
    private void ExportStatsToCsv()
    {
        if (_latestStats == null || _turboWindow == null) return;

        string filePath = _turboWindow.GetExportFilePath();

        // Definícia metrík a ich príznaku, či ide o čas (rovnako ako v UpdateStats)
        var metricsToExport = new List<(string EnglishKey, string SlovakName, bool IsTime)>
        {
            ("TotalPatientsInSystem", "Celkový počet pacientov", false),
            ("TotalWalkInInSystem", "Počet pacientov (Peši)", false),
            ("TotalAmbulancedInSystem", "Počet pacientov (Sanitka)", false),
            ("TotalTimeInSystem", "Priemerný čas v systéme", true),
            ("TotalTimeInSystemWalkIn", "Čas v systéme (Peši)", true),
            ("TotalTimeInSystemAmbulanced", "Čas v systéme (Sanitka)", true),
            ("EntryQueueWaitWalkIn", "Čakacia doba na vstupe (Peši)", true),
            ("EntryQueueWaitAmbulanced", "Čakacia doba na vstupe (Sanitka)", true),
            ("EntryQueueLength", "Priemerná dĺžka radu na vstupe", false),
            ("MedicalTreatWaitingTimeA", "Čakacia doba na vyšetrenie A", true),
            ("MedicalTreatWaitingTimeAB", "Čakacia doba na vyšetrenie AB", true),
            ("MedicalTreatWaitingTimeB", "Čakacia doba na vyšetrenie B", true),
            ("AllDoctorsUtil", "Využitie lekárov", false),
            ("AllNursesUtil", "Využitie sestier", false),
            ("AllRoomAUtil", "Využitie miestností A", false),
            ("AllRoomBUtil", "Využitie miestností B", false),
            ("FromEntryToMedicalWalkIn", "Čas od vstupu po oštrenie (peši)", true),
            ("FromEntryToMedicalAmbulance", "Čas od vstupu po ošetrenie (Sanitka)", true),
            ("TimeFromEntryToMedicalPriority1", "Čas od vstupu po ošetrenie (Priorita 1)", true),
            ("TimeFromEntryToMedicalPriority2", "Čas od vstupu po ošetrenie (Priorita 2)", true),
            ("TimeFromEntryToMedicalPriority3", "Čas od vstupu po ošetrenie (Priorita 3)", true),
            ("TimeFromEntryToMedicalPriority4", "Čas od vstupu po ošetrenie (Priorita 4)", true),
            ("TimeFromEntryToMedicalPriority5", "Čas od vstupu po ošetrenie (Priorita 5)", true)
        };

        try
        {
            using (var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Hlavička CSV: English, Slovak, Average, 95% C.I.
                writer.WriteLine("English Metric,Štatistika,Priemer,95% I.S.");
                writer.WriteLine($"Simulation Replication,Počet replikácii,{_latestStats.Replication + 1},");

                foreach (var item in metricsToExport)
                {
                    if (_latestStats.Stats.TryGetValue(item.EnglishKey, out var s) && s != null)
                    {
                        // Formátovanie štatistiky (Statistic)
                        string avgFormatted = item.IsTime 
                            ? $"{TimeSpan.FromSeconds(s.Avg):hh\\:mm\\:ss}" 
                            : s.Avg.ToString("F3");

                        string ciFormatted = $"\"[{s.LowerBound:F2} - {s.UpperBound:F2}]\"";

                        // Zápis v poradí: English, Slovak, Statistic (Average), CI
                        writer.WriteLine($"{item.EnglishKey},{item.SlovakName},{avgFormatted},{ciFormatted}");
                    }
                }
            }
            Console.WriteLine($"CSV Exported to: {filePath}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export error: {ex.Message}");
        }
    }
    
    private void OnWindowClosed(object? sender, FormClosedEventArgs e)
    {
        if (_currentModel != null)
        {
            _currentModel.StopSimulation();
            // Unsubscribe model from the refresh event to stop background logic
            _currentModel.OnTurboUI -= OnTurboRefresh;
        }

        // 2. Unsubscribe from Window events to prevent memory leaks
        if (_turboWindow != null)
        {
            _turboWindow.OnRunRequested -= OnRunRequested;
            _turboWindow.OnPauseRequested -= OnPauseRequested;
            _turboWindow.OnStopRequested -= OnStopRequested;
            _turboWindow.FormClosed -= OnWindowClosed;
            _turboWindow = null;
        }

        _isStarted = false;
    }
}